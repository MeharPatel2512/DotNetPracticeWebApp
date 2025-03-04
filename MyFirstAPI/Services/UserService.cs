using Microsoft.AspNetCore.Mvc;
using MyFirstAPI.Models;
using System.Collections.Generic;
using MyFirstAPI.Data;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MyFirstAPI.Services
{
    
    public class UserService : IUserRepository
    {
        private readonly IExecuteStoredProcedure _executeStroedProcedure;

        public UserService(IExecuteStoredProcedure executeStroedProcedure)
        {
            _executeStroedProcedure = executeStroedProcedure;
        }

        public async Task<ApiResponse<IEnumerable<User>>> GetAllUsers()
        {
            try{
                var dataset = await _executeStroedProcedure.CallStoredProcedure("Practice.get_all_users", null);

                if(dataset != null && dataset.Tables[0].Rows.Count > 0 && dataset.Tables.Count > 0){
                    List<User> resultList = [];
                    DataTable dt = dataset.Tables[0];
                    foreach (DataRow row in dt.Rows){
                        resultList.Add(new User{
                            Id = row.Table.Columns.Contains("Id") ? row.Field<int>("Id") : 0,
                            Name = row.Table.Columns.Contains("Name") ? row.Field<String>("Name") : null,
                            Email = row.Table.Columns.Contains("Email") ? row.Field<String>("Email") : null,
                            Address = row.Table.Columns.Contains("Address") ? row.Field<String>("Address") : null,
                            Mobile = row.Table.Columns.Contains("Mobile") ? row.Field<long>("Mobile") : 0,
                            Is_active = row.Table.Columns.Contains("is_active")
                        });
                    }
                    return new ApiResponse<IEnumerable<User>> (resultList, "All Users Fetch Success!", "GET_ALL_USERS_SUCCESSFUL_200", false);
                }
                else{
                    return new ApiResponse<IEnumerable<User>> (null, "There does not exist any User!", "GET_ALL_USERS_NO_DATA_FOUND_204", false);
                }
            }
            catch(Exception exception){
                return new ApiResponse<IEnumerable<User>> (null, $"Error occurred while fetching user data!, {exception}", "GET_ALL_USERS_FAILED_500", true);
            }
        }

        public async Task<ApiResponse<User>> GetOneUser(int Id)
        {
            if (Id <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.");
            }
            var parameter = new Dictionary<String, Object>{
                {"@Id", Id}
            };

            try{
                var dataset = await _executeStroedProcedure.CallStoredProcedure("Practice.get_one_user", parameter);
                if(dataset != null && dataset.Tables[0].Rows.Count > 0 && dataset.Tables.Count > 0){
                    User user = new User();
                    var dt = dataset.Tables[0].Rows[0];
                        user.Id = dt.Field<int>("Id");
                        user.Name = dt.Field<String>("Name");
                        user.Email = dt.Field<String>("Email");
                        user.Address = dt.Field<String>("Address");
                        user.Mobile = dt.Field<long>("Mobile");
                        user.Is_active = dt.Field<bool>("is_active");
                    return new ApiResponse<User> (user, $"Data for User Id {Id} Fetch Success!", "GET_ONE_USER_SUCCESSFUL_200", false);
                }
                return new ApiResponse<User> (null, $"There does not exist a user with the Id {Id}", "GET_ONE_USER_NO_DATA_FOUND_204", false);
            }
            catch(Exception exception){
                return new ApiResponse<User> (null, $"Error occurred while fetching data for user Id {Id}, {exception}", "GET_ONE_USER_FAILED_500", true);
            }
        }

        public async Task<ApiResponse<object>> AddOneUser(List<User> users){
            DataTable table = new DataTable();
            table.Columns.Add("User_Id", typeof(int));
            table.Columns.Add("User_Name", typeof(string));
            table.Columns.Add("User_Email", typeof(string));
            table.Columns.Add("User_Address", typeof(string));
            table.Columns.Add("User_Mobile", typeof(long));
            table.Columns.Add("User_is_active", typeof(bool));
            foreach (var user in users)
            {
                table.Rows.Add(user.Id, user.Name, user.Email, user.Address, user.Mobile, user.Is_active);
            }
            var parameter = new Dictionary<String, Object>{
                {"@User_Data", table}
            };
            try{
                await _executeStroedProcedure.CallStoredProcedure("Practice.upsert_user", parameter);
                return new ApiResponse<object> (null, "User Upsert Successful!", "USER_UPSERT_SUCCESS_200", false);
            }
            catch(Exception exception){
                return new ApiResponse<object> (null, $"User Upsert Failed!, {exception}", "USER_UPSERT_FAILED_500", true);
            }
        }

        public async Task<ApiResponse<User>> DeleteOneUser(int Id){
            var parameter = new Dictionary<String, Object>{
                {"@id", Id}
            };

            try{
                var dataset = await _executeStroedProcedure.CallStoredProcedure("Practice.delete_one_user", parameter);

                if(dataset != null && dataset.Tables[0].Rows.Count > 0 && dataset.Tables.Count > 0){
                    User user = new User();
                    var dt = dataset.Tables[0].Rows[0];
                        user.Name = dt.Field<String>("Name");
                        user.Email = dt.Field<String>("Email");
                        user.Address = dt.Field<String>("Address");
                        user.Mobile = dt.Field<long>("Mobile");
                        user.Is_active = dt.Field<bool>("is_active");
                    return new ApiResponse<User> (user, $"User with the Id {Id} deleted successfully!", "USER_DELETE_SUCCESS_200", false);
                }
                else{
                    return new ApiResponse<User> (null, $"No User Found With the Id {Id}", "USER_DELETE_NO_DATA_FOUND_204", false);
                }
            }
            catch(Exception exception){
                return new ApiResponse<User> (null, $"Error occurred while deletion Id {Id}, {exception}", "USER_DELETE_FAILED_500", true);
            }
        }
    }
}
