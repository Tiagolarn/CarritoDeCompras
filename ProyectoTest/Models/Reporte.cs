using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoTest.Models
{
    public class Reporte
    {
        public int IdDetalleCompra { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string FechaCompra { get; set; }
    }
}