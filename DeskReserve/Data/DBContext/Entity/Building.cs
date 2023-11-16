using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("Building")]
    public class Building
    {
        [Key]
        public Guid BuildingId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(85, ErrorMessage = "City cannot exceed 85 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street Address is required.")]
        [StringLength(255, ErrorMessage = "Street Address cannot exceed 255 characters.")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Neighbourhood is required.")]
        [StringLength(100, ErrorMessage = "Neighbourhood cannot exceed 100 characters.")]
        public string Neighbourhood { get; set; }

        [Required(ErrorMessage = "Floors are required.")]
        [DefaultValue(1)]
        public int FloorsCount { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }
    }
}