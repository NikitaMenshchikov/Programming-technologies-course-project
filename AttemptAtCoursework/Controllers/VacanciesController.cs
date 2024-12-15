using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AttemptAtCoursework.Data;
using AttemptAtCoursework.Models;
using Microsoft.AspNetCore.Authorization;

namespace AttemptAtCoursework.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacanciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vacancies
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            return View(await _context.Vacancy.ToListAsync());
        }

        public IActionResult Vacancies(int? vacancyId)
        {
            var workPositions = _context.WorkPosition.ToList();
            //ViewBag.WorkPositions = workPositions;
            var workPositionsUsed = new List<WorkPosition>();

            //var recordLabels = _context.Vacancy.ToList();
            //ViewBag.RecordLabel = recordLabels;
            var vacancies = _context.Vacancy.Where(e => e.Status == Status.Active).ToList() ?? Enumerable.Empty<Vacancy>();
            if (vacancyId == null)
            {
                foreach (var vacancy in vacancies)
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
                return View(vacancies);
            }

            vacancies = vacancies.Where(e => e.Id == vacancyId).ToList();
            foreach (var vacancy in vacancies)
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
            return View(vacancies);
        }

        // GET: Vacancies/Details/5
        public async Task<IActionResult> Details(uint? id)
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

        // GET: Vacancies/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            var workPositions = _context.WorkPosition.ToList();
            ViewBag.WorkPositions = workPositions;
            return View();
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WorkPositionId,NumberOfRequiredApplicants,NumberOfApplicantsPlaced,Description,RequiredExperience,TypeOfEmployment,Status")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
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
        public async Task<IActionResult> Edit(uint id, [Bind("Id,WorkPositionId,NumberOfRequiredApplicants,NumberOfApplicantsPlaced,Description,RequiredExperience,TypeOfEmployment,Status")] Vacancy vacancy)
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
