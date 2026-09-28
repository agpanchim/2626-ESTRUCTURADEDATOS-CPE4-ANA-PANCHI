namespace SistemaVuelosEcuador.Modelos
{
    public class Vuelo
    {
        public string Origen { get; set; }
        public string Destino { get; set; }
        public decimal Precio { get; set; }

        public Vuelo(string origen, string destino, decimal precio)
        {
            Origen = origen.ToUpper();
            Destino = destino.ToUpper();
            Precio = precio;
        }
    }
}