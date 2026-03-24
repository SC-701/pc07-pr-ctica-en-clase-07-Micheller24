using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Servicios;
using DA;
using DA.Repositorios;
using Flujo;
using Reglas;
using Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Arquitectura
builder.Services.AddScoped<IProductoFlujo, ProductoFlujo>();
builder.Services.AddScoped<IProductoDA, ProductoDA>();
builder.Services.AddScoped<IRepositorioDapper, RepositorioDapper>();

builder.Services.AddScoped<IConfiguracion, Configuracion>();
builder.Services.AddScoped<IProductoReglas, ProductoReglas>();

builder.Services.AddScoped<ITipoCambioServicio, TipoCambioServicio>();

// 🔹 HttpClient (CLAVE)
builder.Services.AddHttpClient("ServicioTipoCambio");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();