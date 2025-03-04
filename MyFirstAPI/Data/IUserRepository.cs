using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyFirstAPI.Models;

namespace MyFirstAPI.Data
{
    public interface IUserRepository
    {
    public Task<ApiResponse<IEnumerable<User>>> GetAllUsers();
        public Task<ApiResponse<User>> GetOneUser(int Id);
        public Task<ApiResponse<object>> AddOneUser(List<User> users);
        public Task<ApiResponse<User>> DeleteOneUser(int Id);
    }
}
