using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.Enums;

namespace TaskManager.DomainLayer.Models
{
	/// <summary>
	/// Task Model Data Access Object
	/// </summary>
	/// <remarks>Used to connect to the Task table in database to perform actions like save,update,delete and search</remarks>

#nullable disable
	public class UserTask : BaseModel
	{
		/// <summary>
		/// Description 
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Due Date 
		/// </summary>
		public DateTime DueDate { get; set; }

		/// <summary>
		/// Priority
		/// </summary>
	  [EnumDataType(typeof(Priority))]
		public Priority Priority { get; set; }

		/// <summary>
		/// Status
		/// </summary>
	  [EnumDataType(typeof(Status))]
		public Status Status { get; set; }

		/// <summary>
		/// The User Primary Identifier
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// The User 
		/// </summary>
		public User User { get; set; }

		


		/// <summary>
		/// The Project Primary Identifier
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// The Project
		/// </summary>
		public Project Project { get; set; } 

		/// <summary>
		/// Is Notified
		/// </summary>
		public bool IsNotified { get; set; } = false;




	}
}
