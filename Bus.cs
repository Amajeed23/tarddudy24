using NuGet.Packaging.Signing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FirstAttempt.Models
{
    public class Bus
    {
        [Key]public int BusID { get; set; }
        [Required]
        public int BusNumber { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan BusWorkhour { get; set; }
        public bool BusStatus { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan BusStart { get; set; }

        public int DriID { get;set; } 
       
    }
   
}
