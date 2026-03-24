using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IProductoReglas _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ProductoResponse> Productos { get; set; } = new();

        public IndexModel(IProductoReglas configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos");

            var cliente = _httpClientFactory.CreateClient();
            var respuesta = await cliente.GetAsync(endpoint);
            respuesta.EnsureSuccessStatusCode();

            var json = await respuesta.Content.ReadAsStringAsync();

            Productos = JsonSerializer.Deserialize<List<ProductoResponse>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }
}