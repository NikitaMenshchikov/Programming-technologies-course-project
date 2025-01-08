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
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
//using static AttemptAtCoursework.Models.StatusforCompany;

namespace AttemptAtCoursework.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Companies
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Company.ToListAsync());
        }

        public IActionResult SearchCompanyForStatus(uint? statusValue)
        {
            var companies = _context.Company.ToList();
            if (statusValue == null)
                return PartialView("_partialIndexTableWithCompanies", companies);
            companies = _context.Company.Where(e => (uint)e.Status == statusValue).ToList();
            return PartialView("_partialIndexTableWithCompanies", companies);
        }

        public IActionResult SearchCompanyForName(string? companyName)
        {
            var companies = _context.Company.ToList();
            if (companyName == null)
                return PartialView("_partialIndexTableWithCompanies", companies);
            companies = _context.Company.Where(e => e.Name.Contains(companyName)).ToList();
            return PartialView("_partialIndexTableWithCompanies", companies);
        }

        public async Task<IActionResult> CompanyInfo(uint? id)
        {
            var vacancies = _context.Vacancy.Where(e => e.Status == Status.Active).ToList() ?? Enumerable.Empty<Vacancy>();
            vacancies = vacancies.Where(e => e.CompanyId == id).ToList();
            ViewBag.Vacancies = vacancies;
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }


        public IActionResult EmployerCreatedCompanies()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return NotFound();
            }
            var companies = _context.Company.Where(e => e.EmployerId == userId).ToList(); ;
            //if (companies == null)
            //{
            //    return NotFound();
            //}

            return View(companies);
        }

        // GET: Companies/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,City,TypeOfProduction,Description,DirectorsMail,CompaniesMail,EmployerId,Status")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompany([Bind("Id,Name,City,TypeOfProduction,Description,DirectorsMail,CompaniesMail,EmployerId,Status")] Company company)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                company.DirectorsMail = user.Email;
                company.EmployerId = userId;
                company.Status = StatusforCompany.ConsideredByTheManager;
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EmployerCreatedCompanies));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Name,City,TypeOfProduction,Description,DirectorsMail,CompaniesMail,EmployerId,Status")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }


        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> RecallCompany(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost, ActionName("RecallCompany")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecallCompanyConfirmed(uint id)
        {
            var company = await _context.Company.FindAsync(id);
            if (company != null)
            {
                company.Status = StatusforCompany.Inactive;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var company = await _context.Company.FindAsync(id);
            if (company != null)
            {
                _context.Company.Remove(company);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(uint id)
        {
            return _context.Company.Any(e => e.Id == id);
        }
    }
}
