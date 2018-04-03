using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace QuizPond.Data.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    

        public DbSet<NewGame> NewGames { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        

            // base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Quiz>()
            //    .HasMany(q => q.NewGames)
            //    .WithOne(n => n.Quiz)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewGame>().HasKey(n => new { n.GameCodeId });

            //modelBuilder.Entity<NewGame>()
            //    .HasMany(n => n.Players)
            //    .WithOne(u => u.NewGame)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Player>().HasKey(p => new { p.PlayerId });
            modelBuilder.Entity<Player>()
                .HasOne(p => p.NewGame)
                .WithMany(n => n.Players)
                .HasForeignKey(p => p.GameCodeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        }
    }
