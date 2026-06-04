using ChicaizaSuite.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChicaizaSuite.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            var defaultTenant = await context.Tenants.FirstOrDefaultAsync(t => t.Id == 1);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant
                {
                    Nombre = "ChicaizaSuite Demo Corp",
                    Ruc = "0000000000001",
                    Estado = "Activo"
                };
                context.Tenants.Add(defaultTenant);
                await context.SaveChangesAsync();
            }

            if (!await context.Usuarios.AnyAsync())
            {
                context.Usuarios.Add(new User
                {
                    TenantId = defaultTenant.Id,
                    Nombre = "Admin Local",
                    Email = "admin@chicaizasuite.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
                    Rol = "Administrador",
                    Estado = "Activo",
                    IsActive = true,
                    CreadoEn = DateTime.UtcNow,
                    ActualizadoEn = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }

            if (!await context.Productos.AnyAsync())
            {
                context.Productos.AddRange(
                    new Product { TenantId = defaultTenant.Id, Codigo = "PROD-001", CodigoBarras = "1234567890123", Nombre = "Camiseta Básica Blanca", Descripcion = "Camiseta de algodón", Categoria = "Ropa", PrecioCosto = 5.00m, PrecioVenta = 15.00m, IvaTipo = "15", IvaIncluido = true, StockGeneral = 100, Estado = "Activo" },
                    new Product { TenantId = defaultTenant.Id, Codigo = "PROD-002", CodigoBarras = "1234567890124", Nombre = "Pantalón Jean Azul", Descripcion = "Jean clásico", Categoria = "Ropa", PrecioCosto = 15.00m, PrecioVenta = 35.00m, IvaTipo = "15", IvaIncluido = true, StockGeneral = 50, Estado = "Activo" },
                    new Product { TenantId = defaultTenant.Id, Codigo = "PROD-003", CodigoBarras = "1234567890125", Nombre = "Zapatos Deportivos Negros", Descripcion = "Zapatos para correr", Categoria = "Calzado", PrecioCosto = 20.00m, PrecioVenta = 50.00m, IvaTipo = "15", IvaIncluido = true, StockGeneral = 30, Estado = "Activo" }
                );
                await context.SaveChangesAsync();
            }

            if (!await context.Clientes.AnyAsync())
            {
                context.Clientes.AddRange(
                    new Cliente { TenantId = defaultTenant.Id, Identificacion = "0991274545001", RazonSocial = "INMOBILIARIA DEL SOL S.A. MOBILSOL", Email = "info@mobilsol.com", Telefono = "0999999999", Direccion = "Av. Siempre Viva 123", Ciudad = "Guayaquil", Estado = "Activo" },
                    new Cliente { TenantId = defaultTenant.Id, Identificacion = "0928668771", RazonSocial = "ESPINOZA RUIZ FERNANDO", Email = "fernando@example.com", Telefono = "0988888888", Direccion = "Calle Falsa 123", Ciudad = "Quito", Estado = "Activo" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
