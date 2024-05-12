using System;

namespace TraxAct.Models
{
	/// <summary>
	/// Represents exercise data for a specific day.
	/// </summary>
	public class ExerciseByDay
	{
		public DateTime Date { get; set; }

		public double TotalExerciseHours { get; set; }
	}
}
