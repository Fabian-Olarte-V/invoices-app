IF TYPE_ID('dbo.InvoiceItemTVP') IS NOT NULL
    DROP TYPE dbo.InvoiceItemTVP;


CREATE TYPE dbo.InvoiceItemTVP AS TABLE
(
    IdProducto              INT           NOT NULL,
    CantidadDelProducto     INT           NOT NULL,
    PrecioUnitarioProducto  DECIMAL(12,2) NOT NULL,
    SubtotalProducto        DECIMAL(12,2) NOT NULL,
    Notas                    VARCHAR(200)  NULL
);



IF OBJECT_ID('dbo.sp_Invoice_Create','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Invoice_Create;
GO

CREATE PROCEDURE dbo.sp_Invoice_Create
    @FechaEmisionFactura   DATETIME,
    @IdCliente            INT,
    @NumeroFactura        INT,
    @NumeroTotalArticulos INT,
    @SubTotalFactura      DECIMAL(12,2),
    @TotalImpuesto       DECIMAL(12,2),
    @TotalFactura         DECIMAL(12,2),
    @Items                dbo.InvoiceItemTVP READONLY,  
    @NewId                INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        INSERT INTO dbo.TblFacturas
        (
            FechaEmisionFactura, IdCliente, NumeroFactura,
            NumeroTotalArticulos, SubTotalFactura, TotalImpuesto, TotalFactura
        )
        VALUES
        (
            @FechaEmisionFactura, @IdCliente, @NumeroFactura,
            @NumeroTotalArticulos, @SubTotalFactura, @TotalImpuesto, @TotalFactura
        );

        SET @NewId = SCOPE_IDENTITY();
        INSERT INTO dbo.TblDetallesFactura
        (
            IdFactura, IdProducto, CantidadDelProducto,
            PrecioUnitarioProducto, SubtotalProducto, Notas
        )
        SELECT
            @NewId, i.IdProducto, i.CantidadDelProducto,
            i.PrecioUnitarioProducto, i.SubtotalProducto, i.Notas
        FROM @Items AS i;


        COMMIT TRAN;
        SELECT
            f.Id                 AS IdFactura,
            f.FechaEmisionFactura,
            f.IdCliente,
            f.NumeroFactura,
            f.NumeroTotalArticulos,
            f.SubTotalFactura,
            f.TotalImpuesto,
            f.TotalFactura
        FROM dbo.TblFacturas f
        WHERE f.Id = @NewId;
    END TRY

    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRAN;
        DECLARE @Err INT = ERROR_NUMBER(), @Msg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('sp_Invoice_Create failed (%d): %s', 16, 1, @Err, @Msg);
    END CATCH
END

