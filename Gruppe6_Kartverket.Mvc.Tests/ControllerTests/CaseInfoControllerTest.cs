using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;

public class CaseInfoControllerTests
{
    [Fact]
    // checks if CaseInfo action in CaseInfoController returns a ViewResult.
    public async Task CaseInfoReturnsCorrectView()
    {
        // Arrange - Creates an instance of the CaseInfoController.
        var unitUnderTest = SetupUnitUnderTest();
        var caseRecordId = 1;

        // Act - Calls the CaseInfo method on the controller
        var result = await unitUnderTest.CaseInfo(caseRecordId) as ViewResult;

        // Assert - Checks if the result is of type ViewResult
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    // checks if CaseInfo action in CaseInfoController returns NotFound if the case record is not found.
    public async Task CaseInfoReturnsNotFoundIfCaseRecordNotFound()
    {
        // Arrange - Creates an instance of the CaseInfoController.
        var unitUnderTest = SetupUnitUnderTest();
        var caseRecordId = 5; // Assuming this ID does not exist

        // Act - Calls the CaseInfo method on the controller
        var result = await unitUnderTest.CaseInfo(caseRecordId);

        // Assert - Checks if the result is of type NotFoundResult
        Assert.IsType<NotFoundResult>(result);
    }

    // SetupUnitUnderTest: Creates and returns an instance of the CaseInfoController with a mocked ApplicationDbContext.
    private static CaseInfoController SetupUnitUnderTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ApplicationDbContext(options);

        // Seed the in-memory database with test data
        context.CaseRecords.Add(new CaseRecord
        {
            CaseRecordId = 1, // Ensure this ID is unique or use a different ID if needed
            CaseDate = DateTime.Now,
            CaseTitle = "Test Case",
            CaseIssueType = "Issue Type",
            CaseDescription = "Description",
            CaseStatus = "Open",
            CaseLocation = new CaseLocation
            {
                LocationId = 1,
                GeoJSON = "{}",
                Municipality = "Test Municipality",
                County = "Test County"
            },
            User = CreateTestUser()
        });
        context.SaveChanges();

        return new CaseInfoController(context);
    }

    // Factory method to create a test user with dynamically generated values
    private static User CreateTestUser()
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            UserName = "TestUser_" + Guid.NewGuid(),
            UserPassword = "TestPassword_" + Guid.NewGuid(),
            UserType = "TU" // Ensure this is a valid value for UserType
        };
    }


}
