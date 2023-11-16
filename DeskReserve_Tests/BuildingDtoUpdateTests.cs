using DeskReserve.Domain;

namespace DeskReserve_Tests
{
    public class BuildingDtoUpdateTests
    {
        [Test]
        public void UpgradeDto_WhenCalled_ReturnsBuildingWithNewId()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            var result = buildingDto.ToBuilding();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.BuildingId);
            Assert.AreEqual(buildingDto.City, result.City);
            Assert.AreEqual(buildingDto.StreetAddress, result.StreetAddress);
            Assert.AreEqual(buildingDto.Neighbourhood, result.Neighbourhood);
            Assert.AreEqual(buildingDto.FloorsCount, result.FloorsCount);
        }

        [Test]
        public void UpgradeDtoWithId_WhenCalled_ReturnsBuildingWithSpecifiedId()
        {
            var id = Guid.NewGuid();

            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            var result = buildingDto.ToBuilding(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.BuildingId);
            Assert.AreEqual(buildingDto.City, result.City);
            Assert.AreEqual(buildingDto.StreetAddress, result.StreetAddress);
            Assert.AreEqual(buildingDto.Neighbourhood, result.Neighbourhood);
            Assert.AreEqual(buildingDto.FloorsCount, result.FloorsCount);
        }
    }
}
