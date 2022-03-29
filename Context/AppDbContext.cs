using DogukanTURHAL.Covid19.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogukanTURHAL.Covid19.API.Context
{
    public class AppDbContext: DbContext
    {
        /**
         * Bu bölümde entity mi db üzerinde entityframeworkCore kullanarak oluşturma işlemini yaptım. 
         */

        /**
         * Refactor yapılacak olursa generic bir yapıya geçişi yapılır ve yeni eklenecek olan entitylerin repositoryleri daha kolay entegre edilebilir ve scalable olur.
         */
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Covid> Covids { get; set; }
    }
}
