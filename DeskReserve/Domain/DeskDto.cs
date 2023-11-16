using System.ComponentModel;

namespace DeskReserve.Domain
{
	public class DeskDto
	{
		public uint DeskNumber { get; set; }
		public bool IsOccupied { get; set; }
		public bool IsStatic { get; set; }
	}
}