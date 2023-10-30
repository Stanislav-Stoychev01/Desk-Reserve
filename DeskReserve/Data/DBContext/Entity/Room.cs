﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeskReserve.Data.DBContext.Entity
{
	[Table("Room")]
	public class Room
	{
		[Key]
		public Guid RoomId { get; set; }
		public int RoomNumber { get; set; }
		public int FloorId { get; set; }
		public bool HasAirConditioner  { get; set; }
	}
}