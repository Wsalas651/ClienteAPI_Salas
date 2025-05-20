-- Crear base de datos (si no existe)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BD_CLIENTES')
BEGIN
    CREATE DATABASE BD_CLIENTES;
END
GO

-- Usar la base de datos
USE BD_CLIENTES;
GO

-- Crear una tabla básica de clientes
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Clientes' AND xtype='U')
BEGIN
    CREATE TABLE Clientes (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100),
        Correo NVARCHAR(100),
        Telefono NVARCHAR(20)
    );
END
GO

-- Insertar algunos datos de prueba
INSERT INTO Clientes (Nombre, Correo, Telefono)
VALUES 
('Juan Pérez', 'juan@example.com', '999888777'),
('Ana Torres', 'ana@example.com', '987654321');
GO
