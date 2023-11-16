using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeskReserve.Utils;

namespace DeskReserve.Data.DBContext.Entity
{
	[Table("Request")]
	public class Request
	{
		[Key]
		public Guid RequestId { get; set; }

		[Required]
		public DateTime? RequestStartDate { get; set; }

		[Required]
		public DateTime? RequestEndDate { get; set; }

		[Required]
		public BookingState State { get; set; } = BookingState.Requested;

		[DefaultValue(false)]
		public bool isPermanentlyOccupied { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[Required]
		public Guid AdminId { get; set; }

		[Required]
		public Guid DeskId { get; set; }
		public Desk Desk { get; set; }
	}
}
