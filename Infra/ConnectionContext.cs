using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroveStart.Model;
using Microsoft.EntityFrameworkCore;

namespace GroveStart.Infra
{
    public class ConnectionContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Chat> Chats { get; set; }

        public ConnectionContext(DbContextOptions<ConnectionContext> options) : base(options)
        {
        }
    }
}