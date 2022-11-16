namespace Inlm_1_backend.Models
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        
    }
}
