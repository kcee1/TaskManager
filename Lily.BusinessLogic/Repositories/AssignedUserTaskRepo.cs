using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.EmailService.Interfaces;
using ServiceLibrary.EmailService.Model;
using TaskManager.BusinessLogic.GenericRepository;
using TaskManager.BusinessLogic.IRepositries;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Enums;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.Repositories
{
	public class AssignedUserTaskRepo : GenericRepo<AssignedUserTask>, IAssignedUserTask
	{
		#region Fields
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		#endregion

		#region Constructor
		public AssignedUserTaskRepo(ApplicationDbContext context, IMapper mapper, IEmailService emailService) : base(context)
		{
			_mapper = mapper;
			_emailService = emailService;
		}
		#endregion

		#region Implementation For Repository

		/// <summary>
		/// Insert Assigned User Task object into the database.
		/// </summary>
		/// <param name="model">AssignedUserTaskDTO Entity to be Saved</param>
		/// <returns>Returns the saved AssignedUserTaskDTO Entity</returns>
		public async Task<AssignedUserTaskDTO> CreateAssignedUserTask(AssignedUserTaskDTO model)
		{
			model.IsNotified = false;
			await this.Insert(_mapper.Map<AssignedUserTask>(model));

			return model;
		}

		/// <summary>
		/// Responsible for deleting a single Item from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns true after deletion</returns>
		public async Task<bool> DeleteAssignedUserTask(int id)
		{
			await this.Delete(id);
			return true;
		}


		/// <summary>
		/// Update a Assigned User Task Entity infromation into the database.
		/// </summary>
		/// <param name="model">AssignedUserTaskDTO Entity to be Saved</param>
		/// <returns>Returns the modified AssignedUserTaskDTO Entity</returns>
		public async Task<AssignedUserTaskDTO> EditAssignedUserTask(AssignedUserTaskDTO model)
		{
			AssignedUserTask AssignedTask = await this.Get(x => x.Id == model.Id);
			if (AssignedTask == null)
			{
				throw new ArgumentException($"{nameof(model.Name)} does not exist");
			}

			AssignedUserTask mappedUserTask = _mapper.Map<AssignedUserTask>(model);

			AssignedTask.UserId = mappedUserTask.UserId;
			AssignedTask.UserTaskId = mappedUserTask.UserTaskId;
			AssignedTask.Name = AssignedTask.Name;
			

			this.Update(AssignedTask);
			return _mapper.Map<AssignedUserTaskDTO>(AssignedTask);
		}


		/// <summary>
		/// Responsible for returning a single Assigned User Task from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns the AssignedUserTaskDTO</returns>
		public async Task<AssignedUserTaskDTO> GetAssignedUserTaskById(int id)
		{
			return _mapper.Map<AssignedUserTaskDTO>(await this.Get(x => x.Id == id));
		}


		/// <summary>
		/// Responsible for returning all Assigned User Task from the database entity table.
		/// </summary>
		/// <returns>Returns List Of AssignedUserTaskDTO Object</returns>
		public async Task<IList<AssignedUserTaskDTO>> GetAssignedUserTasks()
		{
			return _mapper.Map<IList<AssignedUserTaskDTO>>(await this.GetAll());
		}


		/// <summary>
		/// Responsible for task assigned notification once  a task is assigned to a user.
		/// </summary>
		/// <returns>Returns List Of AssignedUserTaskDTO Object</returns>
		public async Task SendTaskAssignedNotification()
		{
			// Implement logic to send notifications to users with due dates in 48 hours
			var userDuedTask = await this.GetAll(task => task.IsNotified == false, Includes: new List<string> { "User" });
		


			foreach (var item in userDuedTask)
			{
				// Send notifications to users
				MailData mailData = new MailData(
			  new List<string> { item.User.Email },
			   NotificationType.StatusUpdate.ToString(), "You Have Just Been Assigned A New Task");

				//send notifications to the corresponding users and log Success details.
				await _emailService.SendAsync(mailData, new CancellationToken());


			}
		}

		#endregion
	}
}
