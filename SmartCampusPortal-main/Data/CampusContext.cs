using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Data
{
    public class CampusContext : DbContext
    {
        public CampusContext(DbContextOptions<CampusContext> options) : base(options) { }

        public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    }
}