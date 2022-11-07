using Inlm_1_backend.Data;
using Inlm_1_backend.Models;
using Inlm_1_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inlm_1_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserHandler userHandler;
        private readonly AuthHandler auth;

        public UsersController(UserHandler userHandler, AuthHandler auth)
        {
            this.userHandler = userHandler;
            this.auth = auth;
        }

        /*  REGISTER   */

        [HttpPost("register")]
        
        public async Task<IActionResult> Create(UserRequest req)
        {
            try
            {
                
                var payload = await userHandler.CreateAsync(req);
                var token = auth.CreateToken(payload);

                

                return new OkObjectResult(token);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return new OkObjectResult(await userHandler.GetUsersAsync());
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }

        /*  LOGIN   */

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest req)
        {
            try
            {
                var user = await userHandler.LoginUserAsync(req);
                if(user != null)
                {
                    var token = auth.CreateToken(user);

                    return new OkObjectResult(token);
                }
                    
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BadRequestResult();
        }
    }
}
