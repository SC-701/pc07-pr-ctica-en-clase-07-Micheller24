namespace Abstracciones.Modelos.Servicios
{
    public class APIEndPoint
    {
        public string UrlBase { get; set; } = string.Empty;
        public List<Metodo> Metodos { get; set; } = new();
    }

    public class Metodo
    {
        public string Nombre { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}