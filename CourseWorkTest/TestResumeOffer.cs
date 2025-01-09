using AttemptAtCoursework.Controllers;
using AttemptAtCoursework.Data;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkTest
{
    public class TestResumeOffer
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<UserManager<ApplicationUser>> _mockYourService;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public TestResumeOffer()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            _mockYourService = new Mock<UserManager<ApplicationUser>>();

            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task Offer_ValidResume_ReturnsRedirectToAction()
        {
            var userId = "test-user-id";
            var user = new ApplicationUser { Id = userId, Email = "applicant@example.com" };

            var resume = new Resume
            {
                WorkPositionId = 1,
                Content = "Резюме",
                Experience = Experience.FromOneYearToThreeYears,
                AdvertisedEmploymentType = AdvertisedEmploymentType.FullEmployment,
                VacancyId = 1,
                Status = StatusforResume.Active
            };

            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            _mockUserManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _mockUserManager.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                var controller = new ResumesController(_mockUserManager.Object, context);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, user.Email)
            };
                var identity = new ClaimsIdentity(claims, "TestAuthType");
                var principal = new ClaimsPrincipal(identity);
                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                };

                var result = await controller.Offer(resume);

                var redirectResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("ResumesOfferedByApplicant", redirectResult.ActionName);
            }
        }
    }
}
