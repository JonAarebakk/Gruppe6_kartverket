using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class LandingPageControllerTests
{
    [Fact]
    // checks if LandingPage action in LandingPageController returns a ViewResult.
    public void LandingPageReturnsCorrectView()
    {
        // Arrange - Creates an instance of the LandingPageController.
        var unitUnderTest = SetupUnitUnderTest();

        // Act - Calls the LandingPage method on the controller
        var result = unitUnderTest.LandingPage() as ViewResult;

        // Assert - Checks if the result is of type ViewResult and not null
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    // checks if LinkPage action in LandingPageController returns a ViewResult.
    public void LinkPageReturnsCorrectView()
    {
        // Arrange - Creates an instance of the LandingPageController.
        var unitUnderTest = SetupUnitUnderTest();

        // Act - Calls the LinkPage method on the controller
        var result = unitUnderTest.LinkPage() as ViewResult;

        // Assert - Checks if the result is of type ViewResult and not null
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    // checks if Error action in LandingPageController returns a ViewResult with an ErrorViewModel.
    public void ErrorReturnsCorrectViewWithModel()
    {
        // Arrange - Creates an instance of the LandingPageController.
        var unitUnderTest = SetupUnitUnderTest();
        unitUnderTest.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        unitUnderTest.HttpContext.TraceIdentifier = "TestTraceIdentifier";

        // Act - Calls the Error method on the controller
        var result = unitUnderTest.Error() as ViewResult;

        // Assert - Checks if the result and model are not null and of correct type
        Assert.NotNull(result);
        Assert.NotNull(result.Model);
        Assert.IsType<ErrorViewModel>(result.Model);
    }

    // SetupUnitUnderTest: Creates and returns an instance of the LandingPageController.
    private static LandingPageController SetupUnitUnderTest()
    {
        var unitUnderTest = new LandingPageController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        return unitUnderTest;
    }
}

