using TaskManager.DomainLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.DAL.Data
{
#nullable disable

	public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<AssignedUserTask> AssignedUserTasks { get; set; }




	}
}
