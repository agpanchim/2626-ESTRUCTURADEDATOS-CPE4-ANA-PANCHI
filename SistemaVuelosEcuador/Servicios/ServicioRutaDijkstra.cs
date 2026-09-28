using System;
using System.Collections.Generic;
using SistemaVuelosEcuador.Modelos;

namespace SistemaVuelosEcuador.Servicios
{
    public class ServicioRutaDijkstra
    {
        private readonly GrafoVuelos _grafo;

        public ServicioRutaDijkstra(GrafoVuelos grafo)
        {
            _grafo = grafo;
        }

        public ResultadoRuta CalcularRutaMasBarata(string origen, string destino)
        {
            var distancias = new Dictionary<string, decimal>();
            var anteriores = new Dictionary<string, string?>();
            var noVisitados = new HashSet<string>();

            var todos = _grafo.ObtenerTodosAeropuertos();

            foreach (var aero in todos)
            {
                distancias[aero.Codigo] = decimal.MaxValue;
                anteriores[aero.Codigo] = null;
                noVisitados.Add(aero.Codigo);
            }

            if (!distancias.ContainsKey(origen) || !distancias.ContainsKey(destino))
            {
                return new ResultadoRuta { ExisteRuta = false, CostoTotal = 0 };
            }

            distancias[origen] = 0;

            while (noVisitados.Count > 0)
            {
                string? actual = null;
                decimal menorDistancia = decimal.MaxValue;

                foreach (var codigo in noVisitados)
                {
                    if (distancias[codigo] < menorDistancia)
                    {
                        menorDistancia = distancias[codigo];
                        actual = codigo;
                    }
                }

                if (actual == null || actual == destino || distancias[actual] == decimal.MaxValue)
                {
                    break;
                }

                noVisitados.Remove(actual);

                var aeroActual = _grafo.ObtenerAeropuerto(actual);
                if (aeroActual?.VuelosSalida != null)
                {
                    foreach (var vuelo in aeroActual.VuelosSalida)
                    {
                        if (noVisitados.Contains(vuelo.Destino))
                        {
                            decimal alt = distancias[actual] + vuelo.Precio;
                            if (alt < distancias[vuelo.Destino])
                            {
                                distancias[vuelo.Destino] = alt;
                                anteriores[vuelo.Destino] = actual;
                            }
                        }
                    }
                }
            }

            if (distancias[destino] == decimal.MaxValue)
            {
                return new ResultadoRuta { ExisteRuta = false, CostoTotal = 0 };
            }

            var camino = new List<string>();
            string? paso = destino;

            while (paso != null)
            {
                camino.Insert(0, paso);
                paso = anteriores[paso];
            }

            return new ResultadoRuta
            {
                ExisteRuta = true,
                CostoTotal = distancias[destino],
                Camino = camino
            };
        }
    }
}