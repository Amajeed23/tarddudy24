using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FirstAttempt.Models
{
    public class Passenger
    {

		[Key]public int PassID  { get; set; }
        public string Passname { get; set; }
        public string Password { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime Passbirth { get; set; }

        public int Passphone { get; set; }
        public int PassCondition { get; set; }

     
    }
}
