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
	/// Notification Model Data Access Object
	/// </summary>
	/// <remarks>Used to connect to the Notification table in database to perform actions like save,update,delete and search</remarks>

#nullable disable
	public class Notification : BaseModel
	{
		/// <summary>
		/// Notification Type
		/// </summary>
	   [EnumDataType(typeof(NotificationType))]
		public NotificationType NotificationType { get; set; }

		/// <summary>
		/// Message
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Time Stamp
		/// </summary>
		public DateTime? TimeStamp { get; set; }

		/// <summary>
		/// Notification Status
		/// </summary>
		public NotificationStatus NotificationStatus { get; set; }


	}
}
