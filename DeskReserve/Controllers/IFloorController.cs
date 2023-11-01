using DeskReserve.Data.DBContext.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DeskReserve.Controllers
{
    public interface IFloorController
    {
        Task<ActionResult<IEnumerable<Floor>>> GetAll();

        Task<ActionResult<Floor>> GetById(Guid id);

        Task<IActionResult> UpdateOne(Guid id, Floor floor);

        Task<ActionResult<Floor>> Create(Floor floor);

        Task<IActionResult> Delete(Guid id);
    }
}
