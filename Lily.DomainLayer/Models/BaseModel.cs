using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.Models
{

	/// <summary>
	/// The base model. 
	/// </summary>
	/// <remarks>Contains all the properties that can be found in all models</remarks>
	#nullable disable
	public class BaseModel
    {
		/// <summary>
		/// Primary Identifier. 
		/// </summary> 
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// The Name 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The IsActive 
		/// </summary>
		public bool IsActive { get; set; }


    }
}
