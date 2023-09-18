using Microsoft.EntityFrameworkCore;
using ServiceLibrary.EmailService.Interfaces;
using ServiceLibrary.EmailService.Model;
using System.Linq;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.Enums;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.BackgroundServices
{
	public class NotificationService
	{
		#region Fields
		private readonly IUnitOfWork unitOfWork;

		#endregion

		#region Constructor
		public NotificationService(IUnitOfWork unitOfWork)
		{
			
			this.unitOfWork = unitOfWork;
		}
		#endregion


		#region Sends Due Date Notification
		public async Task NotificationForDueDate()
		{

			await unitOfWork.UserTask.DueDateNotification();
			
		}
		#endregion



	}

}
