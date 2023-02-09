using backend.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace backend.DAL
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new List<IdentityRole> {
                new IdentityRole
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Id = Guid.NewGuid().ToString(),
                    Name = "Student",
                    NormalizedName = "STUDENT",
                },

                new IdentityRole
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },

                new IdentityRole
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Id = Guid.NewGuid().ToString(),
                    Name = "Teacher",
                    NormalizedName = "TEACHER",
                }
            });

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

        public new DbSet<User> Users { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ConfirmCode> ConfirmCodes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }
}

