using PupuseriaSalvadorena.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace PupuseriaSalvadorena.Conexion
{
    public class MiDbContext: DbContext
    {
        public MiDbContext(DbContextOptions<MiDbContext> options): base(options)
        {}

        public DbSet<AlertaCuentaPagar> AlertaCuentaPagar { get; set; }
        public DbSet<CorreoElectronico> CorreoElectronico { get; set; }
        public DbSet<Provincia> Provincia { get; set; }
        public DbSet<Canton> Canton { get; set; }
        public DbSet<Distrito> Distrito { get; set; }
        public DbSet<Direccion> Direccion { get; set; }
        public DbSet<Telefonos> Telefonos { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Negocio> Negocio { get; set; }
        public DbSet<RegistroBancario> RegistroBancario { get; set; }
        public DbSet<Impuesto> Impuesto { get; set; }
        public DbSet<TipoTransacciones> TipoTransacciones { get; set; }
        public DbSet<RegistroLibro> RegistroLibro { get; set; }
        public DbSet<DetalleTransaccion> DetalleTransaccion { get; set; }
        public DbSet<ConciliacionBancaria> ConciliacionBancaria { get; set; }
        public DbSet<CategoriaPresupuesto> CategoriaPresupuesto { get; set; }
        public DbSet<Presupuesto> Presupuesto { get; set; }
        public DbSet<DetallePresupuesto> DetallePresupuesto { get; set; }
        public DbSet<DeclaracionImpuesto> DeclaracionImpuesto { get; set; }
        public DbSet<TipoPago> TipoPago { get; set; }
        public DbSet<TipoFactura> TipoFactura { get; set; }
        public DbSet<FacturaVenta> FacturaVenta { get; set; }
        public DbSet<EnvioFactura> EnvioFactura { get; set; }
        public DbSet<FacturaCompra> FacturaCompra { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<MateriaPrima> MateriaPrima { get; set; }
        public DbSet<HistorialCompra> HistorialCompra { get; set; }
        public DbSet<CuentaPagar> CuentaPagar { get; set; }
        public DbSet<DetalleCuenta> DetalleCuenta { get; set; }
        public DbSet<Platillo> Platillo { get; set; }
        public DbSet<TipoVenta> TipoVenta { get; set; }
        public DbSet<HistorialVenta> HistorialVenta { get; set; }
        public DbSet<Pronostico> Pronostico { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DetalleTransaccion>()
                .HasKey(dt => new { dt.IdRegistroLibros, dt.IdTransaccion });

            modelBuilder.Entity<DetallePresupuesto>()
                .HasKey(dp => new { dp.IdPresupuesto, dp.IdTransaccion });

            modelBuilder.Entity<HistorialCompra>()
                .HasKey(hc => new { hc.IdFacturaCompra, hc.IdCompra });

            modelBuilder.Entity<HistorialVenta>()
                .HasKey(hv => new { hv.IdVenta, hv.IdPlatillo, hv.IdFacturaVenta });
        }
    }
}
