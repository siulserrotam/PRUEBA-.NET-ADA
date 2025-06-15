using Infraestructure.Data;
using Infraestructure.Repositories;
using Infraestructure.Interfaces;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Inyectar dependencias (repositorios y servicios)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// 3. Agregar servicios de controladores
builder.Services.AddControllers();

// 4. Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Carrito de Compras API",
        Version = "v1",
        Description = "API REST para gestión de productos y usuarios - ADA S.A.S"
    });
});

// 5. Construir la aplicación
var app = builder.Build();

// 7. Middleware personalizado para autorización por rol
app.UseMiddleware<RoleAuthorizationMiddleware>();

// 8. Configurar entorno
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carrito de Compras API v1");
        c.RoutePrefix = string.Empty;
    });
}

// 9. Middleware ASP.NET
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// 10. Mapeo de controladores
app.MapControllers();

// 11. Ejecutar la aplicación
app.Run();
