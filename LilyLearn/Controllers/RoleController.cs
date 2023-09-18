using TaskManagerApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {

        
        private readonly IUnitOfWork unitOfWork;
        public RoleController(IUnitOfWork unitOfWork)
        {

            
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            try
            {
                var roles = unitOfWork.Role.GetRoles(out string message);
                if (roles.Count <= 0)
                {
                    return Ok(message);
                }
                return Ok(roles);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var roles = await unitOfWork.Role.GetRole(id);
                return Ok(roles);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(string name)
        {
            try
            {
                var result = await unitOfWork.Role.CreateRole(name);
                return Ok("Created Successfully");
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                var result = await unitOfWork.Role.DeleteRole(name);
                return Ok("Deleted Successfully");
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }

         [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Update(string name)
        {
            try
            {
                var result = await unitOfWork.Role.DeleteRole(name);
                return Ok(result);
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }


    }
}






