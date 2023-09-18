using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.IRepositries
{
	public interface INotificationRepo
	{
		Task<NotificationDTO> GetNotificationById(int id);
		Task<IList<NotificationDTO>> GetNotifications();
		Task<bool> DeleteNotification(int id);
		Task<NotificationDTO> EditNotification(NotificationDTO model);
		Task<NotificationDTO> CreateNotification(NotificationDTO model);
	}
}
