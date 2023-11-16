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

		public Guid FloorId { get; set; }

		public bool HasAirConditioner  { get; set; }

		[ForeignKey(nameof(FloorId))]
		public Floor Floor { get; set; }

		public virtual ICollection<Desk> Desks { get; set; }
	}
}