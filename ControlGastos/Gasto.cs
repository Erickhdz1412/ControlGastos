using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ControlGastos
{
    public class Gasto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public double Monto { get; set; }

        public string Categoria { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }
    }

    // Clase para mostrar el resumen por categoria (no se guarda en la base de datos)
    public class ResumenCategoria
    {
        public string Categoria { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public double Total { get; set; }
    }
}
