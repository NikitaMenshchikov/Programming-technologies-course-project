﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttemptAtCoursework.Data;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Identity;

namespace AttemptAtCoursework.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VacanciesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Vacancies
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            return View(await _context.Vacancy.ToListAsync());
        }

        public IActionResult Vacancies(int? vacancyId)
        {
            var workPositions = _context.WorkPosition.ToList();
            //ViewBag.WorkPositions = workPositions;
            var workPositionsUsed = new List<WorkPosition>();

            var companies = _context.Company.ToList();
            //ViewBag.WorkPositions = workPositions;
            var companiesUsed = new List<Company>();

            //var recordLabels = _context.Vacancy.ToList();
            //ViewBag.RecordLabel = recordLabels;

            var vacancies = _context.Vacancy.Where(e => e.Status == Status.Active).ToList();
            var vacancyUsed = new List<Vacancy>();
            if (vacancyId == null)
            {
                foreach (var vacancy in vacancies)
                {
                    foreach (var company in companies)
                    {
                        if (company.Id == vacancy.CompanyId && company.Status == StatusforCompany.Active)
                        {
                            companiesUsed.Add(company);
                            vacancyUsed.Add(vacancy);
                        }
                    }
                }


                foreach (var vacancy in vacancyUsed)
                {
                    foreach (var workPosition in workPositions)
                    {
                        if (workPosition.Id == vacancy.WorkPositionId)
                        {
                            workPositionsUsed.Add(workPosition);
                        }
                    }
                }

                ViewBag.WorkPositions = workPositionsUsed;
                ViewBag.Companies = companiesUsed;
                return View(vacancyUsed);
            }

            vacancies = vacancies.Where(e => e.Id == vacancyId).ToList();

            foreach (var vacancy in vacancies)
            {
                foreach (var company in companies)
                {
                    if (company.Id == vacancy.CompanyId && company.Status == StatusforCompany.Active)
                    {
                        companiesUsed.Add(company);
                        vacancyUsed.Add(vacancy);

                    }
                }
            }

            foreach (var vacancy in vacancyUsed)
            {
                foreach (var workPosition in workPositions)
                {
                    if (workPosition.Id == vacancy.WorkPositionId)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
            }
            //workPositionsUsed = _context.WorkPosition.Where(e => e.Id == vacancyId).ToList();
            ViewBag.WorkPositions = workPositionsUsed;
            ViewBag.Companies = companiesUsed;
            return View(vacancyUsed);
        }

        public IActionResult SearchVacancyForStatus(uint? statusValue)
        {
            var vacancies = _context.Vacancy.ToList();
            if (statusValue == null)
                return PartialView("_partialIndexTableWithVacancies", vacancies);
            vacancies = _context.Vacancy.Where(e => (uint)e.Status == statusValue).ToList();
            return PartialView("_partialIndexTableWithVacancies", vacancies);
        }

        public IActionResult VacanciesOfferedByEmployer()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return NotFound();
            }
            var companies = _context.Company.Where(e => e.EmployerId == userId).ToList();
            var vacancies = _context.Vacancy.ToList();
            var vacanciesUsed = new List<Vacancy>();

            var workPositions = _context.WorkPosition.ToList();
            var workPositionsUsed = new List<WorkPosition>();

            var companiesUsed = new List<Company>();

            foreach (var company in companies)
            {
                foreach (var vacancy in vacancies)
                {
                    if (company.Id == vacancy.CompanyId)
                    {
                        vacanciesUsed.Add(vacancy);
                    }
                }
            }

            foreach (var vacancy in vacanciesUsed)
            {
                foreach (var workPosition in workPositions)
                {
                    if (workPosition.Id == vacancy.WorkPositionId)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
            }

            foreach(var vacancy in vacanciesUsed)
            {
                foreach (var company in companies)
                {
                    if (company.Id == vacancy.CompanyId)
                    {
                        companiesUsed.Add(company);
                    }
                }
            }
            ViewBag.Companies = companiesUsed;
            if(!companiesUsed.Any())
            {
                ViewBag.Companies = companies;
            }
            ViewBag.WorkPositions = workPositionsUsed;

            return View(vacanciesUsed);
        }

        // GET: Vacancies/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(uint? id)
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        // GET: Vacancies/Create
       [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            return View();
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkPositionId,NumberOfRequiredApplicants,NumberOfApplicantsPlaced,Description,RequiredExperience,TypeOfEmployment,CompanyId,Status")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                if(User.IsInRole("Employer"))
                { return RedirectToAction(nameof(Vacancies));
                }
                if (User.IsInRole("Manager")) { 
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(vacancy);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult AddVacancy(int? companyId)
        {
            if (companyId == null)
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                var companies = _context.Company.Where(e => e.EmployerId == userId).ToList();
                ViewBag.Companies = companies;
                var workPositions = _context.WorkPosition.ToList();
                ViewBag.WorkPositions = workPositions;
                return View();
            }
            else
            {
                var companies = _context.Company.Where(e => e.Id == companyId).ToList();
                ViewBag.Companies = companies;
                var workPositions = _context.WorkPosition.ToList();
                ViewBag.WorkPositions = workPositions;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVacancy([Bind("Id,WorkPositionId,NumberOfRequiredApplicants,NumberOfApplicantsPlaced,Description,RequiredExperience,TypeOfEmployment,CompanyId,Status")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                vacancy.Status = Status.ConsideredByTheManager;
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                if (User.IsInRole("Employer"))
                {
                    return RedirectToAction(nameof(VacanciesOfferedByEmployer));
                }
                if (User.IsInRole("Manager"))
                {
                    return RedirectToAction(nameof(Index));
                }
                //               return RedirectToAction(nameof(Index));
            }
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(uint? id)
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,WorkPositionId,NumberOfRequiredApplicants,NumberOfApplicantsPlaced,Description,RequiredExperience,TypeOfEmployment,CompanyId, Status")] Vacancy vacancy)
        {
            if (id != vacancy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacancyExists(vacancy.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vacancy);
        }

        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> RecallVacancy(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }
            var workPosition = await _context.WorkPosition.FirstOrDefaultAsync(m => m.Id == vacancy.WorkPositionId);
            var company = await _context.Company.FirstOrDefaultAsync(m => m.Id == vacancy.CompanyId);
            ViewBag.WorkPosition = workPosition;
            ViewBag.Company = company;
            return View(vacancy);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost, ActionName("RecallVacancy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecallVacancyConfirmed(uint id)
        {
            var vacancy = await _context.Vacancy.FindAsync(id);
            if (vacancy != null)
            {
                vacancy.Status = Status.Inactive;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VacanciesOfferedByEmployer));
        }

        // GET: Vacancies/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var vacancy = await _context.Vacancy.FindAsync(id);
            if (vacancy != null)
            {
                _context.Vacancy.Remove(vacancy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacancyExists(uint id)
        {
            return _context.Vacancy.Any(e => e.Id == id);
        }
    }
}
