using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;
using Gruppe6_Kartverket.Mvc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Security.Claims;

public class MapPageControllerTests
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly IKartverketApiService _kartverketApiService;
    private readonly MapPageController _controller;

    // Constructor to set up the controller and its dependencies
    public MapPageControllerTests()
    {
        _userManager = Substitute.For<UserManager<IdentityUser>>(
            Substitute.For<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
        _dbContext = Substitute.For<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        _kartverketApiService = Substitute.For<IKartverketApiService>();
        _controller = new MapPageController(_userManager, _dbContext, _kartverketApiService);
    }
    
    [Fact]
    // Test that the MapPage action returns a ViewResult
    public void MapPage_Get_ReturnsViewResult()
    {
        // Arrange - Set up a mock user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim("name", "testuser"),
        }, "mock"));

        // Set the controller's context to use the mock user
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act - Call the MapPage action
        var result = _controller.MapPage();

        // Assert - Check that the result is a ViewResult and that the HideFooter property is set to true
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.True((bool?)viewResult.ViewData["HideFooter"] ?? false);
    }

    [Fact]
    public async Task MapPage_Post_InvalidModel_ReturnsViewWithModel()
    {
        // Arrange - Create a new CaseRegistrationModel and add a model error
        var model = new CaseRegistrationModel();
        _controller.ModelState.AddModelError("CaseTitle", "Required");

        // Act - Call the MapPage action
        var result = await _controller.MapPage(model);

        // Assert - Check that the result is a ViewResult
        // and that the model is passed back to the view
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
    }

}