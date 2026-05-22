using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GroveStart.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroveStart.Infra
{
    public class ConnectionContext : DbContext
    {
        private readonly ILogger<ConnectionContext> _logger;

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Chat> Chats { get; set; }

        public ConnectionContext(DbContextOptions<ConnectionContext> options, ILogger<ConnectionContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var written = await base.SaveChangesAsync(cancellationToken);
            _logger.LogInformation(
                "ConnectionContext.SaveChangesAsync: {Count} alteração(ões) enviada(s) ao banco.",
                written);
            return written;
        }
    }
}