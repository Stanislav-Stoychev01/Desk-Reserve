using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("Floors")]
    public class Floor
    {
        [Key]
        public Guid FloorId { get; set; }

        public int FloorNumber{ get; set; }

        public bool HasElevator { get; set; }

        [StringLength(150)]
        public string FloorCoveringType { get; set; }
    }
}
