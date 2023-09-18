using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.BusinessLogic.IValidationFactoryInterface;
using TaskManager.DomainLayer.DTOs;
using TaskManger.BusinessLogic.UnitOfWorks;

namespace TaskManagerApi.Controllers
{
	/// <summary>
	/// Project Controller
	/// </summary>
	/// <remarks>Projects all the endpoints with regards to Project object</remarks>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ProjectController : ControllerBase
	{
		#region Constructor
		private readonly IUnitOfWork unitOfWork;
		private readonly IValidationFactory validationFactory;


		/// <summary>
		/// Responsible for initializing  the validationFactory, unitOfWork objects etc.
		/// </summary>
		/// <param name="unitOfWork">A Project factory object</param>
		/// <param name="validationFactory">A Project repository object</param>
		public ProjectController(IUnitOfWork unitOfWork, IValidationFactory validationFactory)
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
		public async Task<ActionResult<ApiResponse<ProjectDTO>>> Create(ProjectDTO model)
		{
			string errorMsg = string.Empty;
			try
			{

				//Validate Model
				if (!validationFactory.Projects.ValidateModel(model, false, 0, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				// Map ProjectDTO to Project And Call Create method
				ProjectDTO createdProject = await unitOfWork.Project.CreateProject(model);

				//Save Changes
				await unitOfWork.Complete();

				return Ok(new ApiResponse<ProjectDTO>(createdProject));

			}
			catch (DbUpdateException ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<ProjectDTO>(model, false, ex.Message));
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					// Handle exceptions and return an error response
					return BadRequest(new ApiResponse<ProjectDTO>(model, false, ex.InnerException.Message));

				}

				//   Log.Error(ex);
				return BadRequest(new ApiResponse<ProjectDTO>(model, false, ex.Message));
			}

		}


		/// <summary>
		/// Get a Project using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A Project with the specified id</returns>
		/// <response code = "200" > Returns the Project with the specified identifier</response>
		/// <response code = "400" > If there is no item with the specified primary identifier</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ApiResponse<ProjectDTO>>> Get(int id)
		{
			try
			{
				if (!(id <= 0))
				{
					ProjectDTO project = await unitOfWork.Project.GetProjectById(id);

					if (project is null)
					{
						return NotFound(new ApiResponse<ProjectDTO>(new ProjectDTO(), false, "Not Found"));

					}
					return Ok(new ApiResponse<ProjectDTO>(project, true));

				}


				return BadRequest(new ApiResponse<ProjectDTO>(new ProjectDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<ProjectDTO>(new ProjectDTO(), false, ex.Message)); ;
			}

		}






		/// <summary>
		/// Retrieve all Project available on the platform
		/// </summary>
		/// <returns> All the Project available on the platform</returns>
		/// <response code = "200" > Returns all project in the platform</response>
		/// <response code = "204" > If there is item with the specified primary identifier</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDTO>>>> GetAll()
		{
			IList<ProjectDTO> projects = await unitOfWork.Project.GetProjects();
			if (projects.Count == 0)
			{
				return NotFound(new ApiResponse<ProjectDTO>(new ProjectDTO(), false, "No User Project available"));

				
			}
			return Ok(new ApiResponse<IList<ProjectDTO>>(projects));

		}


		/// <summary>
		/// Get a project using it's primary identifier
		/// </summary>
		/// <param name = "id" >Primary identifier</param>
		/// <returns> A projects with the specified id</returns>
		/// <response code = "200" > Returns the project with the specified identifier</response>
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
					ProjectDTO project = await unitOfWork.Project.GetProjectById(id);

					if (project is null)
					{
						return NotFound(new ApiResponse<int>(id, false, "Not Found"));

					}

					UserTaskDTO userTask = await unitOfWork.UserTask.GetTaskInProject(id);
					if(userTask is not null)
					{
						return BadRequest(new ApiResponse<int>(id, false, "unable to delete project: has task assigned to it.."));
					}

					bool userTaskDeleteResult = await unitOfWork.Project.DeleteProject(id);

					//Save Changes
					await unitOfWork.Complete();

					return Ok("Deleted Successfully");
				}


				return BadRequest(new ApiResponse<ProjectDTO>(new ProjectDTO(), false, "Invalid Parameter"));

			}
			catch (Exception ex)
			{

				// Handle exceptions and return an error response
				return BadRequest(new ApiResponse<ProjectDTO>(new ProjectDTO(), false, ex.Message));
			}

		}



		/// <summary>
		/// Update an existing User Task
		/// </summary>
		/// <param name="model">Project model</param>
		/// <returns>An updated Project  </returns> 
		/// <response code="200">Returns the updated Project</response>
		/// <response code="400">If an exception occurs</response>     
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<ProjectDTO>>> Update(ProjectDTO model)
		{
			string errorMsg = string.Empty;
			try
			{
				//Validate Model
				if (!validationFactory.Projects.ValidateModel(model, true, model.Id, out errorMsg))
				{
					return BadRequest(errorMsg);
				}

				//Updat Task
				ProjectDTO UpdatedTask = await unitOfWork.Project.EditProject(model);


				//Save Changes
				await unitOfWork.Complete();

				// return response
				return Ok(new ApiResponse<ProjectDTO>(UpdatedTask));


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
