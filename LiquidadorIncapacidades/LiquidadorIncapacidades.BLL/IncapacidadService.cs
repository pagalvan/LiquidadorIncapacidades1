using LiquidadorIncapacidades.DAL;
using LiquidadorIncapacidades.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquidadorIncapacidades.BLL
{
    public class IncapacidadService : IIncapacidadService
    {
        private readonly IIncapacidadRepositorio _repository;
        private readonly Random _random;
        private readonly List<IObligadoPagar> _obligadosPagar;

        // Constantes
        private const decimal SALARIO_MINIMO = 1300000m;

        public IncapacidadService(IIncapacidadRepositorio repository)
        {
            _repository = repository;
            _random = new Random();

            // Inicializar los obligados a pagar
            _obligadosPagar = new List<IObligadoPagar>
            {
                new ObligadoPagarBase(ObligadoPagar.Empleador, 1, 2),
                new ObligadoPagarBase(ObligadoPagar.EPS, 3, 90),
                new FondoPensiones() // Agregamos el nuevo obligado sin modificar las clases iniciales
            };
        }

        public Incapacidad RegistrarIncapacidad(decimal salarioDevengado, int diasIncapacidad)
        {
            try
            {
                // Validar que el salario no sea menor al mínimo
                if (salarioDevengado < SALARIO_MINIMO)
                {
                    throw new Exception($"El salario no puede ser menor al salario mínimo ({SALARIO_MINIMO:C})");
                }

                // Generar número de liquidación único
                int numeroLiquidacion;
                do
                {
                    numeroLiquidacion = _random.Next(1000, 9999);
                } while (_repository.ExisteNumeroLiquidacion(numeroLiquidacion));

                // Crear la incapacidad con datos básicos
                Incapacidad incapacidad = new Incapacidad(numeroLiquidacion, salarioDevengado, diasIncapacidad);

                // Calcular la liquidación
                CalcularLiquidacion(incapacidad);

                // Guardar en el repositorio
                _repository.GuardarIncapacidad(incapacidad);

                return incapacidad;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar la incapacidad: {ex.Message}");
            }
        }

        // Método para calcular la liquidación
        private void CalcularLiquidacion(Incapacidad incapacidad)
        {
            // Validar salario mínimo
            if (incapacidad.SalarioDevengado < SALARIO_MINIMO)
            {
                incapacidad.SalarioDevengado = SALARIO_MINIMO;
            }

            // Calcular el salario diario
            incapacidad.SalarioDiario = incapacidad.SalarioDevengado / 30;

            // Calcular el valor dejado de percibir
            incapacidad.ValorDejadoPercibir = incapacidad.SalarioDiario * incapacidad.DiasIncapacidad;

            // Determinar el obligado a pagar y el porcentaje aplicado
            DeterminarObligadoPagar(incapacidad);

            // Calcular el valor de la incapacidad según el porcentaje
            incapacidad.ValorCalculadoIncapacidad = incapacidad.ValorDejadoPercibir * incapacidad.PorcentajeAplicado;

            // Calcular el valor mínimo según SMLMD
            decimal salarioMinimoDiario = SALARIO_MINIMO / 30;
            incapacidad.ValorIncapacidadSMLMD = salarioMinimoDiario * incapacidad.DiasIncapacidad;

            // Determinar el valor final a pagar
            incapacidad.ValorAPagar = Math.Max(incapacidad.ValorCalculadoIncapacidad, incapacidad.ValorIncapacidadSMLMD);
        }

        private void DeterminarObligadoPagar(Incapacidad incapacidad)
        {
            // Buscar el obligado a pagar que aplica para los días de incapacidad
            foreach (var obligado in _obligadosPagar)
            {
                if (obligado.AplicaParaDias(incapacidad.DiasIncapacidad))
                {
                    incapacidad.ObligadoPagar = obligado.Nombre;
                    incapacidad.PorcentajeAplicado = obligado.ObtenerPorcentaje(incapacidad.DiasIncapacidad);
                    return;
                }
            }

            throw new Exception("No se ha definido un obligado a pagar para el número de días especificado.");
        }

        public List<Incapacidad> ConsultarIncapacidades()
        {
            try
            {
                return _repository.ObtenerIncapacidades();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar las incapacidades: {ex.Message}");
            }
        }

        public bool EliminarIncapacidad(int numeroLiquidacion)
        {
            try
            {
                return _repository.EliminarIncapacidad(numeroLiquidacion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la incapacidad: {ex.Message}");
            }
        }
    }
}
