using Microsoft.Extensions.Configuration;
using models.DTO;
using models.Entidades;
using models.Responses;
using Newtonsoft.Json;
using services.Handlers;
using servicios.Handlers;
using servicios.Repositories;

namespace servicios.v1

{
    public class LoginService : ILoginRepository
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public Task<bool> GetLogin()
        {
            string query = $"select * from Login";
            return Task.FromResult(SqliteHandler.Exec(query));
        }

        public Task<bool> CreateLogin(CreateDTO create)
        {
            string query = $"insert into Login (Usuario, Clave, Nombre) values ('{create.usuario}', '{EncriptHandler.Encode(create.clave)}', '{create.nombre}')";//Clave Encriptada
            return Task.FromResult(SqliteHandler.Exec(query));
        }

        public Task<bool> DeleteLogin(string id)
        {
            string query = $"delete from Login where Id = '{id}'";
            return Task.FromResult(SqliteHandler.Exec(query));
        }

        public Task<bool> UpdateLogin(string estado, string id)
        {
            string query = $"update Login set Estado = '{estado}' where  Id = '{id}'";
            return Task.FromResult(SqliteHandler.Exec(query));
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

            query = $"update Login set FechaLogin = '{result.FechaLogin}' where Id = '{userDb.Id}'"; //Actualizamos la fecha de login del usuario en la BD.
            bool updateLogin = SqliteHandler.Exec(query); //lineas agregadas a lo ultimo de la clase
            JwtHandler jwt = new JwtHandler(_configuration);

            result.Token = jwt.CrearJWT(userDb.Usuario, userDb.Id, userDb.Nombre);//Generamos el token en base a la informacion del usuario obtenido de la BD, utilizando el metodo CrearJWT que se encuentra mas abajo.
            return result;

        }//cerrado


    }
}
