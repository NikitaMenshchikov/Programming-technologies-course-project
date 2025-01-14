﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttemptAtCoursework.Data;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Humanizer;
using Microsoft.AspNetCore.Authorization;

namespace AttemptAtCoursework.Controllers
{
    public class ResumesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResumesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Resumes
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var vacancies = _context.Vacancy.ToList();
 
            var workPositions = _context.WorkPosition.ToList();

            var resumes = _context.Resume.ToList();
            var workPositionsUsed = new List<WorkPosition>();
            var vacanciesUsed = new List<Vacancy>();
            foreach (var resume in resumes)
            {
                foreach (var workPosition in workPositions) 
                {
                    if (resume.WorkPositionId == workPosition.Id)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
                foreach(var vacancy in vacancies)
                {
                    if (resume.VacancyId == vacancy.Id)
                    {
                        vacanciesUsed.Add(vacancy);
                    }
                }
            }


            ViewBag.WorkPositions = workPositionsUsed;
            ViewBag.Vacancies = vacanciesUsed;
            return View(await _context.Resume.ToListAsync());
        }

        public IActionResult SearchResumeForStatus(uint? statusValue)
        {
            var resumes = _context.Resume.ToList();

            var vacancies = _context.Vacancy.ToList();

            var workPositions = _context.WorkPosition.ToList();
            var workPositionsUsed = new List<WorkPosition>();
            var vacanciesUsed = new List<Vacancy>();

            if (statusValue == null)
            {
                foreach (var resume in resumes)
                {
                    foreach (var workPosition in workPositions)
                    {
                        if (resume.WorkPositionId == workPosition.Id)
                        {
                            workPositionsUsed.Add(workPosition);
                        }
                    }
                    foreach (var vacancy in vacancies)
                    {
                        if (resume.VacancyId == vacancy.Id)
                        {
                            vacanciesUsed.Add(vacancy);
                        }
                    }
                }

                ViewBag.WorkPositions = workPositionsUsed;
                ViewBag.Vacancies = vacanciesUsed;
                return PartialView("_partialIndexTableWithResumes", resumes);
            }
            resumes = _context.Resume.Where(e => (uint) e.Status == statusValue).ToList();
            foreach (var resume in resumes)
            {
                foreach (var workPosition in workPositions)
                {
                    if (resume.WorkPositionId == workPosition.Id)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
                foreach (var vacancy in vacancies)
                {
                    if (resume.VacancyId == vacancy.Id)
                    {
                        vacanciesUsed.Add(vacancy);
                    }
                }
            }

            ViewBag.WorkPositions = workPositionsUsed;
            ViewBag.Vacancies = vacanciesUsed;
            return PartialView("_partialIndexTableWithResumes", resumes);
        }
        public IActionResult SearchResumeForApplicantMail(string? applicantMail)
        {
            var resumes = _context.Resume.ToList();

            var vacancies = _context.Vacancy.ToList();

            var workPositions = _context.WorkPosition.ToList();

            var workPositionsUsed = new List<WorkPosition>();

            var vacanciesUsed = new List<Vacancy>();
            if (applicantMail == null)
            {
                            foreach (var resume in resumes)
            {
                foreach (var workPosition in workPositions)
                {
                    if (resume.WorkPositionId == workPosition.Id)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
                    foreach (var vacancy in vacancies)
                    {
                        if (resume.VacancyId == vacancy.Id)
                        {
                            vacanciesUsed.Add(vacancy);
                        }
                    }
                }

            ViewBag.WorkPositions = workPositionsUsed;
            ViewBag.Vacancies = vacanciesUsed;
                return PartialView("_partialIndexTableWithResumes", resumes);
            }
            resumes = _context.Resume.Where(e => e.ApplicantMail.Contains(applicantMail)).ToList();
            foreach (var resume in resumes)
            {
                foreach (var workPosition in workPositions)
                {
                    if (resume.WorkPositionId == workPosition.Id)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
                foreach (var vacancy in vacancies)
                {
                    if (resume.VacancyId == vacancy.Id)
                    {
                        vacanciesUsed.Add(vacancy);
                    }
                }
            }

            ViewBag.WorkPositions = workPositionsUsed;
            ViewBag.Vacancies = vacanciesUsed;
            return PartialView("_partialIndexTableWithResumes", resumes);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult ShowResumesRecommendedByManager()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return NotFound();
            }

            var resumes = _context.Resume.Where(e => e.RecommendedToEmployerId == userId && e.Status == StatusforResume.Active).ToList();

            var workPositions = _context.WorkPosition.ToList();
            var workPositionsUsed = new List<WorkPosition>();

            foreach (var resume in resumes)
            {
                foreach (var workPosition in workPositions)
                {
                    if (workPosition.Id == resume.WorkPositionId)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
            }

            ViewBag.WorkPositions = workPositionsUsed;

            return View(resumes);
        }

        public IActionResult ResumesOfferedForVacancy(uint vacancyId)
        {
            var vacancy = _context.Vacancy.FirstOrDefault(e => e.Id == vacancyId);

            var workPosition = _context.WorkPosition.FirstOrDefault(e => e.Id == vacancy.WorkPositionId);

            var company = _context.Company.FirstOrDefault(e => e.Id == vacancy.CompanyId);

             var resumes = _context.Resume.Where(e => e.VacancyId == vacancyId).ToList();

                ViewBag.WorkPosition = workPosition;
                ViewBag.Company = company;
                ViewBag.Vacancy = vacancy;

                return View(resumes);
            

        }

        [Authorize(Roles = "Applicant")]
        public IActionResult ResumesOfferedByApplicant()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return NotFound();
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            string applicantMail = user.Email;

            var resumes = _context.Resume.Where(e => e.ApplicantMail == applicantMail).ToList();

            var workPositions = _context.WorkPosition.ToList();
            var workPositionsUsed = new List<WorkPosition>();

            var vacancies = _context.Vacancy.ToList();
            var vacanciesUsed = new List<Vacancy>();

            var companies = _context.Company.ToList();
            var companiesUsed = new List<Company>();

            foreach (var resume in resumes)
            {
                foreach (var workPosition in workPositions)
                {
                    if (workPosition.Id == resume.WorkPositionId)
                    {
                        workPositionsUsed.Add(workPosition);
                    }
                }
            }

            foreach (var resume in resumes)
            {
                foreach (var vacancy in vacancies)
                {
                    if (vacancy.Id == resume.VacancyId)
                    {
                        vacanciesUsed.Add(vacancy);
                    }
                }
            }

            foreach (var vacancy in vacanciesUsed)
            {
                foreach (var company in companies) 
                {
                 if (company.Id == vacancy.CompanyId)
                    {
                        companiesUsed.Add(company);
                    }
                }
            }


            ViewBag.WorkPositions = workPositionsUsed;
            ViewBag.Companies = companiesUsed;
            //ViewBag.Vacancies = vacanciesUsed;
            //if (companies == null)
            //{
            //    return NotFound();
            //}

            return View(resumes);
        }

        // GET: Resumes/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(uint? id)
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var vacancies = _context.Vacancy.ToList();
            ViewBag.Vacancies = vacancies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resume
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // GET: Resumes/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var vacancies = _context.Vacancy.ToList();
            ViewBag.Vacancies = vacancies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            return View();
        }

        // POST: Resumes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkPositionId,Content,Experience,AdvertisedEmploymentType,VacancyId,ApplicantMail,RecommendedToEmployerId,Status")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resume);
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult Offer(uint? vacancyId)
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var vacancies = _context.Vacancy.Where(e => e.Status == Status.Active).ToList() ?? Enumerable.Empty<Vacancy>();
            vacancies = vacancies.Where(e => e.Id == vacancyId).ToList();
            ViewBag.Vacancies = vacancies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Offer([Bind("Id,WorkPositionId,Content,Experience,AdvertisedEmploymentType,VacancyId,ApplicantMail,RecommendedToEmployerId,Status")] Resume resume)
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var vacancies = _context.Vacancy.Where(e => e.Status == Status.Active).ToList() ?? Enumerable.Empty<Vacancy>();
            ViewBag.Vacancies = vacancies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            string userId = _userManager.GetUserId(HttpContext.User);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            resume.ApplicantMail = user.Email;

            if (ModelState.IsValid)
            {
                resume.Status = StatusforResume.ConsideredByTheManager;
                resume.RecommendedToEmployerId = "-";
                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ResumesOfferedByApplicant));
            }
            //var workPositions = _context.WorkPosition.ToList();
            //ViewBag.WorkPositions = workPositions;
            return View(resume);
        }

        // GET: Resumes/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(uint? id)
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var vacancies = _context.Vacancy.ToList();
            ViewBag.Vacancies = vacancies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resume.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }
            return View(resume);
        }

        // POST: Resumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,WorkPositionId,Content,Experience,AdvertisedEmploymentType,VacancyId,ApplicantMail,RecommendedToEmployerId,Status")] Resume resume)
        {
            if (id != resume.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resume);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResumeExists(resume.Id))
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
            return View(resume);
        }


        public async Task<IActionResult> RecommendResumeToAnEmployer(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resume
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        [HttpPost, ActionName("RecommendResumeToAnEmployer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecommendResumeToAnEmployerConfirmed(uint id)
        {
            var resume = await _context.Resume.FindAsync(id);
            if (resume != null)
            {
                var vacancyId = resume.VacancyId;
                var vacancy = await _context.Vacancy.FindAsync(vacancyId);
                var companyId = vacancy.CompanyId;
                var company = await _context.Company.FindAsync(companyId);
                var employerId = company.EmployerId;
                resume.RecommendedToEmployerId = employerId;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Employer")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CancelResumeRecommendation(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resume
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        [HttpPost, ActionName("CancelResumeRecommendation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelResumeRecommendationConfirmed(uint id)
        {
            var resume = await _context.Resume.FindAsync(id);
            if (resume != null)
            {
                resume.RecommendedToEmployerId = "-";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> RecallResume(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resume
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }
            var workPosition = await _context.WorkPosition.FirstOrDefaultAsync(m => m.Id == resume.WorkPositionId);
            ViewBag.WorkPosition = workPosition;
            return View(resume);
        }

        [HttpPost, ActionName("RecallResume")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecallResumeConfirmed(uint id)
        {
            var resume = await _context.Resume.FindAsync(id);
            if (resume != null)
            {
                resume.Status = StatusforResume.Inactive;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Resumes/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resume
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var resume = await _context.Resume.FindAsync(id);
            if (resume != null)
            {
                _context.Resume.Remove(resume);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResumeExists(uint id)
        {
            return _context.Resume.Any(e => e.Id == id);
        }
    }
}
