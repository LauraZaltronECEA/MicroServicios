
using models.DTO;
using models.Entidades;
using models.Responses;

namespace servicios.Repositories
{
    public interface ILoginRepository
    {
        public Task<LoginResponse> Login(LoginDTO login); //En la task podemos manipular que sea Asincronico o no.

        public Task<bool> CreateLogin(CreateDTO create);

        public Task<bool> UpdateLogin(string estado, string id);

        public Task<bool> DeleteLogin(string id);

        public Task<bool> GetLogin();

    }
}
