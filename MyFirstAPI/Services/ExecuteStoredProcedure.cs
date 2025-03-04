using Microsoft.AspNetCore.Mvc;
using MyFirstAPI.Models;
using System.Collections.Generic;
using MyFirstAPI.Data;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations;

namespace MyFirstAPI.Services
{
    public class ExecuteStoredProcedure : IExecuteStoredProcedure
    {
        public readonly string ConnectionString;

        public ExecuteStoredProcedure(IConfiguration configuration){
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<DataSet> CallStoredProcedure(string StoredProcedure, Dictionary<string, object> ?Parameters)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString)){
                await sqlConnection.OpenAsync();

                using(SqlCommand sqlCommand = new SqlCommand(StoredProcedure, sqlConnection)){
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if(Parameters != null){
                        foreach (var para in Parameters)
                        {
                            sqlCommand.Parameters.AddWithValue(para.Key, para.Value?? DBNull.Value);
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                    DataSet ds = new DataSet();

                    adapter.Fill(ds);

                    return ds;
                }
            }
        }
    }
}
