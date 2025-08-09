using Microsoft.EntityFrameworkCore;

namespace BodegaSystem.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BodegaDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<BodegaDBContext>>()))
            {
                if (context.Categorias.Any() && context.Proveedores.Any() && context.Productos.Any())
                {
                    return; // DB ya tiene datos
                }

                // Categorías
                var categorias = new List<Categoria>
                {
                    new Categoria { Nombre = "Bebidas" },
                    new Categoria { Nombre = "Lácteos" },
                    new Categoria { Nombre = "Abarrotes" },
                    new Categoria { Nombre = "Limpieza" },
                    new Categoria { Nombre = "Higiene Personal" },
                    new Categoria { Nombre = "Panadería" },
                    new Categoria { Nombre = "Carnes" },
                    new Categoria { Nombre = "Frutas" },
                    new Categoria { Nombre = "Verduras" },
                    new Categoria { Nombre = "Snacks" }
                };
                context.Categorias.AddRange(categorias);
                context.SaveChanges();

                // Proveedores
                var proveedores = new List<Proveedor>
                {
                    new Proveedor { NombreEmpresa = "Distribuidora La Fuente", EmpleadoContacto = "Juan Pérez", Direccion = "Calle Central 123", Telefono = "7777-1111", CorreoElectronico = "contacto@lafuente.com" },
                    new Proveedor { NombreEmpresa = "Lácteos del Campo", EmpleadoContacto = "María López", Direccion = "Av. Los Ganaderos 45", Telefono = "7777-2222", CorreoElectronico = "ventas@lacteoscampo.com" },
                    new Proveedor { NombreEmpresa = "Alimentos Selectos", EmpleadoContacto = "Carlos Gómez", Direccion = "Calle Comercio 56", Telefono = "7777-3333", CorreoElectronico = "info@aliselectos.com" },
                    new Proveedor { NombreEmpresa = "Panadería El Trigal", EmpleadoContacto = "Sofía Martínez", Direccion = "Av. Panamericana 87", Telefono = "7777-4444", CorreoElectronico = "pedidos@eltrigal.com" },
                    new Proveedor { NombreEmpresa = "Carnes Premium", EmpleadoContacto = "Luis Torres", Direccion = "Calle Ganado 23", Telefono = "7777-5555", CorreoElectronico = "ventas@carnespremium.com" },
                    new Proveedor { NombreEmpresa = "Frutas y Verduras La Huerta", EmpleadoContacto = "Ana Ramírez", Direccion = "Av. Agrícola 90", Telefono = "7777-6666", CorreoElectronico = "ventas@lahuerta.com" },
                    new Proveedor { NombreEmpresa = "Limpieza Total", EmpleadoContacto = "Pedro Sánchez", Direccion = "Calle Higiene 12", Telefono = "7777-7777", CorreoElectronico = "info@limpiezatotal.com" },
                    new Proveedor { NombreEmpresa = "Higiene Hogar", EmpleadoContacto = "Gloria Castro", Direccion = "Boulevard Salud 76", Telefono = "7777-8888", CorreoElectronico = "ventas@higienehogar.com" },
                    new Proveedor { NombreEmpresa = "Snacks y Más", EmpleadoContacto = "Roberto Reyes", Direccion = "Calle Dulce 15", Telefono = "7777-9999", CorreoElectronico = "info@snacksy.com" },
                    new Proveedor { NombreEmpresa = "Agua Pura Cristal", EmpleadoContacto = "Daniel Méndez", Direccion = "Av. Agua 30", Telefono = "7888-0000", CorreoElectronico = "contacto@aguacristal.com" }
                };
                context.Proveedores.AddRange(proveedores);
                context.SaveChanges();

                // Productos
                var productos = new List<Producto>
                {
                    new Producto { Nombre = "Coca-Cola 1.5L", Descripcion = "Refresco gaseoso", CategoriaId = 1, ProveedorId = 1, Precio = 1.50M, Existencia = 100 },
                    new Producto { Nombre = "Leche Entera 1L", Descripcion = "Lácteo pasteurizado", CategoriaId = 2, ProveedorId = 2, Precio = 0.90M, Existencia = 50 },
                    new Producto { Nombre = "Arroz Blanco 5kg", Descripcion = "Abarrote básico", CategoriaId = 3, ProveedorId = 3, Precio = 5.00M, Existencia = 80 },
                    new Producto { Nombre = "Jabón en barra", Descripcion = "Producto de limpieza", CategoriaId = 4, ProveedorId = 7, Precio = 0.50M, Existencia = 200 },
                    new Producto { Nombre = "Shampoo 500ml", Descripcion = "Higiene personal", CategoriaId = 5, ProveedorId = 8, Precio = 3.25M, Existencia = 60 },
                    new Producto { Nombre = "Pan francés", Descripcion = "Panadería fresca", CategoriaId = 6, ProveedorId = 4, Precio = 0.15M, Existencia = 300 },
                    new Producto { Nombre = "Carne de res 1kg", Descripcion = "Carne premium", CategoriaId = 7, ProveedorId = 5, Precio = 7.00M, Existencia = 40 },
                    new Producto { Nombre = "Manzana roja", Descripcion = "Fruta fresca", CategoriaId = 8, ProveedorId = 6, Precio = 0.60M, Existencia = 150 },
                    new Producto { Nombre = "Tomate", Descripcion = "Verdura fresca", CategoriaId = 9, ProveedorId = 6, Precio = 0.40M, Existencia = 120 },
                    new Producto { Nombre = "Papas fritas 150g", Descripcion = "Snack salado", CategoriaId = 10, ProveedorId = 9, Precio = 1.20M, Existencia = 90 }
                };
                context.Productos.AddRange(productos);
                context.SaveChanges();
            }
        }
    }
}
