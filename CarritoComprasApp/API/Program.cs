
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios de controllers
builder.Services.AddControllers();

// Configurar CORS para permitir solicitudes desde el frontend
builder.Services.AddScoped<UsuarioRepositorio>();

// Configurar Swagger/OpenAPI
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

var app = builder.Build();

// Mostrar detalles de errores en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Esta línea muestra los errores internos en detalle

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carrito de Compras API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Redirección HTTPS
app.UseHttpsRedirection();

// Autenticación y autorización (si lo usas en el futuro)
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Ejecutar la aplicación
app.Run();
