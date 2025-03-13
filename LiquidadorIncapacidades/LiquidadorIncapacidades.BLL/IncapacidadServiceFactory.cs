using LiquidadorIncapacidades.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.BLL
{
    public static class IncapacidadServiceFactory
    {
        public static IIncapacidadService CrearServicio()
        {
            // Aquí se crea la instancia del repositorio y se inyecta en el servicio
            IIncapacidadRepositorio repository = new IncapacidadRepositorio();
            return new IncapacidadService(repository);
        }
    }
}
