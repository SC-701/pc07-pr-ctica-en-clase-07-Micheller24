using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EditarModel : PageModel
    {
        private readonly IProductoReglas _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public ProductoRequest Producto { get; set; } = new();

        [BindProperty]
        public Guid Id { get; set; }

        public EditarModel(IProductoReglas configuracion, IHttpClientFactory httpClientFactory)
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
            var producto = JsonSerializer.Deserialize<ProductoResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (producto == null)
                return RedirectToPage("Index");

            Id = producto.Id;
            Producto = new ProductoRequest
            {
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                IdSubCategoria = producto.IdSubCategoria
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");
            endpoint = string.Format(endpoint, Id);

            var cliente = _httpClientFactory.CreateClient();

            var contenido = new StringContent(
                JsonSerializer.Serialize(Producto),
                Encoding.UTF8,
                "application/json");

            var respuesta = await cliente.PutAsync(endpoint, contenido);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("Index");
        }
    }
}