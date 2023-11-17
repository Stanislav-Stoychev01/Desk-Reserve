using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeskReserve.Services;
using DeskReserve.Interfaces;

namespace Desk_Reserve.Tests
{
    [TestFixture]
	public class DeskServiceTests
	{
		private Mock<IDeskRepository> repositoryMock;
		private DeskService deskService;

		[SetUp]
		public void SetUp()
		{
			repositoryMock = new Mock<IDeskRepository>();
			deskService = new DeskService(repositoryMock.Object);
		}

		[Test]
		public async Task GetAllAsync_ReturnsAllDesks()
		{
			repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Desk>());

			var desks = await deskService.GetAllAsync();

			Assert.IsNotNull(desks);
		}

		[Test]
		public async Task GetOneAsync_ValidId_ReturnsDeskDto()
		{
			Guid id = Guid.NewGuid();
			Desk desk = new Desk { DeskId = id };

			repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(desk);

			var result = await deskService.GetAsync(id);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task GetOneAsync_InvalidId_ReturnsNull()
		{
			Guid id = Guid.NewGuid();

			repositoryMock.Setup(repo => repo.GetById(id)).ThrowsAsync(new EntityNotFoundException());

			async Task Act() => await deskService.GetAsync(id);

			Assert.ThrowsAsync<EntityNotFoundException>(Act);
		}

		[Test]
		public async Task UpdateOneAsync_ValidIdAndDto_ReturnsTrue()
		{
			Guid id = Guid.NewGuid();

			Desk desk = new Desk { DeskId = id };

			DeskDto deskDto = new DeskDto
			{
				DeskNumber = 1,
				IsOccupied = true,
				IsStatic = false
			};

			repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(desk);
			repositoryMock.Setup(repo => repo.Update(desk)).ReturnsAsync(true);

			var result = await deskService.UpdateAsync(id, deskDto);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteOneAsync_ValidId_ReturnsTrue()
		{
			Guid id = Guid.NewGuid();

			repositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(true);

			var result = await deskService.DeleteAsync(id);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteOneAsync_InvalidId_ReturnsFalse()
		{
			var id = Guid.NewGuid();

			repositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(false);

			var result = await deskService.DeleteAsync(id);

			Assert.IsFalse(result);
		}
	}
}