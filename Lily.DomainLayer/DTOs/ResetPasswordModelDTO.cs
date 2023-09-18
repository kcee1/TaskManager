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
	/// Reset Password DTO Transfer Object
	/// </summary>
	public class ResetPasswordModelDTO
    {
		/// <summary>
		/// Password Hash
		/// </summary>
		[Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

		/// <summary>
		/// Confirm Password
		/// </summary>
		[DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

		/// <summary>
		/// Email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Token
		/// </summary>
		public string Token { get; set; }
    }
}
