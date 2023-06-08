using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAttempt.Models
{
    public class TicketTime
    {
        [Key]public int TicketID { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime TicketDate { get; set; }

        public int PassID { get; set; } 
        public int ScheduleID { get; set; }
        public int Feedback { get; set; }
        

    }
}
