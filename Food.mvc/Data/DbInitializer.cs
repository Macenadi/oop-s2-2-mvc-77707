using Food.domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Food.mvc.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (context.Premises.Any())
            {
                return;
            }

            var premises = new List<Premise>
            {
                new Premise { Name = "Sunrise Cafe", Address = "12 Main Street", Town = "Dublin", RiskRating = "Low" },
                new Premise { Name = "Green Bowl", Address = "45 River Road", Town = "Dublin", RiskRating = "Medium" },
                new Premise { Name = "Ocean Bites", Address = "8 Harbour View", Town = "Dublin", RiskRating = "High" },
                new Premise { Name = "Bella Pasta", Address = "23 Market Lane", Town = "Cork", RiskRating = "Medium" },
                new Premise { Name = "Spice House", Address = "67 King Street", Town = "Cork", RiskRating = "High" },
                new Premise { Name = "Fresh Farm Deli", Address = "101 Station Road", Town = "Cork", RiskRating = "Low" },
                new Premise { Name = "Golden Grill", Address = "9 Bridge Street", Town = "Galway", RiskRating = "High" },
                new Premise { Name = "Healthy Corner", Address = "14 Oak Avenue", Town = "Galway", RiskRating = "Low" },
                new Premise { Name = "Urban Pizza", Address = "77 High Street", Town = "Galway", RiskRating = "Medium" },
                new Premise { Name = "Morning Brew", Address = "31 Elm Road", Town = "Limerick", RiskRating = "Low" },
                new Premise { Name = "River Diner", Address = "52 Mill Lane", Town = "Limerick", RiskRating = "Medium" },
                new Premise { Name = "Fire Wok", Address = "88 Castle Street", Town = "Limerick", RiskRating = "High" }
            };

            context.Premises.AddRange(premises);
            context.SaveChanges();

            var inspections = new List<Inspection>
            {
                new Inspection { PremiseId = premises[0].Id, InspectionDate = DateTime.Today.AddDays(-10), Score = 92, Outcome = "Pass", Notes = "Very clean and well organised." },
                new Inspection { PremiseId = premises[0].Id, InspectionDate = DateTime.Today.AddMonths(-2), Score = 88, Outcome = "Pass", Notes = "Minor documentation issue." },

                new Inspection { PremiseId = premises[1].Id, InspectionDate = DateTime.Today.AddDays(-20), Score = 74, Outcome = "Fail", Notes = "Food storage temperature issue." },
                new Inspection { PremiseId = premises[1].Id, InspectionDate = DateTime.Today.AddMonths(-3), Score = 81, Outcome = "Pass", Notes = "Improved since last review." },

                new Inspection { PremiseId = premises[2].Id, InspectionDate = DateTime.Today.AddDays(-5), Score = 61, Outcome = "Fail", Notes = "Cross-contamination risk found." },
                new Inspection { PremiseId = premises[2].Id, InspectionDate = DateTime.Today.AddMonths(-4), Score = 70, Outcome = "Fail", Notes = "Cleaning procedures incomplete." },

                new Inspection { PremiseId = premises[3].Id, InspectionDate = DateTime.Today.AddDays(-15), Score = 85, Outcome = "Pass", Notes = "Good hygiene standards." },
                new Inspection { PremiseId = premises[3].Id, InspectionDate = DateTime.Today.AddMonths(-1), Score = 79, Outcome = "Fail", Notes = "Handwashing station issue." },

                new Inspection { PremiseId = premises[4].Id, InspectionDate = DateTime.Today.AddDays(-8), Score = 65, Outcome = "Fail", Notes = "Pest control log missing." },
                new Inspection { PremiseId = premises[4].Id, InspectionDate = DateTime.Today.AddMonths(-5), Score = 72, Outcome = "Fail", Notes = "Poor storage practices." },

                new Inspection { PremiseId = premises[5].Id, InspectionDate = DateTime.Today.AddDays(-12), Score = 93, Outcome = "Pass", Notes = "Excellent compliance." },
                new Inspection { PremiseId = premises[5].Id, InspectionDate = DateTime.Today.AddMonths(-2), Score = 90, Outcome = "Pass", Notes = "No major concerns." },

                new Inspection { PremiseId = premises[6].Id, InspectionDate = DateTime.Today.AddDays(-6), Score = 58, Outcome = "Fail", Notes = "Severe cleanliness issue." },
                new Inspection { PremiseId = premises[6].Id, InspectionDate = DateTime.Today.AddMonths(-2), Score = 69, Outcome = "Fail", Notes = "Waste disposal problem." },

                new Inspection { PremiseId = premises[7].Id, InspectionDate = DateTime.Today.AddDays(-18), Score = 95, Outcome = "Pass", Notes = "Very high standards." },
                new Inspection { PremiseId = premises[7].Id, InspectionDate = DateTime.Today.AddMonths(-3), Score = 91, Outcome = "Pass", Notes = "Consistently strong result." },

                new Inspection { PremiseId = premises[8].Id, InspectionDate = DateTime.Today.AddDays(-25), Score = 77, Outcome = "Fail", Notes = "Fridge temperature too high." },
                new Inspection { PremiseId = premises[8].Id, InspectionDate = DateTime.Today.AddMonths(-4), Score = 84, Outcome = "Pass", Notes = "Generally good." },

                new Inspection { PremiseId = premises[9].Id, InspectionDate = DateTime.Today.AddDays(-7), Score = 89, Outcome = "Pass", Notes = "Good record keeping." },
                new Inspection { PremiseId = premises[9].Id, InspectionDate = DateTime.Today.AddMonths(-2), Score = 86, Outcome = "Pass", Notes = "Clean preparation area." },

                new Inspection { PremiseId = premises[10].Id, InspectionDate = DateTime.Today.AddDays(-9), Score = 73, Outcome = "Fail", Notes = "Cleaning schedule not followed." },
                new Inspection { PremiseId = premises[10].Id, InspectionDate = DateTime.Today.AddMonths(-3), Score = 80, Outcome = "Pass", Notes = "Acceptable overall." },

                new Inspection { PremiseId = premises[11].Id, InspectionDate = DateTime.Today.AddDays(-4), Score = 62, Outcome = "Fail", Notes = "Improper raw food handling." },
                new Inspection { PremiseId = premises[11].Id, InspectionDate = DateTime.Today.AddMonths(-1), Score = 68, Outcome = "Fail", Notes = "Sanitation issue found." },

                new Inspection { PremiseId = premises[3].Id, InspectionDate = DateTime.Today.AddDays(-40), Score = 87, Outcome = "Pass", Notes = "Follow-up from prior concern closed." }
            };

            context.Inspections.AddRange(inspections);
            context.SaveChanges();

            var failedInspections = inspections.Where(i => i.Outcome == "Fail").ToList();

            var followUps = new List<FollowUp>
            {
                new FollowUp { InspectionId = failedInspections[0].Id, DueDate = DateTime.Today.AddDays(-5), Status = "Open", ClosedDate = null },
                new FollowUp { InspectionId = failedInspections[1].Id, DueDate = DateTime.Today.AddDays(7), Status = "Open", ClosedDate = null },
                new FollowUp { InspectionId = failedInspections[2].Id, DueDate = DateTime.Today.AddDays(-2), Status = "Closed", ClosedDate = DateTime.Today.AddDays(-1) },
                new FollowUp { InspectionId = failedInspections[3].Id, DueDate = DateTime.Today.AddDays(10), Status = "Open", ClosedDate = null },
                new FollowUp { InspectionId = failedInspections[4].Id, DueDate = DateTime.Today.AddDays(-8), Status = "Open", ClosedDate = null },
                new FollowUp { InspectionId = failedInspections[5].Id, DueDate = DateTime.Today.AddDays(14), Status = "Closed", ClosedDate = DateTime.Today },
                new FollowUp { InspectionId = failedInspections[6].Id, DueDate = DateTime.Today.AddDays(-12), Status = "Open", ClosedDate = null },
                new FollowUp { InspectionId = failedInspections[7].Id, DueDate = DateTime.Today.AddDays(5), Status = "Open", ClosedDate = null },
                new FollowUp { InspectionId = failedInspections[8].Id, DueDate = DateTime.Today.AddDays(-1), Status = "Closed", ClosedDate = DateTime.Today },
                new FollowUp { InspectionId = failedInspections[9].Id, DueDate = DateTime.Today.AddDays(3), Status = "Open", ClosedDate = null }
            };

            context.FollowUps.AddRange(followUps);
            context.SaveChanges();
        }
    }
}