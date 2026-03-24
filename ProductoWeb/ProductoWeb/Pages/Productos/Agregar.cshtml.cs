using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class AgregarModel : PageModel
    {
        private readonly IProductoReglas _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public ProductoRequest Producto { get; set; } = new();

        public AgregarModel(IProductoReglas configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarProducto");

            var cliente = _httpClientFactory.CreateClient();

            var contenido = new StringContent(
                JsonSerializer.Serialize(Producto),
                Encoding.UTF8,
                "application/json");

            var respuesta = await cliente.PostAsync(endpoint, contenido);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("Index");
        }
    }
}