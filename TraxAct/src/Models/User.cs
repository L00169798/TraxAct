using SQLite;

namespace TraxAct.Models
{
	/// <summary>
	/// User class properties
	/// </summary>
	[Table("user")]
	public class User
	{
		[PrimaryKey]
		[Column("user_id")]
		public string UserId { get; set; }
	}
}