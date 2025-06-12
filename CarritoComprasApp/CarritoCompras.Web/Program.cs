using CarritoCompras.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Agrega contexto HTTP
builder.Services.AddHttpContextAccessor();

// Configura EF Core con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura HttpClient para la API externa
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:5160");
});

// Agrega soporte para autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

// Agrega soporte para MVC y sesiones
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Construir la aplicación
var app = builder.Build();

// Ejecutar seeding desde JSON al iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();

    try
    {
        dbContext.SeedFromJsonAsync().GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error durante el seeding desde JSON: " + ex.Message);
        if (ex.InnerException != null)
        {
            Console.WriteLine("Detalle interno: " + ex.InnerException.Message);
        }
    }
}

// Configuración del middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
