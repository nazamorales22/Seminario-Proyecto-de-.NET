using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Expedientes; 
using SGE.Dominio.Tramites;    

namespace SGE.Infraestructura;

public class SGEDbContext : DbContext
{
    // 1. Acá definís las tablas que va a tener tu base de datos SQLite
    public DbSet<Expediente> Expedientes { get; set; }
    public DbSet<Tramite> Tramites { get; set; }

    // 2. Configuración de dónde se guarda físicamente el archivo de la BD
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Esto creará un archivo llamado "SGE.sqlite" en tu carpeta
        optionsBuilder.UseSqlite("Data Source=SGE.sqlite");
    }

    // 3. Acá mapeás las relaciones (Claves primarias y el borrado en cascada)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuramos la tabla de Expedientes
        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(e => e.Id); // Clave primaria

            // 🛠️ ¡NUEVA MEJORA! (Owned Type para Caratula)
            // Le avisamos a EF Core que Caratula es un Value Object embebido.
            // Esto meterá los campos de Caratula como columnas de la tabla Expedientes.
            entity.OwnsOne(e => e.Caratula); 
        });

        // Configuramos la tabla de Trámites y la relación con Expediente
        modelBuilder.Entity<Tramite>(entity =>
        {
            entity.HasKey(t => t.Id); // Clave primaria

            // Owned Type para el Contenido del trámite (el que agregamos antes)
            entity.OwnsOne(t => t.Contenido); 

            // Relación de 1 a muchos con borrado en cascada implícito
            entity.HasOne<Expediente>() 
                  .WithMany() 
                  .HasForeignKey(t => t.ExpedienteId)
                  .OnDelete(DeleteBehavior.Cascade); // ¡La magia del borrado automático!
        });

        base.OnModelCreating(modelBuilder);
    }
}