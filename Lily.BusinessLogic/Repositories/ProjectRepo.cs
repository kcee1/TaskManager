using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic.GenericRepository;
using TaskManager.BusinessLogic.IRepositries;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.Repositories
{
	/// <summary>
	/// Project Respository
	/// </summary>
	/// <remarks>all the business logic code with regards the Project entity</remarks>
	public class ProjectRepo : GenericRepo<Project>, IProjectRepo
	{


		#region Fields
		private readonly IMapper _mapper;
		#endregion

		#region Constructor
		public ProjectRepo(ApplicationDbContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		#endregion


		#region Implementation For Repository
		/// <summary>
		/// Insert Project object into the database.
		/// </summary>
		/// <param name="model">ProjectDTO Entity to be Saved</param>
		/// <returns>Returns the saved ProjectDTO Entity</returns>
		public async Task<ProjectDTO> CreateProject(ProjectDTO model)
		{
			await this.Insert(_mapper.Map<Project>(model));
			return model;
		}


		/// <summary>
		/// Update a Project Entity infromation into the database.
		/// </summary>
		/// <param name="model">ProjectDTO Entity to be Saved</param>
		/// <returns>Returns the modified ProjectDTO Entity</returns>
		public async Task<ProjectDTO> EditProject(ProjectDTO model)
		{
			Project projectTask = await this.Get(x => x.Id == model.Id);
			if (projectTask == null)
			{
				throw new ArgumentException($"{nameof(model.Name)} does not exist");
			}

			UserTask mappedUserTask = _mapper.Map<UserTask>(model);

			projectTask.Description = mappedUserTask.Description;
			projectTask.Name = mappedUserTask.Name;

			this.Update(projectTask);
			return model;
		}


		/// <summary>
		/// Responsible for deleting a single Item from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns true after deletion</returns>
		public async Task<bool> DeleteProject(int id)
		{
			await this.Delete(id);
			return true;
		}

	

		/// <summary>
		/// Responsible for returning a single task from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns the ProjectDTO</returns>
		public async Task<ProjectDTO> GetProjectById(int id)
		{

			return _mapper.Map<ProjectDTO>(await this.Get(x => x.Id == id));
		}


		/// <summary>
		/// Responsible for returning all task from the database entity table.
		/// </summary>
	   /// <returns>Returns List Of ProjectDTO Object</returns>
		public async Task<IList<ProjectDTO>> GetProjects()
		{
			return _mapper.Map<IList<ProjectDTO>>(await this.GetAll());
		}

		#endregion
	}
}
