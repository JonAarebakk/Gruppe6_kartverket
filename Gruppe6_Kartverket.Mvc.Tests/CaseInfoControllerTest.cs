using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Gruppe6_Kartverket.Mvc.Tests.ControllerTests
{
    public class CaseInfoControllerTests
    {
        [Fact]
        public void CaseInfoViewWithModel_InvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange - Creates an instance of CaseInfoController using the GetUnitUnderTest
            // Creates a CaseRecord model and adds model state error to simulate an invalid model
            var controller = GetUnitUnderTest();
            var model = new CaseRecord();
            controller.ModelState.AddModelError("Error", "Model is invalid");

            // Act - Calls the CaseInfoViewWithModel method on the controller with the invalid model
            var result = controller.CaseInfoViewWithModel(model);

            // Assert - Verifies that:
            // 1. the result is of type ViewResult
            // 2. the model in the view result is the same as the provided model
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public void CaseInfoViewWithModel_ValidModel_ReturnsViewResultWithModel()
        {
            // Arrange -
            // 1. Creates an instance of CaseInfoController using the GetUnitUnderTest helper method.
            // 2. Creates a CaseRecord model.
            var controller = GetUnitUnderTest();
            var model = new CaseRecord();

            // Act - Calls the CaseInfoViewWithModel method on the controller with the valid model.
            var result = controller.CaseInfoViewWithModel(model);

            // Assert - Verifies that:
            // 1. the result is of type ViewResult
            // 2. the model in the view result is the same as the provided model
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        private CaseInfoController GetUnitUnderTest()
        {
            var controller = new CaseInfoController();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            return controller;
        }
    }
}
