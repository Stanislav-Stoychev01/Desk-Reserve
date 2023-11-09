using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
