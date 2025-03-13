using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.Entities
{
    public class Incapacidad
    {
        // Propiedades
        public int NumeroLiquidacion { get; set; }
        public decimal SalarioDevengado { get; set; }
        public int DiasIncapacidad { get; set; }
        public string ObligadoPagar { get; set; }
        public decimal SalarioDiario { get; set; }
        public decimal ValorDejadoPercibir { get; set; }
        public decimal PorcentajeAplicado { get; set; }
        public decimal ValorCalculadoIncapacidad { get; set; }
        public decimal ValorIncapacidadSMLMD { get; set; }
        public decimal ValorAPagar { get; set; }

        // Constructor
        public Incapacidad()
        {
        }

        // Constructor con parámetros básicos
        public Incapacidad(int numeroLiquidacion, decimal salarioDevengado, int diasIncapacidad)
        {
            NumeroLiquidacion = numeroLiquidacion;
            SalarioDevengado = salarioDevengado;
            DiasIncapacidad = diasIncapacidad;
        }
    }
}
