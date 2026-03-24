using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Reglas
{
    public interface IProductoReglas
    {
        string ObtenerMetodo(string seccion, string nombreMetodo);
    }

    public class ProductoReglas : IProductoReglas
    {
        private readonly IConfiguration _configuration;

        public ProductoReglas(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObtenerMetodo(string seccion, string nombreMetodo)
        {
            string urlBase = _configuration.GetSection($"{seccion}:UrlBase").Value ?? string.Empty;

            var metodos = _configuration
                .GetSection($"{seccion}:Metodos")
                .Get<List<MetodoConfig>>() ?? new List<MetodoConfig>();

            var metodo = metodos.FirstOrDefault(x => x.Nombre == nombreMetodo);

            if (metodo == null)
                throw new Exception($"No se encontró el método {nombreMetodo} en la sección {seccion}");

            return urlBase + metodo.Valor;
        }
    }

    public class MetodoConfig
    {
        public string Nombre { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}