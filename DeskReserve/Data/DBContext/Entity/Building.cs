using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("building")]
    public class Building
    {
        [Key]
        public Guid BuildingId { get; set; }
        public string City { get; set; }
        public string StreetAdress { get; set; }
        public string Neighbourhood { get; set; }
    }
}
