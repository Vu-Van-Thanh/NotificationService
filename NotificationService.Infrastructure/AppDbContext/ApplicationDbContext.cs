using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NotificationService.Core.Domain.Entities;


namespace NotificationService.Infrastructure.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailLog> EmailLog { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
           

            // Seed data
            var bases = LoadSeedData<EmailTemplate>("SeedData/EmailTemplates.json");
            builder.Entity<EmailTemplate>().HasData(bases);

            var hists = LoadSeedData<EmailLog>("SeedData/EmailLogs.json");
            builder.Entity<EmailLog>().HasData(hists);

            
        }

        private static List<T> LoadSeedData<T>(string filePath)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.Parent.FullName;
            string fullPath = Path.Combine(projectRoot, "NotificationService.Infrastructure", filePath);


            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Không tìm thấy file seed data: {fullPath}");

            var jsonData = File.ReadAllText(fullPath);
            var items = JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();

            return items;
        }
    }
}
