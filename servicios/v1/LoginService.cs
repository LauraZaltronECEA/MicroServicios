using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using models.DTO;
using models.Entidades;
using models.Responses;
using Newtonsoft.Json;
using servicios.Handlers;
using servicios.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace servicios.v1

{
    public class LoginService : ILoginRepository
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LoginResponse> Login(LoginDTO login)
        {
            string query = $"select * from Login where " + 
                $"Usuario = '{login.usuario}' and "+
                $"Clave = '{login.clave}'";

            string json = SqliteHandler.GetJson(query); //JSON de la BD, retorna una LISTA
            LoginResponse result = new LoginResponse();

            if (json == "[]")
            {
                result.Estado = false;
                result.Codigo = 0;
                result.Mensaje = "Credenciales Invalidas o usuario cancelado";
                result.FechaLogin = "";
                result.Token = "";
                return await Task.FromResult(result);
            }

            var userList = JsonConvert.DeserializeObject<List<Login>>(json);//En esta linea utilizamos el JSON para convertirlo a una lista de objetos del tipo Login, que es la entidad que representa la tabla Login en la BD.
            var userDb = userList?.FirstOrDefault(); //Obtenemos el primer usuario de la lista, que es el que coincide con las credenciales ingresadas.

            result.Estado= true;
            result.Codigo = 1;
            result.Mensaje = "Login Completado Satisfactoriamente";
            result.FechaLogin = DateTime.Now.ToString();
            result.Token = CrearJWT(userDb.Usuario, userDb.Id, userDb.Nombre);//Generamos el token en base a la informacion del usuario obtenido de la BD, utilizando el metodo CrearJWT que se encuentra mas abajo.
            return result;

        }//cerrado


        private string CrearJWT(string usuario, int? idUsuario, string nombre)
        {
            var jwt = _configuration.GetSection("Jwt"); //En base a la info q trae este archivo, generamos el token
            var secret = jwt["Secret"] ?? throw new InvalidOperationException("Jwt: Secret no configurado");
            var issuer = jwt["Issuer"] ?? "microservicio.login";
            var audience = jwt["Audience"] ?? "microservicio.login";
            var minutes = int.TryParse(jwt["ExpirationMinutes"], out var m) ? m : 0;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name,usuario)
            };

            if (idUsuario.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, idUsuario.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, nombre));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)); //Esta linea termina creando eltoken con todo lo de arriba
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//HMCACSHA256 es el algoritmo de encriptacion-

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
