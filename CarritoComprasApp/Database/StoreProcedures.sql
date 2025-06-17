using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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
");

            migrationBuilder.Sql(@"
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
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE usp_LoginUsuario
    @UsuarioLogin NVARCHAR(50),
    @Clave NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Usuarios
    WHERE UsuarioLogin = @UsuarioLogin AND Clave = @Clave
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE usp_TransaccionesPorUsuario
    @UsuarioId INT
AS
BEGIN
    SELECT T.Id, P.Nombre, T.Cantidad, T.Fecha
    FROM Transacciones T
    INNER JOIN Productos P ON T.ProductoId = P.Id
    WHERE T.UsuarioId = @UsuarioId
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE usp_ProductosDisponibles
AS
BEGIN
    SELECT * FROM Productos WHERE CantidadDisponible > 0
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE usp_UsuariosCompradores
AS
BEGIN
    SELECT * FROM Usuarios WHERE Rol = 'Cliente'
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE usp_ActualizarProducto
    @ProductoId INT,
    @NuevaCantidad INT
AS
BEGIN
    UPDATE Productos
    SET CantidadDisponible = @NuevaCantidad
    WHERE Id = @ProductoId
END
");

            migrationBuilder.Sql(@"
CREATE PROCEDURE usp_ObtenerHistorialTransacciones
AS
BEGIN
    SELECT 
        T.Id,
        T.UsuarioId,
        U.Nombre AS NombreUsuario,
        T.ProductoId,
        P.Nombre AS NombreProducto,
        T.Cantidad,
        T.Fecha
    FROM Transacciones T
    INNER JOIN Usuarios U ON T.UsuarioId = U.Id
    INNER JOIN Productos P ON T.ProductoId = P.Id
    ORDER BY T.Fecha DESC
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_InsertarTransaccion");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_RegistrarUsuario");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_LoginUsuario");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_TransaccionesPorUsuario");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_ProductosDisponibles");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_UsuariosCompradores");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_ActualizarProducto");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS usp_ObtenerHistorialTransacciones");
        }
    }
}
