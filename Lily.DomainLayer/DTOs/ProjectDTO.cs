using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.Models;

namespace TaskManager.DomainLayer.DTOs
{
	/// <summary>
	/// Project Data Transfer Object
	/// </summary>
#nullable disable
	public class ProjectDTO : BaseDTO
	{
		/// <summary>
		/// Description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Tasks Entities
		/// </summary>
		//public IList<UserTaskDTO> Tasks { get; set; }
	}
}
