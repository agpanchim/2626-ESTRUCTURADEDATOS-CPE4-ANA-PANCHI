using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using SistemaVuelosEcuador.Servicios;

namespace SistemaVuelosEcuador.Utilidades
{
    public static class GeneradorGraficoGraphviz
    {
        public static void GenerarDatosCSAcademy(GrafoVuelos grafo, string rutaArchivo)
        {
            var sb = new StringBuilder();

            var aeropuertos = grafo.ObtenerTodosAeropuertos();
            foreach (var aero in aeropuertos)
            {
                if (aero.VuelosSalida != null)
                {
                    foreach (var v in aero.VuelosSalida)
                    {
                        sb.AppendLine($"{v.Origen} {v.Destino} {v.Precio:F0}");
                    }
                }
            }

            File.WriteAllText(rutaArchivo, sb.ToString());
        }

        public static void AbrirImagenGrafo(string rutaImagen)
        {
            if (File.Exists(rutaImagen))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = rutaImagen,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n [!] No se pudo abrir la imagen automáticamente: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"\n [!] No se encontró el archivo '{rutaImagen}'. Guarde la imagen descargada con ese nombre en la carpeta del proyecto.");
            }
        }
    }
}