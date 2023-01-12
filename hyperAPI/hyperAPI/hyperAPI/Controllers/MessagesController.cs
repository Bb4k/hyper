using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hyperAPI.Controllers
{
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private readonly DataContext _context;
        public MessagesController(DataContext context)
        {
            _context = context;
        }

        /*
        Add message
        */
        [HttpPost]
        [Route("/send-message")]
        public async Task<ActionResult<List<Message>>> SendMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(await _context.Messages.ToListAsync());
        }

        /*
         Get conversations
         */
        [HttpGet]
        [Route("/conversations")]
        public async Task<ActionResult<List<Object>>> GetConversations(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var dbFriendships = await _context.Friendships.Where(u => (u.User1Id == id || u.User2Id == id) && u.Status == 1).ToListAsync();
            
            List<User> users = new List<User>();

            foreach (var friendship in dbFriendships)
            {
                var friendId = friendship.User1Id != id ? friendship.User1Id : friendship.User2Id;
                users.Add(await _context.Users.FindAsync(friendId));
            }

            return Ok(users);
        }

    }
}
