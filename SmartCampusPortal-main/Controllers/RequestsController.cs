using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly CampusContext _db;
        public RequestsController(CampusContext db) { _db = db; }

        // GET: api/requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetAll()
            => Ok(await _db.ServiceRequests.ToListAsync());

        // GET: api/requests/stats  (feature extension: summary counts)
        [HttpGet("stats")]
        public async Task<IActionResult> Stats()
        {
            return Ok(new
            {
                total = await _db.ServiceRequests.CountAsync(),
                pending = await _db.ServiceRequests.CountAsync(r => r.Status == "Pending"),
                inProgress = await _db.ServiceRequests.CountAsync(r => r.Status == "In Progress"),
                resolved = await _db.ServiceRequests.CountAsync(r => r.Status == "Resolved")
            });
        }

        // GET: api/requests/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceRequest>> GetById(int id)
        {
            var r = await _db.ServiceRequests.FindAsync(id);
            if (r == null) return NotFound("Request not found");
            return Ok(r);
        }

        // POST: api/requests
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> Add(ServiceRequest model)
        {
            model.Id = 0;
            model.CreatedDate = DateTime.Now;
            if (string.IsNullOrEmpty(model.Status)) model.Status = "Pending";
            _db.ServiceRequests.Add(model);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/requests/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ServiceRequest model)
        {
            var r = await _db.ServiceRequests.FindAsync(id);
            if (r == null) return NotFound("Request not found");

            r.StudentName = model.StudentName;
            r.Category = model.Category;
            r.Description = model.Description;
            r.Status = model.Status;
            await _db.SaveChangesAsync();
            return Ok(r);
        }

        // DELETE: api/requests/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.ServiceRequests.FindAsync(id);
            if (r == null) return NotFound("Request not found");

            _db.ServiceRequests.Remove(r);
            await _db.SaveChangesAsync();
            return Ok("Request deleted successfully");
        }
    }
}