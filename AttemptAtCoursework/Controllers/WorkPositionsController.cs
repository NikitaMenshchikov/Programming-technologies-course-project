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
    public class WorkPositionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkPositionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkPositions
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkPosition.ToListAsync());
        }

        public IActionResult SearchWorkPositionForStatus(uint? statusValue)
        {
            var workPositions = _context.WorkPosition.ToList();
            if (statusValue == null)
                return PartialView("_partialIndexTableWithWorkPositions", workPositions);
            workPositions = _context.WorkPosition.Where(e => (uint)e.Status == statusValue).ToList();
            return PartialView("_partialIndexTableWithWorkPositions", workPositions);
        }

        // GET: WorkPositions/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workPosition = await _context.WorkPosition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workPosition == null)
            {
                return NotFound();
            }

            return View(workPosition);
        }

        // GET: WorkPositions/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkPositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status")] WorkPosition workPosition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workPosition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workPosition);
        }

        // GET: WorkPositions/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workPosition = await _context.WorkPosition.FindAsync(id);
            if (workPosition == null)
            {
                return NotFound();
            }
            return View(workPosition);
        }

        // POST: WorkPositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Id,Name,Status")] WorkPosition workPosition)
        {
            if (id != workPosition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workPosition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkPositionExists(workPosition.Id))
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
            return View(workPosition);
        }

        // GET: WorkPositions/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workPosition = await _context.WorkPosition
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workPosition == null)
            {
                return NotFound();
            }

            return View(workPosition);
        }

        // POST: WorkPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            var workPosition = await _context.WorkPosition.FindAsync(id);
            if (workPosition != null)
            {
                _context.WorkPosition.Remove(workPosition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkPositionExists(uint id)
        {
            return _context.WorkPosition.Any(e => e.Id == id);
        }
    }
}
