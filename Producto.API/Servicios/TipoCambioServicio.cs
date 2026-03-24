using Abstracciones.Modelos;
using Abstracciones.Servicios;
using Microsoft.Extensions.Configuration;

using System.Net.Http.Headers;
using System.Text.Json;

namespace Servicios
{
    public class TipoCambioServicio : ITipoCambioServicio
    {
        private readonly IConfiguration _configuracion;
        private readonly IHttpClientFactory _httpClient;

        public TipoCambioServicio(
            IConfiguration configuracion,
            IHttpClientFactory httpClient)
        {
            _configuracion = configuracion;
            _httpClient = httpClient;
        }

        public async Task<decimal> ObtenerTipoCambioVenta()
        {
           
            string endpoint = _configuracion["ApiEndPointsTipoCambio:ObtenerTipoCambioVenta"] ?? string.Empty;
            string bearerToken = _configuracion["BancoCentralToken"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new Exception("No se encontró el endpoint del tipo de cambio en appsettings.");
            }

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                throw new Exception("No se encontró el token del Banco Central en appsettings.");
            }

          
            var servicioTipoCambio = _httpClient.CreateClient("ServicioTipoCambio");

            servicioTipoCambio.DefaultRequestHeaders.Clear();
            servicioTipoCambio.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);

          
            string fechaActual = DateTime.Today.ToString("yyyy/MM/dd");
            string url = string.Format(endpoint, fechaActual, fechaActual);

            var respuesta = await servicioTipoCambio.GetAsync(url);
            respuesta.EnsureSuccessStatusCode();

            string resultado = await respuesta.Content.ReadAsStringAsync();

        
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