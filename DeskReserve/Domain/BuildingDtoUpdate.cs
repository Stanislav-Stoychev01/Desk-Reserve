using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Domain
{
    public static class BuildingDtoUpdate
    {
        public static Building ToBuilding(this BuildingDto buildingdto)
        {
            return new Building
            {
                BuildingId = Guid.NewGuid(),
                City = buildingdto.City,
                StreetAddress = buildingdto.StreetAddress,
                Neighbourhood = buildingdto.Neighbourhood,
                FloorsCount = buildingdto.FloorsCount
            };
        }

        public static Building ToBuilding(this BuildingDto buildingdto, Guid id)
        {
            return new Building
            {
                BuildingId = id,
                City = buildingdto.City,
                StreetAddress = buildingdto.StreetAddress,
                Neighbourhood = buildingdto.Neighbourhood,
                FloorsCount = buildingdto.FloorsCount
            };
        }
    }
}
