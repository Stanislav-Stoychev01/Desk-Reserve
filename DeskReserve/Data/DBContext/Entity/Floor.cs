using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("Floor")]
    public class Floor
    {
        [Key]
        public Guid FloorId { get; set; }

        [Required]
        public int FloorNumber{ get; set; }

        [Required]
        public bool HasElevator { get; set; }

        public string? FloorCoveringType { get; set; }
        
        public Guid BuildingId { get; set; }

		[ForeignKey(nameof(BuildingId))]
        public Building Building { get; set; }

		public virtual ICollection<Room> Rooms { get; set; }
	}
}
