using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarritoCompras.Web.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProcedimientos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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

                    IF @Cantidad <= @CantidadDisponible
                    BEGIN
                        INSERT INTO Transacciones (UsuarioId, ProductoId, Cantidad, Fecha)
                        VALUES (@Usuario, @ProductoId, @Cantidad, GETDATE())

                        UPDATE Productos
                        SET Cantidad = Cantidad - @Cantidad
                        WHERE Id = @ProductoId
                    END
                    ELSE
                    BEGIN
                        RAISERROR ('No hay suficiente stock.', 16, 1)
                    END
                END
            ");

            migrationBuilder.Sql(@"
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
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ObtenerProductosDisponibles
                AS
                BEGIN
                    SELECT *
                    FROM Productos
                    WHERE Cantidad > 0
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ObtenerUsuariosCompradores
                AS
                BEGIN
                    SELECT *
                    FROM Usuarios
                    WHERE Rol = 'Comprador'
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ActualizarProducto
                    @Id INT,
                    @Cantidad INT,
                    @Usuario NVARCHAR(100)
                AS
                BEGIN
                    UPDATE Productos
                    SET Cantidad = @Cantidad
                    WHERE Id = @Id
                END
            ");
        }

    }
}