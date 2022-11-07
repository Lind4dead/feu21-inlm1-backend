using Inlm_1_backend.Data;
using Inlm_1_backend.Models;
using Inlm_1_backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Inlm_1_backend.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly AuthHandler _auth;

        public IssuesController(DataContext context, AuthHandler auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Create(IssueRequest req)
        {
            try
            {
                var authorization = Request.Headers[HeaderNames.Authorization];
                if(AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
                {
                    var bearer = headerValue.Scheme;
                    var token = headerValue.Parameter;
                    Console.WriteLine(token);
                    var _id = _auth.GetIdFromToken(token!);

                    
                var _issue = new Issue()
                {
                    Subject = req.Subject,
                    Description = req.Message,
                    UserId = _id
                    
                };
                    _context.Add(_issue);
                    await _context.SaveChangesAsync();

                    var issue = await _context.Issues.Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == _issue.Id);
                    var response = new IssueResponse
                    {
                        Id = issue.Id,
                        Created = issue.Created,
                        Subject = issue.Subject,
                        Message = issue.Description,
                        Status = issue.Status.Name
                    };
                return new OkObjectResult(response);
                }


                

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var issues = new List<IssueResponse>();
            foreach (var issue in await _context.Issues.Include(x => x.Status).Include(x => x.User).ToListAsync())
                issues.Add(new IssueResponse
                {
                    Id = issue.Id,
                    Created = issue.Created,
                    Subject = issue.Subject,
                    Message = issue.Description,
                    Status = issue.Status.Name,
                    Email = issue.User.Email
                });

            return new OkObjectResult(issues);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var issue = await _context.Issues.Include(x => x.Status).Include(x => x.Comments).Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
                var comments = new List<CommentResponse>();
                foreach(var comment in await _context.Comments.Include(x => x.User).ToListAsync())
                    if(comment.IssueId == id)
                    {
                        comments.Add(new CommentResponse
                        {
                            Id = comment.Id,
                            Created = comment.Created,
                            Message = comment.Message,
                            UserName = comment.User.FirstName
                        });
                    }

                if (issue != null)
                    return new OkObjectResult(new IssueResponse
                    {
                        Id = issue.Id,
                        Created = issue.Created,
                        Subject = issue.Subject,
                        Message = issue.Description,
                        Comments = comments,
                        Status = issue.Status.Name,
                        StatusId = issue.StatusId,
                        Email = issue.User.Email
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new NotFoundResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IssueUpdateRequest req)
        {
            try
            {
                var _issue = await _context.Issues.FindAsync(id);
                _issue.StatusId = req.StatusId;
                Console.WriteLine(id);
                Console.WriteLine(_issue.StatusId);
                

                _context.Entry(_issue).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var issue = await _context.Issues.Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == _issue.Id);
                return new OkObjectResult(new IssueResponse
                {
                    Id = issue.Id,
                    Created = issue.Created,
                    Subject = issue.Subject,
                    Message = issue.Description,
                    Status = issue.Status.Name,
                    StatusId = issue.StatusId
                });
            }
            catch (Exception ex)
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
                var issue = await _context.Issues.FirstOrDefaultAsync(x => x.Id == id);
                _context.Remove(issue).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return new OkResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new NotFoundResult();
        }

    }
}
