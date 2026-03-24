using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ProductoDA : IProductoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.obtenerRepositorio();
        }

        #region Operaciones

        public async Task Agregar(ProductoRequest producto)
        {
            string query = "AgregarProducto";

            await _sqlConnection.ExecuteAsync(
                query,
                new
                {
                    producto.IdSubCategoria,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    producto.CodigoBarras
                },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task Editar(Guid id, ProductoRequest producto)
        {
            await VerificarProductoExiste(id);

            string query = "EditarProducto";

            await _sqlConnection.ExecuteAsync(
                query,
                new
                {
                    Id = id,
                    producto.IdSubCategoria,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    producto.CodigoBarras
                },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task Eliminar(Guid id)
        {
            await VerificarProductoExiste(id);

            string query = "EliminarProducto";

            await _sqlConnection.ExecuteAsync(
                query,
                new { Id = id },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            string query = "ObtenerProductos";

            return await _sqlConnection.QueryAsync<ProductoResponse>(
                query,
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<ProductoDetalle?> Obtener(Guid id)
        {
            string query = "ObtenerProducto";

            var resultado = await _sqlConnection.QueryAsync<ProductoDetalle>(
                query,
                new { Id = id },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return resultado.FirstOrDefault();
        }

        Task<Guid> IProductoDA.Agregar(ProductoRequest producto)
        {
            throw new NotImplementedException();
        }

        Task<Guid> IProductoDA.Editar(Guid Id, ProductoRequest producto)
        {
            throw new NotImplementedException();
        }

        Task<Guid> IProductoDA.Eliminar(Guid Id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        private async Task VerificarProductoExiste(Guid id)
        {
            var producto = await Obtener(id);
            if (producto == null)
                throw new Exception("No se encontró el producto");
        }

        #endregion
    }
}
