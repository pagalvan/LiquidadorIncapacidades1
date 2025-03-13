using LiquidadorIncapacidades.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.BLL
{
    public interface IIncapacidadService
    {
        Incapacidad RegistrarIncapacidad(decimal salarioDevengado, int diasIncapacidad);
        List<Incapacidad> ConsultarIncapacidades();
        bool EliminarIncapacidad(int numeroLiquidacion);
    }
}
