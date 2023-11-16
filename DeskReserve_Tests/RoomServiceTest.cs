using DeskReserve.Domain.Service;
using DeskReserve.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeskReserve.Domain.Service;
using DeskReserve.Domain;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Exceptions;

namespace DeskReserve_Tests
{
	[TestFixture]
	internal class RoomServiceTest
	{
		private Mock<IRoomRepository> _roomRepositoryMock;
		private RoomService _roomService;

		[SetUp]
		public void Setup() 
		{
			_roomRepositoryMock = new Mock<IRoomRepository>();
			_roomService = new RoomService( _roomRepositoryMock.Object );
		}

		[Test]
		public async Task GetAllAsync_ReturnAllRooms()
		{
			_roomRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Room>());

			var rooms = await _roomService.GetAll();

			Assert.IsNotNull(rooms);
		}

		[Test]
		public async Task GetOneAsync_ValidId_ReturnsDto()
		{
			Guid roomId = Guid.NewGuid();

			Room room = new Room
			{
				RoomId = roomId,
				RoomNumber = 2,
				HasAirConditioner = false,
			};

			_roomRepositoryMock.Setup(repo => repo.GetById(roomId)).ReturnsAsync(room);

			var result = await _roomService.Get(roomId);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task GetOneAsync_InvalidId_ReturnsNull()
		{
			Guid id = Guid.NewGuid();

			_roomRepositoryMock.Setup(repo => repo.GetById(id)).ThrowsAsync(new EntityNotFoundException());

			async Task Act() => await _roomService.Get(id);

			Assert.ThrowsAsync<EntityNotFoundException>(Act);
		}

		[Test]
		public async Task UpdateOneAsync_ValidId_ReturnsDto()
		{
			var roomId = Guid.NewGuid();
			Room room = new Room
			{
				RoomId = roomId,
				RoomNumber = 2,
				HasAirConditioner = false,
			};

			RoomDto roomDto = new RoomDto
			{
				RoomNumber = 1,
				HasAirConditioner = true
			};

			_roomRepositoryMock.Setup(repo => repo.GetById(roomId)).ReturnsAsync(room);
			_roomRepositoryMock.Setup(repo => repo.Update(room)).ReturnsAsync(true);

			var result = await _roomService.Update(roomId, roomDto);

			Assert.IsTrue(result);				 
		}

		[Test]
		public async Task DeleteOneAsync_ValidId_ReturnsTrue()
		{
			Guid id = Guid.NewGuid();

			_roomRepositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(true);

			var result = await _roomService.Delete(id);

			Assert.IsTrue(result);
		}
		[Test]
		public async Task DeleteOneAsync_InvalidId_ReturnsFalse()
		{
			var id = Guid.NewGuid();

			_roomRepositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(false);

			var result = await _roomService.Delete(id);

			Assert.IsFalse(result);
		}
	}
}
