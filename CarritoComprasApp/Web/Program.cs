using Application.Interfaces;
using Application.Services;
using Infraestructure.Data;
using Infraestructure.Interfaces;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// 1. Configurar servicios
// -------------------------

// HttpContext y HttpClient
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:5113"); // URL de tu API REST
});

// DbContext EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios y servicios de la aplicación
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";       // Página de inicio de sesión
        options.LogoutPath = "/Login/Logout";     // Página de cierre de sesión
        options.AccessDeniedPath = "/Login/AccessDenied"; // Página si no tiene permisos
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Razor + recompilación en tiempo real (útil en desarrollo)
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Configuración de sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// -------------------------
// 2. Middleware
// -------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS redirection y archivos estáticos
app.UseHttpsRedirection();
app.UseStaticFiles();

// Enrutamiento, sesión y autenticación
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// -------------------------
// 3. Ruta por defecto
// -------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
