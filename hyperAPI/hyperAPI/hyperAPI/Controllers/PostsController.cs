using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace hyperAPI.Controllers
{
    [ApiController]
    public class PostsController : ControllerBase
    {

        private readonly DataContext _context;

        public PostsController(DataContext context)
        {
            _context = context;
        }

        /*
        Add post
        */
        [HttpPost]
        [Route("/add-post")]
        public async Task<ActionResult<List<Post>>> AddPost(Post post)
        {
            _context.Posts.Add(post);
            UserPR userPR =  new UserPR();
            userPR.PrId = post.PrId;
            userPR.UserId = post.UserId;
            userPR.Weight = post.Weight;
            /*Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine(JsonSerializer.Serialize(userPR));
            Console.WriteLine("----------------------------------------------------------------");*/
            _context.UserPRs.Add(userPR);
            await _context.SaveChangesAsync();

            return Ok(await _context.Posts.ToListAsync());
        }

    }
}
