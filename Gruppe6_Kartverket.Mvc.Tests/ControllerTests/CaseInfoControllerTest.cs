using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Models.Database;

public class CaseInfoControllerTests
{
    [Fact]
    // checks if CaseInfoViewWithModel action in CaseInfoController returns a ViewResult.
    public void CaseInfoViewWithModelReturnsCorrectView()
    {
        // Arrange - Creates an instance of the CaseInfoController.
        var unitUnderTest = SetupUnitUnderTest();
        var model = new CaseRecord
        {
            CaseRecordId = 1,
            CaseDate = DateTime.Now,
            CaseTitle = "Test Case",
            CaseIssueType = "Issue Type",
            CaseDescription = "Description",
            CaseStatus = "Open"
        };

        // Act - Calls the CaseInfoViewWithModel method on the controller
        var result = unitUnderTest.CaseInfoViewWithModel(model) as ViewResult;

        // Assert - Checks if the result is of type ViewResult
        Assert.NotNull(result);
        Assert.Same("CaseInfo", result.ViewName);
    }

    [Fact]
    // checks if CaseInfoViewWithModel action in CaseInfoController returns a ViewResult with a CaseRecord model.
    public void CaseInfoViewWithModelReturnsCorrectModel()
    {
        // Arrange - Creates an instance of the CaseInfoController.
        var unitUnderTest = SetupUnitUnderTest();
        var model = new CaseRecord
        {
            CaseRecordId = 1,
            CaseDate = DateTime.Now,
            CaseTitle = "Test Case",
            CaseIssueType = "Issue Type",
            CaseDescription = "Description",
            CaseStatus = "Open"
        };

        // Act - Calls the CaseInfoViewWithModel method on the controller
        var result = unitUnderTest.CaseInfoViewWithModel(model) as ViewResult;

        // Assert  - Checks if the model is of type CaseRecord
        Assert.NotNull(result.Model);
        Assert.IsType<CaseRecord>(result.Model);
    }

    [Fact]
    // checks if CaseInfoViewWithModel action in CaseInfoController returns a ViewResult with the same model if the model is invalid.
    public void CaseInfoViewWithInvalidModelReturnsSameView()
    {
        // Arrange - Creates an instance of the CaseInfoController.
        var unitUnderTest = SetupUnitUnderTest();
        unitUnderTest.ModelState.AddModelError("CaseTitle", "Required");
        var model = new CaseRecord
        {
            CaseRecordId = 1,
            CaseDate = DateTime.Now,
            CaseTitle = "",
            CaseIssueType = "Issue Type",
            CaseDescription = "Description",
            CaseStatus = "Open"
        };

        // Act - Calls the CaseInfoViewWithModel method on the controller
        var result = unitUnderTest.CaseInfoViewWithModel(model) as ViewResult;

        // Assert - Checks if the result is of type ViewResult and the model is the same as the input model
        Assert.Same("CaseInfo", result.ViewName);
        Assert.Same(model, result.Model);
        Assert.NotNull(result);
        Assert.NotNull(result.Model);
    }

    // SetupUnitUnderTest: Creates and returns an instance of the CaseInfoController.
    private static CaseInfoController SetupUnitUnderTest()
    {
        var unitUnderTest = new CaseInfoController();
        return unitUnderTest;
    }
}
