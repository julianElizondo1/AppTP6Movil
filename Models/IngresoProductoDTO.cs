using System;
using System.Collections.Generic;

namespace AppTP6Movil.Models
{
    public class IngresoProductoDTO
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; }

        public int? UsuarioId { get; set; }

        public UsuarioResumenDTO? Usuario { get; set; }
    }

    public class RespuestaIngresosPaginados
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRegistros { get; set; }

        public int TotalPaginas { get; set; }

        public List<IngresoProductoDTO>? Datos { get; set; }
    }
}