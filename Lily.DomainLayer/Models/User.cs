using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.Models
{
	/// <summary>
	/// User Model Data Access Object
	/// </summary>
	/// <remarks>Used to connect to the User table in database to perform actions like save,update,delete and search</remarks>

#nullable disable
	public class User : IdentityUser
    {
		/// <summary>
		/// First Name  
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Last Name  
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Disabled  
		/// </summary>
		public bool Disabled { get; set; }

		

	}
}
