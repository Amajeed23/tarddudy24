using MessagePack;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FirstAttempt.Models
{
    public class Ticket
    {
        public int? TicketID { get; set; }
        [BindProperty, DataType(DataType.Date)]
        public DateTime TicketDate { get; set; }
        
        public int? PassID { get; set; }
        [System.ComponentModel.DataAnnotations.Key] public int? ScheduleID { get; set; }
        public string? StationFrom { get; set; }
        public string? StationTo { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? Time { get; set; }
    }
}
