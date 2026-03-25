IF OBJECT_ID('dbo.sp_Client_List','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_Client_List;
GO

CREATE PROCEDURE dbo.sp_Client_List
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.Id,
        c.RazonSocial,
        c.IdTipoCliente,
        c.FechaCreacion,
        c.RFC
    FROM dbo.TblClientes AS c
END
GO
