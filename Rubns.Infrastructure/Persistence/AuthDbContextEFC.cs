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
        public DbSet<Rol> Rols { get; set; }

        public DbSet<ResetPassword> ResetPasswords { get; set; }

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

            modelBuilder.Entity<Rol>(e =>
            {
                e.ToTable("Rols", schema: "dbo");
                e.HasKey(k => k.RolID);
            });

            modelBuilder.Entity<ResetPassword>(e =>
            {
                e.ToTable("ResetPasswords", schema: "dbo");
                e.HasKey(k => k.ResetPasswordID);
                e.Property(p => p.Registed)
                .HasDefaultValueSql("SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)'")
                .ValueGeneratedOnAdd();
            });


            base.OnModelCreating(modelBuilder);
        }

    }
}
