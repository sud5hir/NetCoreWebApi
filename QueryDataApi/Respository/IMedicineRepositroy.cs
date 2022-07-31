using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Model;

namespace WebApplication1.Respository
{
   public interface IMedicineRepositroy
    {
        Task<List<MedicineModel>> GetMedicines();

        Task<int> AddMedicine(MedicineModel post);      
    }
}
