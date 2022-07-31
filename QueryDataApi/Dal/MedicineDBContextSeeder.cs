using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace MedicineProject.Dal
{
    public static class MedicineDBContextSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicineModel>().HasData(
                new MedicineModel
                {
                    Id = 1,
                    Name = "Sudhir12",

                },
                new MedicineModel
                {
                    Id = 2,
                    Name = "Arnav1",

                },
                 new MedicineModel
                 {
                     Id = 3,
                     Name = "nitu11",

                 },
                 new MedicineModel
                 {
                     Id = 4,
                     Name = "nitu1",

                 },
                  new MedicineModel
                  {
                      Id = 5,
                      Name = "nitu1",

                  }
            );
        }
    }
}
