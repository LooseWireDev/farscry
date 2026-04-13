using Microsoft.EntityFrameworkCore;
using Farscry.Api.Models;

namespace Farscry.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Family> Families { get; set; }
    public DbSet<UserContact> UserContacts { get; set; }
    public DbSet<Call> Calls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(user => user.InitiatedCalls)
            .WithOne(call => call.Initiator)
            .HasForeignKey(call => call.InitiatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(user => user.ReceivedCalls)
            .WithOne(call => call.Receiver)
            .HasForeignKey(call => call.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(user => user.Contacts)
            .WithOne(contact => contact.User)
            .HasForeignKey(contact => contact.UserId);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.FriendId)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<Family>()
            .HasMany(family => family.Users)
            .WithOne(user => user.Family)
            .HasForeignKey(user => user.FamilyId);

        modelBuilder.Entity<Family>()
            .HasOne(family => family.Owner)
            .WithMany()
            .HasForeignKey(family => family.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserContact>()
            .HasOne(userContact => userContact.Contact)
            .WithMany()
            .HasForeignKey(userContact => userContact.ContactId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
