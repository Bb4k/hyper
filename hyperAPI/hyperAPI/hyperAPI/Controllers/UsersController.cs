using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace hyperAPI.Controllers
{

    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        /*
        List Users
        */
        [HttpGet]
        [Route("/users")]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        /*
        List User by id
        */
        [HttpGet]
        [Route("/user/{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found.");
            return Ok(user);
        }

        /*
        Find user by username
        */
        [HttpGet]
        [Route("/user/{current_user_id}/{username_to_find}")]
        public async Task<ActionResult<Object>> GetUserByUsername(int current_user_id, string username_to_find)
        {
            var usersFromDb = await _context.Users.Where(u => u.Username.StartsWith(username_to_find)).ToListAsync();

            List<Object> suggestedUsers = new List<Object>();

            foreach (var user in usersFromDb)
            {

                var dbFriendship = await _context.Friendships.Where(u => (u.User1Id == current_user_id && u.User2Id == user.Id) ||
                                                                        (u.User1Id == user.Id && u.User2Id == current_user_id) && u.Status == 1).FirstOrDefaultAsync();

                var are_friends = false;
                if (dbFriendship != null)
                    are_friends = true;

                var obj = new Dictionary<string, Object>(){
                    {"user", user},
                    {"are_friends", are_friends}
                };
                suggestedUsers.Add(obj);
            }


            return Ok(suggestedUsers);
        }

        /*
        Register User
        */
        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(await _context.Users.ToListAsync());
        }

        /*
        Update User
        */

        [HttpPut]
        [Route("/update-user")]
        public async Task<ActionResult<List<User>>> UpdateUser(User request)
        {
            var dbUser = await _context.Users.FindAsync(request.Id);
            if (dbUser == null)
                return BadRequest("User not found.");
            if (request.Email != dbUser.Email)
            {
                dbUser.Email = request.Email;
            }            
            if (request.Height != dbUser.Height)
            {
                dbUser.Height = request.Height;
            }            
            if (request.Weight != dbUser.Weight)
            {
                dbUser.Weight = request.Weight;
            }            
            if (request.Picture != dbUser.Picture && request.Picture != "")
            {
                dbUser.Picture = request.Picture;
            }
            if (request.Private != dbUser.Private)
            {
                dbUser.Private = request.Private;
            }

            await _context.SaveChangesAsync();

            return Ok(await _context.Users.ToListAsync());
        }

        /*
        Delete User
        */
        [HttpDelete]
        [Route("/delete-user/{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null)
                return BadRequest("User not found.");

            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();

            return Ok(await _context.Users.ToListAsync());
        }

        /*
        Login User
        */
        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<User>> LoginUser(User loginData)
        {
            var dbUser = await _context.Users.Where(u => u.Username == loginData.Username).FirstOrDefaultAsync();
            if (dbUser == null)
                return BadRequest("User not found.");

            if (loginData.Password != dbUser.Password)
            {
                return BadRequest("Invalid password.");
            }

            return Ok(dbUser);

        }

        /*
        Profile User
        */
        [HttpGet]
        [Route("/user-profile/{current_user}/{user_to_view}")]
        public async Task<ActionResult<Object>> GetProfile(int current_user, int user_to_view)
        {
            var user = await _context.Users.FindAsync(user_to_view);
            if (user == null)
                return BadRequest("User not found.");
            var posts = await _context.Posts.Where(u => u.UserId == user_to_view).ToListAsync();
            var usersPr = await _context.UserPRs.Where(u => u.UserId == user_to_view).ToListAsync();
            var friendships = await _context.Friendships.Where(u => (u.User2Id == user_to_view || u.User1Id == user_to_view) && u.Status == 1).ToListAsync();
            var currentFriendship = await _context.Friendships.Where(u => (u.User1Id == current_user && u.User2Id == user_to_view) ||
                                                         (u.User1Id == user_to_view && u.User2Id == current_user)).FirstOrDefaultAsync();
            var areFriends = 1; // are friends
            if (currentFriendship == null)
            {
                areFriends = 0; // no friendship
            }
            else if (currentFriendship.Status == 0)
            {
                areFriends = 2; // pending friendship
            }

            var result = new Dictionary<string, Object>(){
                {"user", user},
                {"posts", posts},
                {"usersPr", usersPr},
                {"friends", friendships.Count},
                {"are_friends", areFriends }
            };

            return Ok(result);
        }

        /*
        Feed User
        */
        [HttpGet]
        [Route("/user-feed/{id}")]
        public async Task<ActionResult<Object>> GetFeed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found.");

            // get all friendships for current user
            var friendships = await _context.Friendships.Where(u => (u.User2Id == id || u.User1Id == id) && u.Status == 1).ToListAsync();

            // create feed
            List<Object> feed = new List<Object>();

            // get all the posts from friends
            foreach (var friendship in friendships)
            {
                var friendId = friendship.User2Id; ;
                if (friendship.User1Id != id)
                    friendId = friendship.User1Id;
                
                var friend = await _context.Users.FindAsync(friendId);
                if (friend == null)
                    return BadRequest("Friend not found.");

                var posts = await _context.Posts.Where(u => u.UserId == friendId).ToListAsync();

                foreach (var post in posts) {

                    var comments = await _context.Comments.Where(u => u.PostId == post.Id).ToListAsync();
                    var dataForPost = new Dictionary<string, Object>(){
                        {"user", friend},
                        {"post", post},
                        {"comments", comments.Count}
                    };
                    feed.Add(dataForPost);
                }
            }

            feed.Sort(delegate(dynamic x, dynamic y)
            {
                return y["post"].Timestamp.CompareTo(x["post"].Timestamp);
            });

            return Ok(feed);
        }

        /*
        Feed admin
        */
        [HttpGet]
        [Route("/user-admin")]
        public async Task<ActionResult<Object>> GetFeedAdmin()
        {
            var user = await _context.Users.Where(u => u.Role == "admin").FirstOrDefaultAsync();
            if (user == null)
                return BadRequest("User not found.");

            // get all friendships for current user
            var friendships = await _context.Friendships.ToListAsync();

            // create feed
            List<Object> feed = new List<Object>();

            // get all the posts from friends
            foreach (var friendship in friendships)
            {
                var friendId = friendship.User2Id; ;
                if (friendship.User1Id != user.Id)
                    friendId = friendship.User1Id;

                var friend = await _context.Users.FindAsync(friendId);
                if (friend == null)
                    return BadRequest("Friend not found.");

                var posts = await _context.Posts.Where(u => u.UserId == friendId).ToListAsync();

                foreach (var post in posts)
                {
                    var comments = await _context.Comments.Where(u => u.PostId == post.Id).ToListAsync();
                    var dataForPost = new Dictionary<string, Object>(){
                        {"user", friend},
                        {"post", post},
                        {"comments", comments.Count}
                    };
                    feed.Add(dataForPost);
                }
            }

            feed.Sort(delegate (dynamic x, dynamic y)
            {
                return y["post"].Timestamp.CompareTo(x["post"].Timestamp);
            });

            return Ok(feed);
        }

    }
}
