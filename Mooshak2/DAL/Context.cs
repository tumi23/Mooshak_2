namespace Mooshak2.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Assignment> Assignment { get; set; }
        public virtual DbSet<AssignmentGradeList> AssignmentGradeList { get; set; }
        public virtual DbSet<AssignmentList> AssignmentList { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Milestone> Milestone { get; set; }
        public virtual DbSet<MilestoneGradeList> MilestoneGradeList { get; set; }
        public virtual DbSet<MilestoneList> MilestoneList { get; set; }
        public virtual DbSet<StudentCourseList> StudentCourseList { get; set; }
        public virtual DbSet<Submit> Submit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Assignment>()
                .Property(e => e.AllowedProgrammingLanguage)
                .IsUnicode(false);

            modelBuilder.Entity<AssignmentGradeList>()
                .Property(e => e.grade)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Course>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Milestone>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<MilestoneGradeList>()
                .Property(e => e.grade)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StudentCourseList>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Submit>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Submit>()
                .Property(e => e.Code)
                .IsUnicode(false);
        }
    }
}
