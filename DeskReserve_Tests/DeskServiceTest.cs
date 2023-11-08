using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exception;
using DeskReserve.Repository;
using Moq;

namespace Desk_Reserve.Tests
{
	[TestFixture]
	public class FloorServiceTests
	{
		[Test]
		public async Task GetAllAsync_ReturnsAllDesks()
		{
			var repositoryMock = new Mock<IDeskRepository>();
			repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Desk>());

			var service = new DeskService(repositoryMock.Object);

			var desks = await service.GetAllAsync();

			Assert.IsNotNull(desks);
			Assert.AreEqual(0, desks.Count());
		}

		[Test]
		public async Task GetOneAsync_ValidId_ReturnsDeskDto()
		{
			Guid id = Guid.NewGuid();
			Desk desk = new Desk { DeskId = id };

			var repositoryMock = new Mock<IDeskRepository>();
			repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(desk);

			var deskService = new DeskService(repositoryMock.Object);

			var result = await deskService.GetOneAsync(id);

			Assert.IsInstanceOf<DeskDto>(result);
			Assert.IsNotNull(result);
		}

		[Test]
		public async Task GetOneAsync_InvalidId_ReturnsNull()
		{
			Guid id = Guid.NewGuid();
			Desk desk = null;

			var repositoryMock = new Mock<IDeskRepository>();
			repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(desk);

			var deskService = new DeskService(repositoryMock.Object);

			var result = await deskService.GetOneAsync(id);

			Assert.Throws<EntityNotFoundException>(() => { });
		}

		[Test]
		public async Task UpdateOneAsync_ValidIdAndDto_ReturnsTrue()
		{
			Guid id = Guid.NewGuid();

			DeskDto deskDto = new DeskDto
			{
				DeskNumber = 1,
				IsOccupied = true,
				IsStatic = false
			};

			var repositoryMock = new Mock<IDeskRepository>();
			repositoryMock.Setup(repo => repo.Update(It.IsAny<Desk>())).ReturnsAsync(true);

			var deskService = new DeskService(repositoryMock.Object);

			var result = await deskService.UpdateOneAsync(id, deskDto);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteOneAsync_ValidId_ReturnsTrue()
		{
			Guid id = Guid.NewGuid();

			var repositoryMock = new Mock<IDeskRepository>();
			repositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(true);

			var deskService = new DeskService(repositoryMock.Object);

			var result = await deskService.DeleteOneAsync(id);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteOneAsync_InvalidId_ReturnsFalse()
		{
			var id = Guid.NewGuid();

			var repositoryMock = new Mock<IDeskRepository>();
			repositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(false);

			var deskService = new DeskService(repositoryMock.Object);

			var result = await deskService.DeleteOneAsync(id);

			Assert.IsFalse(result);
		}
	}
}