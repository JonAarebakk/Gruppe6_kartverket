using Gruppe6_Kartverket.Mvc.Controllers;
using Gruppe6_Kartverket.Mvc.Data;
using Gruppe6_Kartverket.Mvc.Models.Database;
using Gruppe6_Kartverket.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Gruppe6_Kartverket.Mvc.Tests.ControllerTests
{
    public class CaseInfoControllerTests
    {
        [Fact]
        public async Task CaseInfo_InvalidModel_ReturnsNotFound()
        {
            // Arrange
            var controller = GetUnitUnderTest();
            var caseRecordId = 1;

            // Act
            var result = await controller.CaseInfo(caseRecordId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CaseInfo_ValidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var controller = GetUnitUnderTest();
            var caseRecordId = 1;
            var caseRecord = new CaseRecord
            {
                CaseRecordId = caseRecordId,
                CaseTitle = "Test Case"
            };

            var dbContext = Substitute.For<ApplicationDbContext>();
            dbContext.CaseRecords.FindAsync(caseRecordId).ReturnsForAnyArgs(new ValueTask<CaseRecord>(caseRecord));

            // Act
            var result = await controller.CaseInfo(caseRecordId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CaseDetailsViewModel>(viewResult.Model);
            Assert.Equal(caseRecordId, model.CaseRecordId);
        }

        private CaseInfoController GetUnitUnderTest()
        {
            var dbContext = Substitute.For<ApplicationDbContext>();
            var controller = new CaseInfoController(dbContext);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            return controller;
        }
    }
}