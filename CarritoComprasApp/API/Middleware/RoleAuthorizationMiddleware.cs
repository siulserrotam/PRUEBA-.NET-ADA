using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace API.Middleware;
public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public RoleAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Solo proteger rutas específicas
        var path = context.Request.Path.ToString().ToLower();

        if (path.Contains("/api/productos") || path.Contains("/api/usuarios"))
        {
            var rol = context.Request.Headers["Rol"].ToString();

            if (rol != "Administrador")
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Acceso denegado. Solo administradores.");
                return;
            }
        }

        await _next(context);
    }
}
