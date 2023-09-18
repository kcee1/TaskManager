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

	/// <summary>
	/// UserTask Respository
	/// </summary>
	/// <remarks>all the business logic code with regards the UserTask entity</remarks>
	public class UserTaskRepo : GenericRepo<UserTask>, IUserTaskRepo
	{

		#region Fields
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		#endregion


		#region Constructor
		public UserTaskRepo(ApplicationDbContext context, IMapper mapper, IEmailService emailService) : base(context)
		{
			_mapper = mapper;
			_emailService = emailService;
		}
		#endregion

		#region Implementation For Repository
		/// <summary>
		/// Insert UserTask object into the database.
		/// </summary>
		/// <param name="model">UserTaskDTO Entity to be Saved</param>
		/// <returns>Returns the UserTaskDTO Entity</returns>
		public async Task<UserTaskDTO> CreateTask(UserTaskDTO model)
		{
			model.IsNotified = false;
			await this.Insert(_mapper.Map<UserTask>(model));
			return model;

		}


		/// <summary>
		/// Update a UserTask Entity infromation into the database.
		/// </summary>
		/// <param name="model">UserTask Entity to be Saved</param>
		/// <returns>Returns the modified UserTask Entity</returns>
		public async Task<UserTaskDTO> EditTask(UserTaskDTO model)
		{
			UserTask userTask = await this.Get(x => x.Id == model.Id);
			if (userTask == null)
			{
				throw new ArgumentException($"{nameof(model.Name)} does not exist");
			}

			UserTask mappedUserTask = _mapper.Map<UserTask>(model);

			userTask.Description = mappedUserTask.Description;
			userTask.Name = mappedUserTask.Name;
			userTask.Priority = mappedUserTask.Priority;

			this.Update(userTask);
			return model;

		}

		/// <summary>
		/// Assign UserTask Entity to another project and update into the database.
		/// </summary>
		/// <param name="taskId">taskId to be updated</param>
		/// <param name="projectId">projectId to be assigned</param>
		/// <returns>Returns the modified UserTask Entity</returns>
		public async Task<UserTaskDTO> AssignTaskToProject(int taskId, int projectId)
		{
			UserTask userTask = await this.Get(x => x.Id == taskId);
			if (userTask == null)
			{
				throw new ArgumentException($"Task with {nameof(taskId)} does not exist");
			}


			userTask.ProjectId = projectId;
			
			this.Update(userTask);
			return _mapper.Map<UserTaskDTO>(userTask);

		}


		/// <summary>
		/// Responsible for deleting a single Item from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns true after deletion</returns>
		public async Task<bool> DeleteTask(int id)
		{
			await this.Delete(id);
			return true;
		}


		/// <summary>
		/// Responsible for returning a single task from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns the UserTaskDTO</returns>
		public async Task<UserTaskDTO> GetTaskById(int id)
		{
			return _mapper.Map<UserTaskDTO>(await this.Get(x => x.Id == id));
		}


		/// <summary>
		/// Responsible for returning a list of task from the database entity table that is due for the current week.
		/// </summary>
		/// <returns>Returns the UserTaskDTO entities</returns>
		public async Task<IList<UserTaskDTO>> GetDueTaskForTheNextOneWeek()
		{
			// Get the current week's start and end dates
			DateTime today = DateTime.Today;
			DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
			DateTime endOfWeek = startOfWeek.AddDays(6);

			// Filter tasks due for the current week and return
			return _mapper.Map<IList<UserTaskDTO>>(await this.GetAll(task => task.DueDate >= startOfWeek && task.DueDate <= endOfWeek));
		}


		/// <summary>
		/// Responsible for returning a list of task from the database entity table that matches parameter.
		/// </summary>
		/// <param name="priority">A priority parameter which is the priority of this table</param>
		/// <param name="status">A status parameter which is the status of this table</param>
		/// <returns>Returns UserTaskDTO entities</returns>
		public async Task<IList<UserTaskDTO>> GetTaskByStatusOrPriority(Priority? priority, Status? status)
		{
			if(priority is not null && status is not null)
			{
				return _mapper.Map<IList<UserTaskDTO>>(await this.GetAll(x => x.Priority == priority && x.Status == status));

			}
			if (priority is not null)
			{
				return _mapper.Map<IList<UserTaskDTO>>(await this.GetAll(x => x.Priority == priority));

			}
			if(status is not null)
			{
				return _mapper.Map<IList<UserTaskDTO>>(await this.GetAll(x => x.Status == status));

			}
			
			return new List<UserTaskDTO>();
			


		}



		/// <summary>
		/// Responsible for returning a single task from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An projectId parameter which is the foreign key of this table for project entites</param>
		/// <returns>Returns the UserTaskDTO</returns>
		public async Task<UserTaskDTO> GetTaskInProject(int projectId)
		{
			return _mapper.Map<UserTaskDTO>(await this.Get(x => x.ProjectId == projectId));
		}





		/// <summary>
		/// Responsible for returning all task from the database entity table.
		/// </summary>
		/// /// <returns>Returns List Of UserTaskDTO Object</returns>
		public async Task<IList<UserTaskDTO>> GetTasks()
		{
			return _mapper.Map<IList<UserTaskDTO>>(await this.GetAll());
		}


		/// <summary>
		/// Responsible for returning all tasks from the database entity table that matches parameter.
		/// </summary>
		/// <param name="UserId">A UserId parameter which is the primary key of this table</param>
		/// <returns>Returns a list of UserTaskDTO Object</returns>
		public async Task<IList<UserTaskDTO>> GetUnicUserTasks(string UserId)
		{
			return _mapper.Map<IList<UserTaskDTO>>(await this.GetAll(x => x.UserId == UserId));
		}


		/// <summary>
		/// Update a UserTask Entity infromation into the database.
		/// </summary>
		/// <param name="taskId">UserTask Entity to be Saved</param>
		/// <param name="status">UserTask Entity to be Saved</param>
		/// <returns>Returns the modified UserTask Entity</returns>
		public async Task<UserTaskDTO> UpdateTaskStatus(int taskId, Status status)
		{
			UserTask userTask = await this.Get(x => x.Id == taskId);
			if (userTask == null)
			{
				throw new ArgumentException($"{nameof(taskId)} does not exist");
			}

			userTask.Status = status;
		
			this.Update(userTask);
			return _mapper.Map<UserTaskDTO>(userTask);

		}


		/// <summary>
		/// Send notification via email to users whose task is due within the next 48 hours
		/// </summary>
		public async Task DueDateNotification()
		{
			// Calculate the current date and the date 48 hours from now
			DateTime now = DateTime.Now;
			DateTime dueDateThreshold = now.AddHours(48);

			// Implement logic to send notifications to users with due dates in 48 hours
			var userDuedTask = await this.GetAll(task => task.DueDate >= now && task.DueDate <= dueDateThreshold && task.Status != Status.Completed, Includes: new List<string> { "User" });


			foreach (var item in userDuedTask)
			{
				// Send notifications to users
				MailData mailData = new MailData(
			  new List<string> { item.User.Email },
			   NotificationType.DueDateReminder.ToString(), "Task Is Due");

				//send notifications to the corresponding users and log Success details.
				await _emailService.SendAsync(mailData, new CancellationToken());


			}

		}


		/// <summary>
		/// Send notification via email to the user who created tast when task is completed
		/// </summary>
		public async Task TaskCompleteNotification()
		{


			// Calculate the current date and the date 48 hours from now
			DateTime now = DateTime.Now;
			DateTime dueDateThreshold = now.AddHours(48);

			// Implement logic to send notifications to users with due dates in 48 hours
			var userDuedTask = await this.GetAll(task => task.Status == Status.Completed && task.IsNotified == false, Includes: new List<string> { "User"});
		  

			foreach (var item in userDuedTask)
			{
				// Send notifications to users
				MailData mailData = new MailData(
			  new List<string> { item.User.Email },
			   NotificationType.StatusUpdate.ToString(), "Task Have Benn Completed");

				//send notifications to the corresponding users and log Success details.
				if (await _emailService.SendAsync(mailData, new CancellationToken()))
				{
					item.IsNotified = true;
					this.Update(item);
					this.SaveUpdate();
				}



			}
		}


		#endregion


	}
}
