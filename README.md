
# 🛒 CarritoComprasApp

Este es un sistema de carrito de compras desarrollado como prueba técnica para ADA S.A.S, basado en la arquitectura por capas y utilizando tecnologías como ASP.NET MVC, Web API REST, Entity Framework Core y procedimientos almacenados en SQL Server.

## 📌 Descripción General

La aplicación permite a los usuarios autenticarse, registrarse si son nuevos, y realizar compras de productos disponibles. Según el tipo de usuario (Administrador o Comprador), se redirige a distintas interfaces con funcionalidades específicas.

---

## 🧪 Requisitos de la Prueba

**Punto 1: Desarrollo del Aplicativo**

- Sistema de login que redirige a interfaces distintas según el perfil:
  - **Administrador**:
    - Visualiza las transacciones realizadas por los compradores.
    - Consulta productos y usuarios compradores.
    - Actualiza el stock de productos.
  - **Usuario Comprador**:
    - Visualiza productos disponibles.
    - Puede comprar productos únicamente si hay suficiente stock.
    - En caso de stock insuficiente, se ofrece la opción de comprar la cantidad disponible.

- En caso de ser un usuario nuevo:
  - Se redirige al formulario de registro con campos: nombres, dirección, teléfono, usuario, identificación y contraseña.

- Características de los productos:
  - Nombre, cantidad disponible y descripción.

**Punto 2: API REST**

- Servicios disponibles:
  - **GET /api/productos**: Lista todos los productos disponibles.
  - **GET /api/usuarios**: Lista los usuarios compradores.
  - **PUT /api/productos/{id}**: Actualiza la cantidad de un producto (solo administradores).

- Acceso restringido por rol a las operaciones críticas.
- Toda transacción es gestionada por procedimientos almacenados en SQL Server.
- Validaciones recomendadas con JavaScript en el cliente.

---

## 🧱 Estructura del Proyecto

```plaintext
CarritoComprasApp.sln
│
├── Domain/
│   └── Models/
│       ├── Usuario.cs
│       ├── Producto.cs
│       └── Transaccion.cs
│
├── Application/
│   ├── Interfaces/
│   └── Services/
│       ├── UsuarioService.cs
│       ├── ProductoService.cs
│       └── TransaccionService.cs
│
├── Infraestructure/
│   ├── Data/
│   ├── Interfaces/
│   └── Repositories/
│
├── Web/
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/
│   ├── Program.cs
│   └── appsettings.json
│
├── API/
│   ├── Controllers/
│   ├── Middlewares/
│   ├── Program.cs
│   └── appsettings.json
│
└── Database/
    └── StoreProcedures/
```

---

## ⚙️ Características Técnicas

- **Backend**: ASP.NET Core 9 (MVC y Web API)
- **Frontend**: Razor Views (.cshtml) + JavaScript
- **Base de Datos**: SQL Server con procedimientos almacenados
- **ORM**: Entity Framework Core
- **Autenticación y Autorización**: Gestión básica de sesión + middleware personalizado para validación de roles
- **Validaciones**: Lógicas del lado servidor y cliente (JS)
- **Patrón de Arquitectura**: Capas separadas (Domain, Application, Infrastructure, Web, API)

---

## ▶️ Funcionalidades Clave

### Autenticación y Registro
- Inicio de sesión por rol
- Registro de nuevos usuarios con validación

### Usuario Administrador
- Visualización de transacciones
- Gestión de productos (actualización de stock)
- Consulta de usuarios compradores

### Usuario Comprador
- Visualización de productos disponibles
- Proceso de compra con validación de stock y confirmación de cantidades

### API REST
- Consumo de servicios seguros por rol
- Integración de API con capa MVC por `HttpClient`

---

## 📂 Procedimientos Almacenados

Los procedimientos almacenados se encuentran en el directorio `Database/StoreProcedures` y permiten:
- Registrar transacciones
- Consultar historial
- Obtener productos disponibles
- Actualizar stock
- Listar usuarios compradores

---

## 🚀 Cómo Ejecutar

1. Clonar el repositorio y abrir la solución en Visual Studio Code.
2. Configurar la conexión a SQL Server en `appsettings.json`.
3. Ejecutar las migraciones con `dotnet ef database update`.
4. Ejecutar los proyectos `Web` y `API` desde consola:
   ```bash
   dotnet run --project CarritoCompras.Web
   dotnet run --project CarritoCompras.API
   ```

---

## 📫 Contacto

Proyecto desarrollado como parte de una prueba técnica para **ADA S.A.S**.  
Para más información:  
📧 info@ada.co  
🌐 [www.ada.co](http://www.ada.co)

---

## ✅ Evaluación

Se cumplen los criterios solicitados por ADA S.A.S:

- [x] Identificación de roles (Administrador / Comprador)
- [x] Registro de nuevos usuarios
- [x] Transacciones con procedimientos almacenados
- [x] Arquitectura en capas (MVC)
- [x] Consumo de servicios web restringidos por rol
- [x] Validaciones del lado cliente (JS)

---
