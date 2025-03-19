using PRN231.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PRN231.DAL;

public partial class SmartHeadContext :IdentityDbContext<User, Role, int>
{
    public SmartHeadContext()
    {
    }

    public SmartHeadContext(DbContextOptions<SmartHeadContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Credential> Credentials { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingUser> BookingUsers { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }
    
    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Db"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Student)
            .WithMany(x => x.StudentFeedbacks)
            .HasForeignKey(f => f.StudentId)
            .OnDelete(DeleteBehavior.NoAction); // Use the appropriate delete behavior for your scenario

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Tutor)
            .WithMany(x => x.TutorFeedbacks)
            .HasForeignKey(f => f.TutorId)
            .OnDelete(DeleteBehavior.NoAction);

        // modelBuilder.Entity<Credential>()
        //     .HasOne(f => f.Subject)
        //     .WithOne(x => x.Credential)
        //     .HasForeignKey<Subject>(x => x.CredentialId)
        //     .OnDelete(DeleteBehavior.NoAction);

        // modelBuilder.Entity<Subject>()
        //     .HasOne(f => f.Credential)
        //     .WithOne(x => x.Subject)
        //     .HasForeignKey<Credential>(x => x.SubjectId)
        //     .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<BookingUser>()
            .HasOne(f => f.Booking)
            .WithMany(x => x.BookingUsers)
            .HasForeignKey(f => f.BookingId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<BookingUser>()
            .HasOne(f => f.User)
            .WithMany(x => x.BookingUsers)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Transaction>()
            .HasOne(f => f.User)
            .WithMany(x => x.SentTransactions)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Transaction>()
            .HasOne(f => f.Receiver)
            .WithMany(x => x.ReceivedTransactions)
            .HasForeignKey(f => f.ReceiverId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PostRating>()
            .HasOne(f => f.Post)
            .WithMany(x => x.Ratings)
            .HasForeignKey(f => f.PostId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.Property(e => e.StartTime).HasColumnType("time");
        });

        /*modelBuilder.Entity<Service>()
            .HasOne(f => f.Subject)
            .WithMany(x => x.Services)
            .HasForeignKey(f => f.SubjectId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Schedule>()
            .HasOne(f => f.Service)
            .WithMany(x => x.Schedules)
            .HasForeignKey(f => f.ServiceId)
            .OnDelete(DeleteBehavior.NoAction);*/


        /*modelBuilder.Entity<User>()
            .HasMany(f => f.StudentFeedbacks)
            .WithOne()
            .HasForeignKey(f => f.StudentId)
            .OnDelete(DeleteBehavior.NoAction); // Use the appropriate delete behavior for your scenario

        modelBuilder.Entity<User>()
            .HasMany(f => f.TutorFeedbacks)
            .WithOne()
            .HasForeignKey(f => f.TutorId)
            .OnDelete(DeleteBehavior.NoAction);*/
        
       
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
