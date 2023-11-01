using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Interfaces;
using DeskReserve.Domain;
using AutoMapper;

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
            var entity = await _repository.GetById(id);
            var floor = _mapper.Map<Floor, FloorDto>(entity);

            return floor;
        }

        public async Task<bool> UpdateOneAsync(Guid id, FloorDto floorDto)
        {
            var floor = _mapper.Map<Floor>(await _repository.GetById(id));

            if (floor == null)
            {
                return false;
            }

            CopyProperties(floorDto, floor);

            return await _repository.Update(floor);
        }

        public async Task<bool> CreateOneAsync(FloorDto floor)
        {
            var floorEntity = _mapper.Map<Floor>(floor);

            var success = await _repository.Add(floorEntity);

            return success ? true : false;
        }

        public async Task<bool> DeleteOneAsync(Guid id)
        {
            var floor = await _repository.GetById(id);

            if (floor == null)
            {
                return false;
            }

            var success = await _repository.Delete(id);
            return success ? true : false;
        }

        public static void CopyProperties(object source, object destination)
        {
            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
                if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                }
            }
        }
    }
}
