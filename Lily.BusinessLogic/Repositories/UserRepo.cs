using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.EmailService.Interfaces;
using ServiceLibrary.EmailService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;
using TaskManger.BusinessLogic.IRepositries;

namespace TaskManager.BusinessLogic.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserRepo(UserManager<User> userManager, IMapper mapper, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<UserDetailsDTO> Create(UserDTO userdto)
        {
            Guid obj = Guid.NewGuid();
            var user = _mapper.Map<User>(userdto);
                user.UserName = user.Email;
                user.Id = obj.ToString();
                
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (!result.Succeeded)
                {
                    List<IdentityError> errorList = result.Errors.ToList();
                    var errors = string.Join(", ", errorList.Select(e => e.Description));
                    throw new ArgumentException(errors);
                }
               
            return _mapper.Map<UserDetailsDTO>(user);
        }


        public async Task<bool> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("{id} cannot be null", id);

            var user = await _userManager.FindByIdAsync(id);
            if(user != null) 
            { 
                await _userManager.DeleteAsync(user);  
                return true; 
            }
            return false;

        }

        public async Task<UserDTO> GetUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("{id} cannot be null", id);
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            return new UserDTO();

        }

        public async Task<IList<UserDTO>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IList<UserDTO>>(users);
        }

        public async Task<UserDTO> Update(UserDTO user)
        {
            var PreviodUserDetails = await _userManager.FindByIdAsync(user.Id);
            if (PreviodUserDetails is null)
                throw new ArgumentException("User Does Not Exist");

            PreviodUserDetails.FirstName = user.FirstName;
            PreviodUserDetails.LastName = user.LastName;

            await _userManager.UpdateAsync(PreviodUserDetails);
            return _mapper.Map<UserDTO>(PreviodUserDetails);
        }

        public async Task<bool> Disable(bool Enabled, string id)
        {
            var PreviodUserDetails = await _userManager.FindByIdAsync(id);
            if (PreviodUserDetails is null)
                throw new ArgumentException("User Does Not Exist");

            PreviodUserDetails.Disabled = Enabled;
            await _userManager.UpdateAsync(PreviodUserDetails);
            return true;
        }

        public async Task<string> ForgetPasswordTokenGenerate(ForgetPasswordModelDTO forgetPasswordModelDTO)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordModelDTO.Email);
            if (user is null)
                throw new ArgumentException("User Does Not Exist");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;

        }

        public async Task<bool> ForgetPassword(string callback, ForgetPasswordModelDTO forgetPasswordModelDTO)
        {
            MailData mailData = new MailData(
               new List<string> { forgetPasswordModelDTO.Email },
               "Reset password token", callback);
            return await _emailService.SendAsync(mailData, new CancellationToken());

        }
        
        public async Task<bool> ResetPassword(ResetPasswordModelDTO resetPasswordModelDTO)
        {

            var user = await _userManager.FindByEmailAsync(resetPasswordModelDTO.Email);
            if (user is null)
                throw new ArgumentException("User Does Not Exist");
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModelDTO.Token, resetPasswordModelDTO.PasswordHash);
            if (!resetPassResult.Succeeded)
                throw new ArgumentException("Unable to reset Password");

            return true;
            
        }

        public async Task<bool> AddUserToRole(string id, string roleName)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("id and roleName cannot be null or white space");

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                throw new ArgumentException("User Does Not Exist");
           
            if (await _userManager.IsInRoleAsync(user, roleName))
                throw new ArgumentException("User Already Exist In This Role");

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw new ArgumentException(errors);
            }

            return true;

        }

       




    }
}
