using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.BusinessLogic.Factory;

namespace TaskManager.BusinessLogic.IValidationFactoryInterface
{
	/// <summary>
	/// Validation factory interface
	/// </summary>
	/// <remarks>Show the blueprint of Inventory app factory</remarks>
	public interface IValidationFactory
	{
		/// <summary>
		/// Notifications Factory
		/// </summary>
		NotificationFactory Notifications { set; get; }

		/// <summary>
		/// User Task Factory
		/// </summary>
		UserTaskFactory UserTasks { set; get; }

		/// <summary>
		/// Project Factory
		/// </summary>
		ProjectFactory Projects { set; get; }

		/// <summary>
		/// Assigned User Task
		/// </summary>
		AssignedUserTaskFactory AssignedUserTask { set; get; }

	}
}
