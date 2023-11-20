using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using DeskReserve.Repository;
using DeskReserve.Services;
using Moq;
using RequestReserve.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskReserve_Tests
{
	[TestFixture]
	internal class RequestServiceTests
	{
		private Mock<IRequestRepository> repositoryMock;
		private RequestService requestService;

		[SetUp]
		public void SetUp()
		{
			repositoryMock = new Mock<IRequestRepository>();
			requestService = new RequestService(repositoryMock.Object);
		}

		[Test]
		public async Task GetAllAsync_ReturnsAllRequests()
		{
			repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Request>());

			var desks = await requestService.GetAllAsync();

			Assert.IsNotNull(desks);
		}

		[Test]
		public async Task GetOneAsync_ValidId_ReturnRequestDto()
		{
			Guid id = Guid.NewGuid();
			Request desk = new Request { DeskId = id };

			repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(desk);

			var result = await requestService.GetAsync(id);

			Assert.IsNotNull(result);
		}

		[Test]
		public async Task GetOneAsync_InvalidId_ReturnsNull()
		{
			Guid id = Guid.NewGuid();

			repositoryMock.Setup(repo => repo.GetById(id)).ThrowsAsync(new EntityNotFoundException());

			async Task Act() => await requestService.GetAsync(id);

			Assert.ThrowsAsync<EntityNotFoundException>(Act);
		}
	}
}
