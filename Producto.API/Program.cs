var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ServicioTipoCambio", client =>
{
    client.BaseAddress = new Uri("https://api.bancocentral.example/");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

// Registrar TipoCambioServicio e IConfiguracion
builder.Services.AddTransient<ITipoCambioServicio, TipoCambioServicio>();