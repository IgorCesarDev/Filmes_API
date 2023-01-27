using FilmesFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FilmesFinal.Data
{
    public class UserDbContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Genero> Generos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CustomIdentityUser admin = new CustomIdentityUser
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = 99999
            };
            PasswordHasher<CustomIdentityUser> hasher = new PasswordHasher<CustomIdentityUser>();
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");
            builder.Entity<CustomIdentityUser>().HasData(admin);
            builder.Entity<IdentityRole<int>>().HasData(new IdentityRole<int> { Id = 99999, Name = "admin", NormalizedName = "ADMIN"});
            builder.Entity<IdentityRole<int>>().HasData(new IdentityRole<int> { Id = 99997, Name = "regular", NormalizedName = "REGULAR" });

            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int> { RoleId = 99999, UserId = 99999 });
        }
    }
}
