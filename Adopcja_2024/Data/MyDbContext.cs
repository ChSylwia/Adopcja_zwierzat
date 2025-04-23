namespace Adopcja_2024.Data
{
    using Adopcja_2024.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Defines the <see cref="MyDbContext" />
    /// </summary>
    public class MyDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the osoba
        /// </summary>
        public DbSet<Osoba> osoba { get; set; }

        /// <summary>
        /// Gets or sets the miasto
        /// </summary>
        public DbSet<Miasto> miasto { get; set; }

        /// <summary>
        /// Gets or sets the ogloszenia
        /// </summary>
        public DbSet<Ogloszenie> ogloszenie { get; set; }

        public DbSet<Zwierzak> zwierzak { get; set; }
        public DbSet<Rasa> rasa { get; set; }
        public DbSet<Gatunek> gatunek { get; set; }
        public DbSet<Wiek> wiek { get; set; }
        public DbSet<Wielkosc> wielkosc { get; set; }

        public DbSet<Rezerwacja> rezerwuj { get; set; }
        public DbSet<Instytucja> instytucja { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="MyDbContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{MyDbContext}"/></param>
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// The OnModelCreating
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder<see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Osoba>(entity =>
            {
                entity.HasKey(e => e.id_osoba);
                entity.HasOne(o => o.Miasto)
                   .WithMany(m => m.Osoby)
                   .HasForeignKey(o => o.id_miasto) // Wskazanie klucza obcego
                   .IsRequired();
                entity.HasMany(o => o.Ogloszenia) 
                    .WithOne(o => o.Osoba) 
                    .HasForeignKey(o => o.id_osoba) 
                    .IsRequired();

                entity.HasOne(o => o.Instytucja)
                .WithOne(i => i.Osoba)
                .HasForeignKey<Osoba>(o => o.id_instytucja)
                .IsRequired(false);
            });

            modelBuilder.Entity<Miasto>(entity =>
            {
                entity.HasKey(m => m.id_miasto);
            });

            modelBuilder.Entity<Instytucja>(entity =>
            {
                entity.HasKey(i => i.id_instytucja);
                entity.HasOne(o => o.Miasto)
                   .WithMany(m => m.Instytucje)
                   .HasForeignKey(o => o.id_miasto) // Wskazanie klucza obcego
                   .IsRequired();
            });

            modelBuilder.Entity<Ogloszenie>(entity =>
            {
                entity.ToTable("Ogloszenie"); 
                entity.HasKey(o => o.id_ogloszenie);

                entity.HasOne(o => o.Osoba)
                      .WithMany(o => o.Ogloszenia)
                      .HasForeignKey(o => o.id_osoba)
                      .IsRequired();
            });

            modelBuilder.Entity<Zwierzak>(entity =>
            {
                entity.HasKey(z => z.id_zwierzak);
                entity.HasOne(z => z.Rasa)
                    .WithMany(z => z.Zwierzak)
                    .HasForeignKey(z => z.id_rasa)
                    .IsRequired();
                entity.HasOne(z => z.Wiek)
                    .WithMany(z => z.Zwierzak)
                    .HasForeignKey(z => z.id_wiek)
                    .IsRequired();
                entity.HasOne(z => z.Wielkosc)
                    .WithMany(z => z.Zwierzak)
                    .HasForeignKey(z => z.id_wielkosc)
                    .IsRequired();
            });

            modelBuilder.Entity<Rasa>(entity =>
            {
                entity.HasKey(r => r.id_rasa);
                entity.HasOne(r => r.Gatunek)
                    .WithMany(r=>r.Rasa)
                    .HasForeignKey(r => r.id_gatunek)
                    .IsRequired();
            });

            modelBuilder.Entity<Gatunek>(entity =>
            {
                entity.HasKey(g => g.id_gatunek);
            });

            modelBuilder.Entity<Wiek>(entity =>
            {
                entity.HasKey(w => w.id_wiek);
            });

            modelBuilder.Entity<Wielkosc>(entity =>
            {
                entity.HasKey(w => w.id_wielkosc);
            });

            modelBuilder.Entity<Rezerwacja>(entity =>
            {
                entity.HasKey(r => r.id_rezerwacja);

                entity.HasOne(r => r.Zwierzak)
                    .WithOne(z => z.Rezerwacja)
                    .HasForeignKey<Rezerwacja>(r => r.id_zwierzak)
                    .IsRequired();

                entity.HasOne(r => r.Osoba)
                    .WithMany(o => o.Rezerwacja) 
                    .HasForeignKey(r => r.id_osoba)
                    .IsRequired();
            });
        }
    }
}
