using BackendPlayground.Server.Models;
using BackendPlayground.Server.Repositories;

namespace BackendPlayground.Server.Services
{
    public interface IUserService
    {
        public void AddUser(User user);
        public void DeleteUser(User user);
        public void UpdateUser(User user);
        public User GetUser(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepo) 
        {
            _repository = userRepo;
        }

        public void AddUser(User u)
        {
            _repository.Add(u);
        }

        public void DeleteUser(User u)
        {
            _repository.Delete(u);
        }

        public void UpdateUser(User u)
        {
            _repository.Update(u);
        }
        public User GetUser(int id)
        {
            var user = _repository.Get(u => u.Id == id);            
            return user;
        }
    }
}