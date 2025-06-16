🛒 CarritoComprasApp
Sistema de carrito de compras desarrollado como prueba técnica para ADA S.A.S, usando ASP.NET Core MVC, Entity Framework Core y SQL Server.

🚀 Requisitos Técnicos
.NET 9 SDK
SQL Server
Visual Studio o VS Code
Navegador web moderno
📂 Estructura del Proyecto
CarritoComprasApp.sln │ ├── CarritoCompras.Domain/ # Entidades del sistema ├── CarritoCompras.Application/ # Interfaces y lógica de negocio ├── CarritoCompras.Infrastructure/ # EF Core y acceso a datos ├── CarritoCompras.Web/ # Aplicación web MVC ├── CarritoCompras.API/ # API REST (solo para administrador) └── CarritoCompras.Database/ # Migraciones y procedimientos almacenados

yaml Copiar Editar

🛠️ Tecnologías Utilizadas
ASP.NET Core MVC
Entity Framework Core
SQL Server
JavaScript (validaciones del lado cliente)
Arquitectura en capas (Domain, Application, Infrastructure, Web, API)
📌 Requisitos Funcionales
🧑‍💼 Login y Roles
Redirección según el rol (Administrador o Cliente).
Usuario nuevo puede registrarse con nombre, dirección, teléfono, identificación, login y clave.
Contraseña encriptada con hash SHA256 (o superior).
👨‍💻 Cliente
Puede ver productos disponibles.
Seleccionar cantidad y confirmar compra.
Valida si hay suficiente stock.
Si la cantidad solicitada excede la disponible, se pregunta si desea continuar.
🧑‍💼 Administrador
Puede consultar:
Historial de transacciones de clientes.
Lista de productos y su stock.
Lista de usuarios compradores.
Puede actualizar cantidad disponible de un producto (vía API REST).
🔐 Seguridad y Validaciones
Validación de usuario y rol en sesión.
Autenticación en controladores del administrador.
Validaciones del lado cliente con JavaScript (wwwroot/js/validaciones.js).
Validaciones del lado servidor con anotaciones [Required], [StringLength], etc.
🔗 API REST (CarritoCompras.API)
Método	Ruta	Descripción	Autenticación
GET	/api/productos	Lista todos los productos	Solo Admin
GET	/api/productos/{id}	Detalles de un producto	Solo Admin
PUT	/api/productos/{id}	Actualiza cantidad del producto	Solo Admin
GET	/api/usuarios	Lista usuarios compradores	Solo Admin
GET	/api/usuarios/{id}	Detalles de un usuario comprador	Solo Admin
🧪 Procedimientos Almacenados
Se usan procedimientos almacenados para:

Registrar transacciones
Obtener historial de compras
Consultar productos disponibles
Consultar usuarios compradores
Ver archivo: CarritoCompras.Database/StoreProcedures.sql

▶️ Pasos para Ejecutar
Clona el repositorio:
git clone https://github.com/tu-usuario/CarritoComprasApp.git
cd CarritoComprasApp
Configura la cadena de conexión en:

appsettings.json (Web y API)

Aplica las migraciones:

bash Copiar Editar dotnet ef database update --project CarritoCompras.Infrastructure Inicia el proyecto:

bash Copiar Editar dotnet run --project CarritoCompras.Web 📋 Observaciones Las validaciones JavaScript son complementarias, la lógica crítica está validada en servidor.

El sistema fue diseñado siguiendo el patrón MVC + Clean Architecture.

👨‍💻 Autor Luis Torres Desarrollador .NET | Bases de Datos | MVC | EF Core
