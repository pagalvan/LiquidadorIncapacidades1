using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.Entities
{
    public interface IObligadoPagar
    {
        string Nombre { get; }
        bool AplicaParaDias(int dias);
        decimal ObtenerPorcentaje(int dias);
    }
}
