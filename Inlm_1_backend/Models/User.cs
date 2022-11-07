using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Inlm_1_backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Issue> Issues { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public byte[] PasswordHash { get; private set; }
        public byte[] Salt { get; private set; }


        public void GenerateSecurePassword(string password)
        {
            using var hmac = new HMACSHA512();
            Salt = hmac.Key;
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public bool ValidatePassword(string password)
        {
            using var hmac = new HMACSHA512(Salt);
            var _hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < _hash.Length; i++)
                if (_hash[i] != PasswordHash[i])
                    return false;

            return true;
        }
    }
}
