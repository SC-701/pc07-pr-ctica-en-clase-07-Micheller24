using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class DetalleModel : PageModel
    {
        private readonly IProductoReglas _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductoResponse Producto { get; set; } = new();

        public DetalleModel(IProductoReglas configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            endpoint = string.Format(endpoint, id);

            var cliente = _httpClientFactory.CreateClient();
            var respuesta = await cliente.GetAsync(endpoint);

            if (!respuesta.IsSuccessStatusCode)
                return RedirectToPage("Index");

            var json = await respuesta.Content.ReadAsStringAsync();

            Producto = JsonSerializer.Deserialize<ProductoResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();

            return Page();
        }
    }
}