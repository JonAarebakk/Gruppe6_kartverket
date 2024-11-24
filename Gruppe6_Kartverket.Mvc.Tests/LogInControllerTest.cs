/*
 using Gruppe6_Kartverket.Mvc.Controllers; // provides access to controllers
using Gruppe6_Kartverket.Mvc.Models; // provides access to models
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Gruppe6_Kartverket.Mvc.Tests.ControllerTests
{
    public class LogInControllerTests
    {
        [Fact]
        public void LogIn_ReturnsViewResult()
        {
            // Arrange - Creates an instance of LogInController using the GetUnitUnderTest helper method.
            var controller = GetUnitUnderTest();

            // Act - Calls the LogIn method on the controller
            var result = controller.LogIn();

            // Assert - Verifies that the result is of type ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void RegistrationForm_Get_ReturnsViewResult()
        {
            // Arrange 
            var controller = GetUnitUnderTest();

            // Act - Calls the RegistrationForm method on the controller
            var result = controller.RegistrationForm();

            // Assert - Verifies that the result is of type ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void RegistrationForm_Post_ReturnsViewResultWithModel()
        {
            // Arrange:
            // 1. Creates an instance of LogInController using the GetUnitUnderTest helper method.
            // 2. Creates a RegistrationFormModel with valid data.
            var controller = GetUnitUnderTest();
            var model = new RegistrationFormModel
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Password = "password",
                ConfirmPassword = "password"
            };

            // Act - Calls the RegistrationForm method on the controller with the valid model.
            var result = controller.RegistrationForm(model);

            // Assert - Verifies that:
            // 1. the result is of type ViewResult
            // 2. that the view name is "Overview"
            // 3. that the model in the view result is the same as the provided model
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Overview", viewResult.ViewName);
            Assert.Equal(model, viewResult.Model);
        }

        private LogInController GetUnitUnderTest()
        {
            var controller = new LogInController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            return controller;
        }
    }
}
*/
