using Microsoft.AspNetCore.Mvc;
using CarritoCompras.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CarritoCompras.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario model)
        {
            if (string.IsNullOrEmpty(model.UsuarioLogin) || string.IsNullOrEmpty(model.Clave))
            {
                ViewBag.Error = "Debe ingresar usuario y clave";
                return View(model);
            }

            var hash = CalcularHash(model.Clave);
            var usuario = ObtenerUsuarioDesdeBD(model.UsuarioLogin, hash);

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales inválidas";
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombres),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("UsuarioLogin", usuario.UsuarioLogin)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (usuario.Rol == "Administrador")
                return RedirectToAction("Administrador", "Admin");
            else
                return RedirectToAction("Productos", "Comprador");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult Registro(Usuario model)
        {
            if (string.IsNullOrEmpty(model.UsuarioLogin) || string.IsNullOrEmpty(model.Clave))
            {
                ViewBag.Error = "Todos los campos son obligatorios";
                return View(model);
            }

            if (VerificarUsuarioExiste(model.UsuarioLogin))
            {
                ViewBag.Error = "El usuario ya existe";
                return View(model);
            }

            model.Clave = CalcularHash(model.Clave);
            model.Rol = "Comprador";

            GuardarUsuarioEnBD(model);

            TempData["Mensaje"] = "Usuario registrado correctamente";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private string CalcularHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private Usuario? ObtenerUsuarioDesdeBD(string usuarioLogin, string claveHash)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new SqlConnection(connectionString);
            var query = @"SELECT TOP 1 * FROM Usuarios 
                          WHERE UsuarioLogin = @UsuarioLogin AND Clave = @Clave";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UsuarioLogin", usuarioLogin);
            command.Parameters.AddWithValue("@Clave", claveHash);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = (int)reader["Id"],
                    Nombres = reader["Nombres"].ToString(),
                    Direccion = reader["Direccion"].ToString(),
                    Telefono = reader["Telefono"].ToString(),
                    UsuarioLogin = reader["UsuarioLogin"].ToString(),
                    Identificacion = reader["Identificacion"].ToString(),
                    Clave = reader["Clave"].ToString(),
                    Rol = reader["Rol"].ToString()
                };
            }

            return null;
        }

        private bool VerificarUsuarioExiste(string usuarioLogin)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new SqlConnection(connectionString);
            var query = @"SELECT COUNT(*) FROM Usuarios WHERE UsuarioLogin = @UsuarioLogin";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UsuarioLogin", usuarioLogin);
            connection.Open();

            int count = (int)command.ExecuteScalar();
            return count > 0;
        }

        private void GuardarUsuarioEnBD(Usuario model)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var connection = new SqlConnection(connectionString);
            var query = @"
                INSERT INTO Usuarios 
                (Nombres, Direccion, Telefono, UsuarioLogin, Identificacion, Clave, Rol)
                VALUES 
                (@Nombres, @Direccion, @Telefono, @UsuarioLogin, @Identificacion, @Clave, @Rol)";
            
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Nombres", model.Nombres ?? "Usuario");
            command.Parameters.AddWithValue("@Direccion", model.Direccion ?? "");
            command.Parameters.AddWithValue("@Telefono", model.Telefono ?? "");
            command.Parameters.AddWithValue("@UsuarioLogin", model.UsuarioLogin);
            command.Parameters.AddWithValue("@Identificacion", model.Identificacion ?? "");
            command.Parameters.AddWithValue("@Clave", model.Clave);
            command.Parameters.AddWithValue("@Rol", model.Rol);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
