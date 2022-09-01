using Microsoft.EntityFrameworkCore;
using DevTechTest.Models;

namespace DevTechTest.Data
{
    public class JobDbContext: DbContext
    {
        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options) { }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobNote> JobNotes { get; set; }

        public DbSet<Client> Clients { get; set; }
    }
}
