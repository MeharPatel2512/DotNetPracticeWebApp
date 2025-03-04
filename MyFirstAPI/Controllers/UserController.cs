using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyFirstAPI.Data;
using MyFirstAPI.Models;
using System.Collections.Generic;

namespace MyFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetAllUsers(){
            var response = await _repository.GetAllUsers();
            if(response.Code == "GET_ALL_USERS_SUCCESSFUL_200" || response.Code == "GET_ALL_USERS_NO_DATA_FOUND_204") return Ok(response);
            else return BadRequest(response);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetOneUser(int Id){
            var response = await _repository.GetOneUser(Id);
            if(response.Code == "GET_ONE_USER_SUCCESSFUL_200" || response.Code == "GET_ONE_USER_NO_DATA_FOUND_204") return Ok(response);
            else return BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<object>> AddOneUser(List<User> users){
            var response = await _repository.AddOneUser(users);
            if(response.Code == "USER_UPSERT_SUCCESS_200") return Ok(response);
            else return BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<User>>> DeleteOneUser(int Id){
            var response = await _repository.DeleteOneUser(Id);
            if(response.Code == "USER_DELETE_SUCCESS_200") return Ok(response);
            else return BadRequest(response);
        }
    }
}
