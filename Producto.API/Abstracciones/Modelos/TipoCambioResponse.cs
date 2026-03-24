namespace Abstracciones.Modelos
{
    public class TipoCambioResponse
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public List<TipoCambioDato> datos { get; set; }
    }

    public class TipoCambioDato
    {
        public string titulo { get; set; }
        public string periodicidad { get; set; }
        public List<TipoCambioIndicador> indicadores { get; set; }
    }

    public class TipoCambioIndicador
    {
        public string codigoIndicador { get; set; }
        public string nombreIndicador { get; set; }
        public List<TipoCambioSerie> series { get; set; }
    }

    public class TipoCambioSerie
    {
        public DateTime fecha { get; set; }
        public decimal valorDatoPorPeriodo { get; set; }
    }
}