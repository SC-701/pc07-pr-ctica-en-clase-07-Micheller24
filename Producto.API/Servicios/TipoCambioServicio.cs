using Abstracciones.Interfaces.Reglas;

using Abstracciones.Modelos;

using Abstracciones.Servicios;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Servicios
{
    public class TipoCambioServicio : ITipoCambioServicio
    {
        private readonly IConfiguracion _configuracion;
        private readonly System.Net.Http.IHttpClientFactory _httpClient;

        public TipoCambioServicio(
            IConfiguracion configuracion,
            System.Net.Http.IHttpClientFactory httpClient)
        {
            _configuracion = configuracion;
            _httpClient = httpClient;
        }

        public async Task<decimal> ObtenerTipoCambioVenta()
        {
            var endpoint = _configuracion.ObtenerMetodo("ApiEndPointsTipoCambio", "ObtenerTipoCambioVenta");
            var bearerToken = _configuracion.ObtenerValor("BancoCentralToken");

            var servicioTipoCambio = _httpClient.CreateClient("ServicioTipoCambio");

            servicioTipoCambio.DefaultRequestHeaders.Clear();
            servicioTipoCambio.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);

            string fechaActual = DateTime.Today.ToString("yyyy/MM/dd");
            string url = string.Format(endpoint, fechaActual, fechaActual);

            var respuesta = await servicioTipoCambio.GetAsync(url);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resultadoDeserializado = JsonSerializer.Deserialize<TipoCambioResponse>(resultado, opciones);

            var tipoCambio = resultadoDeserializado?
                .datos?
                .FirstOrDefault()?
                .indicadores?
                .FirstOrDefault()?
                .series?
                .FirstOrDefault()?
                .valorDatoPorPeriodo;

            if (tipoCambio == null || tipoCambio <= 0)
            {
                throw new Exception("No fue posible obtener el tipo de cambio de venta.");
            }

            return tipoCambio.Value;
        }
    }
}