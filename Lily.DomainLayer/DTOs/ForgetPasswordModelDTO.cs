using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.DTOs
{
	/// <summary>
	/// Forget Password DTO Data Transfer Object
	/// </summary>
	public class ForgetPasswordModelDTO
    {
		/// <summary>
		/// Email
		/// </summary>
		[Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
