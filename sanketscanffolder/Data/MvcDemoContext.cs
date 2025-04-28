using sanketscanffolder.Models;
using Microsoft.EntityFrameworkCore;

namespace sanketscanffolder.Data
{
    public class MVCDemoDbContext : DbContext
    {
        public MVCDemoDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Semester> Semesters { get; set; }
    }
}
