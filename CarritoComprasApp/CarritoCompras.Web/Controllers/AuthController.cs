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

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string query = "SELECT * FROM Usuarios WHERE UsuarioLogin = @UsuarioLogin AND Clave = @Clave";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UsuarioLogin", model.UsuarioLogin);
                cmd.Parameters.AddWithValue("@Clave", hash);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombres = reader["Nombres"].ToString(),
                        Rol = reader["Rol"].ToString(),
                        UsuarioLogin = reader["UsuarioLogin"].ToString()
                    };

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nombres),
                        new Claim(ClaimTypes.Role, usuario.Rol),
                        new Claim("UsuarioLogin", usuario.UsuarioLogin)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return usuario.Rol == "Administrador"
                        ? RedirectToAction("Administrador", "Admin")
                        : RedirectToAction("Productos", "Comprador");
                }

                ViewBag.Error = "Credenciales inválidas";
                return View(model);
            }
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

            var hash = CalcularHash(model.Clave);

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string verificar = "SELECT COUNT(*) FROM Usuarios WHERE UsuarioLogin = @UsuarioLogin";
                SqlCommand cmdVerificar = new SqlCommand(verificar, conn);
                cmdVerificar.Parameters.AddWithValue("@UsuarioLogin", model.UsuarioLogin);

                conn.Open();
                int count = (int)cmdVerificar.ExecuteScalar();

                if (count > 0)
                {
                    ViewBag.Error = "El usuario ya existe";
                    return View(model);
                }

                string insert = @"INSERT INTO Usuarios (Nombres, Direccion, Telefono, UsuarioLogin, Identificacion, Clave, Rol)
                                  VALUES (@Nombres, @Direccion, @Telefono, @UsuarioLogin, @Identificacion, @Clave, @Rol)";
                SqlCommand cmd = new SqlCommand(insert, conn);
                cmd.Parameters.AddWithValue("@Nombres", model.Nombres);
                cmd.Parameters.AddWithValue("@Direccion", model.Direccion ?? "");
                cmd.Parameters.AddWithValue("@Telefono", model.Telefono ?? "");
                cmd.Parameters.AddWithValue("@UsuarioLogin", model.UsuarioLogin);
                cmd.Parameters.AddWithValue("@Identificacion", model.Identificacion ?? "");
                cmd.Parameters.AddWithValue("@Clave", hash);
                cmd.Parameters.AddWithValue("@Rol", "Comprador");

                cmd.ExecuteNonQuery();
            }

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
    }
}
