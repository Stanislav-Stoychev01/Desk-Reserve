using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        //public string FirstName { get; set; }

        //public string LastName { get; set; }

        //public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
