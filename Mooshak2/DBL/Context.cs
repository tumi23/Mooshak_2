namespace Mooshak2.DBL
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
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<AssignmentList> AssignmentLists { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Milestone> Milestones { get; set; }
        public virtual DbSet<MilestoneList> MilestoneLists { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StudentCourseList> StudentCourseLists { get; set; }
        public virtual DbSet<Submit> Submits { get; set; }
        public virtual DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserRoles)
                .WithRequired(e => e.AspNetUser)
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

            modelBuilder.Entity<Assignment>()
                .Property(e => e.FinalGrade)
                .HasPrecision(20, 0);

            modelBuilder.Entity<Course>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Assignments)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.StudentCourseLists)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Milestone>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Milestone>()
                .HasMany(e => e.MilestoneLists)
                .WithRequired(e => e.Milestone)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Role1)
                .IsUnicode(false);

            modelBuilder.Entity<Submit>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Table>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Table>()
                .Property(e => e.FinalGrade)
                .HasPrecision(20, 0);
        }
    }
}
