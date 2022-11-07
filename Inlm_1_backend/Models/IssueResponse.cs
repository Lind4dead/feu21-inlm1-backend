namespace Inlm_1_backend.Models
{
    public class IssueResponse
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public ICollection<CommentResponse> Comments { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
    }
}
