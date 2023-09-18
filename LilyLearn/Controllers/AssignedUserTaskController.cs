using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.BusinessLogic.IValidationFactoryInterface;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AssignedUserTaskController : Controller
	{
		#region Fields
		private readonly IUnitOfWork unitOfWork;
		private readonly IValidationFactory validationFactory;
		#endregion

		#region Constructor
		/// <summary>
		/// Responsible for initializing  the validationFactory, unitOfWork objects etc.
		/// </summary>
		/// <param name="unitOfWork">A Notification factory object</param>
		/// <param name="validationFactory">A Notification repository object</param>
		public AssignedUserTaskController(IUnitOfWork unitOfWork, IValidationFactory validationFactory)
		{
			this.unitOfWork = unitOfWork;
			this.validationFactory = validationFactory;
		}
		#endregion



		/// <summary>
		/// Create a new AssignedUserTask
		/// </summary>
		/// <param name="model">AssignedUserTask model</param>
		/// <returns>A newly created AssignedUserTask  </returns>
		/// <response code="200">Returns the newly created AssignedUserTask</response>
		/// <response code="404">If the user or the task does not exist</response>
		/// <response code="400">If the item is null </response>   
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<AssignedUserTaskDTO>>> Create(AssignedUserTaskDTO model)
		{
			string errorMsg = string.Empty;
			try
			{

				//Validate Model
				if (!validationFactory.AssignedUserTask.ValidateModel(model, false, 0, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				//Validate If User Exists
				UserDTO ExistingUser = await unitOfWork.User.GetUser(model.UserId);
				if(ExistingUser == null)
				{
					return NotFound(new ApiResponse<AssignedUserTaskDTO>(model, false, "User Does Not Exist"));

				}

				//Validate If Task Exists
				UserTaskDTO ExistingTask = await unitOfWork.UserTask.GetTaskById(model.UserTaskId);
				if (ExistingTask == null)
				{
					return NotFound(new ApiResponse<AssignedUserTaskDTO>(model, false, "Task Does Not Exist"));

				}


				// Map AssignedUserTaskDTO to AssignedUserTask And Call Create method
				AssignedUserTaskDTO createdAssignedUserTask = await unitOfWork.AssignedUserTask.CreateAssignedUserTask(model);

				//Save Changes
				await unitOfWork.Complete();

				return Ok(new ApiResponse<AssignedUserTaskDTO>(createdAssignedUserTask));

			}
			catch (DbUpdateException ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<AssignedUserTaskDTO>(model, false, ex.Message));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					// Handle exceptions and return an error response
					return BadRequest(new ApiResponse<AssignedUserTaskDTO>(model, false, ex.InnerException.Message));

				}

				//   Log.Error(ex);
				return BadRequest(new ApiResponse<AssignedUserTaskDTO>(model, false, ex.Message));
			}

		}



		/// <summary>
		/// Get a AssignedUserTask using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A AssignedUserTask with the specified id</returns>
		/// <response code = "200" > Returns the AssignedUserTask with the specified identifier</response>
		/// <response code = "400" > If there is no item with the specified primary identifier</response>
		/// <response code="404">If the assigned user task does not exist</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<AssignedUserTaskDTO>>> Get(int id)
		{
			try
			{
				if (!(id <= 0))
				{
					AssignedUserTaskDTO notification = await unitOfWork.AssignedUserTask.GetAssignedUserTaskById(id);

					if (notification is null)
					{
						return NotFound(new ApiResponse<AssignedUserTaskDTO>(new AssignedUserTaskDTO(), false, "Not Found"));

					}
					return Ok(new ApiResponse<AssignedUserTaskDTO>(notification, true));

				}


				return BadRequest(new ApiResponse<AssignedUserTaskDTO>(new AssignedUserTaskDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<AssignedUserTaskDTO>(new AssignedUserTaskDTO(), false, ex.Message)); ;
			}

		}



		/// <summary>
		/// Retrieve all AssignedUserTask available on the platform
		/// </summary>
		/// <returns> All the AssignedUserTask available on the platform</returns>
		/// <response code = "200" > Returns all AssignedUserTask in the platform</response>
		/// <response code = "204" > If there is no item </response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ApiResponse<IEnumerable<AssignedUserTaskDTO>>>> GetAll()
		{
			IList<AssignedUserTaskDTO> projects = await unitOfWork.AssignedUserTask.GetAssignedUserTasks();
			if (projects.Count == 0)
			{
				return NotFound(new ApiResponse<AssignedUserTaskDTO>(new AssignedUserTaskDTO(), false, "No Assigned User Task Available"));
			}
			return Ok(new ApiResponse<IList<AssignedUserTaskDTO>>(projects));

		}



		/// <summary>
		/// Get a AssignedUserTask using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A AssignedUserTask with the specified id</returns>
		/// <response code = "200" > Returns the AssignedUserTask with the specified identifier</response>
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
					AssignedUserTaskDTO userTask = await unitOfWork.AssignedUserTask.GetAssignedUserTaskById(id);

					if (userTask is null)
					{
						return NotFound(new ApiResponse<int>(id, false, "Not Found"));

					}


					bool userTaskDeleteResult = await unitOfWork.Notification.DeleteNotification(id);
					return Ok("Deleted Successfully");
				}


				return BadRequest(new ApiResponse<AssignedUserTaskDTO>(new AssignedUserTaskDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<AssignedUserTaskDTO>(new AssignedUserTaskDTO(), false, ex.Message));
			}

		}


		/// <summary>
		/// Update an existing AssignedUserTask
		/// </summary>
		/// <param name="model">AssignedUserTask model</param>
		/// <returns>An updated AssignedUserTask  </returns> 
		/// <response code="200">Returns the updated AssignedUserTask</response>
		/// <response code="400">If an exception occurs</response>     
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<AssignedUserTaskDTO>>> Update(AssignedUserTaskDTO model)
		{
			string errorMsg = string.Empty;
			try
			{
				//Validate Model
				if (!validationFactory.AssignedUserTask.ValidateModel(model, true, model.Id, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				//Validate If User Exists
				UserDTO ExistingUser = await unitOfWork.User.GetUser(model.UserId);
				if (ExistingUser == null)
				{
					return NotFound(new ApiResponse<AssignedUserTaskDTO>(model, false, "User Does Not Exist"));

				}

				//Validate If Task Exists
				UserTaskDTO ExistingTask = await unitOfWork.UserTask.GetTaskById(model.UserTaskId);
				if (ExistingTask == null)
				{
					return NotFound(new ApiResponse<AssignedUserTaskDTO>(model, false, "Task Does Not Exist"));

				}




				//Update Task
				AssignedUserTaskDTO UpdatedTask = await unitOfWork.AssignedUserTask.EditAssignedUserTask(model);


				//Save Changes
				await unitOfWork.Complete();

				// return response
				return Ok(new ApiResponse<AssignedUserTaskDTO>(UpdatedTask));


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






	}
}
