CREATE PROCEDURE usp_InsertarTransaccion
    @UsuarioId INT,
    @ProductoId INT,
    @Cantidad INT
AS
BEGIN
    DECLARE @CantidadActual INT

    SELECT @CantidadActual = CantidadDisponible 
    FROM Productos 
    WHERE Id = @ProductoId

    IF @CantidadActual >= @Cantidad
    BEGIN
        INSERT INTO Transacciones (UsuarioId, ProductoId, Cantidad, Fecha)
        VALUES (@UsuarioId, @ProductoId, @Cantidad, GETDATE())

        UPDATE Productos
        SET CantidadDisponible = CantidadDisponible - @Cantidad
        WHERE Id = @ProductoId
    END
    ELSE
    BEGIN
        RAISERROR ('Cantidad insuficiente', 16, 1)
    END
END
-- ================================
-- CREAR PROCEDIMIENTO: Registrar usuario
-- ================================
CREATE PROCEDURE usp_RegistrarUsuario
    @Nombre NVARCHAR(100),
    @Direccion NVARCHAR(150),
    @Telefono NVARCHAR(20),
    @UsuarioLogin NVARCHAR(50),
    @Identificacion NVARCHAR(50),
    @Clave NVARCHAR(255),
    @Rol NVARCHAR(20)
AS
BEGIN
    INSERT INTO Usuarios (Nombre, Direccion, Telefono, UsuarioLogin, Identificacion, Clave, Rol)
    VALUES (@Nombre, @Direccion, @Telefono, @UsuarioLogin, @Identificacion, @Clave, @Rol)
END

-- ================================
-- CREAR PROCEDIMIENTO: Login usuario
-- ================================
CREATE PROCEDURE usp_LoginUsuario
    @UsuarioLogin NVARCHAR(50),
    @Clave NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Usuarios
    WHERE UsuarioLogin = @UsuarioLogin AND Clave = @Clave
END

-- ================================
-- CREAR PROCEDIMIENTO: Insertar transacción
-- ================================
CREATE PROCEDURE usp_InsertarTransaccion
    @UsuarioId INT,
    @ProductoId INT,
    @Cantidad INT,
    @Fecha DATETIME
AS
BEGIN
    INSERT INTO Transacciones (UsuarioId, ProductoId, Cantidad, Fecha)
    VALUES (@UsuarioId, @ProductoId, @Cantidad, @Fecha)

    -- Descontar stock
    UPDATE Productos
    SET CantidadDisponible = CantidadDisponible - @Cantidad
    WHERE Id = @ProductoId
END

-- ================================
-- CREAR PROCEDIMIENTO: Obtener transacciones por usuario
-- ================================
CREATE PROCEDURE usp_TransaccionesPorUsuario
    @UsuarioId INT
AS
BEGIN
    SELECT T.Id, P.Nombre, T.Cantidad, T.Fecha
    FROM Transacciones T
    INNER JOIN Productos P ON T.ProductoId = P.Id
    WHERE T.UsuarioId = @UsuarioId
END

-- ================================
-- CREAR PROCEDIMIENTO: Obtener productos disponibles
-- ================================
CREATE PROCEDURE usp_ProductosDisponibles
AS
BEGIN
    SELECT * FROM Productos WHERE CantidadDisponible > 0
END

-- ================================
-- CREAR PROCEDIMIENTO: Obtener usuarios compradores
-- ================================
CREATE PROCEDURE usp_UsuariosCompradores
AS
BEGIN
    SELECT * FROM Usuarios WHERE Rol = 'Cliente'
END

-- ================================
-- CREAR PROCEDIMIENTO: Actualizar producto
-- ================================
CREATE PROCEDURE usp_ActualizarProducto
    @ProductoId INT,
    @NuevaCantidad INT
AS
BEGIN
    UPDATE Productos
    SET CantidadDisponible = @NuevaCantidad
    WHERE Id = @ProductoId
END

