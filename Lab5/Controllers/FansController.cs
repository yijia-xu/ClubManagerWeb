using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5.Data;
using Lab5.Models;
using Lab5.Models.ViewModels;

namespace Lab5.Controllers
{
    public class FansController : Controller
    {
        private readonly SportsDbContext _context;

        public FansController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: Fans
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new SportClubViewModel
            {
                Fans = await _context.Fans.ToListAsync(),
                SportClubs = new List<SportClub>()
            };


            if (id.HasValue)
            {
                viewModel.SportClubs = await _context.Subscriptions
                    .Where(s => s.FanId == id.Value)
                    .Select(s => s.SportClubs)
                    .Distinct()
                    .ToListAsync();
                ViewData["SelectedFanId"] = id.Value;
            }
            else
            {
                ViewData["HasSubscriptions"] = null;
            }

            return View(viewModel);
        }

        // GET: Fans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // GET: Fans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fan);
        }

        // GET: Fans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans.FindAsync(id);
            if (fan == null)
            {
                return NotFound();
            }
            return View(fan);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (id != fan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanExists(fan.Id))
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
            return View(fan);
        }

        public async Task<IActionResult> EditSubscriptions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                        .Include(f => f.Subscriptions)
                        .ThenInclude(s => s.SportClubs)
                        .FirstOrDefaultAsync(f => f.Id == id);

            if (fan == null)
            {
                return NotFound();
            }

            var sportClubs = await _context.SportClubs.ToListAsync();
            var selectedClubIds = fan.Subscriptions.Select(s => s.SportClubId).ToList();

            var subscribedViewModels = fan.Subscriptions.Select(s => new SportClubSubscriptionViewModel
            {
                SportClubId = s.SportClubId,
                Title = s.SportClubs.Title,
                IsMember = true
            }).ToList();

            var notSubscribedViewModels = sportClubs
                .Where(s => !selectedClubIds.Contains(s.Id))
                .Select(s => new SportClubSubscriptionViewModel
                {
                    SportClubId = s.Id,
                    Title = s.Title,
                    IsMember = false
                }).ToList();

            var allFanViewModel = new FanSubscriptionViewModel
            {
                Fan = fan,
                Subscriptions = subscribedViewModels.OrderBy(s => s.Title)
                                  .Concat(notSubscribedViewModels.OrderBy(s => s.Title))
                                  .ToList()
            };

            return View(allFanViewModel);
        }


        public async Task<IActionResult> AddSubscriptions(int fanId, string sportClubId)
        {
            var subscription = new Subscription
            {
                FanId = fanId,
                SportClubId = sportClubId
            };
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditSubscriptions), new {id = fanId});
        }

        public async Task<IActionResult> RemoveSubscriptions(int fanId, string sportClubId)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.FanId == fanId && s.SportClubId == sportClubId);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(EditSubscriptions), new { id = fanId });
        }


        // GET: Fans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                _context.Fans.Remove(fan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FanExists(int id)
        {
            return _context.Fans.Any(e => e.Id == id);
        }
    }
}
