using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UtterlyAPI.Models;

public class UtterlyContext : IdentityDbContext<UtterlyUser>
{
    public UtterlyContext(DbContextOptions<UtterlyContext> options)
        : base(options)
    {
    }

    public DbSet<UtterlyPost> UtterlyPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UtterlyPost>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
