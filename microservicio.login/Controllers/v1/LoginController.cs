

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

        [HttpPost("Login")]
        public async Task<LoginResponse> Login(LoginDTO login)
        {
            return await Task.Run(() => _service.Login(login));
        }

        [HttpDelete]
        public bool DeleteLogin(string id)
        {
            return _service.DeleteLogin(id).Result;
        }

        [HttpPost("Create")]
        public bool CreateLogin(CreateLoginDTO create)
        {
            return _service.CreateLogin(create).Result;
        }

        [HttpPut("Update")]
        public bool UpdateLogin(string estado, string id)
        {
            return _service.UpdateLogin(estado, id).Result;
        }

        [HttpGet]
        public string GetLogin()
        {
            return _service.GetLogin().Result.ToString();
        }
    }
}
