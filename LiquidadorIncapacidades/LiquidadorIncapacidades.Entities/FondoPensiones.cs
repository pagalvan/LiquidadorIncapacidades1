using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.Entities
{
    public class FondoPensiones : IObligadoPagar
    {
        public string Nombre => "Fondo de Pensiones";

        public bool AplicaParaDias(int dias)
        {
            return dias >= 91 && dias <= 540;
        }

        public decimal ObtenerPorcentaje(int dias)
        {
            return 0.50m; // 50%
        }
    }
}
