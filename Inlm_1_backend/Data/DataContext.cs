using Inlm_1_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Inlm_1_backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
