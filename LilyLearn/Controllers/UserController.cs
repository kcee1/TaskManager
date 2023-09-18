using TaskManagerApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
       
        private readonly ITokenAuth tokenAuth;
        private readonly IUnitOfWork unitOfWork;
        public UserController(ITokenAuth tokenAuth, IUnitOfWork unitOfWork)
        {
            
            this.tokenAuth = tokenAuth;
            this.unitOfWork = unitOfWork;
        }

       


        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginUser()
        {
            var user = new User();
            user.FirstName = "UserName";
            user.UserName = "UserEmail";
            user.LastName = "LastName";
            user.FirstName = "FirstName";
            user.Email = "User@gmail.com";
            user.Id = "UserId";

            return Ok(tokenAuth.GenerateToken(user, "Admin"));
            
        }

         [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await unitOfWork.User.Create(userDTO);
                    return Ok(result);
                }
                return BadRequest("Invalid Model state");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                    var result = await unitOfWork.User.Delete(id);
                if (result)
                    return Ok();
                return BadRequest("Unable To Delete User Account");
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
                    var result = await unitOfWork.User.GetUser(id);
                    return Ok(result);
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

         [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                    var result = await unitOfWork.User.GetUsers();
                    return Ok(result);
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Update(UserDTO userdto)
        {
            try
            {
                var result = await unitOfWork.User.Update(userdto);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Enable(bool Enable, string id)
        {
            try
            {
                var result = await unitOfWork.User.Disable(Enable, id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModelDTO forgetPasswordModelDTO)
        {
            try
            {
                var token = await unitOfWork.User.ForgetPasswordTokenGenerate(forgetPasswordModelDTO);
                var callback = Url.Action(nameof(ResetPassword), "User", new { token, email = forgetPasswordModelDTO.Email }, Request.Scheme);
                if(string.IsNullOrWhiteSpace(callback)) 
                    throw new ArgumentException("Could Not Generate CallBackUrl"); 
                var result = await unitOfWork.User.ForgetPassword(callback, forgetPasswordModelDTO);
                if (result)
                    return Ok(result);
                return BadRequest("Could Not Send Email");
            }
            catch (Exception ex)
            {

                return BadRequest("Could Not Send Email Because " + ex.Message);
            }
           
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelDTO resetPasswordModelDTO)
        {
            try
            {
                var result = await unitOfWork.User.ResetPassword(resetPasswordModelDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string id, string roleName)
        {
            try
            {
                var result = await unitOfWork.User.AddUserToRole(id, roleName);
                return Ok("Created Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }



    }
}
