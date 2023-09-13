using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Peliculas.Api.Contexts;
using Peliculas.Api.Dtos;
using Peliculas.Api.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Peliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CuentasController(UserManager<IdentityUser> userManager, 
            IConfiguration configuration, 
            SignInManager<IdentityUser> signInManager,
            AppDbContext context,
            IMapper mapper
        )
        {
            this._userManager = userManager;
            this._configuration = configuration;
            this._signInManager = signInManager;
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaAutenticacionDto>> Post([FromBody] CredencialesUsuarioDto credenciales)
        {
            var usuario = new IdentityUser
            {
                UserName = credenciales.Email,
                Email = credenciales.Email,
            };
            var resultado = await _userManager.CreateAsync(usuario, credenciales.Password);
            if (resultado.Succeeded)
            {
                return await Construir(credenciales);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        private async Task<RespuestaAutenticacionDto> Construir(CredencialesUsuarioDto credenciales)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credenciales.Email)

            };

            var usuario = await _userManager.FindByEmailAsync(credenciales.Email);
            var claimsDb = await _userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDb);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LLaveJwt"]));
            var credential = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: credential);

            return new RespuestaAutenticacionDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion,
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<RespuestaAutenticacionDto>> Login([FromBody] CredencialesUsuarioDto credenciales)
        {
            var resultado = await _signInManager.PasswordSignInAsync(credenciales.Email, credenciales.Password, isPersistent:false, lockoutOnFailure:false);

            if(resultado.Succeeded)
            {
                return await Construir(credenciales);
            }
            else
            {
                return BadRequest("Datos erroneos");
            }
        }

        [HttpPost("HacerAdministrador")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> HacerAdmin([FromBody]string usuarioId)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);
            await _userManager.AddClaimAsync(usuario, new Claim("Role", "admin"));

            return NoContent();
        }

        [HttpPost("RemoverAdministrador")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> RemoverAdmin(string usuarioId)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);
            await _userManager.RemoveClaimAsync(usuario, new Claim("Role", "admin"));

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> ObtenerUsuarios([FromQuery] PaginacionDto paginacion)
        {
            var queryable = _context.Users.AsQueryable();
            await HttpContext.InsertarParametrosDePaginacionEnCabecera(queryable);
            var usuarios = await queryable.OrderBy(x => x.Email).Paginar(paginacion).ToListAsync();

            return _mapper.Map<List<UsuarioDto>>(usuarios);
        }
    }
}
