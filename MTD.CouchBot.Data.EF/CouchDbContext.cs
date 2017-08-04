using Microsoft.EntityFrameworkCore;
using MTD.CouchBot.Domain.Dtos.Bot;
using MTD.CouchBot.Domain.Dtos.Discord;
using MTD.CouchBot.Domain.Dtos.Live;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTD.CouchBot.Data.EF
{
    public class CouchDbContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<Statistics> Statististics { get; set; }
        public DbSet<Queue> LiveQueue { get; set; }

        public CouchDbContext(DbContextOptions<CouchDbContext> contextOptions)
            : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildChannel>()
                .HasKey(gc => new { gc.GuildId, gc.ChannelId });

            modelBuilder.Entity<GuildChannel>()
                .HasOne(gc => gc.Guild)
                .WithMany(gc => gc.GuildChannels)
                .HasForeignKey(gc => gc.GuildId);

            modelBuilder.Entity<GuildChannel>()
                .HasOne(gc => gc.Channel)
                .WithMany(gc => gc.GuildChannels)
                .HasForeignKey(gc => gc.ChannelId);

            modelBuilder.Entity<GuildUser>()
                .HasKey(gc => new { gc.GuildId, gc.UserId });

            modelBuilder.Entity<GuildUser>()
                .HasOne(gc => gc.Guild)
                .WithMany(gc => gc.GuildUsers)
                .HasForeignKey(gc => gc.GuildId);

            modelBuilder.Entity<GuildUser>()
                .HasOne(gc => gc.User)
                .WithMany(gc => gc.GuildUsers)
                .HasForeignKey(gc => gc.UserId);
        }
    }
}
