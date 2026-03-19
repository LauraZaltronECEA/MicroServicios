

using Microsoft.AspNetCore.Mvc;
using models.DTO;
using models.Entidades;
using models.Responses;
using servicios.Repositories;

namespace microservicio.login.Controllers.v1
{

    [Route("api/login")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _service;

        public LoginController(ILoginRepository service) //Constructor
        { 
            _service = service; //Inicializo el atributo con el valor que me otorga de parametro.
        }

        [HttpPost]
        public async Task<LoginResponse> Login(LoginDTO login) 
        {
            return await Task.Run(() => _service.Login(login));
        }
    }
}
