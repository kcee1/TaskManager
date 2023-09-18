using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManger.BusinessLogic.IRepositries
{
    public interface IRoleRepo
    {
        IList<IdentityRole> GetRoles(out string message);
        Task<IdentityRole> GetRole(string id);
        Task<bool> DeleteRole(string roleName);
        Task<string> UpdateRole(string roleName);
        Task<bool> CreateRole(string roleName);

    }
}
