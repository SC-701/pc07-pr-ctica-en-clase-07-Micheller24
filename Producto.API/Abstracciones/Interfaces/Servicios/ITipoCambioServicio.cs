namespace Abstracciones.Servicios
{
    public interface ITipoCambioServicio
    {
        Task<decimal> ObtenerTipoCambioVenta();
    }
}