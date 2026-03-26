using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Food.mvc.Data;
using Food.domain.Models;

namespace Food.mvc.Controllers
{
    [Authorize(Roles = "Admin,Inspector,Reader")]
    public class InspectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InspectionsController> _logger;

        public InspectionsController(ApplicationDbContext context, ILogger<InspectionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Admin e Inspector podem ver
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Inspections.Include(i => i.Premise);
            return View(await applicationDbContext.ToListAsync());
        }

        // Admin e Inspector podem ver
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var inspection = await _context.Inspections
                .Include(i => i.Premise)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inspection == null) return NotFound();

            var relatedInspections = await _context.Inspections
                .Include(i => i.Premise)
                .Where(i => i.Id != inspection.Id && i.PremiseId == inspection.PremiseId)
                .OrderByDescending(i => i.InspectionDate)
                .ToListAsync();

            ViewBag.RelatedInspections = relatedInspections;

            return View(inspection);
        }

        // Admin e Inspector podem criar
        [Authorize(Roles = "Admin,Inspector")]
        public IActionResult Create()
        {
            ViewData["PremiseId"] = new SelectList(_context.Premises, "Id", "Name");
            return View();
        }

        // Admin e Inspector podem criar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
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

        // Admin e Inspector podem editar
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection == null) return NotFound();

            ViewData["PremiseId"] = new SelectList(_context.Premises, "Id", "Name", inspection.PremiseId);
            return View(inspection);
        }

        // Admin e Inspector podem editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
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

        // Admin e Inspector podem apagar
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inspection = await _context.Inspections
                .Include(i => i.Premise)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inspection == null) return NotFound();

            return View(inspection);
        }

        // Admin e Inspector podem apagar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
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