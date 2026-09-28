using System.Collections.Generic;

namespace SistemaVuelosEcuador.Modelos
{
    public class ResultadoRuta
    {
        public bool ExisteRuta { get; set; }
        public decimal CostoTotal { get; set; }
        public List<string> Camino { get; set; } = new List<string>();
    }
}