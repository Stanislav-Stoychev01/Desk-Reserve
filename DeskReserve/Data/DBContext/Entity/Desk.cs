using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
	[Table("desk")]
	public class Desk
	{
		[Key]
		public Guid DeskId { get; set; }
		public int DeskNumber { get; set; }
		public Guid RoomId { get; set; }
		public bool IsOccupied { get; set; }
	}
}