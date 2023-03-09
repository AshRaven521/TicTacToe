using Microsoft.EntityFrameworkCore;
using TicTacToe.Models;

namespace TicTacToe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<PlayerBoard> PlayerBoards { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(p => p.Name).IsUnique();
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.Property(b => b.NumberOfColumn).IsRequired().HasMaxLength(1);
                entity.Property(p => p.NumberOfRows).IsRequired().HasMaxLength(1);
            });

            modelBuilder.Entity<PlayerBoard>()
                .HasKey(pb => new { pb.PlayerId, pb.BoardId });

            modelBuilder.Entity<PlayerBoard>()
                .HasOne(pb => pb.Player)
                .WithMany(p => p.PlayerBoards)
                .HasForeignKey(pb => pb.PlayerId);

            modelBuilder.Entity<PlayerBoard>()
                .HasOne(pb => pb.Board)
                .WithMany(b => b.PlayerBoards)
                .HasForeignKey(pb => pb.BoardId);

            modelBuilder.Entity<PlayerBoard>(entity =>
            {
                entity.HasIndex(pb => new { pb.BoardId, pb.PlayerId }).IsUnique();
            });

            modelBuilder.Entity<Movement>(entity =>
            {
                entity.HasIndex(m => new { m.BoardId, m.Position }).IsUnique();
            });
        }
    }
}
