CREATE PROCEDURE dbo.ObtenerProductos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Producto.Id,
        Producto.Nombre,
        Producto.Descripcion,
        Producto.Precio,
        Producto.Stock,
        Producto.CodigoBarras,
        SubCategorias.Id AS IdSubCategoria,
        SubCategorias.Nombre AS SubCategoria,
        Categorias.Id AS IdCategoria,
        Categorias.Nombre AS Categoria
    FROM Producto 
    INNER JOIN SubCategorias  ON Producto.IdSubCategoria = SubCategorias.Id
    INNER JOIN Categorias  ON SubCategorias.IdCategoria = Categorias.Id;
END;