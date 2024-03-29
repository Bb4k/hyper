﻿using Microsoft.EntityFrameworkCore;

namespace hyperAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PR> PRs { get; set; }
        public DbSet<UserPR> UserPRs { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Warning> Warnings { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
