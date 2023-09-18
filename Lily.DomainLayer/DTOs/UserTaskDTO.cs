using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Enums;
using TaskManager.DomainLayer.Models;

namespace TaskManager.DomainLayer.DTOs
{
	/// <summary>
	/// Task Data Transfer Object
	/// </summary>
#nullable disable
	public class UserTaskDTO : BaseDTO
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
		public Priority Priority { get; set; }

		/// <summary>
		/// Status 
		/// </summary>
		public Status Status { get; set; }

		/// <summary>
		/// The User Primary Identifier
		/// </summary>
		public string UserId { get; set; }

		///// <summary>
		///// The User 
		///// </summary>
		//public UserDetailsDTO User { get; set; }

		


		/// <summary>
		/// The Project Primary Identifier
		/// </summary>
		public int ProjectId { get; set; }

		/// <summary>
		/// The Project 
		/// </summary>
		//public ProjectDTO Project { get; set; }

		/// <summary>
		/// Is Notified
		/// </summary>
		public bool IsNotified { get; set; }




	}
}
