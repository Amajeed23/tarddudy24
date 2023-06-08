using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAttempt.Models
{
	public class Shift
	{

		[Key]public int BusID { get; set; }
		public int BusNumber { get; set; }
		public bool BusStatus { get; set; }

    }
}
