using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ServiceLibrary.EmailService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManger.BusinessLogic.IRepositries;

namespace TaskManager.BusinessLogic.Repositories
{
    public class RoleRepo : IRoleRepo
    {
      
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepo(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
           
        }


        public async Task<bool> CreateRole(string roleName)
        {
            if(string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException(nameof(roleName));

            var Response = await _roleManager.FindByNameAsync(roleName);
            if (!(Response is null))
                throw new ArgumentException("Role Name Already Exist");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw new ArgumentException(errors);
            } 
            return true;
        }

        public async Task<bool> DeleteRole(string roleName)
        {
            if(string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException(nameof(roleName));

            var Response = await _roleManager.FindByNameAsync(roleName);
            if (Response is null)
                throw new ArgumentException("Role Does Not Exist");

            var result = await _roleManager.DeleteAsync(Response);
            if (!result.Succeeded)
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw new ArgumentException(errors);
            }
            return true;

        }

        public async Task<IdentityRole> GetRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            var result = await _roleManager.FindByIdAsync(id);

            if(result is null)
            {
                throw new ArgumentException("Role Not Found");
            }

            return result;
        }

        public IList<IdentityRole> GetRoles( out string message)
        {
            var result = _roleManager.Roles.ToList();
            if(result.Count <= 0)
            {
                message = "No role currently exist";
                return new List<IdentityRole>();
            }
            message = "successfull";
            return result;
        }

        public async Task<string> UpdateRole(string roleName)
        {
            if(string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException(nameof(roleName));
            var Response = await _roleManager.FindByNameAsync(roleName);
            if (Response is null)
                throw new ArgumentException("Role Does Not Exist");


            var result = await _roleManager.UpdateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
            {
                List<IdentityError> errorList = result.Errors.ToList();
                var errors = string.Join(", ", errorList.Select(e => e.Description));
                throw new ArgumentException(errors);
            }
            return roleName;
        }
    }
}
