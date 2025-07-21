using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using Backend.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IConfiguration _config;


    public AuthController(IUsuarioService usuarioService, IConfiguration config)
    {
        _usuarioService = usuarioService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _usuarioService.GetByCorreoAsync(request.Correo);
        if (usuario == null || !_usuarioService.VerificarContrasena(request.Contrasena, usuario.ContrasenaHash))
        {
            return Unauthorized("Credenciales inválidas");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim("UserId", usuario.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "clave_super_secreta"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}
