namespace WebApi.Helpers;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<Team> Team { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Invite> Invite { get; set; }
    public DbSet<TrainingStatus> TrainingStatus { get; set; }
    public DbSet<Training> Training { get; set; }
    public DbSet<TrainingRegistration> TrainingRegistration { get; set; }
    public DbSet<TrainingRegistrationStatus> TrainingRegistrationStatus { get; set; }
    public DbSet<AttendanceStatus> AttendanceStatus { get; set; }
    public DbSet<Attendance> Attendance { get; set; }
    public DbSet<FeedbackType> FeedbackType { get; set; }
    public DbSet<Feedback> Feedback { get; set; }

}