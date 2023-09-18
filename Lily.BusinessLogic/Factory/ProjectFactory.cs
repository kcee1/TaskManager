using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DomainLayer.DTOs;
using TaskManager.DomainLayer.Models;

namespace TaskManager.BusinessLogic.Factory
{

	/// <summary>
	/// Project Factory
	/// </summary>
	/// <remarks>Responsible for handling action which have to with validating Project DTO</remarks>
	public class ProjectFactory
	{
		/// <summary>
		/// Validate Project model
		/// </summary>
		/// <param name="model">the model</param>
		/// <param name="isUpdate">Check if is action to be done is an update</param>
		/// <param name="id">The Id of the object to be updated</param>
		/// <param name="errorMsg">error message containing the reason why model is invalid</param>
		/// <returns>True/False</returns>
		public bool ValidateModel(ProjectDTO model, bool isUpdate, long id, out string errorMsg)
		{
			errorMsg = string.Empty;
			if (model == null)
			{
				errorMsg = "Invalid Model";
				return false;
			}

			if (isUpdate)
			{
				if (model.Id != id)
				{
					errorMsg = "Invalid Route";
					return false;
				}
			}

			if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Name?.Trim()))
			{
				errorMsg = "Project Name is required";
				return false;
			}

			if (string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.Description?.Trim()))
			{
				errorMsg = "Project Description is required";
				return false;
			}

			return true;
		}

	}
}
