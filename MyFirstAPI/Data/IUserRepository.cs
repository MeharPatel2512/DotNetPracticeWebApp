    using System.Collections.Generic;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using MyFirstAPI.Models;

    namespace MyFirstAPI.Data
    {
        public interface IUserRepository
        {

            public Task <IEnumerable<User>> GetAllUsers();

            public Task <User> GetOneUser(int Id);

            public Task AddOneUser(String Name, String Email);

            public Task<User> DeleteOneUser(int Id);
        }
    }
