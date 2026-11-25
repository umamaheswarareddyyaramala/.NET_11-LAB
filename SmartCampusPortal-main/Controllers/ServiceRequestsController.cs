using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly CampusContext _db;
        public ServiceRequestsController(CampusContext db) { _db = db; }

        public async Task<IActionResult> Index(string? status)
        {
            var query = _db.ServiceRequests.AsQueryable();
            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            ViewBag.Total = await _db.ServiceRequests.CountAsync();
            ViewBag.Pending = await _db.ServiceRequests.CountAsync(r => r.Status == "Pending");
            ViewBag.InProgress = await _db.ServiceRequests.CountAsync(r => r.Status == "In Progress");
            ViewBag.Resolved = await _db.ServiceRequests.CountAsync(r => r.Status == "Resolved");

            return View(await query.OrderByDescending(r => r.CreatedDate).ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest model)
        {
            if (!ModelState.IsValid) return View(model);
            model.Status = "Pending";
            model.CreatedDate = DateTime.Now;
            _db.ServiceRequests.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var r = await _db.ServiceRequests.FindAsync(id);
            if (r == null) return NotFound();
            return View(r);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest model)
        {
            if (!ModelState.IsValid) return View(model);
            var r = await _db.ServiceRequests.FindAsync(id);
            if (r == null) return NotFound();

            r.StudentName = model.StudentName;
            r.Category = model.Category;
            r.Description = model.Description;
            r.Status = model.Status;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.ServiceRequests.FindAsync(id);
            if (r != null)
            {
                _db.ServiceRequests.Remove(r);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}