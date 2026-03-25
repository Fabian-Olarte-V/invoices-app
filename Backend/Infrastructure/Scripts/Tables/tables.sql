IF OBJECT_ID('dbo.TblDetallesFactura','U') IS NOT NULL DROP TABLE dbo.TblDetallesFactura;
IF OBJECT_ID('dbo.TblFacturas','U') IS NOT NULL DROP TABLE dbo.TblFacturas;
IF OBJECT_ID('dbo.TblClientes','U') IS NOT NULL DROP TABLE dbo.TblClientes;
IF OBJECT_ID('dbo.CatProductos','U') IS NOT NULL DROP TABLE dbo.CatProductos;
IF OBJECT_ID('dbo.CatTipoCliente','U') IS NOT NULL DROP TABLE dbo.CatTipoCliente;

CREATE TABLE dbo.CatTipoCliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TipoCliente VARCHAR(50) NOT NULL
);

CREATE TABLE dbo.CatProductos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto VARCHAR(50) NOT NULL,
    ImagenProducto IMAGE NULL,
    PrecioUnitario DECIMAL(12,2) NOT NULL,
    ext VARCHAR(5) NULL
);

CREATE TABLE dbo.TblClientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial VARCHAR(200) NOT NULL,
    IdTipoCliente INT NOT NULL FOREIGN KEY REFERENCES dbo.CatTipoCliente(Id),
    FechaCreacion DATE NOT NULL,
    RFC VARCHAR(50) NOT NULL
);

CREATE TABLE dbo.TblFacturas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaEmisionFactura DATETIME NOT NULL,
    IdCliente INT NOT NULL FOREIGN KEY REFERENCES dbo.TblClientes(Id),
    NumeroFactura INT NOT NULL UNIQUE,
    NumeroTotalArticulos INT NOT NULL,
    SubTotalFactura DECIMAL(12,2) NOT NULL,
    TotalImpuesto DECIMAL(12,2) NOT NULL,
    TotalFactura DECIMAL(12,2) NOT NULL
);

CREATE TABLE dbo.TblDetallesFactura (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura INT NOT NULL FOREIGN KEY REFERENCES dbo.TblFacturas(Id),
    IdProducto INT NOT NULL FOREIGN KEY REFERENCES dbo.CatProductos(Id),
    CantidadDelProducto INT NOT NULL,
    PrecioUnitarioProducto DECIMAL(12,2) NOT NULL,
    SubtotalProducto DECIMAL(12,2) NOT NULL,
    Notas VARCHAR(200) NULL
);
