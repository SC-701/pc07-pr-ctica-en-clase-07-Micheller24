CREATE PROCEDURE dbo.AgregarProducto
    @IdSubCategoria UNIQUEIDENTIFIER,
    @Nombre         VARCHAR(MAX),
    @Descripcion    VARCHAR(MAX),
    @Precio         DECIMAL(18,2),
    @Stock          INT,
    @CodigoBarras   VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Producto
    (
        Id,
        IdSubCategoria,
        Nombre,
        Descripcion,
        Precio,
        Stock,
        CodigoBarras
    )
    VALUES
    (
        NEWID(),
        @IdSubCategoria,
        @Nombre,
        @Descripcion,
        @Precio,
        @Stock,
        @CodigoBarras
    );
END;