using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DomainLayer.DTOs
{
#nullable disable
	/// <summary>
	/// The base DTO. 
	/// </summary>
	/// <remarks>Contains all the properties that can be found in all DTO</remarks>
	public class BaseDTO 
    {
		/// <summary>
		/// Primary Identifier. 
		/// </summary> 
		public int Id { get; set; }

		/// <summary>
		/// Name. 
		/// </summary> 
		public string Name { get; set; }
    }
}
