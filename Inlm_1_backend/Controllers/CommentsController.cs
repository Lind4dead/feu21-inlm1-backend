using Inlm_1_backend.Data;
using Inlm_1_backend.Models;
using Inlm_1_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Inlm_1_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly AuthHandler _auth;

        public CommentsController(DataContext context, AuthHandler auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create(CommentRequest req)
        {
            try
            {

                var authorization = Request.Headers[HeaderNames.Authorization];
                if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var bearer = headerValue.Scheme;
                    var token = headerValue.Parameter;
                    Console.WriteLine(token);
                    var _id = _auth.GetIdFromToken(token!);
                    var _comment = new Comment
                {
                    Message = req.Message,
                    IssueId = req.IssueId,
                    UserId = _id
                };
                _context.Add(_comment);
                await _context.SaveChangesAsync();

                var comment = await _context.Comments.Include(x => x.Issue).FirstOrDefaultAsync(x => x.Id == _comment.Id);
                var response = new CommentResponse
                {
                    Id = _comment.Id,
                    Message = _comment.Message,
                    Created = _comment.Created
                };
                return new OkObjectResult(response);
                }


            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            try
            {
                var comments = new List<CommentResponse>();
                foreach (var comment in await _context.Comments.ToListAsync())
                    if(comment.IssueId == id)
                    {
                        comments.Add(new CommentResponse
                        {
                            Id = comment.Id,
                            Message = comment.Message,
                            Created = comment.Created,
                        });
                    }
                    

                return new OkObjectResult(comments);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return new NotFoundResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CommentUpdateRequest req)
        {
            try
            {
                var _comment = await _context.Comments.FindAsync(id);
                _comment.Message = req.Message;

                _context.Entry(_comment).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
                return new OkObjectResult(new CommentResponse
                {
                    Id = comment.Id,
                    Created = comment.Created,
                    Message = comment.Message,

                });
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new NotFoundResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var _comment = await _context.Comments.FindAsync(id);

                _context.Remove(_comment).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return new OkResult();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new NotFoundResult();
        }
    }
}
