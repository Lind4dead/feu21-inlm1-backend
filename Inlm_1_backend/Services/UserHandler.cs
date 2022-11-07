using Inlm_1_backend.Data;
using Inlm_1_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Inlm_1_backend.Services
{
    public class UserHandler
    {
        private readonly DataContext _context;
        

        public UserHandler(DataContext context)
        {
            _context = context;
        }

        /*  REGISTER   */
        public async Task<User> CreateAsync(UserRequest req)
        {
            var _user = new User()
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,
            };
            _user.GenerateSecurePassword(req.Password);

            _context.Users.Add(_user);
            
            await _context.SaveChangesAsync();

            return _user;

            
        }

        /*  GET USERS   */
        public async Task<User> GetUserAsync(int id)
        {
            try
            {
                var _user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (_user != null)
                    return new User()
                    {
                        Id = _user.Id,
                        FirstName = _user.FirstName,
                        LastName = _user.LastName,
                        Email = _user.Email,
                    };
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<UserResponse>> GetUsersAsync()
        {
            var users = new List<UserResponse>();

            try
            {
                foreach (var user in await _context.Users.ToListAsync())
                    users.Add(new UserResponse
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    });

                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return users;
        }
        /*  UPDATE   */
        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                var _user = await _context.Users.FindAsync(user.Id);
                if(_user != null)
                {
                    _user.FirstName = user.FirstName;
                    _user.LastName = user.LastName;
                    _user.Email = user.Email;

                    _context.Entry(_user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    user.FirstName = _user.FirstName;
                    user.LastName = _user.LastName;
                    user.Email = _user.Email;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return user;
        }

        /*  LOGIN   */

        public async Task<User> LoginUserAsync(UserLoginRequest req)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
                if(user != null)
                {
                    var validated = user.ValidatePassword(req.Password);
                    if (validated != false)
                        return user;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

    }
}
