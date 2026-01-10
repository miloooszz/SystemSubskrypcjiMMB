using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SystemSubskrypcjiMMB.Models;

namespace SystemSubskrypcjiMMB.Data
{
    public class DbContextMMB : IdentityDbContext<ApplicationUser>
    {
        public DbContextMMB(DbContextOptions<DbContextMMB> options) : base(options) { }

        public DbSet<SubskrypcjaMMB> Subskrypcje { get; set; }
        public DbSet<KategoriaMMB> Kategorie { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relacja Subskrypcja -> Użytkownik
            builder.Entity<SubskrypcjaMMB>()
                .HasOne(s => s.Uzytkownik)
                .WithMany(u => u.Subskrypcje)
                .HasForeignKey(s => s.UzytkownikId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Subskrypcja -> Kategoria
            builder.Entity<SubskrypcjaMMB>()
                .HasOne(s => s.Kategoria)
                .WithMany(k => k.Subskrypcje)
                .HasForeignKey(s => s.KategoriaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
