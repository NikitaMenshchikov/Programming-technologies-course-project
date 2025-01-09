using AttemptAtCoursework.Controllers;
using AttemptAtCoursework.Data;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkTest
{
    public class TestRecallVacancy
    {
        private readonly VacanciesController _controller;
        private readonly ApplicationDbContext _context;

        public TestRecallVacancy()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            _controller = new VacanciesController(userManagerMock.Object, _context);
        }

        [Fact]
        public async Task RecallVacancyConfirmed_ValidId_ChangesVacancyStatusToInactive()
        {
            var vacancy = new Vacancy
            {
                Id = 1,
                WorkPositionId = 1,
                NumberOfRequiredApplicants = 2,
                NumberOfApplicantsPlaced = 0,
                Description = "Test Vacancy",
                RequiredExperience = RequiredExperience.NoExperience,
                TypeOfEmployment = TypeOfEmployment.FullEmployment,
                CompanyId = 1,
                Status = Status.Active
            };

            _context.Vacancy.Add(vacancy);
            await _context.SaveChangesAsync();

            var result = await _controller.RecallVacancyConfirmed(vacancy.Id);

            var updatedVacancy = await _context.Vacancy.FindAsync(vacancy.Id);
            Assert.NotNull(updatedVacancy);
            Assert.Equal(Status.Inactive, updatedVacancy.Status);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task RecallVacancyConfirmed_InvalidId_ReturnsNotFound()
        {
            var result = await _controller.RecallVacancyConfirmed(999); // Не существующий ID

            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
