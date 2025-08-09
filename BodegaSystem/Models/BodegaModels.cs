using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BodegaSystem.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$)[a-zA-ZÀ-ÿ0-9\s]+$", ErrorMessage = "El nombre de la categoría no puede contener solo espacios.")]
        public string Nombre { get; set; }

        public ICollection<Producto>? Productos { get; set; }
    }

    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(150)]
        [RegularExpression(@"^(?!\s+$)[a-zA-ZÀ-ÿ0-9\s]+$", ErrorMessage = "El nombre de la empresa no puede contener solo espacios.")]
        public string NombreEmpresa { get; set; }

        [Required(ErrorMessage = "El nombre del empleado de contacto es obligatorio.")]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s+$)[a-zA-ZÀ-ÿ0-9\s]+$", ErrorMessage = "El nombre del empleado no puede contener solo espacios.")]
        public string EmpleadoContacto { get; set; }

        [StringLength(250)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Formato de teléfono inválido. Ejemplo: 7777-1234")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "El teléfono debe tener el formato ####-####.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        public string CorreoElectronico { get; set; }

        public ICollection<Producto>? Productos { get; set; }
    }

    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(150)]
        [RegularExpression(@"^(?!\s+$)[a-zA-ZÀ-ÿ0-9\s]+$", ErrorMessage = "El nombre solo puede contener letras, números y espacios.")]
        public string Nombre { get; set; }

        [StringLength(300)]
        public string Descripcion { get; set; }

        [Required]
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        [Required]
        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        [Required]
        [Range(0.01, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La existencia no puede ser negativa.")]
        public int Existencia { get; set; }

        public ICollection<MovimientoInventario>? Movimientos { get; set; }
    }

    public class MovimientoInventario
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression("Entrada|Salida", ErrorMessage = "El tipo de movimiento debe ser 'Entrada' o 'Salida'.")]
        public string TipoMovimiento { get; set; }
    }
}
