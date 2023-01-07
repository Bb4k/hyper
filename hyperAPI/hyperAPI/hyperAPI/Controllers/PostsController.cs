using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            return Ok(await _context.Posts.ToListAsync());
        }

    }
}
