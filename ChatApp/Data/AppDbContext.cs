using ChatApp.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<ChatGroup>().HasOne(x => x.Owner).WithMany().OnDelete(DeleteBehavior.NoAction);
            /*modelBuilder.Entity<ChatGroup>()
                .HasOne(cg => cg.Owner)
                .WithMany()
                .HasForeignKey(cg => cg.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);*/

            /*modelBuilder.Entity<ChatGroup>()
                .Ignore(cg => cg.Owner);*/

            modelBuilder.Entity<Message>().HasIndex(x => x.CreatedAt).IsUnique(false);
        }
    }
}
