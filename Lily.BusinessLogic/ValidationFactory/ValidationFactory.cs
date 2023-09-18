using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic.Factory;
using TaskManager.BusinessLogic.IValidationFactoryInterface;

namespace TaskManager.BusinessLogic.ValidationFactory
{
	/// <summary>
	/// Validation Factory
	/// </summary>
#nullable disable
	public class ValidationFactory : IValidationFactory
	{
		public ValidationFactory()
		{

		}

		#region Declaration
		private NotificationFactory _notificationFactory;
		private UserTaskFactory _userTaskFactory;
		private ProjectFactory _projectFactory;
		private AssignedUserTaskFactory _assignedUserTaskFactory;
		#endregion

		/// <summary>
		/// Get the Notification factory.
		/// </summary>
		/// <remarks>if the factory hasn't been initialized create a new instance else get existing instance</remarks>
		public NotificationFactory Notifications
		{
			get { if (_notificationFactory == null) { _notificationFactory = new NotificationFactory(); } return _notificationFactory; }

			set { }
		}

		public UserTaskFactory UserTasks
		{
			get { if (_userTaskFactory == null) { _userTaskFactory = new UserTaskFactory(); } return _userTaskFactory; }

			set { }
		}

		public ProjectFactory Projects
		{
			get { if (_projectFactory == null) { _projectFactory = new ProjectFactory(); } return _projectFactory; }

			set { }
		}

		public AssignedUserTaskFactory AssignedUserTask
		{
			get { if (_assignedUserTaskFactory == null) { _assignedUserTaskFactory = new AssignedUserTaskFactory(); } return _assignedUserTaskFactory; }

			set { }
		}


	}
}
