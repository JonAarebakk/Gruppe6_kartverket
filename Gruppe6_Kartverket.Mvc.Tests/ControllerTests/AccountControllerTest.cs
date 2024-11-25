using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Models;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Gruppe6_Kartverket.Mvc.Tests.ControllerTests
{
    public class AccountControllerTests
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _userManager = Substitute.For<UserManager<IdentityUser>>(
                Substitute.For<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            _signInManager = Substitute.For<SignInManager<IdentityUser>>(
                _userManager, Substitute.For<IHttpContextAccessor>(), Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
            _dbContext = Substitute.For<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            _controller = new AccountController(_userManager, _signInManager, _dbContext);
        }

        [Fact]
        public async Task LogOut_ReturnsRedirectToActionResult()
        {
            // Arrange - Set up the SignOutAsync method to return a completed task
            _signInManager.SignOutAsync().Returns(Task.CompletedTask);

            // Act - Call the LogOut method
            var result = await _controller.LogOut();

            // Assert - Check that the result is a RedirectToActionResult
            // and that it redirects to the LandingPage action
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("LandingPage", redirectToActionResult.ActionName);
            Assert.Equal("LandingPage", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void LogIn_Get_ReturnsViewResult()
        {
            // Act - Call the LogIn method
            var result = _controller.LogIn();

            // Assert - Check that the result is a ViewResult
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task LogIn_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange - Add a model error
            _controller.ModelState.AddModelError("Email", "Required");

            // Act - Call the LogIn method
            var result = await _controller.LogIn(new LogInModel());

            // Assert - Check that the result is a ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<LogInModel>(viewResult.Model);
        }

        [Fact]
        public void Register_Get_ReturnsViewResult()
        {
            // Act - Call the Register method
            var result = _controller.Register();

            // Assert - Check that the result is a ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Register_Post_InvalidModel_ReturnsViewResult()
        {
            // Arrange - Add a model error
            _controller.ModelState.AddModelError("Email", "Required");

            // Act - Call the Register method
            var result = await _controller.Register(new RegistrationFormModel());

            // Assert - Check that the result is a ViewResult
            // and that the model is passed back to the view
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<RegistrationFormModel>(viewResult.Model);
        }
    }
}