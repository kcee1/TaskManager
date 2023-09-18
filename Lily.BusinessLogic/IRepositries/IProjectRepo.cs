using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.IRepositries
{
	public interface IProjectRepo
	{
		Task<ProjectDTO> GetProjectById(int id);
		Task<IList<ProjectDTO>> GetProjects();
		Task<bool> DeleteProject(int id);
		Task<ProjectDTO> EditProject(ProjectDTO model);
		Task<ProjectDTO> CreateProject(ProjectDTO model);
	}
}
