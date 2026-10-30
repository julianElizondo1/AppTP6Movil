
using AppTP6Movil.Models;

namespace AppTP6Movil.Models
{
    public class ItemCarrito
    {
        public Producto Producto { get; set; } = new();

        public int Cantidad { get; set; } = 1;

        public decimal Subtotal => Producto.Precio * Cantidad;
    }
}
