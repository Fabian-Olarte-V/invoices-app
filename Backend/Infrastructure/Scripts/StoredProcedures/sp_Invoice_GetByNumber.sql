IF OBJECT_ID('dbo.sp_Invoice_GetByNumber','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Invoice_GetByNumber;
GO

CREATE PROCEDURE dbo.sp_Invoice_GetByNumber
    @NumeroFactura INT
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
    WHERE f.NumeroFactura = @NumeroFactura;
END;
    
    