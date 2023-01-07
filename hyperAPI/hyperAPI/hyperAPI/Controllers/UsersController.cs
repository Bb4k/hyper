using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (request.Height != request.Height)
            {
                dbUser.Height = request.Height;
            }            
            if (request.Weight != request.Weight)
            {
                dbUser.Weight = request.Weight;
            }            
            if (request.Picture != request.Picture)
            {
                dbUser.Picture = request.Picture;
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
        [Route("/user-profile/{id}")]
        public async Task<ActionResult<User>> GetProfile(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return BadRequest("User not found.");
            var posts = await _context.Posts.Where(u => u.UserId == id).ToListAsync();
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine(JsonSerializer.Serialize(user));
            Console.WriteLine(JsonSerializer.Serialize(posts));
            Console.WriteLine("----------------------------------------------------------------");
            return Ok(user);
        }
    }
}
