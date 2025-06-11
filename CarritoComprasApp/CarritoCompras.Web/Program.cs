using CarritoCompras.Web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agrega contexto HTTP para uso en servicios
builder.Services.AddHttpContextAccessor();

// Configura EF Core con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura HttpClient para consumo de la API
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:5160");
});

// MVC y sesiones
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// Ejecutar seeding de datos desde JSON al iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Asegura que la BD exista
    dbContext.Database.EnsureCreated();

    try
    {
        // Ejecutar el seeding
        await dbContext.SeedFromJsonAsync();
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

// Configurar el pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Rutas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
