﻿using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Interfaces;
using DeskReserve.Domain;

namespace DeskReserve.Services
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _repository;
        private readonly IMapper _mapper;

        public FloorService(IFloorRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Floor>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<FloorDto> GetOneAsync(Guid id)
        {
            var floorEntity = await _repository.GetById(id);

            var floorDto = new FloorDto();

            floorDto = _mapper.MapProperties(floorEntity, floorDto);

            return floorDto;
        }

        public async Task<bool> UpdateOneAsync(Guid id, FloorDto floorDto)
        {
            var floorEntity = await _repository.GetById(id);

            if (floorEntity == null)
            {
                return false;
            }

            floorEntity = _mapper.MapProperties(floorDto, floorEntity);

            return await _repository.Update(floorEntity);
        }

        public async Task<bool> CreateOneAsync(FloorDto floorDto)
        {
            var floorEntity = new Floor();
            floorEntity = _mapper.MapProperties(floorDto, floorEntity);

            var success = await _repository.Add(floorEntity);

            return success ? true : false;
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var floorEntity = await _repository.GetById(id);

            if (floorEntity == null)
            {
                return false;
            }

            var success = await _repository.Delete(id);
            return success ? true : false;
        }
    }
}
