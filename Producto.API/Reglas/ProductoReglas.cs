using Abstracciones.Interfaces.Reglas;
using Abstracciones.Servicios;

namespace Reglas
{
    public class ProductoReglas : IProductoReglas
    {
        private readonly ITipoCambioServicio _tipoCambioServicio;

        public ProductoReglas(ITipoCambioServicio tipoCambioServicio)
        {
            _tipoCambioServicio = tipoCambioServicio;
        }

        public async Task<decimal> CalcularPrecioUSD(decimal precioCRC)
        {
            var tipoCambio = await _tipoCambioServicio.ObtenerTipoCambioVenta();

            if (tipoCambio <= 0)
            {
                throw new Exception("El tipo de cambio no es válido.");
            }

            return Math.Round(precioCRC / tipoCambio, 2);
        }
    }
}