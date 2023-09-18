using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.Models
{
	/// <summary>
	/// Assigned User Task Model Data Access Object
	/// </summary>
	/// <remarks>Used to connect to the AssignedUserTask table in database to perform actions like save,update,delete and search</remarks>

#nullable disable
	public class AssignedUserTask : BaseModel
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
		public bool IsNotified { get; set; } = false;


	}
}
