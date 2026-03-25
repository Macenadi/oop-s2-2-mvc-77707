using Food.mvc.Data;
using Food.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Food.mvc.Controllers
{
    [Authorize(Roles = "Admin,Viewer,Inspector")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? town, string? riskRating)
        {
            var now = DateTime.Today;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var inspectionsQuery = _context.Inspections
                .Include(i => i.Premise)
                .AsQueryable();

            var followUpsQuery = _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premise)
                .AsQueryable();

            if (!string.IsNullOrEmpty(town))
            {
                inspectionsQuery = inspectionsQuery.Where(i => i.Premise.Town == town);
                followUpsQuery = followUpsQuery.Where(f => f.Inspection.Premise.Town == town);
            }

            if (!string.IsNullOrEmpty(riskRating))
            {
                inspectionsQuery = inspectionsQuery.Where(i => i.Premise.RiskRating == riskRating);
                followUpsQuery = followUpsQuery.Where(f => f.Inspection.Premise.RiskRating == riskRating);
            }

            var model = new DashboardViewModel
            {
                InspectionsThisMonth = await inspectionsQuery.CountAsync(i =>
                    i.InspectionDate >= startOfMonth && i.InspectionDate < endOfMonth),

                FailedInspectionsThisMonth = await inspectionsQuery.CountAsync(i =>
                    i.InspectionDate >= startOfMonth &&
                    i.InspectionDate < endOfMonth &&
                    i.Outcome == "Fail"),

                OpenOverdueFollowUps = await followUpsQuery.CountAsync(f =>
                    f.Status == "Open" && f.DueDate < now),

                SelectedTown = town,
                SelectedRiskRating = riskRating,

                Towns = await _context.Premises
                    .Select(p => p.Town)
                    .Distinct()
                    .OrderBy(t => t)
                    .ToListAsync(),

                RiskRatings = await _context.Premises
                    .Select(p => p.RiskRating)
                    .Distinct()
                    .OrderBy(r => r)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}