using SQLite;

namespace TraxAct.Models
{
    [Table("event")]
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        [Column("event_id")]
        public int EventId { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("event_title")]
        public string Title { get; set; } 

        [Column("exercise_type")]
        public string ExerciseType { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("distance")]
        public double Distance { get; set; }

        [Column("sets")]
        public int Sets { get; set; }

        [Column("reps")]
        public int Reps { get; set; }

		}

	}


