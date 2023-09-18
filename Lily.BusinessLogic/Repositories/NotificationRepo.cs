using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.EmailService.Interfaces;
using ServiceLibrary.EmailService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic.GenericRepository;
using TaskManager.BusinessLogic.IRepositries;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Enums;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.Repositories
{
	/// <summary>
	/// Notification Respository
	/// </summary>
	/// <remarks>all the business logic code with regards the Notification entity</remarks>
	public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
	{
		#region Fields
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		#endregion

		#region
		public NotificationRepo(ApplicationDbContext context, IMapper mapper, IEmailService emailService) : base(context)
		{
			_mapper = mapper;
			_emailService = emailService;

		}
		#endregion

		#region Implementation For Repository
		/// <summary>
		/// Insert Notification object into the database.
		/// </summary>
		/// <param name="model">NotificationDTO Entity to be Saved</param>
		/// <returns>Returns the saved NotificationDTO Entity</returns>
		public async Task<NotificationDTO> CreateNotification(NotificationDTO model)
		{
			model.NotificationStatus = NotificationStatus.Unread;
			model.TimeStamp = DateTime.Now;
			await this.Insert(_mapper.Map<Notification>(model));
			
			return model;
		}

		


		/// <summary>
		/// Update a Notification Entity infromation into the database.
		/// </summary>
		/// <param name="model">NotificationDTO Entity to be Saved</param>
		/// <returns>Returns the modified NotificationDTO Entity</returns>
		public async Task<NotificationDTO> EditNotification(NotificationDTO model)
		{
			Notification projectTask = await this.Get(x => x.Id == model.Id);
			if (projectTask == null)
			{
				throw new ArgumentException($"{nameof(model.Name)} does not exist");
			}

			Notification mappedUserTask = _mapper.Map<Notification>(model);

			projectTask.Message = mappedUserTask.Message;
			projectTask.Name = mappedUserTask.Name;
			projectTask.NotificationType = mappedUserTask.NotificationType;

			this.Update(projectTask);
			return model;
		}



		/// <summary>
		/// Responsible for deleting a single Item from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns true after deletion</returns>
		public async Task<bool> DeleteNotification(int id)
		{
			await this.Delete(id);
			return true;
		}


		/// <summary>
		/// Responsible for returning a single Notification from the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns the NotificationDTO</returns>
		public async Task<NotificationDTO> GetNotificationById(int id)
		{
			
			return _mapper.Map<NotificationDTO>(await this.Get(x => x.Id == id));
		}


		/// <summary>
		/// Responsible for returning all Notification from the database entity table.
		/// </summary>
		/// <returns>Returns List Of NotificationDTO Object</returns>
		public async Task<IList<NotificationDTO>> GetNotifications()
		{
			return _mapper.Map<IList<NotificationDTO>>(await this.GetAll());
		}


		/// <summary>
		/// Responsible for notifications.
		/// </summary>
		/// <returns>Returns NotificationDTO Object</returns>
		public async Task<NotificationDTO> SendNotificationsToAllUser(IList<UserDTO> users, int notificationId)
		{


			// Implement logic to send notifications to users with due dates in 48 hours
			Notification existingNotification = await this.Get(q => q.Id == notificationId);


				foreach (var item in users)
				{
					// Send notifications to users
					MailData mailData = new MailData(
				  new List<string> { item.Email },
				   existingNotification.NotificationType.ToString(), existingNotification.Message);

					//send notifications to the corresponding users.
					await _emailService.SendAsync(mailData, new CancellationToken());
				
				}

				//Returns Notification that was sent
				return _mapper.Map<NotificationDTO>(existingNotification);

		}



		/// <summary>
		/// Responsible for marking a notification as read or unread in  the database entity table that matches parameter.
		/// </summary>
		/// <param name="id">An id parameter which is the primary key of this table</param>
		/// <returns>Returns the NotificationDTO</returns>
		public async Task<NotificationDTO> MarkAsReadOrUnRead(int id)
		{
			Notification notification = await this.Get(x => x.Id == id);
			if(notification is null)
			{
				throw new Exception("Notification Does Not Exist");
			}
			NotificationDTO notificationMapped = _mapper.Map<NotificationDTO>(notification);

			if (notification.NotificationStatus is NotificationStatus.Unread)
			{
				notification.NotificationStatus = NotificationStatus.Read;
			}

			notification.NotificationStatus = NotificationStatus.Unread;
			return notificationMapped;
		}

		#endregion



	}
}
