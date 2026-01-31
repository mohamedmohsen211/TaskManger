using Microsoft.EntityFrameworkCore;
using TaskManger.API.Models;

namespace TaskManger.API.Data
{
    public class AppDpContext : DbContext
    {
        public AppDpContext(DbContextOptions <AppDpContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<Log> Logs => Set<Log>();
    }
}
