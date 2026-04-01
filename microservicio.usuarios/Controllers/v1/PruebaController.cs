using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace microservicio.usuarios.Controllers.v1
{
    [Route("api/v1/prueba")]
    [Controller]
    [Authorize]
    public class PruebaController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Prueba()
        {
            return await Task.Run(() => "ok");
        }
    }
}
