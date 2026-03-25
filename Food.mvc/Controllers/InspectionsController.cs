using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Food.mvc.Data;
using Food.domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Food.mvc.Controllers
{
    [Authorize(Roles = "Admin,Inspector")]
    public class InspectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InspectionsController> _logger;

        public InspectionsController(ApplicationDbContext context, ILogger<InspectionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Inspections
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Inspections.Include(i => i.Premise);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Inspections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var inspection = await _context.Inspections
                .Include(i => i.Premise)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inspection == null) return NotFound();

            return View(inspection);
        }

        // GET: Inspections/Create
        public IActionResult Create()
        {
            ViewData["PremiseId"] = new SelectList(_context.Premises, "Id", "Name");
            return View();
        }

        // POST: Inspections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PremiseId,InspectionDate,Score,Outcome,Notes")] Inspection inspection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inspection);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Inspection created. PremiseId: {PremiseId}, InspectionId: {InspectionId}",
                    inspection.PremiseId, inspection.Id);

                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning(
                "Invalid inspection create attempt for PremiseId: {PremiseId}",
                inspection.PremiseId);

            ViewData["PremiseId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremiseId);
            return View(inspection);
        }

        // GET: Inspections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection == null) return NotFound();

            ViewData["PremiseId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremiseId);
            return View(inspection);
        }

        // POST: Inspections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PremiseId,InspectionDate,Score,Outcome,Notes")] Inspection inspection)
        {
            if (id != inspection.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inspection);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation(
                        "Inspection updated. InspectionId: {InspectionId}, PremiseId: {PremiseId}",
                        inspection.Id, inspection.PremiseId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InspectionExists(inspection.Id))
                    {
                        _logger.LogWarning(
                            "Inspection update failed because record was not found. InspectionId: {InspectionId}",
                            inspection.Id);

                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(
                            "Concurrency error while updating InspectionId: {InspectionId}",
                            inspection.Id);

                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning(
                "Invalid inspection edit attempt. InspectionId: {InspectionId}, PremiseId: {PremiseId}",
                inspection.Id, inspection.PremiseId);

            ViewData["PremiseId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremiseId);
            return View(inspection);
        }

        // GET: Inspections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inspection = await _context.Inspections
                .Include(i => i.Premise)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inspection == null) return NotFound();

            return View(inspection);
        }

        // POST: Inspections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inspection = await _context.Inspections.FindAsync(id);

            if (inspection != null)
            {
                _context.Inspections.Remove(inspection);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Inspection deleted. InspectionId: {InspectionId}",
                    id);
            }
            else
            {
                _logger.LogWarning(
                    "Inspection delete attempted but record not found. InspectionId: {InspectionId}",
                    id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool InspectionExists(int id)
        {
            return _context.Inspections.Any(e => e.Id == id);
        }
    }
}