using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.Models;

namespace TaskManager.DomainLayer.DTOs
{

	/// <summary>
	/// Assigned User Task Data Transfer Object
	/// </summary>
#nullable disable
	public class AssignedUserTaskDTO : BaseDTO
	{
		/// <summary>
		/// The User Primary Identifier Of The User Who Created has benn assigned to task
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// The User Who Created Task
		/// </summary>
		public User User { get; set; }


		/// <summary>
		/// The Task Primary Identifier 
		/// </summary>
		public int UserTaskId { get; set; }

		/// <summary>
		/// The User Task 
		/// </summary>
		public UserTask UserTask { get; set; }


		/// <summary>
		/// Is Notified
		/// </summary>
		public bool IsNotified { get; set; }
	}
}
