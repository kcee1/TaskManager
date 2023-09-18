using Microsoft.EntityFrameworkCore;
using ServiceLibrary.EmailService.Interfaces;
using ServiceLibrary.EmailService.Model;
using System.Linq;
using TaskManager.DAL.Data;
using TaskManager.DomainLayer.Enums;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.BackgroundServices
{
	public class TaskCompleteService
	{
		#region Fields
		private readonly IUnitOfWork unitOfWork;
		#endregion


		#region Constructor
		public TaskCompleteService(IUnitOfWork unitOfWork)
		{
			
			this.unitOfWork = unitOfWork;
		}
		#endregion


		#region Sends Task Completed Notification 
		public async Task TaskCompleteNotification()
		{

			await unitOfWork.UserTask.TaskCompleteNotification();


		}
		#endregion

	}
}