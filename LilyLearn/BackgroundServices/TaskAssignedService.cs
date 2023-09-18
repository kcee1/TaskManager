using ServiceLibrary.EmailService.Interfaces;
using TaskManager.DAL.Data;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.BackgroundServices
{
	public class TaskAssignedService
	{
		#region Fields
		private readonly IUnitOfWork unitOfWork;
		#endregion

		#region Constructor
		public TaskAssignedService(IUnitOfWork unitOfWork)
		{
			
			this.unitOfWork = unitOfWork;
		}
		#endregion

		#region Send Assigned Task Notification
		public async Task TaskAssignedNotifications()
		{
			// Send notifications to users
			await unitOfWork.AssignedUserTask.SendTaskAssignedNotification();

			
		}
		#endregion
	}

}
