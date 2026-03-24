using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Food.domain.Models;
using Food.mvc.Data;

namespace Food.mvc.Controllers
{
    public class PremisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PremisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Premises
        public async Task<IActionResult> Index()
        {
            return View(await _context.Premises.ToListAsync());
        }

        // GET: Premises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var premise = await _context.Premises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (premise == null)
            {
                return NotFound();
            }

            return View(premise);
        }

        // GET: Premises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Premises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Town,RiskRating")] Premise premise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(premise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(premise);
        }

        // GET: Premises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var premise = await _context.Premises.FindAsync(id);
            if (premise == null)
            {
                return NotFound();
            }
            return View(premise);
        }

        // POST: Premises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Town,RiskRating")] Premise premise)
        {
            if (id != premise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(premise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PremiseExists(premise.Id))
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
            return View(premise);
        }

        // GET: Premises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var premise = await _context.Premises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (premise == null)
            {
                return NotFound();
            }

            return View(premise);
        }

        // POST: Premises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var premise = await _context.Premises.FindAsync(id);
            if (premise != null)
            {
                _context.Premises.Remove(premise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PremiseExists(int id)
        {
            return _context.Premises.Any(e => e.Id == id);
        }
    }
}
