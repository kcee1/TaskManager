using TaskManger.BusinessLogic.IRepositries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic.Repositories;
using TaskManager.DomainLayer.Models;

namespace TaskManger.BusinessLogic.UnitOfWorks
{
    public  interface IUnitOfWork : IDisposable
    {
            UserRepo User { get; }
            RoleRepo Role { get; }
		    UserTaskRepo UserTask { get; }
		    ProjectRepo Project { get; }
		    NotificationRepo Notification { get; }
		    AssignedUserTaskRepo AssignedUserTask { get; }
          
          
          
            Task Complete();
       
    }
}
