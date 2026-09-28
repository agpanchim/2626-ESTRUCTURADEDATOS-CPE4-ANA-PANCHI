using System;
using System.Collections.Generic;
using System.Linq;
using SistemaVuelosEcuador.Servicios;
using SistemaVuelosEcuador.Utilidades;

namespace SistemaVuelosEcuador
{
    class Program
    {
        private static readonly Dictionary<string, string> Ciudades = new Dictionary<string, string>
        {
            { "UIO", "Quito (Mariscal Sucre)" },
            { "GYE", "Guayaquil (José Joaquín de Olmedo)" },
            { "CUE", "Cuenca (Mariscal La Mar)" },
            { "MEC", "Manta (Eloy Alfaro)" },
            { "GPS", "Galápagos (Galápagos)" },
            { "LATA", "Latacunga (Cotopaxi)" }
        };

        static void Main(string[] args)
        {
            var grafo = new GrafoVuelos();
            LectorVuelosArchivo.CargarVuelosDesdeArchivo(grafo, "vuelos.txt");
            var servicioDijkstra = new ServicioRutaDijkstra(grafo);

            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================================================");
                Console.WriteLine("     SISTEMA DE BÚSQUEDA DE RUTAS Y VUELOS BARATOS (ALGORITMO DIJKSTRA) ");
                Console.WriteLine("=======================================================================");
                Console.ResetColor();
                Console.WriteLine(" 1. Ver lista de aeropuertos registrados en Ecuador");
                Console.WriteLine(" 2. Consultar ruta óptima de vuelo (Origen -> Destino)");
                Console.WriteLine(" 3. Visualizar imagen gráfica del Grafo (.png)");
                Console.WriteLine(" 4. Salir");
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.Write(" Seleccione una opción (1-4): ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        MostrarListaAeropuertos(grafo);
                        Pausar();
                        break;
                    case "2":
                        BuscarRutaInteractiva(grafo, servicioDijkstra);
                        Pausar();
                        break;
                    case "3":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("=======================================================================");
                        Console.WriteLine("                 VISUALIZACIÓN DEL GRAFO DE VUELOS                     ");
                        Console.WriteLine("=======================================================================");
                        Console.ResetColor();
                        Console.WriteLine("\n Abriendo la imagen del grafo...");
                        GeneradorGraficoGraphviz.AbrirImagenGrafo("grafovuelos.png");
                        Pausar();
                        break;
                    case "4":
                        salir = true;
                        Console.WriteLine("\n ¡Gracias por utilizar el sistema!");
                        break;
                    default:
                        Console.WriteLine("\n Opción no válida.");
                        Pausar();
                        break;
                }
            }
        }

        private static void Pausar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n-----------------------------------------------------------------------");
            Console.WriteLine(" Presione cualquier tecla para regresar al menú principal...");
            Console.ResetColor();
            Console.ReadKey();
        }

        private static void MostrarListaAeropuertos(GrafoVuelos grafo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=======================================================================");
            Console.WriteLine("                  AEROPUERTOS REGISTRADOS EN LA BASE                   ");
            Console.WriteLine("=======================================================================");
            Console.ResetColor();

            var aeropuertos = grafo.ObtenerTodosAeropuertos();
            int contador = 1;

            foreach (var aero in aeropuertos)
            {
                string nombreCiudad = Ciudades.ContainsKey(aero.Codigo) ? Ciudades[aero.Codigo] : aero.Codigo;
                Console.WriteLine($"  {contador}. [{aero.Codigo}] - {nombreCiudad}");
                contador++;
            }
        }

        private static void BuscarRutaInteractiva(GrafoVuelos grafo, ServicioRutaDijkstra dijkstra)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=======================================================================");
            Console.WriteLine("            CÁLCULO DE LA RUTA Y COSTO MÁS ECONÓMICO                   ");
            Console.WriteLine("=======================================================================");
            Console.ResetColor();

            var listaAeropuertos = grafo.ObtenerTodosAeropuertos();

            Console.WriteLine("\nAEROPUERTOS EN BASE DE DATOS:");
            foreach (var aero in listaAeropuertos)
            {
                string nombre = Ciudades.ContainsKey(aero.Codigo) ? Ciudades[aero.Codigo] : aero.Codigo;
                Console.WriteLine($"  * [{aero.Codigo}] {nombre}");
            }

            Console.WriteLine("-----------------------------------------------------------------------");
            Console.Write(" Ingrese el código del aeropuerto de ORIGEN (ej. LATA): ");
            string origen = Console.ReadLine()?.ToUpper().Trim() ?? "";

            var aeroOrigen = listaAeropuertos.FirstOrDefault(a => a.Codigo.Equals(origen, StringComparison.OrdinalIgnoreCase));
            if (aeroOrigen == null)
            {
                Console.WriteLine("\n [!] El aeropuerto ingresado no existe.");
                return;
            }

            Console.Write(" Ingrese el código del aeropuerto de DESTINO (ej. GPS): ");
            string destino = Console.ReadLine()?.ToUpper().Trim() ?? "";

            var aeroDestino = listaAeropuertos.FirstOrDefault(a => a.Codigo.Equals(destino, StringComparison.OrdinalIgnoreCase));
            if (aeroDestino == null)
            {
                Console.WriteLine("\n [!] El aeropuerto ingresado no existe.");
                return;
            }

            Console.WriteLine("\n Procesando algoritmo de Dijkstra...");
            var resultado = dijkstra.CalcularRutaMasBarata(origen, destino);

            Console.WriteLine("\n=======================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                     RESULTADO DE LA RUTA ÓPTIMA                       ");
            Console.ResetColor();
            Console.WriteLine("=======================================================================");

            if (resultado != null && resultado.ExisteRuta)
            {
                Console.WriteLine($" * Ciudad Origen:  [{origen}] {(Ciudades.ContainsKey(origen) ? Ciudades[origen] : "")}");
                Console.WriteLine($" * Ciudad Destino: [{destino}] {(Ciudades.ContainsKey(destino) ? Ciudades[destino] : "")}");
                Console.WriteLine($" * Escalas/Camino: {string.Join(" -> ", resultado.Camino)}");
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" * Costo Total Óptimo: ${resultado.CostoTotal:F2}");
                Console.ResetColor();

                // Muestra la imagen automáticamente al calcular la ruta
                GeneradorGraficoGraphviz.AbrirImagenGrafo("grafovuelos.png");
            }
            else
            {
                Console.WriteLine($"\n [!] No existe una ruta de vuelos conectada entre [{origen}] y [{destino}].");
            }
        }
    }
}