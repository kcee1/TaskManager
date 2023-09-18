using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.BusinessLogic.IValidationFactoryInterface;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Enums;
using TaskManager.DomainLayer.Models;
using TaskManger.BusinessLogic.UnitOfWorks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace TaskManagerApi.Controllers
{
	/// <summary>
	/// UserTask Controller
	/// </summary>
	/// <remarks>UserTasks all the endpoints with regards to UserTask object</remarks>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class UserTaskController : ControllerBase
	{
		#region Constructor
		private readonly IUnitOfWork unitOfWork;
		private readonly IValidationFactory validationFactory;


		/// <summary>
		/// Responsible for initializing  the validationFactory, unitOfWork objects etc.
		/// </summary>
		/// <param name="unitOfWork">A Project factory object</param>
		/// <param name="validationFactory">A Project repository object</param>
		public UserTaskController(IUnitOfWork unitOfWork, IValidationFactory validationFactory)
		{
			this.unitOfWork = unitOfWork;
			this.validationFactory = validationFactory;
		}
		#endregion


		/// <summary>
		/// Create a new User Task
		/// </summary>
		/// <param name="model">User Task model</param>
		/// <returns>A newly created User Task  </returns>
		/// <response code="201">Returns the newly created User Task</response>
		/// <response code="400">If the item is null </response>   
	    [HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<ApiResponse<UserTaskDTO>>> Create(UserTaskDTO model)
		{
			string errorMsg = string.Empty;
			try
			{
				
				//Validate Model
				if (!validationFactory.UserTasks.ValidateModel(model, false, 0, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				// Map UserTaskDTO to UserTask And Call Create method
				UserTaskDTO createdTask = await unitOfWork.UserTask.CreateTask(model);

				//Save Changes
				await unitOfWork.Complete();

				return Ok(new ApiResponse<UserTaskDTO>(createdTask));

			}
			catch (DbUpdateException ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<UserTaskDTO>(model, false, ex.Message));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					// Handle exceptions and return an error response
					return BadRequest(new ApiResponse<UserTaskDTO>(model, false, ex.InnerException.Message));
				
				}

				//   Log.Error(ex);
				return BadRequest(new ApiResponse<UserTaskDTO>(model, false, ex.Message));
			}

		}


		/// <summary>
		/// Get a User Task using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A User Task with the specified id</returns>
		/// <response code = "200" > Returns the User Task with the specified identifier</response>
		/// <response code = "400" > If there is no item with the specified primary identifier</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<UserTaskDTO>>> Get(int id)
		{
			try
			{
				if (!(id <= 0))
				{
					UserTaskDTO userTask = await unitOfWork.UserTask.GetTaskById(id);

					if(userTask is null)
					{
						return NotFound(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "Not Found"));

					}
					return Ok(new ApiResponse<UserTaskDTO>(userTask, true));

				}


				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, ex.Message));;
			}
		
		}


		/// <summary>
		/// Get a User Task for the next one week ranging from the current day
		/// </summary>
		/// <returns> A list of User Task for the current week</returns>
		/// <response code = "200" > Returns the list User Task</response>
		/// <response code = "400" > If there is no item exist within the next one week</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<IList<UserTaskDTO>>>> GetTaskForCurrentWeek()
		{
			try
			{
				
					IList<UserTaskDTO> userTask = await unitOfWork.UserTask.GetDueTaskForTheNextOneWeek();

					if(userTask.Count == 0)
					{
						return NotFound(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "No task available for the next one week"));

					}
					return Ok(new ApiResponse<IList<UserTaskDTO>>(userTask, true));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, ex.Message));;
			}
		
		}

		/// <summary>
		/// Get a list of User Task using it's priority or status
		/// </summary>
		/// <param name = "priority" >task priority</param>
		/// <param name = "status" >task status</param>
		/// <returns> A list user task with the specified priority or status</returns>
		/// <response code = "200" > Returns the User Task with the specified status or priority</response>
		/// <response code = "400" > If status and priority is null</response>
		/// <response code = "404" > If there is no item with the status or priority</response>
		[HttpGet("{priority} {status}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<IList<UserTaskDTO>>>> GetByStatusOrPriority(Priority? priority = null, Status? status = null)
		{
			try
			{
				if(priority is null && status is null)
				{
					return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "Invalid Parameter"));

				}

				IList<UserTaskDTO> userTask = await unitOfWork.UserTask.GetTaskByStatusOrPriority(priority, status);

				if (userTask.Count == 0)
				{
					return NotFound(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "No Task Available"));

				}

				return Ok(new ApiResponse<IList<UserTaskDTO>>(userTask, true, "successfull"));



			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, ex.Message));;
			}
		
		}



		/// <summary>
		/// Get a User Task using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A User Task with the specified id</returns>
		/// <response code = "200" > Returns the User Task with the specified identifier</response>
		/// <response code = "204" > If there is no item with the specified primary identifier</response>
		/// <response code = "400" > If the specified primary identifier is invalid</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<IList<UserTaskDTO>>>> GetAllUsersTask(string userId, int id)
		{
			try
			{
				if (!(id <= 0))
				{
					UserTaskDTO checkUserTask = await unitOfWork.UserTask.GetTaskById(id);

					if(checkUserTask is null)
					{
						return NotFound(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "Not Found"));

					}

					IList<UserTaskDTO> userTask = await unitOfWork.UserTask.GetUnicUserTasks(userId);
					return Ok(new ApiResponse<IList<UserTaskDTO>>(userTask, true));

				}


				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, ex.Message));;
			}
		
		}


		/// <summary>
		/// Retrieve all User Task available on the platform
		/// </summary>
		/// <returns> All the User Task available on the platform</returns>
		/// <response code = "200" > Returns all User Task in the platform</response>
		/// <response code = "204" > If there is item with the specified primary identifier</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskDTO>>>> GetAll()
		{
			IList<UserTaskDTO> userTask = await unitOfWork.UserTask.GetTasks();
			if (userTask.Count == 0)
			{
				return NotFound(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "No Tasks Avalailable"));
			}
			return Ok(new ApiResponse<IList<UserTaskDTO>>(userTask));

		}


		/// <summary>
		/// Get a User Task using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A User Task with the specified id</returns>
		/// <response code = "200" > Returns the User Task with the specified identifier</response>
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
					UserTaskDTO userTask = await unitOfWork.UserTask.GetTaskById(id);

					if (userTask is null)
					{
						return NotFound(new ApiResponse<int>(id, false, "Not Found"));

					}

					//Delete Task
					bool userTaskDeleteResult = await unitOfWork.UserTask.DeleteTask(id);

					//Save Changes
					await unitOfWork.Complete();

					return Ok("Deleted Successfully");
				}


				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, ex.Message));
			}

		}



		/// <summary>
		/// Update an existing User Task
		/// </summary>
		/// <param name="model">User Task model</param>
		/// <returns>An updated User Task  </returns> 
		/// <response code="200">Returns the updated User Task</response>
		/// <response code="400">If an exception occurs</response>     
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserTaskDTO>>> Update(UserTaskDTO model)
		{
			string errorMsg = string.Empty;
			try
			{
				//Validate Model
				if (!validationFactory.UserTasks.ValidateModel(model, true, model.Id, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				//Updat Task
				UserTaskDTO UpdatedTask = await unitOfWork.UserTask.EditTask(model);
				//Save Changes
				await unitOfWork.Complete();



				// return response
				return Ok(new ApiResponse<UserTaskDTO>(UpdatedTask));


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
		/// Update an existing User Task
		/// </summary>
		/// <param name="taskId">User taskId</param>
		/// <param name="status">Task status</param>
		/// <returns>An updated User Task</returns> 
		/// <response code="200">Returns the updated User Task</response>
		/// <response code="400">If an exception occurs</response>     
		[HttpPut("{taskId}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserTaskDTO>>> UpdateTaskStatus(int taskId,Status status)
		{
			string errorMsg = string.Empty;
			try
			{
				//Validate Model
				

				//Updat Task
				UserTaskDTO UpdatedTask = await unitOfWork.UserTask.UpdateTaskStatus(taskId, status);

				//Save Changes
				await unitOfWork.Complete();

				// return response
				return Ok(new ApiResponse<UserTaskDTO>(UpdatedTask));


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
		/// Update an existing User Task
		/// </summary>
		/// <param name = "taskId" >Primary identifier</param>
		/// <param name="projectId">User Task model</param>
		/// <returns>An updated User Task  </returns> 
		/// <response code = "204" > If there is no item with the specified primary identifier</response>        
		/// <response code="201">Returns the updated User Task</response>
		/// <response code="400">If an exception occurs</response>     
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<UserTaskDTO>>> AssignTaskToProject(int taskId, int projectId)
		{
			string errorMsg = string.Empty;
			try
			{
				//Validate Model
				if(!(taskId <= 0) || !(projectId <= 0))
				{
					//Verify if project exist.
					ProjectDTO project = await unitOfWork.Project.GetProjectById(projectId);
					if(project == null)
					{
						return NotFound(new ApiResponse<UserTaskDTO>(new UserTaskDTO(), false, "Project does not exist"));

					}

					//Assign Task to project
					UserTaskDTO UpdatedTask = await unitOfWork.UserTask.AssignTaskToProject(taskId, projectId);

					//Save Changes
					await unitOfWork.Complete();

					// return response
					return Ok(new ApiResponse<UserTaskDTO>(UpdatedTask));

				}

				return BadRequest("Invalid route parmeter");

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
