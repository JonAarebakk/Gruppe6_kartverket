using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Gruppe6_Kartverket.Mvc.Tests.ControllerTests;

public class LandingPageControllerTests
{
    [Fact]
    public void LandingPage_ReturnsViewResult() 
    // checks if LandingPage action in LandingPageController returns a ViewResult.
    {
        // Act - Calls the LandingPage method on the controller
        var result = GetUnitUnderTest().LandingPage();

        // Assert - Checks if the result is of type ViewResult   
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void LinkPage_ReturnsViewResult()
    {
        // Act  - Calls the LinkPage method on the controller
        var result = GetUnitUnderTest().LinkPage();

        // Assert - Verifies that the result is of type ViewResult
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_ReturnsViewResult_WithErrorViewModel()
    // checks if Error action in LandingPageController returns a ViewResult with an ErrorViewModel.
    {
        // Act - Calls the Error method on the controller
        var result = GetUnitUnderTest().Error();

        // Assert - Verifies that:
        // 1. the result is of type ViewResult
        // 2. the model is of type ErrorViewModel
        // 3. the RequestId property of the model is not null
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.NotNull(model.RequestId);
    }

    //  GetUnitUnderTest: Creates and returns an instance of the LandingPageController with a mocked ILogger and a default HttpContext.
    //	Substitute.For<ILogger>(): Creates a mock logger using NSubstitute.
    //	new LandingPageController() : Instantiates the LandingPageController.
    //	controller.ControllerContext.HttpContext = new DefaultHttpContext() : Sets a default HttpContext for the controller.
    private LandingPageController GetUnitUnderTest()
    {
        var logger = Substitute.For<ILogger<LandingPageController>>();
        var controller = new LandingPageController();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        return controller;
    }

}