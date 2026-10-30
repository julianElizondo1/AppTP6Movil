
using AppTP6Movil.Models;

namespace AppTP6Movil.Services
{
    public class CarritoService
    {
        private readonly List<ItemCarrito> items = new();

        public IReadOnlyList<ItemCarrito> Items => items;

        public int CantidadTotal => items.Sum(i => i.Cantidad);

        public decimal Total => items.Sum(i => i.Subtotal);

        public event Action? CarritoActualizado;

        public bool Agregar(Producto producto)
        {
            if (producto.Stock <= 0)
                return false;

            var existente = items.FirstOrDefault(
                i => i.Producto.Id == producto.Id);

            if (existente != null)
            {
                if (existente.Cantidad >= producto.Stock)
                    return false;

                existente.Cantidad++;
            }
            else
            {
                items.Add(new ItemCarrito
                {
                    Producto = producto,
                    Cantidad = 1
                });
            }

            NotificarCambio();
            return true;
        }

        public void CambiarCantidad(int productoId, int cantidad)
        {
            var item = items.FirstOrDefault(
                i => i.Producto.Id == productoId);

            if (item == null)
                return;

            if (cantidad <= 0)
            {
                Quitar(productoId);
                return;
            }

            item.Cantidad = Math.Min(cantidad, item.Producto.Stock);

            NotificarCambio();
        }

        public void Quitar(int productoId)
        {
            var item = items.FirstOrDefault(
                i => i.Producto.Id == productoId);

            if (item != null)
            {
                items.Remove(item);
                NotificarCambio();
            }
        }

        public void Vaciar()
        {
            items.Clear();
            NotificarCambio();
        }

        private void NotificarCambio()
        {
            CarritoActualizado?.Invoke();
        }
    }
}
