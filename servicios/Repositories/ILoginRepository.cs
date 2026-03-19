
using models.DTO;
using models.Responses;

namespace servicios.Repositories
{
    public interface ILoginRepository
    {
        public Task<LoginResponse> Login(LoginDTO login); //En la task podemos manipular que sea Asincronico o no.

    }
}
