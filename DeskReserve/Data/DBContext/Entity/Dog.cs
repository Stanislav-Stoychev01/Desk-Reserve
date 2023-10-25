using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
    [Table("dog")]
    public class Dog
    {
        [Key]
        public Guid Id { get; set; }
        public int Age { get; set; }
        public String Name { get; set; }
    }
}