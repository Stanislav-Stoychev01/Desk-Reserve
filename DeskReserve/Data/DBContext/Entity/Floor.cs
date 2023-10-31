using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("Floors")]
    public class Floor
    {
        [Key]
        public Guid FloorId { get; set; }

        [Required]
        public int FloorNumber{ get; set; }

        [Required]
        [DefaultValue(false)]
        public bool HasElevator { get; set; }

        [StringLength(150)]
        public string FloorCoveringType { get; set; }
    }
}
