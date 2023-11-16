using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
	[Table("Desk")]
	public class Desk
	{
		[Key]
		public Guid DeskId { get; set; }
		
		[Required]
		public int DeskNumber { get; set; }

		public Guid RoomId { get; set; }

		[DefaultValue(false)]
		public bool IsOccupied { get; set; }

		[DefaultValue(false)]
		public bool IsStatic { get; set; }

		[ForeignKey(nameof(RoomId))]
		public Room Room { get; set; }
	}
}