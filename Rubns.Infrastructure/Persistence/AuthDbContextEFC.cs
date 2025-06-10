namespace Rubns.Infrastructure.Persistence
{
    public class AuthDbContextEFC : DbContext
    {
        public AuthDbContextEFC(DbContextOptions<AuthDbContextEFC> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SessionUser> SessionUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users", schema: "dbo");
                e.HasKey(k => k.UserID);
            });

            modelBuilder.Entity<SessionUser>(e =>
            {
                e.ToTable("SessionUsers", schema: "dbo");
                e.HasKey(k => k.ID);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
