using LoggingAndLogMonitoring.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoggingAndLogMonitoring.Data
{
    public class HangfireSendGridDbContext : DbContext
    {
        public HangfireSendGridDbContext(DbContextOptions<HangfireSendGridDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
    }
}