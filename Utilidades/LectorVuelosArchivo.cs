using System;
using System.Globalization;
using System.IO;
using SistemaVuelosEcuador.Servicios;

namespace SistemaVuelosEcuador.Utilidades
{
    public static class LectorVuelosArchivo
    {
        public static void CargarVuelosDesdeArchivo(GrafoVuelos grafo, string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo)) return;

            var lineas = File.ReadAllLines(rutaArchivo);
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var partes = linea.Split(',');
                if (partes.Length == 3)
                {
                    string origen = partes[0].Trim();
                    string destino = partes[1].Trim();
                    if (decimal.TryParse(partes[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precio))
                    {
                        grafo.AgregarVuelo(origen, destino, precio);
                    }
                }
            }
        }
    }
}