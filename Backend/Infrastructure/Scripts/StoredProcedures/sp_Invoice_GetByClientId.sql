IF OBJECT_ID('dbo.sp_Invoice_GetByClientId','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Invoice_GetByClientId;
GO

CREATE PROCEDURE dbo.sp_Invoice_GetByClientId
    @ClientId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.Id,
        f.FechaEmisionFactura,
        f.IdCliente,
        f.NumeroFactura,
        f.NumeroTotalArticulos,
        f.TotalFactura,
        f.SubTotalFactura,
        f.TotalImpuesto
    FROM dbo.TblFacturas AS f
    WHERE f.IdCliente = @ClientId;
END;
    