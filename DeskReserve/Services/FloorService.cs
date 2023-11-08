using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Interfaces;
using DeskReserve.Domain;
using DeskReserve.Helper;

namespace DeskReserve.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _repository;

        public FloorService(IFloorRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Floor>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<FloorDto> GetOneAsync(Guid id)
        {
            var floorEntity = await _repository.GetById(id);

            if(ReferenceEquals(floorEntity, null))
            {
                return null;
            }

            FloorDto floorDto = new FloorDto();
            floorEntity.MapProperties(floorDto);

            return floorDto;
        }

        public async Task<bool> UpdateOneAsync(Guid id, FloorDto floorDto)
        {
            var floorEntity = new Floor()
            {
                FloorId = id,
            };

            floorDto.MapProperties(floorEntity);

            return await _repository.Update(floorEntity);
        }

        public async Task<bool> CreateOneAsync(FloorDto floorDto)
        {
            Floor floorEntity = new Floor();
            floorDto.MapProperties(floorEntity);

            return await _repository.Add(floorEntity);
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        { 
            var floorEntity = await _repository.GetById(id);

            return await _repository.Delete(floorEntity);
        }
    }
}
