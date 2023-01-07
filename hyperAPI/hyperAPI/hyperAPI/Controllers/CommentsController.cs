using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hyperAPI.Controllers
{

    [ApiController]
    public class CommentsController : ControllerBase
    {

        private readonly DataContext _context;

        public CommentsController(DataContext context)
        {
            _context = context;
        }

        /*
        Add comment
        */
        [HttpPost]
        [Route("/add-comment")]
        public async Task<ActionResult<List<Comment>>> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(await _context.Comments.ToListAsync());
        }

        /*
        Delete Comment
        */
        [HttpDelete]
        [Route("/delete-comment/{id}")]
        public async Task<ActionResult<List<Comment>>> Delete(int id)
        {
            var dbComment = await _context.Comments.FindAsync(id);
            if (dbComment == null)
                return BadRequest("Comment not found.");

            _context.Comments.Remove(dbComment);
            await _context.SaveChangesAsync();

            return Ok(await _context.Comments.ToListAsync());
        }        
        
        /*
        Get all comments for post + pictures
        */
        [HttpGet]
        [Route("/get-comments-for-post/{id}")]
        public async Task<ActionResult<List<Object>>> GetAllCommentsForPost(int id)
        {
            var comments = await _context.Comments.Where(u => u.PostId == id).ToListAsync();
            if (comments == null)
                return Ok("No comments for this post");

            List<Object> result = new List<Object>(); ;

            foreach (var comment in comments)
            {
                var user = await _context.Users.FindAsync(comment.UserId);
                if (user == null)
                    return BadRequest("User not found.");

                var obj = new Dictionary<string, Object>(){
                    {"comment", comment},
                    {"user", user}
                };
                result.Add(obj);

            }

            var final = new Dictionary<string, Object>(){
                {"result", result}
            };


            return Ok(result);
        }



    }
}
