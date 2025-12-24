using Microsoft.EntityFrameworkCore;
using TravelАgency.Domain.ModelsDb;

namespace TravelАgency.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserDb> UsersDb { get; set; }

        public DbSet<LekarstvaDb> LekarstvaDb { get; set; }

        public DbSet<ZakazDb> ZakazDb { get; set; }

        public DbSet<PictureLekarstvaDb> PicturesLekarstvaDb { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Database.EnsureCreated(); // Раскомментируй, если нужно пересоздать базу
        }
    }
}