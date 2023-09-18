using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.IRepositries
{
	public interface IUserTaskRepo
	{
		Task<UserTaskDTO> GetTaskById(int id);
		Task<IList<UserTaskDTO>> GetTasks();
		Task<bool> DeleteTask(int id);
		Task<UserTaskDTO> EditTask(UserTaskDTO model);
		Task<UserTaskDTO> CreateTask(UserTaskDTO model);
		Task<IList<UserTaskDTO>> GetUnicUserTasks(string UserId);
	}
}
