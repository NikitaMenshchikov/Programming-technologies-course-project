using System;
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
        public async Task<IActionResult> Index()
        {
            var companies = _context.Company.ToList();
            ViewBag.Companies = companies;
            var vacancies = _context.Vacancy.ToList();
            ViewBag.Vacancies = vacancies;
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            return View(await _context.Resume.ToListAsync());
        }

        public IActionResult ResumesOfferedByApplicant()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return NotFound();
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            string applicantMail = user.Email;

            var resumes = _context.Resume.Where(e => e.ApplicantMail == applicantMail).ToList(); ;
            //if (companies == null)
            //{
            //    return NotFound();
            //}

            return View(resumes);
        }

        // GET: Resumes/Details/5
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
        public async Task<IActionResult> Create([Bind("Id,WorkPositionId,Content,Experience,AdvertisedEmploymentType,VacancyId,ApplicantMail,Status")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resume);
        }

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
        public async Task<IActionResult> Offer([Bind("Id,WorkPositionId,Content,Experience,AdvertisedEmploymentType,VacancyId,ApplicantMail,Status")] Resume resume)
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
                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //var workPositions = _context.WorkPosition.ToList();
            //ViewBag.WorkPositions = workPositions;
            return View(resume);
        }

        // GET: Resumes/Edit/5
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
        public async Task<IActionResult> Edit(uint id, [Bind("Id,WorkPositionId,Content,Experience,AdvertisedEmploymentType,VacancyId,ApplicantMail,Status")] Resume resume)
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
