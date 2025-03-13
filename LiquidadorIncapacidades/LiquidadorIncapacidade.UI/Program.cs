using System;
using System.Collections.Generic;
using LiquidadorIncapacidades.BLL;
using LiquidadorIncapacidades.Entities;

namespace LiquidadorIncapacidade.UI
{
    class Program
    {
        private static IIncapacidadService _service;
        private static int _anchoConsola;
        private static int _altoConsola;

        static void Main(string[] args)
        {
            // Configurar la consola
            Console.Title = "Sistema de Liquidación de Incapacidades";
            _anchoConsola = Console.WindowWidth;
            _altoConsola = Console.WindowHeight;

            // Inicializar el servicio usando la fábrica
            _service = IncapacidadServiceFactory.CrearServicio();

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                DibujarTitulo("SISTEMA DE LIQUIDACIÓN DE INCAPACIDADES");

                Console.ForegroundColor = ConsoleColor.Cyan;
                CentrarTexto("1. Registrar incapacidad", _altoConsola / 2 - 3);
                CentrarTexto("2. Consultar incapacidades", _altoConsola / 2 - 2);
                CentrarTexto("3. Eliminar incapacidad", _altoConsola / 2 - 1);
                CentrarTexto("4. Demostrar Fondo de Pensiones", _altoConsola / 2);
                CentrarTexto("5. Salir", _altoConsola / 2 + 1);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto("Seleccione una opción: ", _altoConsola / 2 + 3);
                Console.ResetColor();

                // Posicionar el cursor para la entrada
                Console.SetCursorPosition(_anchoConsola / 2 + 10, _altoConsola / 2 + 3);
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RegistrarIncapacidad();
                        break;
                    case "2":
                        ConsultarIncapacidades();
                        break;
                    case "3":
                        EliminarIncapacidad();
                        break;
                    case "4":
                        DemostrarFondoPensiones();
                        break;
                    case "5":
                        salir = true;
                        break;
                    default:
                        MostrarMensaje("Opción no válida. Presione cualquier tecla para continuar...", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void RegistrarIncapacidad()
        {
            Console.Clear();
            DibujarTitulo("REGISTRAR INCAPACIDAD");

            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto("Ingrese el salario devengado: ", _altoConsola / 2 - 2);
                Console.ResetColor();
                Console.SetCursorPosition(_anchoConsola / 2 + 15, _altoConsola / 2 - 2);
                decimal salarioDevengado = decimal.Parse(Console.ReadLine());

                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto("Ingrese los días de incapacidad: ", _altoConsola / 2);
                Console.ResetColor();
                Console.SetCursorPosition(_anchoConsola / 2 + 15, _altoConsola / 2);
                int diasIncapacidad = int.Parse(Console.ReadLine());

                Incapacidad incapacidad = _service.RegistrarIncapacidad(salarioDevengado, diasIncapacidad);

                Console.ForegroundColor = ConsoleColor.Green;
                CentrarTexto("Incapacidad registrada exitosamente:", _altoConsola / 2 + 2);
                CentrarTexto($"Número de liquidación: {incapacidad.NumeroLiquidacion}", _altoConsola / 2 + 3);
                CentrarTexto($"Obligado a pagar: {incapacidad.ObligadoPagar}", _altoConsola / 2 + 4);
                CentrarTexto($"Valor a pagar: {incapacidad.ValorAPagar:C}", _altoConsola / 2 + 5);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", ConsoleColor.Red);
            }

            MostrarMensaje("\nPresione cualquier tecla para continuar...", ConsoleColor.Yellow, _altoConsola / 2 + 7);
            Console.ReadKey();
        }

        static void ConsultarIncapacidades()
        {
            Console.Clear();
            DibujarTitulo("CONSULTAR INCAPACIDADES");

            try
            {
                List<Incapacidad> incapacidades = _service.ConsultarIncapacidades();

                if (incapacidades.Count == 0)
                {
                    MostrarMensaje("No hay incapacidades registradas.", ConsoleColor.Yellow);
                }
                else
                {
                    // Dibujar encabezado de la tabla
                    int filaInicial = 5;
                    DibujarEncabezadoTabla(filaInicial);

                    // Dibujar filas de datos
                    int fila = filaInicial + 2;
                    foreach (var inc in incapacidades)
                    {
                        DibujarFilaTabla(inc, fila);
                        fila += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", ConsoleColor.Red);
            }

            MostrarMensaje("\nPresione cualquier tecla para continuar...", ConsoleColor.Yellow, _altoConsola - 3);
            Console.ReadKey();
        }

        static void EliminarIncapacidad()
        {
            Console.Clear();
            DibujarTitulo("ELIMINAR INCAPACIDAD");

            try
            {
                // Mostrar las incapacidades antes de eliminar
                Console.ForegroundColor = ConsoleColor.Cyan;
                CentrarTexto("Incapacidades antes de eliminar:", 3);
                Console.ResetColor();

                List<Incapacidad> incapacidadesAntes = _service.ConsultarIncapacidades();
                MostrarIncapacidadesResumidas(incapacidadesAntes, 5);

                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto("\nIngrese el número de liquidación a eliminar: ", _altoConsola / 2);
                Console.ResetColor();
                Console.SetCursorPosition(_anchoConsola / 2 + 20, _altoConsola / 2);
                int numeroLiquidacion = int.Parse(Console.ReadLine());

                bool eliminado = _service.EliminarIncapacidad(numeroLiquidacion);

                if (eliminado)
                {
                    MostrarMensaje($"\nIncapacidad con número {numeroLiquidacion} eliminada exitosamente.", ConsoleColor.Green, _altoConsola / 2 + 2);
                }
                else
                {
                    MostrarMensaje($"\nNo se encontró una incapacidad con el número {numeroLiquidacion}.", ConsoleColor.Red, _altoConsola / 2 + 2);
                }

                // Mostrar las incapacidades después de eliminar
                Console.ForegroundColor = ConsoleColor.Cyan;
                CentrarTexto("\nIncapacidades después de eliminar:", _altoConsola / 2 + 4);
                Console.ResetColor();

                List<Incapacidad> incapacidadesDespues = _service.ConsultarIncapacidades();
                MostrarIncapacidadesResumidas(incapacidadesDespues, _altoConsola / 2 + 6);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", ConsoleColor.Red);
            }

            MostrarMensaje("\nPresione cualquier tecla para continuar...", ConsoleColor.Yellow, _altoConsola - 3);
            Console.ReadKey();
        }

        // Método para demostrar el punto 6: Fondo de Pensiones
        static void DemostrarFondoPensiones()
        {
            Console.Clear();
            DibujarTitulo("DEMOSTRACIÓN FONDO DE PENSIONES (PUNTO 6)");

            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                CentrarTexto("FONDO DE PENSIONES", 5);
                CentrarTexto("", 6);
                CentrarTexto("", 7);
                Console.ResetColor();

                // Solicitar datos para una incapacidad que aplique al Fondo de Pensiones
                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto("Ingrese el salario devengado: ", 10);
                Console.ResetColor();
                Console.SetCursorPosition(_anchoConsola / 2 + 15, 10);
                decimal salarioDevengado = decimal.Parse(Console.ReadLine());

                // Sugerir un valor entre 91 y 540 días
                Console.ForegroundColor = ConsoleColor.Yellow;
                CentrarTexto("Ingrese los días de incapacidad (91-540): ", 12);
                Console.ResetColor();
                Console.SetCursorPosition(_anchoConsola / 2 + 20, 12);
                int diasIncapacidad = int.Parse(Console.ReadLine());

                // Validar que los días estén en el rango del Fondo de Pensiones
                if (diasIncapacidad < 91 || diasIncapacidad > 540)
                {
                    MostrarMensaje("Los días deben estar entre 91 y 540 para aplicar al Fondo de Pensiones.", ConsoleColor.Red, 14);
                    MostrarMensaje("\nPresione cualquier tecla para continuar...", ConsoleColor.Yellow, 16);
                    Console.ReadKey();
                    return;
                }

                // Registrar la incapacidad
                Incapacidad incapacidad = _service.RegistrarIncapacidad(salarioDevengado, diasIncapacidad);

                // Verificar que el obligado a pagar sea el Fondo de Pensiones
                if (incapacidad.ObligadoPagar != "Fondo de Pensiones")
                {
                    MostrarMensaje("Error: El obligado a pagar no es el Fondo de Pensiones.", ConsoleColor.Red, 14);
                }
                else
                {
                    // Mostrar los detalles de la incapacidad
                    Console.ForegroundColor = ConsoleColor.Green;
                    CentrarTexto("¡Demostración exitosa!", 14);
                    CentrarTexto("Se ha registrado una incapacidad con el Fondo de Pensiones como obligado a pagar:", 15);
                    Console.ResetColor();

                    // Dibujar una tabla con los detalles
                    DibujarEncabezadoTabla(17);
                    DibujarFilaTabla(incapacidad, 19);

                    // Mostrar el porcentaje aplicado
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    CentrarTexto($"Porcentaje aplicado: {incapacidad.PorcentajeAplicado:P0} (50%)", 21);
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", ConsoleColor.Red);
            }

            MostrarMensaje("\nPresione cualquier tecla para continuar...", ConsoleColor.Yellow, _altoConsola - 3);
            Console.ReadKey();
        }

        // Métodos auxiliares para la presentación
        static void DibujarTitulo(string titulo)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            // Dibujar barra superior
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', _anchoConsola));

            // Dibujar título centrado
            CentrarTexto(titulo, 0);

            // Dibujar barra inferior
            Console.SetCursorPosition(0, 1);
            Console.Write(new string(' ', _anchoConsola));

            Console.ResetColor();
            Console.WriteLine();
        }

        static void CentrarTexto(string texto, int fila)
        {
            int posicion = (_anchoConsola - texto.Length) / 2;
            if (posicion < 0) posicion = 0; // Asegurarse de que la posición no sea negativa
            Console.SetCursorPosition(posicion, fila);
            Console.Write(texto);
        }

        static void MostrarMensaje(string mensaje, ConsoleColor color, int fila = -1)
        {
            Console.ForegroundColor = color;
            if (fila == -1)
            {
                fila = Console.CursorTop;
            }
            CentrarTexto(mensaje, fila);
            Console.ResetColor();
        }

        static void DibujarEncabezadoTabla(int fila)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;

            CentrarTexto("| N° Liq. | Salario Dev. | Días | Obligado | Salario Diario | Valor Dejado | % Aplicado | Valor Calculado | Valor SMLMD | Valor a Pagar |", fila);

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;

            CentrarTexto("|---------|--------------|------|----------|----------------|--------------|------------|-----------------|-------------|---------------|", fila + 1);

            Console.ResetColor();
        }

        static void DibujarFilaTabla(Incapacidad inc, int fila)
        {
            // Alternar colores para mejorar legibilidad
            if (fila % 2 == 0)
                Console.ForegroundColor = ConsoleColor.Cyan;
            else
                Console.ForegroundColor = ConsoleColor.White;

            string fila_texto = $"| {inc.NumeroLiquidacion,7} | {inc.SalarioDevengado,12:C} | {inc.DiasIncapacidad,4} | {inc.ObligadoPagar,-8} | {inc.SalarioDiario,14:C} | {inc.ValorDejadoPercibir,12:C} | {inc.PorcentajeAplicado,10:P0} | {inc.ValorCalculadoIncapacidad,15:C} | {inc.ValorIncapacidadSMLMD,11:C} | {inc.ValorAPagar,13:C} |";
            CentrarTexto(fila_texto, fila);

            Console.ResetColor();
        }

        static void MostrarIncapacidadesResumidas(List<Incapacidad> incapacidades, int filaInicial)
        {
            if (incapacidades.Count == 0)
            {
                MostrarMensaje("No hay incapacidades registradas.", ConsoleColor.Yellow, filaInicial);
                return;
            }

            // Encabezado de tabla resumida
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            CentrarTexto("| N° Liq. | Salario Dev. | Días | Obligado | Valor a Pagar |", filaInicial);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            CentrarTexto("|---------|--------------|------|----------|---------------|", filaInicial + 1);
            Console.ResetColor();

            // Filas de datos
            int fila = filaInicial + 2;
            foreach (var inc in incapacidades)
            {
                if (fila % 2 == 0)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                string fila_texto = $"| {inc.NumeroLiquidacion,7} | {inc.SalarioDevengado,12:C} | {inc.DiasIncapacidad,4} | {inc.ObligadoPagar,-8} | {inc.ValorAPagar,13:C} |";
                CentrarTexto(fila_texto, fila);
                Console.ResetColor();

                fila++;
            }
        }
    }
}

