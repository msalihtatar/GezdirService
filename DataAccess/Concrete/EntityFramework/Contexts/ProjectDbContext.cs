using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    /// <summary>
    /// Because this context is followed by migration for more than one provider
    /// works on PostGreSql db by default. If you want to pass sql
    /// When adding AddDbContext, use MsDbContext derived from it.
    /// </summary>
    public class ProjectDbContext : DbContext
    {
        /// <summary>
        /// in constructor we get IConfiguration, parallel to more than one db
        /// we can create migration.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        /// <summary>
        /// Let's also implement the general version.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        protected ProjectDbContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;

        }

        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupClaim> GroupClaims { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MobileLogin> MobileLogins { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Translate> Translates { get; set; }
        public DbSet<PlaceType> PlaceTypes { get; set; }
        public DbSet<TransportationType> TransportationTypes { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Communication> Communications { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }

        protected IConfiguration Configuration { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A791D64B609F");

                entity.ToTable("Activity");

                entity.Property(e => e.ActivityContent)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.ActivityHeader)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ActivityType).WithMany(p => p.Activities)
                    .HasForeignKey(d => d.ActivityTypeId)
                    .HasConstraintName("FK__Activity__Activi__4BAC3F29");
            });

            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.HasKey(e => e.ActivityTypeId).HasName("PK__Activity__95CEDE0EA3C3A83B");

                entity.ToTable("ActivityType");

                entity.Property(e => e.ActivityTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2AFB8CA7DF7C");

                entity.ToTable("Address");

                entity.Property(e => e.AddressContent)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("AddressContent");

                entity.HasOne(d => d.Location).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Address__Locatio__37A5467C");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFCA75064DA7");

                entity.ToTable("Comment");

                entity.Property(e => e.CommentContent)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CommentContent");

                entity.HasOne(d => d.Location).WithMany(p => p.Comments)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__Locatio__34C8D9D1");
            });

            modelBuilder.Entity<Communication>(entity =>
            {
                entity.HasKey(e => e.CommunicationId).HasName("PK__Communic__53E565EF5A0CA134");

                entity.ToTable("Communication");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMail");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Location).WithMany(p => p.Communications)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Communica__Locat__3A81B327");
            });

            modelBuilder.Entity<CustomerPreference>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0787161A88");

                entity.ToTable("CustomerPreference");

                entity.HasOne(d => d.Location).WithMany(p => p.CustomerPreferences)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustomerP__Locat__3D5E1FD2");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA49716ADB4AB");

                entity.ToTable("Location");

                entity.Property(e => e.Xcoordinate)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("XCoordinate");
                entity.Property(e => e.Ycoordinate)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("YCoordinate");

                entity.HasOne(d => d.Place).WithMany(p => p.Locations)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Location__PlaceI__2B3F6F97");
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => e.PlaceId).HasName("PK__Place__D5222B6E66334399");

                entity.ToTable("Place");

                entity.Property(e => e.Explanation)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.PlaceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.PlaceType).WithMany(p => p.Places)
                    .HasForeignKey(d => d.PlaceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Place__PlaceType__286302EC");
            });

            modelBuilder.Entity<PlaceType>(entity =>
            {
                entity.HasKey(e => e.PlaceTypeId).HasName("PK__PlaceTyp__516F03B53D4195FA");

                entity.ToTable("PlaceType");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.HasKey(e => e.ScoreId).HasName("PK__Score__7DD229D19BEBB26E");

                entity.ToTable("Score");

                entity.Property(e => e.ScoreNum).HasColumnName("ScoreNum");

                entity.HasOne(d => d.Location).WithMany(p => p.Scores)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Score__LocationI__2E1BDC42");
            });

            modelBuilder.Entity<Transportation>(entity =>
            {
                entity.HasKey(e => e.TransportationId).HasName("PK__Transpor__87E479362931566D");

                entity.ToTable("Transportation");

                entity.Property(e => e.Explanation)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Location).WithMany(p => p.Transportations)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transport__Locat__30F848ED");

                entity.HasOne(d => d.TransportationType).WithMany(p => p.Transportations)
                    .HasForeignKey(d => d.TransportationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transport__Trans__31EC6D26");
            });

            modelBuilder.Entity<TransportationType>(entity =>
            {
                entity.HasKey(e => e.TransportationTypeId).HasName("PK__Transpor__3214EC078AD643B7");

                entity.ToTable("TransportationType");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.UseCollation("Turkish_CI_AS");

            modelBuilder.Entity<UserPreference>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__UserPref__1788CCAC123489AF");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            //OnModelCreatingPartial(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DArchMsContext"))
                    .EnableSensitiveDataLogging());
            }
        }


        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
