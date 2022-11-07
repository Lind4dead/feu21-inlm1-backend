using System.ComponentModel.DataAnnotations;

namespace Inlm_1_backend.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public int IssueId { get; set; }
        public Issue Issue { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
