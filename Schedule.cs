using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FirstAttempt.Models
{
    public class Schedule
    {
        public int ScheduleID { get; set; }
        public string StationFrom { get; set; }
       
        [Column(TypeName = "time")]
        public TimeSpan Time { get; set; }
        public string StationTo { get; set; }
        public int BusID { get; set; }
        public int? Seat { get; set; }

    }
}
