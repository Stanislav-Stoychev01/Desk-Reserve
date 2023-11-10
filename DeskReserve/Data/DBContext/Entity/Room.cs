using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
	[Table("Room")]
	public class Room
	{
		[Key]
		public Guid RoomId { get; set; }
		[Required]
		public int RoomNumber { get; set; }
		[Required]
		public Guid FloorId { get; set; }
		public bool HasAirConditioner  { get; set; }
	}
}