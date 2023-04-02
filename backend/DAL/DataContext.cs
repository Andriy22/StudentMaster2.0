using backend.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.DAL;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions options)
        : base(options)
    {
    }

    public new DbSet<User> Users { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ConfirmCode> ConfirmCodes { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<WorkType> WorkTypes { get; set; }
    public DbSet<Work> Works { get; set; }
    public DbSet<EvaluationCriterion> EvaluationCriteria { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ScheduleItem> ScheduleItems { get; set; }
    public DbSet<ScheduleDay> ScheduleDays { get; set; }
    public DbSet<ScheduleItemType> ScheduleItemTypes { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Ban> Bans { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(new List<IdentityRole>
        {
            new()
            {
                ConcurrencyStamp = "ea6a03d5-c051-49d3-953f-9ea6babb5245",
                Id = "1548c5b9-11a8-40ac-a955-1b065c52da42",
                Name = "Student",
                NormalizedName = "STUDENT"
            },

            new()
            {
                ConcurrencyStamp = "46ca54ad-93be-4aa9-8e9b-004d576d268a",
                Id = "fbc4bb8c-980d-4f38-ba1f-3d4c62e65e80",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },

            new()
            {
                ConcurrencyStamp = "1083da9f-caa5-4fdd-9e11-9dbc4e6b7ba3",
                Id = "fcd7a980-c8d9-4217-8c21-31ede9b72cc9",
                Name = "Teacher",
                NormalizedName = "TEACHER"
            }
        });

        builder.Entity<ScheduleDay>().HasData(new List<ScheduleDay>
        {
            new()
            {
                Id = 1,
                Name = "Понеділок"
            },
            new()
            {
                Id = 2,
                Name = "Вівторок"
            },
            new()
            {
                Id = 3,
                Name = "Середа"
            },
            new()
            {
                Id = 4,
                Name = "Четвер"
            },
            new()
            {
                Id = 5,
                Name = "П'ятниця"
            }
        });


        builder.Entity<ScheduleItemType>().HasData(
            new ScheduleItemType { Id = 1, Name = "Лекція" },
            new ScheduleItemType { Id = 2, Name = "Семінар" },
            new ScheduleItemType { Id = 3, Name = "Лабораторна робота" }
        );

        builder.Entity<TeacherGroup>()
            .HasKey(t => new { t.UserId, t.GroupId });

        builder.Entity<TeacherGroup>()
            .HasOne(sc => sc.User)
            .WithMany(s => s.TeacherGroups)
            .HasForeignKey(sc => sc.UserId);

        builder.Entity<TeacherGroup>()
            .HasOne(sc => sc.Group)
            .WithMany(c => c.TeacherGroups)
            .HasForeignKey(sc => sc.GroupId);

        base.OnModelCreating(builder);
    }
}