using models.DTO;
using models.Responses;
using servicios.Handlers;
using servicios.Repositories;

namespace servicios.v1

{
    public class LoginService : ILoginRepository
    {
        public async Task<LoginResponse> Login(LoginDTO login)
        {
            string query = $"select * from Login where " + 
                $"Usuario = '{login.usuario}' and "+
                $"Clave = '{login.clave}'";

            string json = SqliteHandler.GetJson(query);
            LoginResponse result = new LoginResponse();

            if (json == "[]")
            {
                result.Estado = false;
                result.Codigo = 0;
                result.Mensaje = "Credenciales Invalidas o usuario cancelado";
            }
            else 
            { 
                result.Estado= true;
                result.Codigo = 1;
                result.Mensaje = "Login Completado Satisfactoriamente";
            }
            
            return result;
        }
    }
}
