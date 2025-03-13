using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiquidadorIncapacidades.Entities;

namespace LiquidadorIncapacidades.DAL   
{
    public class IncapacidadRepositorio : IIncapacidadRepositorio
    {
        private readonly string _rutaArchivo = "incapacidades.txt";

        public void GuardarIncapacidad(Incapacidad incapacidad)
        {
            try
            {
                // Formato: NumeroLiquidacion|SalarioDevengado|DiasIncapacidad|ObligadoPagar|SalarioDiario|ValorDejadoPercibir|PorcentajeAplicado|ValorCalculadoIncapacidad|ValorIncapacidadSMLMD|ValorAPagar
                string linea = $"{incapacidad.NumeroLiquidacion}|{incapacidad.SalarioDevengado}|{incapacidad.DiasIncapacidad}|{incapacidad.ObligadoPagar}|{incapacidad.SalarioDiario}|{incapacidad.ValorDejadoPercibir}|{incapacidad.PorcentajeAplicado}|{incapacidad.ValorCalculadoIncapacidad}|{incapacidad.ValorIncapacidadSMLMD}|{incapacidad.ValorAPagar}";

                using (StreamWriter writer = new StreamWriter(_rutaArchivo, true))
                {
                    writer.WriteLine(linea);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar la incapacidad: {ex.Message}");
            }
        }

        public List<Incapacidad> ObtenerIncapacidades()
        {
            List<Incapacidad> incapacidades = new List<Incapacidad>();

            try
            {
                if (File.Exists(_rutaArchivo))
                {
                    using (StreamReader reader = new StreamReader(_rutaArchivo))
                    {
                        string linea;
                        while ((linea = reader.ReadLine()) != null)
                        {
                            string[] datos = linea.Split('|');

                            Incapacidad incapacidad = new Incapacidad
                            {
                                NumeroLiquidacion = int.Parse(datos[0]),
                                SalarioDevengado = decimal.Parse(datos[1]),
                                DiasIncapacidad = int.Parse(datos[2]),
                                ObligadoPagar = datos[3],
                                SalarioDiario = decimal.Parse(datos[4]),
                                ValorDejadoPercibir = decimal.Parse(datos[5]),
                                PorcentajeAplicado = decimal.Parse(datos[6]),
                                ValorCalculadoIncapacidad = decimal.Parse(datos[7]),
                                ValorIncapacidadSMLMD = decimal.Parse(datos[8]),
                                ValorAPagar = decimal.Parse(datos[9])
                            };

                            incapacidades.Add(incapacidad);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las incapacidades: {ex.Message}");
            }

            return incapacidades;
        }

        public bool EliminarIncapacidad(int numeroLiquidacion)
        {
            bool eliminado = false;

            try
            {
                if (File.Exists(_rutaArchivo))
                {
                    List<string> lineas = File.ReadAllLines(_rutaArchivo).ToList();
                    List<string> nuevasLineas = new List<string>();

                    foreach (string linea in lineas)
                    {
                        string[] datos = linea.Split('|');
                        int numero = int.Parse(datos[0]);

                        if (numero != numeroLiquidacion)
                        {
                            nuevasLineas.Add(linea);
                        }
                        else
                        {
                            eliminado = true;
                        }
                    }

                    File.WriteAllLines(_rutaArchivo, nuevasLineas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la incapacidad: {ex.Message}");
            }

            return eliminado;
        }

        public bool ExisteNumeroLiquidacion(int numeroLiquidacion)
        {
            try
            {
                if (File.Exists(_rutaArchivo))
                {
                    using (StreamReader reader = new StreamReader(_rutaArchivo))
                    {
                        string linea;
                        while ((linea = reader.ReadLine()) != null)
                        {
                            string[] datos = linea.Split('|');
                            int numero = int.Parse(datos[0]);

                            if (numero == numeroLiquidacion)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar el número de liquidación: {ex.Message}");
            }

            return false;
        }
    }
}

