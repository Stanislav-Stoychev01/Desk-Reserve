using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DeskReserve.Data.DBContext.Entity
{
	[Table("Desk")]
	public class Desk
	{
		[Key]
		public Guid DeskId { get; set; }
		[Required]
		public int DeskNumber { get; set; }
		[Required]
		public Guid RoomId { get; set; }
		[DefaultValue(false)]
		public bool IsOccupied { get; set; }
		[DefaultValue(false)]
		public bool IsStatic { get; set; }
		[JsonIgnore]
		public ICollection<Request> Requests { get; }
	}
}