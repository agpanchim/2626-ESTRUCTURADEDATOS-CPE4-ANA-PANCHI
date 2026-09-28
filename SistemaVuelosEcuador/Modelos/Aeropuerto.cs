using System.Collections.Generic;

namespace SistemaVuelosEcuador.Modelos
{
    public class Aeropuerto
    {
        public string Codigo { get; set; }
        public List<Vuelo> VuelosSalida { get; private set; }

        public Aeropuerto(string codigo)
        {
            Codigo = codigo.ToUpper();
            VuelosSalida = new List<Vuelo>();
        }

        public void AgregarVueloSalida(Vuelo vuelo)
        {
            VuelosSalida.Add(vuelo);
        }
    }
}