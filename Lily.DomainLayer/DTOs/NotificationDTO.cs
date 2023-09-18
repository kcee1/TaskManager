using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Enums;

namespace TaskManager.DomainLayer.Models
{
	/// <summary>
	/// Notification Data Transfer Object
	/// </summary>
#nullable disable
	public class NotificationDTO : BaseDTO
	{
		/// <summary>
		/// Notification Type
		/// </summary>
		public NotificationType NotificationType { get; set; }

		/// <summary>
		/// Message Type
		/// </summary>
		public string Message { get; set; }


		/// <summary>
		/// Time Stamp
		/// </summary>
		public DateTime TimeStamp { get; set; }

		/// <summary>
		/// Notification Status
		/// </summary>
		public NotificationStatus NotificationStatus { get; set; }


	}
}
