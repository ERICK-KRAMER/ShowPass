using ShowPass.Models;

namespace ShowPass.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserDTO>> GetAll();
        public Task<UserDTO> Get(Guid id);
        public Task<UserDTO> Save(UserRequest request);
        public Task<bool> SendToken(string email);
        public Task<bool> Update(UpdateUser request);

    }

}