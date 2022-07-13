
using System.ComponentModel.DataAnnotations;

namespace LaTiendita.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Range(100, 100000, ErrorMessage = "El valor del precio esta comprendido entre 100 y 100000")]
        public double Precio { get; set; }
        public string Detalle { get; set; }
        [Required]
        public string Imagen { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public List<ProductoTalle> Talles { get; set; } = new List<ProductoTalle>();
    }
}
