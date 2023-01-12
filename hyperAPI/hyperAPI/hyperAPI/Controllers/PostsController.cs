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

        /*
        List posts
        */
        [HttpGet]
        [Route("/posts")]
        public async Task<ActionResult<List<Post>>> ListPosts()
        {
            return Ok(await _context.Posts.ToListAsync());
        }

        /*
        Delete post
        */
        [HttpDelete]
        [Route("/delete-post/{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            var dbPost = await _context.Posts.FindAsync(id);
            if (dbPost == null)
                return BadRequest("Post not found.");

            _context.Posts.Remove(dbPost);
            await _context.SaveChangesAsync();

            return Ok("Post deleted");
        }

        /*
        Get post by id
        */
        [HttpGet]
        [Route("/post/{id}")]
        public async Task<ActionResult<Object>> GetPostById(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
                return BadRequest("Post not found.");
            var comments = await _context.Comments.Where(u => u.PostId == post.Id).ToListAsync();

            var data = new Dictionary<string, Object>(){
                {"post", post},
                {"comments", comments.Count}
            };
            return Ok(data);
        }

    }
}
