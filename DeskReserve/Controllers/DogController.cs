using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DeskReserve.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogController : ControllerBase
    {
        private readonly DogService _dogService;

        public DogController(DogService dogService)
        {
            _dogService = dogService ?? throw new ArgumentNullException(nameof(dogService));
        }

        [HttpGet]
        public IEnumerable<Dog> Get()
        {
            return  _dogService.GetAllDogs();
        }
    }
}