using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models{
    public class WeddingPlannerContext : DbContext{
        public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext>options) : base(options) {}
        
        public DbSet<Users> users { get; set; }
        public DbSet<Wedd> weddings {get; set; }
        public DbSet<WedPlan> weddingplan{ get; set; }
        
    }
}