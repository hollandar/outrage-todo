using Microsoft.EntityFrameworkCore;

namespace Turbulence.Data
{
    public class TurbulenceDbContext: DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<ProjectMember> ProjectMembers{ get; set; }

        public TurbulenceDbContext(DbContextOptions<TurbulenceDbContext> options): base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId);
            modelBuilder.Entity<ApiKey>()
                .HasIndex(a => a.Key)
                .IsUnique();
            modelBuilder.Entity<ApiKey>()
                .HasOne(a => a.Project)
                .WithMany()
                .HasForeignKey(a => a.ProjectId);
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.MemberId, pm.ProjectId });
            modelBuilder.Entity<Project>()
                .HasMany(r => r.Members)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectId);
            modelBuilder.Entity<Member>()
                .HasMany(r => r.Projects)
                .WithOne(r => r.Member)
                .HasForeignKey(r => r.MemberId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
