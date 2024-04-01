using SQLite;

namespace TraxAct.Models
{
    [Table("ActivityColour")]
    public class ActivityColour
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string ExerciseType { get; set; }

        public string Colour { get; set; }
    }
}
