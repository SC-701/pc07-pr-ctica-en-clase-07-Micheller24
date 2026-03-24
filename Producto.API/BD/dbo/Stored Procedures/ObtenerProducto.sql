CREATE PROCEDURE dbo.ObtenerProducto
    @Id UNIQUEIDENTIFIER
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
        SubCategorias.Id,
        SubCategorias.Nombre,
        Categorias.Id,
        Categorias.Nombre
    FROM Producto 
    INNER JOIN SubCategorias  ON Producto.IdSubCategoria = SubCategorias.Id
    INNER JOIN Categorias  ON SubCategorias.IdCategoria = Categorias.Id
    WHERE Producto.Id = @Id;
END;