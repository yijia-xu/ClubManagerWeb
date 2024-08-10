using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5.Data;
using Lab5.Models;
using Azure.Storage.Blobs;
using Azure;

namespace Lab5.Controllers
{
    public class NewsController : Controller
    {
        private readonly SportsDbContext _context;
        private readonly string _containerName = "images";
        private readonly BlobServiceClient _blobServiceClient;

        public NewsController(SportsDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        // GET: News
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs.FindAsync(id);
            if (sportClub == null)
            {
                return NotFound();
            }

            var newsItems = await _context.News
                .Where(n => n.SportClubId == id)
                .ToListAsync();

            ViewData["SportClubName"] = sportClub.Title;
            ViewData["SportClubId"] = id;

            return View(newsItems);
        }


        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.SportClub)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var sportClub = _context.SportClubs.Find(id);
            if (sportClub == null)
            {
                return NotFound();
            }

            ViewData["SportClubId"] = id;
            ViewData["SportClubName"] = sportClub.Title;

            return View();
        }


        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsId,FileName,Url,SportClubId")] News news, string scid, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    news.SportClubId = scid;

                    BlobContainerClient containerClient;
                    try
                    {
                        containerClient = await _blobServiceClient.CreateBlobContainerAsync(_containerName);
                        containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                    }
                    catch (RequestFailedException)
                    {
                        containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                    }

                    if (image != null)
                    {
                        string randomFileName = Path.GetRandomFileName();
                        string blobFileName = randomFileName;
                        BlobClient blobClient = containerClient.GetBlobClient(blobFileName);

                        using (var stream = image.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        news.FileName = blobFileName;
                        news.Url = blobClient.Uri.ToString();
                    }

                    _context.News.Add(news);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Create", new { id = news.SportClubId });
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = $"An error occurred while uploading the file: {ex.Message}";
                }
            }

            ViewData["SportClubId"] = new SelectList(_context.SportClubs, "Id", "Title", news.SportClubId);
            return View(news);
        }


        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,FileName,Url,SportClubId")] News news)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.NewsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SportClubId"] = new SelectList(_context.SportClubs, "Id", "Id", news.SportClubId);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.SportClub)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            ViewData["SportClubId"] = news.SportClubId;
            ViewData["SportClubName"] = news.SportClub.Title;

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {

                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                BlobClient blobClient = containerClient.GetBlobClient(news.FileName);
                await blobClient.DeleteIfExistsAsync();

                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { id = news.SportClubId });
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsId == id);
        }
    }
}
