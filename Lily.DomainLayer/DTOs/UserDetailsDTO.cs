using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.DTOs
{
#nullable disable
	/// <summary>
	/// UserDetails Data Transfer Object
	/// </summary>
	public class UserDetailsDTO
    {
		/// <summary>
		/// First Name 
		/// </summary>
		public string FirstName { get; set; } 

		/// <summary>
		/// Last Name 
		/// </summary>
		[Required(ErrorMessage = "LastName is required")]
		public string LastName { get; set; }

		/// <summary>
		/// Email 
		/// </summary>
		[Required(ErrorMessage = "Email is required")]
        [EmailAddress]
		public string Email { get; set; }


		/// <summary>
		/// Phone Number 
		/// </summary>
		[Required(ErrorMessage = "PhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } 
    }
}
