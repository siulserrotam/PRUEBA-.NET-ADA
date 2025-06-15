using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarritoCompras.API.Migrations
{
    public partial class AgregarProcedimientos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 👉 Procedimiento: Insertar Transacción
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_InsertarTransaccion
                    @UsuarioId INT,
                    @ProductoId INT,
                    @Cantidad INT
                AS
                BEGIN
                    DECLARE @CantidadDisponible INT;

                    SELECT @CantidadDisponible = Cantidad
                    FROM Productos
                    WHERE Id = @ProductoId;

                    IF @Cantidad <= @CantidadDisponible
                    BEGIN
                        INSERT INTO Transacciones (UsuarioId, ProductoId, Cantidad, Fecha)
                        VALUES (@UsuarioId, @ProductoId, @Cantidad, GETDATE());

                        UPDATE Productos
                        SET Cantidad = Cantidad - @Cantidad
                        WHERE Id = @ProductoId;
                    END
                    ELSE
                    BEGIN
                        RAISERROR ('No hay suficiente stock.', 16, 1);
                    END
                END
            ");

            // 👉 Procedimiento: Obtener Transacciones
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ObtenerTransacciones
                AS
                BEGIN
                    SELECT 
                        t.Id,
                        u.Nombres AS Usuario,
                        p.Nombre AS Producto,
                        t.Cantidad,
                        t.Fecha
                    FROM Transacciones t
                    INNER JOIN Productos p ON t.ProductoId = p.Id
                    INNER JOIN Usuarios u ON t.UsuarioId = u.Id
                END
            ");

            // 👉 Procedimiento: Obtener Productos Disponibles
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ObtenerProductosDisponibles
                AS
                BEGIN
                    SELECT *
                    FROM Productos
                    WHERE Cantidad > 0
                END
            ");

            // 👉 Procedimiento: Obtener Usuarios Compradores
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ObtenerUsuariosCompradores
                AS
                BEGIN
                    SELECT *
                    FROM Usuarios
                    WHERE Rol = 'Comprador'
                END
            ");

            // 👉 Procedimiento: Actualizar Producto
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ActualizarProducto
                    @Id INT,
                    @Cantidad INT
                AS
                BEGIN
                    UPDATE Productos
                    SET Cantidad = @Cantidad
                    WHERE Id = @Id
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Elimina los SP al hacer un rollback
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_InsertarTransaccion;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ObtenerTransacciones;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ObtenerProductosDisponibles;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ObtenerUsuariosCompradores;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ActualizarProducto;");
        }
    }
}
