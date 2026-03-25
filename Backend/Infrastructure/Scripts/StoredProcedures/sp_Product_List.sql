IF OBJECT_ID('dbo.sp_Product_List','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Product_List;
GO

CREATE PROCEDURE dbo.sp_Product_List
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.NombreProducto,
        p.ImagenProducto, 
        p.PrecioUnitario,
        p.ext
    FROM dbo.CatProductos AS p
END
GO