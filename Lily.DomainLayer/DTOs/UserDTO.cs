using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.DTOs
{
	/// <summary>
	/// User Data Transfer Object
	/// </summary>
	public class UserDTO 
    {
		/// <summary>
		/// The User Primary Identifier
		/// </summary>
		public string? Id { get; set; }
        [Required(ErrorMessage = "FirstName is required")]

		/// <summary>
		/// First Name
		/// </summary>
		public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "LastName is required")]

		/// <summary>
		/// Last Name
		/// </summary>
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// Email
		/// </summary>
		[Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Password Hash
		/// </summary>
		[Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; } = string.Empty;

		/// <summary>
		/// Confirm Password
		/// </summary>
		[Required]
        [DataType(DataType.Password)]
        [Compare("PasswordHash")]
        public string ConfirmPassword { get; set; } = string.Empty;

		/// <summary>
		/// Phone Number
		/// </summary>
		[Required(ErrorMessage = "PhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;


    }
}
