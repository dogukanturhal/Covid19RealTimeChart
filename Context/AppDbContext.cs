using DogukanTURHAL.Covid19.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogukanTURHAL.Covid19.API.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Covid> Covids { get; set; }
    }
}
