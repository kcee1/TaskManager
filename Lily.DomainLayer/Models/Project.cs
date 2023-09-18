using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.Models
{
	/// <summary>
	/// Project Model Data Access Object
	/// </summary>
	/// <remarks>Used to connect to the Project table in database to perform actions like save,update,delete and search</remarks>

#nullable disable
	public class Project : BaseModel
	{

		/// <summary>
		/// Description 
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Tasks Entities
		/// </summary>
		public IList<UserTask> Tasks { get; set; }
	}
}
