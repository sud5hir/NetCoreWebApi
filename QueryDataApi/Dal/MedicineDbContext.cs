using MedicineProject.Dal;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Dal
{
    public class MedicineDbContext : DbContext
    {
        public DbSet<MedicineModel> medicines { get; set; }

        public MedicineDbContext(DbContextOptions options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.\;Database=EFCoreWebDemo;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}
    }
}
