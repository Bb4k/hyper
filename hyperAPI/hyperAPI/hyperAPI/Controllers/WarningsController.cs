using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hyperAPI.Controllers
{
    [ApiController]
    public class WarningsController : ControllerBase
    {

        private readonly DataContext _context;

        public WarningsController(DataContext context)
        {
            _context = context;
        }

        /*
        List warnings
        */
        [HttpGet]
        [Route("/warnings")]
        public async Task<ActionResult<List<Warning>>> Get()
        {
            return Ok(await _context.Warnings.ToListAsync());
        }

        /*
        Send warning
        */
        [HttpPost]
        [Route("/send-warning")]
        public async Task<ActionResult<List<Warning>>> SendWarning(Warning warning)
        {
            _context.Warnings.Add(warning);
            await _context.SaveChangesAsync();

            return Ok(await _context.Warnings.ToListAsync());
        }

        /*
        Delete Warning
        */
        [HttpDelete]
        [Route("/delete-warning/{id}")]
        public async Task<ActionResult<string>> DeleteWarning(int id)
        {
            var dbWarning = await _context.Warnings.FindAsync(id);
            if (dbWarning == null)
                return BadRequest("Warning not found.");

            _context.Warnings.Remove(dbWarning);
            await _context.SaveChangesAsync();

            return Ok("Warning deleted");

        }
    }
}