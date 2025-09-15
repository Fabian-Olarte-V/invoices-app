IF OBJECT_ID('CatTipoCliente','U') IS NOT NULL DROP TABLE CatTipoCliente;
CREATE TABLE CatTipoCliente (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TipoCliente VARCHAR(50) NOT NULL
);


IF OBJECT_ID('CatProductos','U') IS NOT NULL DROP TABLE CatProductos;
CREATE TABLE CatProductos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto VARCHAR(50) NOT NULL,
    ImagenProducto IMAGE NULL,
    PrecioUnitario DECIMAL(12,2) NOT NULL,
    ext VARCHAR(5) NULL
);


IF OBJECT_ID('TblClientes','U') IS NOT NULL DROP TABLE TblClientes;
CREATE TABLE TblClientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial VARCHAR(200) NOT NULL,
    IdTipoCliente INT NOT NULL FOREIGN KEY REFERENCES CatTipoCliente(Id),
    FechaCreacion DATE NOT NULL,
    RFC VARCHAR(50) NOT NULL
);


IF OBJECT_ID('TblFacturas','U') IS NOT NULL DROP TABLE TblFacturas;
CREATE TABLE TblFacturas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FechaTiempoFactura DATETIME NOT NULL,
    IdCliente INT NOT NULL FOREIGN KEY REFERENCES TblClientes(Id),
    NumeroFactura INT NOT NULL UNIQUE,
    NumeroTotalArticulos INT NOT NULL,
    SubTotalFactura DECIMAL(12,2) NOT NULL,
    TotalImpuestos DECIMAL(12,2) NOT NULL,
    TotalFactura DECIMAL(12,2) NOT NULL
);


IF OBJECT_ID('TblDetallesFactura','U') IS NOT NULL DROP TABLE TblDetallesFactura;
CREATE TABLE TblDetallesFactura (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura INT NOT NULL FOREIGN KEY REFERENCES TblFacturas(Id),
    IdProducto INT NOT NULL FOREIGN KEY REFERENCES CatProductos(Id),
    CantidadDelProducto INT NOT NULL,
    PrecioUnitarioProducto DECIMAL(12,2) NOT NULL,
    SubtotalProducto DECIMAL(12,2) NOT NULL,
    Nota VARCHAR(200) NULL
);