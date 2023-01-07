using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hyperAPI.Controllers
{
    [ApiController]
    public class PRsController : ControllerBase
    {

        private readonly DataContext _context;

        public PRsController(DataContext context)
        {
            _context = context;
        }

        /*
        Add PR
        */
        [HttpPost]
        [Route("/add-pr")]
        public async Task<ActionResult<List<PR>>> AddPR(PR pr)
        {
            _context.PRs.Add(pr);
            await _context.SaveChangesAsync();

            return Ok(await _context.PRs.ToListAsync());
        }

        /*
        List PR by id
        */
        [HttpGet]
        [Route("/pr/{id}")]
        public async Task<ActionResult<PR>> Get(int id)
        {
            var pr = await _context.PRs.FindAsync(id);
            if (pr == null)
                return BadRequest("PR not found.");
            return Ok(pr);
        }

    }
}
