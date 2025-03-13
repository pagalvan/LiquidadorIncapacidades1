using LiquidadorIncapacidades.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.DAL
{
    public interface IIncapacidadRepositorio
    {
        void GuardarIncapacidad(Incapacidad incapacidad);
        List<Incapacidad> ObtenerIncapacidades();
        bool EliminarIncapacidad(int numeroLiquidacion);
        bool ExisteNumeroLiquidacion(int numeroLiquidacion);
    }
}
                                                                