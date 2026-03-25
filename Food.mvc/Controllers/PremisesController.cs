using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Food.mvc.Data;
using Food.domain.Models;

namespace Food.mvc.Controllers
{
    [Authorize(Roles = "Admin,Inspector,Viewer")]
    public class PremisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PremisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin e Inspector podem ver
        public async Task<IActionResult> Index()
        {
            return View(await _context.Premises.ToListAsync());
        }

        // Admin e Inspector podem ver
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var premise = await _context.Premises
                .FirstOrDefaultAsync(m => m.Id == id);

            if (premise == null) return NotFound();

            return View(premise);
        }

        // Só Admin pode criar
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Só Admin pode criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        // Só Admin pode editar
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var premise = await _context.Premises.FindAsync(id);
            if (premise == null) return NotFound();

            return View(premise);
        }

        // Só Admin pode editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Town,RiskRating")] Premise premise)
        {
            if (id != premise.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(premise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Premises.Any(e => e.Id == premise.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(premise);
        }

        // Só Admin pode apagar
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var premise = await _context.Premises
                .FirstOrDefaultAsync(m => m.Id == id);

            if (premise == null) return NotFound();

            return View(premise);
        }

        // Só Admin pode apagar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var premise = await _context.Premises.FindAsync(id);
            if (premise != null)
            {
                _context.Premises.Remove(premise);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}