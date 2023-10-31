using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("building")]
    public class Building
    {
        [Key]
        public Guid BuildingId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(30, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street Address is required.")]
        [StringLength(100, ErrorMessage = "Street Address cannot exceed 100 characters.")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Neighbourhood is required.")]
        [StringLength(50, ErrorMessage = "Neighbourhood cannot exceed 50 characters.")]
        public string Neighbourhood { get; set; }

        [Required(ErrorMessage = "Floors are required.")]
        [DefaultValue(1)]
        public int Floors { get; set; }
    }
}
