using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.BusinessLogic.IValidationFactoryInterface;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		#region Constructor
		private readonly IUnitOfWork unitOfWork;
		private readonly IValidationFactory validationFactory;


		/// <summary>
		/// Responsible for initializing  the validationFactory, unitOfWork objects etc.
		/// </summary>
		/// <param name="unitOfWork">A Notification factory object</param>
		/// <param name="validationFactory">A Notification repository object</param>
		public NotificationController(IUnitOfWork unitOfWork, IValidationFactory validationFactory)
		{
			this.unitOfWork = unitOfWork;
			this.validationFactory = validationFactory;
		}
		#endregion




		/// <summary>
		/// Create a new Notification
		/// </summary>
		/// <param name="model">Notification model</param>
		/// <returns>A newly created Notification  </returns>
		/// <response code="201">Returns the newly created Notification</response>
		/// <response code="400">If the item is null </response>   
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<ApiResponse<NotificationDTO>>> Create(NotificationDTO model)
		{
			string errorMsg = string.Empty;
			try
			{

				//Validate Model
				if (!validationFactory.Notifications.ValidateModel(model, false, 0, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				// Map NotificationDTO to Notification And Call Create method
				NotificationDTO createdNotification = await unitOfWork.Notification.CreateNotification(model);

				//Save Changes
				await unitOfWork.Complete();

				return Ok(new ApiResponse<NotificationDTO>(createdNotification));

			}
			catch (DbUpdateException ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<NotificationDTO>(model, false, ex.Message));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					// Handle exceptions and return an error response
					return BadRequest(new ApiResponse<NotificationDTO>(model, false, ex.InnerException.Message));

				}

				//   Log.Error(ex);
				return BadRequest(new ApiResponse<NotificationDTO>(model, false, ex.Message));
			}

		}


		/// <summary>
		/// Get a Notification using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A Notification with the specified id</returns>
		/// <response code = "200" > Returns the Notification with the specified identifier</response>
		/// <response code = "400" > If there is no item with the specified primary identifier</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<NotificationDTO>>> Get(int id)
		{
			try
			{
				if (!(id <= 0))
				{
					NotificationDTO notification = await unitOfWork.Notification.GetNotificationById(id);

					if (notification is null)
					{
						return NotFound(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, "Not Found"));

					}
					return Ok(new ApiResponse<NotificationDTO>(notification, true));

				}


				return BadRequest(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, ex.Message)); ;
			}

		}


		/// <summary>
		/// Retrieve all Notification available on the platform
		/// </summary>
		/// <returns> All the Notification available on the platform</returns>
		/// <response code = "200" > Returns all Notification in the platform</response>
		/// <response code = "204" > If there is item with the specified primary identifier</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ApiResponse<IEnumerable<NotificationDTO>>>> GetAll()
		{
			IList<NotificationDTO> projects = await unitOfWork.Notification.GetNotifications();
			if (projects.Count == 0)
			{
				return NotFound(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, "No Notification Available"));
			}
			return Ok(new ApiResponse<IList<NotificationDTO>>(projects));

		}


		/// <summary>
		/// Get a Notification using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A projects with the specified id</returns>
		/// <response code = "200" > Returns the Notification with the specified identifier</response>
		/// <response code = "400" > If there is no item with the specified primary identifier</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<int>>> Delete(int id)
		{
			try
			{
				if (!(id <= 0))
				{
					NotificationDTO userTask = await unitOfWork.Notification.GetNotificationById(id);

					if (userTask is null)
					{
						return NotFound(new ApiResponse<int>(id, false, "Not Found"));

					}


					bool userTaskDeleteResult = await unitOfWork.Notification.DeleteNotification(id);
					return Ok("Deleted Successfully");
				}


				return BadRequest(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, ex.Message));
			}

		}



		/// <summary>
		/// Update an existing Notification
		/// </summary>
		/// <param name="model">Notification model</param>
		/// <returns>An updated Notification  </returns> 
		/// <response code="200">Returns the updated Notification</response>
		/// <response code="400">If an exception occurs</response>     
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<ApiResponse<NotificationDTO>>> Update(NotificationDTO model)
		{
			string errorMsg = string.Empty;
			try
			{
				//Validate Model
				if (!validationFactory.Notifications.ValidateModel(model, true, model.Id, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				//Updat Task
				NotificationDTO UpdatedTask = await unitOfWork.Notification.EditNotification(model);


				//Save Changes
				await unitOfWork.Complete();

				// return response
				return Ok(new ApiResponse<NotificationDTO>(UpdatedTask));


			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{

					return BadRequest(ex.InnerException.Message);
				}

				return BadRequest(ex.Message);
			}
		}



		/// <summary>
		/// Mark a Notification as read or unread using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A Notification with the specified id</returns>
		/// <response code = "200" > Returns the Notification with the specified identifier</response>
		/// <response code = "400" > If there is no item with the specified primary identifier</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<NotificationDTO>>> MarkNotificationAsReadOrUnRead(int id)
		{
			try
			{
					//Mark notification as read if it is unread or unread if it is read
					NotificationDTO notification = await unitOfWork.Notification.MarkAsReadOrUnRead(id);


				//Save Changes
				await unitOfWork.Complete();

				//returns updated notification
				return Ok(new ApiResponse<NotificationDTO>(notification, true));

			
			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, ex.Message)); ;
			}

		}


		/// <summary>
		/// Mark a Notification as read or unread using it's primary identifier
		/// </summary>
		/// <param name = "notificationId" >A primary Key for notification Entity</param>
		/// <returns> A notification that was sent</returns>
		/// <response code = "200" > Returns the Notification that was sent</response>
		/// <response code = "400" > If there is no user profiled in the application to notify</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<NotificationDTO>>> SendNotificationToAllUsers(int notificationId)
		{
			try
			{


				//Get All Existing Users
				IList<UserDTO> Users = await unitOfWork.User.GetUsers();

				if(Users.Count == 0)
				{
					return NotFound(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, "No User is currently profiled on Task Management system"));

				}


				//Mark notification as read if it is unread or unread if it is read
				NotificationDTO notification = await unitOfWork.Notification.SendNotificationsToAllUser(Users, notificationId);


				//Save Changes
				await unitOfWork.Complete();

				//returns updated notification
				return Ok(new ApiResponse<NotificationDTO>(notification, true));

			
			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<NotificationDTO>(new NotificationDTO(), false, ex.Message)); ;
			}

		}





	}
}
