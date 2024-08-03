using Azure.Storage.Blobs;
using Lab5.Data;
using Lab5.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Controllers
{
    public class NewsController : Controller
    {
        private readonly SportsDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;

        public NewsController(SportsDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IActionResult> Index(string id)
        {
            var sportClub = await _context.SportClubs
                .Include(sc => sc.News)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (sportClub == null)
            {
                return NotFound();
            }

            var viewModel = new NewsViewModel
            {
                SportClub = sportClub,
                News = sportClub.News
            };

            return View(viewModel);
        }

    }


}
