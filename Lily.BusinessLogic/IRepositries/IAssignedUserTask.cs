using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.IRepositries
{
	public interface IAssignedUserTask
	{
		Task<AssignedUserTaskDTO> GetAssignedUserTaskById(int id);
		Task<IList<AssignedUserTaskDTO>> GetAssignedUserTasks();
		Task<bool> DeleteAssignedUserTask(int id);
		Task<AssignedUserTaskDTO> EditAssignedUserTask(AssignedUserTaskDTO model);
		Task<AssignedUserTaskDTO> CreateAssignedUserTask(AssignedUserTaskDTO model);
	}
}
