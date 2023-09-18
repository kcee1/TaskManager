using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;

namespace TaskManger.BusinessLogic.IRepositries
{
    public interface IUserRepo
    {
        Task<IList<UserDTO>> GetUsers();
        Task<UserDTO> GetUser(string id);
        Task<bool> Delete(string id);
        Task<UserDetailsDTO> Create(UserDTO user);
        Task<UserDTO> Update(UserDTO user);
        Task<bool> Disable(bool Enabled, string id);
        Task<string> ForgetPasswordTokenGenerate(ForgetPasswordModelDTO forgetPasswordModelDTO);
        Task<bool> ForgetPassword(string callback, ForgetPasswordModelDTO forgetPasswordModelDTO);
        Task<bool> ResetPassword(ResetPasswordModelDTO resetPasswordModelDTO);
        Task<bool> AddUserToRole(string id, string roleName);

       

    }
}
