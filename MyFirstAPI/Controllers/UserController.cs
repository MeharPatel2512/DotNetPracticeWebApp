using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers(){
            var data = await _repository.GetAllUsers();
            if(data == null){
                return NoContent();
            }
            return Ok(data);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<User>> GetOneUser(int Id){
            var data = await _repository.GetOneUser(Id);
            if(data == null){
                return NoContent();
            }
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> AddOneUser(String Name, String Email){
            try
            {
                await _repository.AddOneUser(Name, Email);
                return Ok();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<ActionResult<User>> DeleteOneUser(int Id){
            try
            {
                var data = await _repository.DeleteOneUser(Id);
                return Ok(data);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
