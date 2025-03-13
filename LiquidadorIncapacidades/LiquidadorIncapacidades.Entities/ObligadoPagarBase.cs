using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.Entities
{
    public class ObligadoPagarBase : IObligadoPagar
    {
        private readonly ObligadoPagar _tipo;
        private readonly int _diasMinimos;
        private readonly int _diasMaximos;

        public ObligadoPagarBase(ObligadoPagar tipo, int diasMinimos, int diasMaximos)
        {
            _tipo = tipo;
            _diasMinimos = diasMinimos;
            _diasMaximos = diasMaximos;
        }

        public string Nombre => _tipo.ToString();

        public bool AplicaParaDias(int dias)
        {
            return dias >= _diasMinimos && dias <= _diasMaximos;
        }

        public decimal ObtenerPorcentaje(int dias)
        {
            if (_tipo == ObligadoPagar.Empleador)
            {
                return 0.6666m; // 66.66%
            }
            else if (_tipo == ObligadoPagar.EPS)
            {
                if (dias <= 15)
                {
                    return 0.6666m; // 66.66%
                }
                else
                {
                    return 0.60m; // 60%
                }
            }

            return 0;
        }
    }
}
