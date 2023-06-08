using System.ComponentModel.DataAnnotations;

namespace FirstAttempt.Models
{
    public class Condition
    {
        [Key]public int PassID { get; set; }
        public int PassCondition { get; set; }
        public int ScheduleID { get; set; }
    }
}
