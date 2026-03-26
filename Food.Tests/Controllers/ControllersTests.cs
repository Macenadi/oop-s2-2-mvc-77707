using Food.domain.Models;
using Food.mvc.Controllers;
using Food.mvc.Data;
using Food.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Food.Tests.Controllers
{
    public class ControllersTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Premise_Create_ValidPremise_SavesToDatabase()
        {
            var context = GetDbContext(nameof(Premise_Create_ValidPremise_SavesToDatabase));
            var controller = new PremisesController(context);

            var premise = new Premise
            {
                Name = "Test Cafe",
                Address = "123 Main Street",
                Town = "Dublin",
                RiskRating = "Low"
            };

            var result = await controller.Create(premise);

            Assert.Equal(1, context.Premises.Count());
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Premise_Create_DuplicateAddress_ReturnsModelError()
        {
            var context = GetDbContext(nameof(Premise_Create_DuplicateAddress_ReturnsModelError));

            context.Premises.Add(new Premise
            {
                Name = "Existing Cafe",
                Address = "123 Main Street",
                Town = "Cork",
                RiskRating = "High"
            });
            context.SaveChanges();

            var controller = new PremisesController(context);

            var duplicatePremise = new Premise
            {
                Name = "Another Cafe",
                Address = "123 Main Street",
                Town = "Dublin",
                RiskRating = "Low"
            };

            var result = await controller.Create(duplicatePremise);

            Assert.False(controller.ModelState.IsValid);
            Assert.Equal(1, context.Premises.Count());
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task FollowUp_Create_DueDateBeforeInspectionDate_ReturnsModelError()
        {
            var context = GetDbContext(nameof(FollowUp_Create_DueDateBeforeInspectionDate_ReturnsModelError));

            var premise = new Premise
            {
                Id = 1,
                Name = "Test Premise",
                Address = "45 River Road",
                Town = "Dublin",
                RiskRating = "Medium"
            };

            var inspection = new Inspection
            {
                Id = 1,
                PremiseId = 1,
                Premise = premise,
                InspectionDate = new DateTime(2026, 3, 20),
                Score = 80,
                Outcome = "Pass",
                Notes = "All good"
            };

            context.Premises.Add(premise);
            context.Inspections.Add(inspection);
            context.SaveChanges();

            var logger = new Mock<ILogger<FollowUpsController>>();
            var controller = new FollowUpsController(context, logger.Object);

            var followUp = new FollowUp
            {
                InspectionId = 1,
                DueDate = new DateTime(2026, 3, 10),
                Status = "Open"
            };

            var result = await controller.Create(followUp);

            Assert.False(controller.ModelState.IsValid);
            Assert.Contains(controller.ModelState["DueDate"]!.Errors,
                e => e.ErrorMessage == "Due date cannot be before inspection date.");
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Dashboard_Index_ReturnsCorrectCounts()
        {
            var context = GetDbContext(nameof(Dashboard_Index_ReturnsCorrectCounts));

            var today = DateTime.Today;
            var premise = new Premise
            {
                Id = 1,
                Name = "Test Premise",
                Address = "12 Main Street",
                Town = "Dublin",
                RiskRating = "High"
            };

            var inspection1 = new Inspection
            {
                Id = 1,
                PremiseId = 1,
                Premise = premise,
                InspectionDate = today.AddDays(-2),
                Score = 50,
                Outcome = "Fail",
                Notes = "Issue found"
            };

            var inspection2 = new Inspection
            {
                Id = 2,
                PremiseId = 1,
                Premise = premise,
                InspectionDate = today.AddDays(-1),
                Score = 90,
                Outcome = "Pass",
                Notes = "Good"
            };

            var followUp = new FollowUp
            {
                Id = 1,
                InspectionId = 1,
                Inspection = inspection1,
                DueDate = today.AddDays(-1),
                Status = "Open"
            };

            context.Premises.Add(premise);
            context.Inspections.AddRange(inspection1, inspection2);
            context.FollowUps.Add(followUp);
            context.SaveChanges();

            var controller = new DashboardController(context);

            var result = await controller.Index(null, null) as ViewResult;

            Assert.NotNull(result);

            var model = Assert.IsType<DashboardViewModel>(result!.Model);
            Assert.Equal(2, model.InspectionsThisMonth);
            Assert.Equal(1, model.FailedInspectionsThisMonth);
            Assert.Equal(1, model.OpenOverdueFollowUps);
        }
    }
}