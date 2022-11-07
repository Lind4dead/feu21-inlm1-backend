using Inlm_1_backend.Data;
using Inlm_1_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Inlm_1_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        public readonly DataContext _context;

        public StatusesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(StatusRequest req)
        {
            try
            {
                var status = new Status { Name = req.StatusName };
                _context.Add(status);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var statuses = new List<StatusResponse>();
                foreach (var status in await _context.Statuses.ToListAsync())
                    statuses.Add(new StatusResponse { Id = status.Id, StatusName = status.Name });

                return new OkObjectResult(statuses);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }
    }
}
