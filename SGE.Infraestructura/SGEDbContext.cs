using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Usuarios;
using SGE.Dominio.Comun;
using System.Security.Cryptography;
using System.Text;


namespace SGE.Infraestructura;

public class SGEDbContext : DbContext
{
    public DbSet<Expediente> Expedientes { get; set; }
    public DbSet<Tramite> Tramites { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }  

    public SGEDbContext() { }
    public SGEDbContext(DbContextOptions<SGEDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=SGE.sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ComplexProperty(e => e.Caratula, c => 
                c.Property(x => x.Valor).HasColumnName("Caratula"));
        });

        modelBuilder.Entity<Tramite>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.ComplexProperty(t => t.Contenido, c => 
                c.Property(x => x.Valor).HasColumnName("Contenido"));
            entity.HasOne<Expediente>()
                  .WithMany()
                  .HasForeignKey(t => t.ExpedienteId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.ComplexProperty(u => u.CorreoElectronico, c =>
                c.Property(x => x.Valor).HasColumnName("CorreoElectronico"));
            entity.Property(u => u.Nombre);
            entity.Property(u => u.ContrasenaHash);
            entity.Property(u => u.EsAdministrador);
            entity.Property(u => u.PermisosSerializados)
                .HasColumnName("Permisos")
                .HasDefaultValue("");
            entity.Ignore(u => u.Permisos);
        });



        base.OnModelCreating(modelBuilder);
    }

    public static string Hashear(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public void SembrarDatos()
    {
        if (Usuarios.Any()) return; // si ya hay usuarios no sembramos

        // Admin
        var admin = new Usuario(
            "Administrador",
            new CorreoElectronico("admin@sge.com"),
            Hashear("admin123"),
            esAdministrador: true
        );

        // Usuario con permisos parciales
        var usuarioConPermisos = new Usuario(
            "Usuario Con Permisos",
            new CorreoElectronico("usuario@sge.com"),
            Hashear("usuario123")
        );
        usuarioConPermisos.AsignarPermiso(Permiso.ExpedienteAlta);
        usuarioConPermisos.AsignarPermiso(Permiso.TramiteAlta);

        // Usuario sin permisos (solo lectura)
        var usuarioSinPermisos = new Usuario(
            "Usuario Sin Permisos",
            new CorreoElectronico("invitado@sge.com"),
            Hashear("invitado123")
        );

        Usuarios.AddRange(admin, usuarioConPermisos, usuarioSinPermisos);
        SaveChanges();
    }
}