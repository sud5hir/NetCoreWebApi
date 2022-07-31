using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApplication1.Model
{
    public class MedicineModel
    {
      //  [Key]
      
        public int Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public DateTime ExpiryDate { get; set; }

        public int Quantity { get; set; }

        //public decimal Price { get; set; }

        public string Brand { get; set; }
    }
}
