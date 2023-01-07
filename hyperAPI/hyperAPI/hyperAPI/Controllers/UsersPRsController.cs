using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hyperAPI.Controllers
{
    [ApiController]
    public class UsersPRsController : ControllerBase
    {

        private readonly DataContext _context;

        public UsersPRsController(DataContext context)
        {
            _context = context;
        }

        /*
        Add User's PR
        */
        [HttpPost]
        [Route("/add-user-pr")]
        public async Task<ActionResult<List<UserPR>>> AddUserPR(UserPR userPR)
        {
            _context.UserPRs.Add(userPR);
            await _context.SaveChangesAsync();

            return Ok(await _context.UserPRs.ToListAsync());
        }


    }
}
