using System.ComponentModel.DataAnnotations;

namespace Inlm_1_backend.Models
{
    public class Issue
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string Subject { get; set; }
        public string Description { get; set; }

        public int StatusId { get; set; } = 1;
        public Status Status { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        

        
        

    }
}
