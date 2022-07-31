using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Dal;
using WebApplication1.Model;

namespace WebApplication1.Respository
{
    public class MedicineRepositroy : IMedicineRepositroy
    {
        MedicineDbContext _dBContextDal;
        public MedicineRepositroy(MedicineDbContext dBContextDal) 
        {
            _dBContextDal = dBContextDal;

        }

        public async Task<int> AddMedicine(MedicineModel medicine)
        {
            var lastmedicineId = _dBContextDal.medicines.LastOrDefault().Id;
      
            medicine.Id = lastmedicineId + 1;

            try
            {
                await _dBContextDal.medicines.AddAsync(medicine);
                await _dBContextDal.SaveChangesAsync();
                return medicine.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Task<List<MedicineModel>> GetMedicines()
        {
            var medicines = _dBContextDal.medicines;
            if (medicines == null)
                return null;

            return Task.FromResult(_dBContextDal.medicines.ToList());
        }
    }
}
