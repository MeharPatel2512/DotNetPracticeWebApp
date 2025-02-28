using Microsoft.AspNetCore.Mvc;
using MyFirstAPI.Models;
using System.Collections.Generic;
using MyFirstAPI.Data;
using System.Data;

namespace MyFirstAPI.Services
{
    
    public class UserService : IUserRepository
    {
        private readonly IExecuteStoredProcedure _executeStroedProcedure;

        public UserService(IExecuteStoredProcedure executeStroedProcedure)
        {
            _executeStroedProcedure = executeStroedProcedure;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var dataset = await _executeStroedProcedure.CallStoredProcedure("dbo.get_all_users", null);

            List<User> resultList = new();
            if(dataset != null && dataset.Tables[0].Rows.Count > 0 && dataset.Tables.Count > 0){
                DataTable dt = dataset.Tables[0];
                foreach (DataRow row in dt.Rows){
                    resultList.Add(new User{
                        Id = row.Table.Columns.Contains("Id") ? row.Field<int>("Id") : 0,
                        Name = row.Table.Columns.Contains("Name") ? row.Field<String>("Name") : null,
                        Email = row.Table.Columns.Contains("Email") ? row.Field<String>("Email") : null
                    });
                }
            }
            return resultList;
        }

        public async Task<User> GetOneUser(int Id)
        {
            if (Id <= 0)
            {
                throw new ArgumentException("User ID must be greater thanj zero.");
            }
            var parameter = new Dictionary<String, Object>{
                {"@Id", Id}
            };
            var dataset = await _executeStroedProcedure.CallStoredProcedure("dbo.get_one_user", parameter);

            User user = new User();
            if(dataset != null && dataset.Tables[0].Rows.Count > 0 && dataset.Tables.Count > 0){
                var dt = dataset.Tables[0].Rows[0];
                    user.Id = dt.Field<int>("Id");
                    user.Name = dt.Field<String>("Name");
                    user.Email = dt.Field<String>("Email");
            }
            return user;
        }

        public async Task AddOneUser(String Name, String Email){
            var parameter = new Dictionary<String, Object>{
                {"@name", Name},
                {"@email", Email}
            };
            var dataset =  await _executeStroedProcedure.CallStoredProcedure("dbo.add_one_user", parameter);

        }

        public async Task<User> DeleteOneUser(int Id){
            var parameter = new Dictionary<String, Object>{
                {"@id", Id}
            };
            var dataset = await _executeStroedProcedure.CallStoredProcedure("dbo.delete_one_user", parameter);

            User user = new User();
            if(dataset != null && dataset.Tables[0].Rows.Count > 0 && dataset.Tables.Count > 0){
                var dt = dataset.Tables[0].Rows[0];
                    user.Name = dt.Field<String>("Name");
                    user.Email = dt.Field<String>("Email");
            }
            return user;
        }
    }
}
