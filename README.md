Proyecto: Sistema de Carrito de Compras - ADA S.A.S

Tecnologías Utilizadas

.NET 9 (ASP.NET MVC)

Visual Studio Code

SQL Server

ADO.NET

HTML + Bootstrap 5

JavaScript (opcional para validaciones)

Arquitectura del Proyecto

El proyecto está dividido en:

dotnet ef migrations add Inicial
dotnet ef migrations add AgregarProcedimientos
Migrations/20250610123456_AgregarProcedimientos.cs

=

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

dotnet ef database update
dotnet restore
dotnet build 
dotnet watch run

1. Capa Web (ASP.NET MVC)

Controllers/

AuthController.cs: Manejo de login, logout y registro.

AdminController.cs: Funcionalidades exclusivas del administrador.

CompradorController.cs: Funcionalidades para usuarios compradores.

Models/

usuarioLogin admin ; clave = admin123
usuarioLogin juan123 ; clave = demo123

Usuario.cs: Entidad con propiedades: Id, Nombres, Direccion, Telefono, UsuarioLogin, Identificacion, Clave, Rol.

Producto.cs: Entidad con propiedades: Id, Nombre, Cantidad, Descripcion.

Transaccion.cs: Id, UsuarioId, ProductoId, Cantidad, Fecha.

Views/

Auth/

Login.cshtml

Registro.cshtml

Admin/

Administrador.cshtml

HistorialTransacciones.cshtml

Usuarios.cshtml

Productos.cshtml

ActualizarProducto.cshtml

Comprador/

Comprador.cshtml

Comprar.cshtml

_Layout.cshtml: Incluye navegación dinámica basada en roles.

2. Capa de Datos

Conexión mediante ADO.NET

Procedimientos almacenados usados para toda lógica de acceso a datos:

usp_InsertarTransaccion

usp_ObtenerTransacciones

usp_ObtenerProductosDisponibles

usp_ObtenerUsuariosCompradores

usp_ActualizarProducto

usp_LoginUsuario

Flujo del Sistema

Autenticación

Login (AuthController): Se consulta a la base de datos con la contraseña hasheada en SHA-256.

Registro: Los usuarios nuevos deben registrarse ingresando todos sus datos. Se valida existencia previa y se guarda contraseña con hash.

Al iniciar sesión:

Si el usuario tiene rol "Administrador": redirige a AdminController.Index (Administrador.cshtml)

Si es "Comprador": redirige a CompradorController.Productos (Comprador.cshtml)

Funcionalidades por Rol

Rol: Administrador

Panel con acceso a:

Historial de transacciones

Vista de usuarios compradores

Productos disponibles con opción de actualización

Acceso a API REST protegida (por rol) para consultar y modificar datos

Rol: Comprador

Visualiza productos disponibles (vista Comprador.cshtml)

Selecciona producto y cantidad a comprar

Si la cantidad supera el stock:

Informa cantidad máxima y pregunta si desea continuar con lo disponible

API REST

Protegida por autenticación y autorización (solo administradores)

Endpoints:

GET /api/productos: Lista productos disponibles

GET /api/productos/{id}: Producto por ID

PUT /api/productos/{id}: Actualiza cantidad

GET /api/usuarios: Lista usuarios compradores

GET /api/usuarios/{id}: Usuario por ID

Validaciones Implementadas

Validación en formulario de login y registro (campos requeridos)

Hash de contraseña usando SHA-256 en backend antes de consultar o guardar

Validación de rol en Layout y controladores

Validación de stock al comprar

Instrucciones para Ejecutar el Proyecto

Configura la base de datos SQL Server con los procedimientos almacenados mencionados.

Ajusta la cadena de conexión en appsettings.json o directamente en la clase de conexión.

Ejecuta el proyecto desde terminal o Visual Studio Code:

dotnet run --project CarritoCompras.Web

Accede a la ruta /auth/login para iniciar sesión o crear un nuevo usuario.

Evaluación y Cumplimiento

Requisito

Estado

Identificación de usuario por rol

Registro de nuevos usuarios con hash de clave

Acciones de administrador protegidas

Transacciones por procedimientos almacenados

API REST protegida por rol

Validación de stock al comprar

Uso de arquitectura MVC

JavaScript en validaciones (opcional, incluido básico)


Autor

Desarrollado por: Luis Guillermo Torres Mayorga Prueba Técnica ADA S.A.S - 2025
