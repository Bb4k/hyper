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
        [Route("/friendships")]
        public async Task<ActionResult<List<Friendship>>> Get()
        {
            return Ok(await _context.Friendships.ToListAsync());
        }

        /*
        List friend request, comments request and warnigs for user
        */
        [HttpGet]
        [Route("/user-requests/{id}")]
        public async Task<ActionResult<List<Friendship>>> GetFriendRequest(int id)
        {
            var dbFriendrequests = await _context.Friendships.Where(u => (u.User2Id == id) && u.Status == 0).ToListAsync();

            List<Object> friendRequests = new List<Object>();

            foreach (var friendRequest in dbFriendrequests)
            {
                var user = await _context.Users.FindAsync(friendRequest.User1Id);
                if (user == null)
                    return BadRequest("User not found at friend request.");

                var obj = new Dictionary<string, Object>(){
                    {"friendrequest", friendRequest},
                    {"user", user}
                };
                friendRequests.Add(obj);

            }

            var dbComments = await _context.Comments.Where(u => (u.AuthorPostId == id) && u.Status == 0).ToListAsync();

            List<Object> commentsRequests = new List<Object>();

            foreach (var comment in dbComments)
            {
                var user = await _context.Users.FindAsync(comment.UserId);
                if (user == null)
                    return BadRequest("User not found at comments requests.");

                var obj = new Dictionary<string, Object>(){
                    {"comment", comment},
                    {"user", user}
                };
                commentsRequests.Add(obj);

            }

            commentsRequests.Sort(delegate (dynamic x, dynamic y)
            {
                return y["comment"].Timestamp.CompareTo(x["comment"].Timestamp);
            });


            List<Object> warnings = new List<Object>();

            var final = new Dictionary<string, Object>(){
                {"friendships", friendRequests},
                {"comments", commentsRequests},
                {"warnings", warnings}
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
