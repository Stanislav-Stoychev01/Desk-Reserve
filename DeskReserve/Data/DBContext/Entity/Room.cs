using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Floor Floor { get; set; }

        [JsonIgnore]
        public virtual ICollection<Desk> Desks { get; set; }
	}
}