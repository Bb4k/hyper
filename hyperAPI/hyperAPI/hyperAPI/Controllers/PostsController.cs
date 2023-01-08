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
            await _context.SaveChangesAsync();
            UserPR userPR = new UserPR();
            userPR.PrId = post.PrId;
            userPR.UserId = post.UserId;
            userPR.Weight = post.Weight;
            _context.UserPRs.Add(userPR);
            await _context.SaveChangesAsync();

            return Ok(await _context.Posts.ToListAsync());
        }

        /*
        Hype post
        */
        [HttpPost]
        [Route("/hype/{postId}")]
        public async Task<ActionResult<List<Post>>> Hype(int postId)
        {
            
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
                return BadRequest("Post not found.");

            post.Likes += 1;
           
            await _context.SaveChangesAsync();
            return Ok(await _context.Posts.FindAsync(postId));
        }

        /*
        Unhype post
        */
        [HttpPost]
        [Route("/unhype/{postId}")]
        public async Task<ActionResult<List<Post>>> Unhype(int postId)
        {

            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
                return BadRequest("Post not found.");

            if (post.Likes > 1)
            {
                post.Likes -= 1;
            }
            await _context.SaveChangesAsync();
            return Ok(await _context.Posts.FindAsync(postId));
        }



    }
}
