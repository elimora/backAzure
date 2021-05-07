using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; 
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionApp1
{
    public class Producto : TableEntity
    {
        public string NombreProducto { get; set; }
        public string Caracteristicas { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string CorreoFabricante { get; set; }
        public string PaisFab { get; set; }
        public string PrecioEnFormatoDeMoneda { get; set; }
        public int UnidadesDisponibles { get; set; }
        public int UnidadesVendidas { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRevision { get; set; }



        public Producto()
        {
        }


    }
}
