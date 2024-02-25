using Microsoft.EntityFrameworkCore;
using SkillMasteryAPI.Domain.Models;


namespace SkillMasteryAPI.Infrastructure.Data;
public class SkillMasteryContext : DbContext 

{
    public  SkillMasteryContext(DbContextOptions<SkillMasteryContext> options) : base(options)
    { 
    }
    public DbSet<Dificulty> Dificulty { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Skill> Skill { get; set; }
    public DbSet<Goal> Goal { get; set; }
    public DbSet<UserSkill> UserSkill { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)

{ 
        modelBuilder.Entity<User>()
         .HasMany(u => u.UserSkill)
         .WithOne(cu => cu.User)
         .HasForeignKey(cu => cu.UserId);

    modelBuilder.Entity<Skill>()
         .HasMany(u => u.UserSkill)
         .WithOne(cu => cu.Skill)
         .HasForeignKey(cu => cu.SkillId);

    modelBuilder.Entity<Dificulty>()
         .HasMany(c => c.Skill)
         .WithOne(u => u.Dificulty)
         .HasForeignKey(u => u.DificultyId);

    modelBuilder.Entity<UserSkill>()
          .HasMany(c => c.Goal)
          .WithOne(u => u.UserSkill)
          .HasForeignKey(u => u.UserSkillId);
    }
    
}



