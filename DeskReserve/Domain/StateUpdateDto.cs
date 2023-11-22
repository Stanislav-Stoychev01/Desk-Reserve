using System.ComponentModel.DataAnnotations;
using DeskReserve.Utils;

namespace DeskReserve.Domain
{
	public class StateUpdateDto
	{
		[Required]
		public BookingState NewState { get; set; }
	}


}
