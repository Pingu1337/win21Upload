using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace win21.Data
{
    public class TokenDbcontext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"DefaultConnection");

        //}
        public TokenDbcontext(DbContextOptions<TokenDbcontext> options)
            : base(options)
        {
        }

        public DbSet<TokenModel> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TokenModel>().ToTable("TOKEN");
        }
    }
    
}
