
using models.DTO;
using models.Responses;
using System.Data;

namespace servicios.Repositories
{
    public interface ILoginRepository
    {
        public Task<LoginResponse> Login(LoginDTO login); //En la task podemos manipular que sea Asincronico o no.

        public Task<bool> CreateLogin(CreateLoginDTO create);

        public Task<bool> UpdateLogin(string estado, string id);

        public Task<bool> DeleteLogin(string id);

        public Task<string> GetLogin();

    }
}
