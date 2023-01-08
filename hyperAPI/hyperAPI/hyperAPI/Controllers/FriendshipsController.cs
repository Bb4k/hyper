using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hyperAPI.Controllers
{
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly DataContext _context;

        public FriendshipsController(DataContext context)
        {
            _context = context;
        }

        /*
        List frindships
        */
        [HttpGet]
        [Route("/frindships")]
        public async Task<ActionResult<List<Friendship>>> Get()
        {
            return Ok(await _context.Friendships.ToListAsync());
        }

        /*
        List friend request for user
        */
        [HttpGet]
        [Route("/frind-requests/{id}")]
        public async Task<ActionResult<List<Friendship>>> GetFriendRequest(int id)
        {
            var dbFriendrequests = await _context.Friendships.Where(u => (u.User2Id == id) && u.Status == 0).ToListAsync();

            List<Object> result = new List<Object>(); ;

            foreach (var friendrequest in dbFriendrequests)
            {
                var user = await _context.Users.FindAsync(friendrequest.User1Id);
                if (user == null)
                    return BadRequest("User not found.");

                var obj = new Dictionary<string, Object>(){
                    {"friendrequest", friendrequest},
                    {"user", user}
                };
                result.Add(obj);

            }

            var final = new Dictionary<string, Object>(){
                {"result", result}
            };

            return Ok(final);
        }


        /*
        Send friend request
        */
        [HttpPost]
        [Route("/send-friend-request")]
        public async Task<ActionResult<List<Friendship>>> SendFriendRequest(Friendship friendship)
        {
            friendship.Status = 0;
            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();

            return Ok(await _context.Friendships.ToListAsync());
        }


        /*
        Accept friend request
        */
        [HttpPut]
        [Route("/accept-friend-request")]
        public async Task<ActionResult<List<Friendship>>> AcceptFriendRequest(Friendship friendship)
        {
            var dbFriendship = await _context.Friendships.Where(u => (u.User1Id == friendship.User1Id && u.User2Id == friendship.User2Id) ||
                                                                     (u.User1Id == friendship.User2Id && u.User2Id == friendship.User1Id)).FirstOrDefaultAsync();
            if (dbFriendship == null)
                return BadRequest("Friendship not found.");

            dbFriendship.Status = 1;
            await _context.SaveChangesAsync();
            return Ok(await _context.Friendships.ToListAsync());
        }

        /*
        Reject friend request
        */
        [HttpDelete]
        [Route("/reject-friend-request")]
        public async Task<ActionResult<List<Friendship>>> RejectFriendRequest(Friendship friendship)
        {
            var dbFriendship = await _context.Friendships.Where(u => (u.User1Id == friendship.User1Id && u.User2Id == friendship.User2Id) ||
                                                                     (u.User1Id == friendship.User2Id && u.User2Id == friendship.User1Id)).FirstOrDefaultAsync();
            if (dbFriendship == null)
                return BadRequest("Friendship not found.");

            _context.Friendships.Remove(dbFriendship);
            await _context.SaveChangesAsync();
            return Ok(await _context.Friendships.ToListAsync());
        }

    }
}
