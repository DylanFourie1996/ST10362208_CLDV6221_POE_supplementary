using Microsoft.EntityFrameworkCore;
using ST10362208_CLDV6221_POE.REDO.Models;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options) : base(options) { }

    public DbSet<Client> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Cart { get; set; }

    public DbSet<Transactions> Transactions { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cart>()
            .HasOne(c => c.Product)
            .WithMany()
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
