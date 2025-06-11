--1.	Insertar una transacción
CREATE PROCEDURE sp_InsertarTransaccion
    @Usuario NVARCHAR(100),
    @ProductoId INT,
    @Cantidad INT
AS
BEGIN
    DECLARE @CantidadDisponible INT

    SELECT @CantidadDisponible = Cantidad
    FROM Productos
    WHERE Id = @ProductoId

    IF @Cantidad >= @Cantidad
    BEGIN
        -- Insertar transacción
        INSERT INTO Transacciones (UsuarioId, ProductoId, Cantidad, Fecha)
        VALUES (@Usuario, @ProductoId, @Cantidad, GETDATE())

        -- Actualizar inventario
        UPDATE Productos
        SET Cantidad = Cantidad - @Cantidad
        WHERE Id = @ProductoId
    END
    ELSE
    BEGIN
        RAISERROR ('No hay suficiente stock.', 16, 1)
    END
END
GO
--2.	Consultar todas las transacciones
CREATE PROCEDURE sp_ObtenerTransacciones
AS
BEGIN
    SELECT 
        t.Id,
        t.UsuarioId,
        p.Nombre AS Producto,
        t.Cantidad,
        t.Fecha
    FROM Transacciones t
    INNER JOIN Productos p ON t.ProductoId = p.Id
END
GO
--3.	Consultar productos disponibles
CREATE PROCEDURE sp_ObtenerProductosDisponibles
AS
BEGIN
    SELECT *
    FROM Productos
    WHERE Cantidad > 0
END
GO
--4.	Consultar usuarios compradores
CREATE PROCEDURE sp_ObtenerUsuariosCompradores
AS
BEGIN
    SELECT *
    FROM Usuarios
    WHERE Rol = 'Comprador'
END
GO
--5.	Actualizar un producto (por ID)
CREATE PROCEDURE sp_ActualizarProducto
    @Id INT,
    @Cantidad INT,
    @Usuario NVARCHAR(100)
AS
BEGIN
    -- Validar que quien actualiza sea un administrador (opcional en capa lógica)
    UPDATE Productos
    SET Cantidad = @Cantidad
    WHERE Id = @Id
END
