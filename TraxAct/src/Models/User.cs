using SQLite;

namespace TraxAct.Models
{

	[Table("user")]
	public class User
	{
		[PrimaryKey]
		[Column("user_id")]
		public string UserId { get; set; }
	}
}