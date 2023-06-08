using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAttempt.Models
{
    public class AssignDriver
    {
        
        [System.ComponentModel.DataAnnotations.Key]public int BusID { get; set; }
        public int BusNumber { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan BusWorkhour { get; set; }
        public bool BusStatus { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan BusStart { get; set; }

        public int? DriID { get; set; }
        public string? DriName { get; set; }


    }
}
