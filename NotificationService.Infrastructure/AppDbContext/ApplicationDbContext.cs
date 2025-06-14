﻿using System.Text.Json;
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
        
        public DbSet<Mailbox> Mailboxes { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<EmailManager> EmailManagers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            // Seed data
            var bases = LoadSeedData<EmailTemplate>("SeedData/EmailTemplates.json");
            builder.Entity<EmailTemplate>().HasData(bases);

            var hists = LoadSeedData<EmailLog>("SeedData/EmailLogs.json");
            builder.Entity<EmailLog>().HasData(hists);
            var mailboxes = LoadSeedData<Mailbox>("SeedData/MailBoxes.json");
            builder.Entity<Mailbox>().HasData(mailboxes);
            var emails = LoadSeedData<Email>("SeedData/Emails.json");
            builder.Entity<Email>().HasData(emails);
            var emailManagers = LoadSeedData<EmailManager>("SeedData/EmailManagers.json");
            builder.Entity<EmailManager>().HasData(emailManagers);

            // Cấu hình relationships
            builder.Entity<Mailbox>()
                .HasMany(m => m.Emails)
                .WithOne(e => e.Mailbox)
                .HasForeignKey(e => e.MailboxId)
                .OnDelete(DeleteBehavior.Cascade);
                
            
                
            builder.Entity<Email>()
                .HasMany(e => e.EmailManagers)
                .WithOne(em => em.Email)
                .HasForeignKey(em => em.EmailId)
                .OnDelete(DeleteBehavior.Cascade);
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
