using System.Collections.Generic;
using SistemaVuelosEcuador.Modelos;

namespace SistemaVuelosEcuador.Servicios
{
    public class GrafoVuelos
    {
        private readonly List<Aeropuerto> _aeropuertos;

        public GrafoVuelos()
        {
            _aeropuertos = new List<Aeropuerto>();
        }

        public void AgregarAeropuerto(string codigo)
        {
            if (!_aeropuertos.Exists(a => a.Codigo == codigo))
            {
                _aeropuertos.Add(new Aeropuerto(codigo));
            }
        }

        public void AgregarVuelo(string origen, string destino, decimal precio)
        {
            AgregarAeropuerto(origen);
            AgregarAeropuerto(destino);

            var aeroOrigen = _aeropuertos.Find(a => a.Codigo == origen);
            aeroOrigen?.VuelosSalida.Add(new Vuelo(origen, destino, precio));
        }

        public List<Aeropuerto> ObtenerTodosAeropuertos()
        {
            return _aeropuertos;
        }

        public Aeropuerto? ObtenerAeropuerto(string codigo)
        {
            return _aeropuertos.Find(a => a.Codigo == codigo);
        }
    }
}