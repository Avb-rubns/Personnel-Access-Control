namespace Rubns.Infrastructure.Persistence
{
    public class AuthDbContextEFC : DbContext
    {
        public AuthDbContextEFC(DbContextOptions<AuthDbContextEFC> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<UserDb> Users { get; set; }
        public DbSet<SessionUserDb> SessionUser { get; set; }
        public DbSet<RolDB> Rols { get; set; }
        public DbSet<ResetPasswordDb> ResetPasswords { get; set; }
        public DbSet<LinkDb> Links { get; set; }
        public DbSet<ClickDb> Clicks { get; set; }
        public DbSet<QRDb> QRs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDb>(e =>
            {
                e.ToTable("Users", schema: "dbo");
                e.HasKey(k => k.UserID);
            });

            modelBuilder.Entity<SessionUserDb>(e =>
            {
                e.ToTable("SessionUsers", schema: "dbo");
                e.HasKey(k => k.ID);
            });

            modelBuilder.Entity<RolDB>(e =>
            {
                e.ToTable("Rols", schema: "dbo");
                e.HasKey(k => k.RolID);
            });

            modelBuilder.Entity<ResetPasswordDb>(e =>
            {
                e.ToTable("ResetPasswords", schema: "dbo");
                e.HasKey(reset => reset.ResetPasswordID);
                e.Property(p => p.Registed)
                .HasDefaultValueSql("SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)'")
                .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<LinkDb>(e =>
            {
                e.ToTable("Links", schema: "dbo");
                e.HasKey(k => k.ID);
                e.Property(p => p.Registered)
                .HasDefaultValueSql("SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)'")
                .ValueGeneratedOnAdd();
                e.Property(p => p.LastModificated)
                .HasDefaultValueSql("SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)'")
                .ValueGeneratedOnAdd();
                e.ToTable(link => link.HasTrigger("trg_UpdateLastModificated_Links"));
                e.HasOne(link => link.QR).WithOne(qr => qr.Link).HasForeignKey<QRDb>(qr => qr.LinkId);
                e.HasMany(click => click.Clicks).WithOne(link => link.Link).HasForeignKey(link => link.LinkId);
                e.HasOne(user => user.User).WithMany().HasForeignKey(link => link.UserID);
                e.HasOne(user => user.LastUser).WithMany().HasForeignKey(link => link.LastUserID);

            });

            modelBuilder.Entity<QRDb>(e =>
            {
                e.ToTable("QRs", schema: "dbo");
                e.HasKey(k => k.Id);
                e.Property(p => p.ColorDark)
                .HasDefaultValueSql("#000000")
                .ValueGeneratedOnAdd();
                e.Property(p => p.ColorLight)
                .HasDefaultValueSql("#FFFFFF")
                .ValueGeneratedOnAdd();
                e.Property(p => p.DotScale)
                .HasDefaultValueSql("1.0")
                .ValueGeneratedOnAdd();
                e.Property(p => p.QuietZone)
                .HasDefaultValueSql("20")
                .ValueGeneratedOnAdd();
                e.Property(p => p.DotScale)
                .HasColumnType("float");
                e.Property(p => p.LastModificated)
                .HasDefaultValueSql("SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)'")
                .ValueGeneratedOnAdd();
                e.ToTable(qr => qr.HasTrigger("trg_UpdateLastModificated_QRs"));
                e.HasOne(user => user.User).WithMany().HasForeignKey(qr => qr.LastUserID);
            });

            modelBuilder.Entity<ClickDb>(e =>
            {
                e.ToTable("Clicks", schema: "dbo");
                e.HasKey(e => e.ID);
                e.Property(p => p.ClickedAt)
                .HasDefaultValueSql("SYSDATETIMEOFFSET() AT TIME ZONE 'Central Standard Time (Mexico)'")
                .ValueGeneratedOnAdd();
            });


            base.OnModelCreating(modelBuilder);
        }

    }
}
