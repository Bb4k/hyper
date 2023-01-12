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
         Get dms
         */
        [HttpGet]
        [Route("/dms/{id}")]
        public async Task<ActionResult<List<User>>> GetDms(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var dbFriendships = await _context.Friendships.Where(u => (u.User1Id == id || u.User2Id == id) && u.Status == 1).ToListAsync();
            
            List<User> users = new List<User>();

            foreach (var friendship in dbFriendships)
            {
                var friendId = friendship.User1Id != id ? friendship.User1Id : friendship.User2Id;
                var friend = await _context.Users.FindAsync(friendId);
                users.Add(friend);
            }

            return Ok(users);
        }

        /*
         Get messages from conversation
         */
        [HttpGet]
        [Route("/conversation/{current_user}/{user_to_text_with}")]
        public async Task<ActionResult<List<Object>>> GetMessages(int currentUserId, int userToTextWithId)
        {
            var current_user = await _context.Users.FindAsync(currentUserId);
            var friend_user = await _context.Users.FindAsync(userToTextWithId);

            var messages = await _context.Messages.Where(u => (u.UserFromId == currentUserId && u.UserToId == userToTextWithId) ||
                                                              (u.UserFromId == userToTextWithId && u.UserToId == currentUserId)).ToListAsync();

            List<Object> conversations = new List<Object>();

            foreach (var message in messages)
            {
                var conversation = new Dictionary<string, Object>(){
                    {"userId", message.UserFromId},
                    {"text", message.Text}
                };
                conversations.Add(conversation);
            }

            var obj = new Dictionary<string, Object>(){
                {"user", current_user},
                {"friend_user", friend_user},
                {"conversations", conversations}
            };

            return Ok(obj);
        }

    }
}
