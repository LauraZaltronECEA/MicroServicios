

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

        [HttpDelete]
        public bool DeleteLogin(string id)
        {
            return _service.DeleteLogin(id).Result;
        }

        [HttpPost("create")]
        public bool CreateLogin(CreateDTO create)
        {
            return _service.CreateLogin(create).Result;
        }

        [HttpPost("update")]
        public bool UpdateLogin(string estado, string id)
        {
            return _service.UpdateLogin(estado, id).Result;
        }

        [HttpGet]
        public string GetLogin()
        {
            return _service.GetLogin().Result;
        }
    }
}
