using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FirstAttempt.Models
{
    public class Driver
    {
        [Key]public int DriID { get; set; }
        public string? DriName { get; set; }
        public string? DriPassword { get; set; }
        public int DriPhone { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime DriBirth { get; set; }
        public bool Drivershift { get; set; }
       
    }
}
