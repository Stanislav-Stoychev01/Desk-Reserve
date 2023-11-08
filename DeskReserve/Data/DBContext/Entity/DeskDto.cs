using System.ComponentModel;

namespace DeskReserve.Data.DBContext.Entity
{
	public class DeskDto
	{
		public int DeskNumber { get; set; }
		public bool IsOccupied { get; set; }
		public bool IsStatic { get; set; }
	}
}
