using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public class TicTacToeDbContext : DbContext
{
    public DbSet<PlayerModel> Players { get; private set; }
    public DbSet<CellModel> Cells { get; private set; }
    public DbSet<SessionModel> Sessions { get; private set; }

    public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerModel>()
            .HasMany<SessionModel>()
            .WithOne(session => session.PlayerO)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerModel>()
            .HasMany<SessionModel>()
            .WithOne(session => session.PlayerX)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SessionModel>()
            .HasMany<CellModel>()
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SessionModel>()
            .HasOne<PlayerModel>(session => session.PlayerO)
            .WithMany(player => player.Sessions)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<SessionModel>()
            .HasOne<PlayerModel>(session => session.PlayerX)
            .WithMany(player => player.Sessions)
            .OnDelete(DeleteBehavior.SetNull);
    }
}