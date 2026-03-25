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
    public class FollowUpsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FollowUpsController> _logger;

        public FollowUpsController(ApplicationDbContext context, ILogger<FollowUpsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: FollowUps
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FollowUps.Include(f => f.Inspection);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FollowUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var followUp = await _context.FollowUps
                .Include(f => f.Inspection)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (followUp == null) return NotFound();

            return View(followUp);
        }

        // GET: FollowUps/Create
        public IActionResult Create()
        {
            ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id");
            return View();
        }

        // POST: FollowUps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InspectionId,DueDate,Status,ClosedDate")] FollowUp followUp)
        {
            var inspection = await _context.Inspections.FindAsync(followUp.InspectionId);

            if (inspection != null && followUp.DueDate < inspection.InspectionDate)
            {
                ModelState.AddModelError("DueDate", "Due date cannot be before inspection date.");

                _logger.LogWarning(
                    "Invalid follow-up create attempt. InspectionId: {InspectionId}, DueDate: {DueDate}, InspectionDate: {InspectionDate}",
                    followUp.InspectionId, followUp.DueDate, inspection.InspectionDate);
            }

            if (ModelState.IsValid)
            {
                _context.Add(followUp);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Follow-up created. FollowUpId: {FollowUpId}, InspectionId: {InspectionId}",
                    followUp.Id, followUp.InspectionId);

                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning(
                "Invalid follow-up create attempt for InspectionId: {InspectionId}",
                followUp.InspectionId);

            ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id", followUp.InspectionId);
            return View(followUp);
        }

        // GET: FollowUps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp == null) return NotFound();

            ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id", followUp.InspectionId);
            return View(followUp);
        }

        // POST: FollowUps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InspectionId,DueDate,Status,ClosedDate")] FollowUp followUp)
        {
            if (id != followUp.Id) return NotFound();

            var inspection = await _context.Inspections.FindAsync(followUp.InspectionId);

            if (inspection != null && followUp.DueDate < inspection.InspectionDate)
            {
                ModelState.AddModelError("DueDate", "Due date cannot be before inspection date.");

                _logger.LogWarning(
                    "Invalid follow-up edit attempt. FollowUpId: {FollowUpId}, InspectionId: {InspectionId}, DueDate: {DueDate}, InspectionDate: {InspectionDate}",
                    followUp.Id, followUp.InspectionId, followUp.DueDate, inspection.InspectionDate);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(followUp);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation(
                        "Follow-up updated. FollowUpId: {FollowUpId}, InspectionId: {InspectionId}",
                        followUp.Id, followUp.InspectionId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowUpExists(followUp.Id))
                    {
                        _logger.LogWarning(
                            "Follow-up update failed because record was not found. FollowUpId: {FollowUpId}",
                            followUp.Id);

                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(
                            "Concurrency error while updating FollowUpId: {FollowUpId}",
                            followUp.Id);

                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning(
                "Invalid follow-up edit attempt. FollowUpId: {FollowUpId}, InspectionId: {InspectionId}",
                followUp.Id, followUp.InspectionId);

            ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id", followUp.InspectionId);
            return View(followUp);
        }

        // GET: FollowUps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var followUp = await _context.FollowUps
                .Include(f => f.Inspection)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (followUp == null) return NotFound();

            return View(followUp);
        }

        // POST: FollowUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var followUp = await _context.FollowUps.FindAsync(id);

            if (followUp != null)
            {
                _context.FollowUps.Remove(followUp);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Follow-up deleted. FollowUpId: {FollowUpId}",
                    id);
            }
            else
            {
                _logger.LogWarning(
                    "Follow-up delete attempted but record not found. FollowUpId: {FollowUpId}",
                    id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FollowUpExists(int id)
        {
            return _context.FollowUps.Any(e => e.Id == id);
        }
    }
}