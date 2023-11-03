using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DeskReserve.Controllers
{
    public interface IBuildingController
    {
        Task<ActionResult<List<Building>>> Get();

        Task<ActionResult<Building>> GetById(Guid id);

        Task<ActionResult<Building>> Post(BuildingDto building);

        Task<IActionResult> Delete(Guid id);

        Task<IActionResult> Put(Guid id, BuildingDto building);
    }
}