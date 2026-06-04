using Microsoft.EntityFrameworkCore;
using ChicaizaSuite.Api.Models;

namespace ChicaizaSuite.Api.Data
{
    public class AppDbContext : DbContext
    {
        private readonly int _tenantId;

        public AppDbContext(DbContextOptions<AppDbContext> options, ChicaizaSuite.Api.Services.ITenantService tenantService) : base(options) 
        { 
            _tenantId = tenantService.GetTenantId();
        }

        public DbSet<User> Usuarios { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Product> Productos { get; set; }
        public DbSet<ProductVariant> Variantes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
        public DbSet<CuentaCobrar> CuentasCobrar { get; set; }
        public DbSet<CuentaPagar> CuentasPagar { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<CajaPOS> CajasPOS { get; set; }
        public DbSet<SesionPOS> SesionesPOS { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Filtros Globales Multi-Tenant
            modelBuilder.Entity<User>().HasQueryFilter(e => e.TenantId == _tenantId);
            modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenantId == _tenantId);
            modelBuilder.Entity<Factura>().HasQueryFilter(e => e.TenantId == _tenantId);
            modelBuilder.Entity<Cliente>().HasQueryFilter(e => e.TenantId == _tenantId);
            modelBuilder.Entity<CajaPOS>().HasQueryFilter(e => e.TenantId == _tenantId);
            modelBuilder.Entity<SesionPOS>().HasQueryFilter(e => e.TenantId == _tenantId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Codigo)
                .IsUnique();
        }
    }
}
