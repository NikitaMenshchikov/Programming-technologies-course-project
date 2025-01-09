using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AttemptAtCoursework.Controllers;
using AttemptAtCoursework.Data;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace CourseWorkTest
{
    public class CreateWorkPositionTest
    {
        private readonly WorkPositionsController _controller;
        private readonly ApplicationDbContext _context;

        public CreateWorkPositionTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "aspnet-AttemptAtCoursework-71afbff8-b1a3-4ea3-ac7e-a19ff9fe2c10")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new WorkPositionsController(_context);
        }

        [Fact]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            var workPosition = new WorkPosition
            {
                Id = 100,
                Name = "Тестовый Программист",
                Status = StatusForWorkPosition.Active
            };

            var result = await _controller.Create(workPosition);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            Assert.Single(await _context.WorkPosition.ToListAsync());
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsViewWithModel()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var workPosition = new WorkPosition();

            var result = await _controller.Create(workPosition);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(workPosition, viewResult.Model);
        }
    }
}