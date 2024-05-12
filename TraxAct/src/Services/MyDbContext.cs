using SQLite;
using Syncfusion.Maui.Scheduler;
using System.Diagnostics;
using TraxAct.Models;

namespace TraxAct.Services
{
	/// <summary>
	/// Provides methods to interact with the SQLite database.
	/// </summary>
	public class MyDbContext
	{
		const string TraxActDB = "TraxActDB.db3";
		static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, TraxActDB);
		const SQLite.SQLiteOpenFlags Flags =
			SQLite.SQLiteOpenFlags.ReadWrite |
			SQLite.SQLiteOpenFlags.Create |
			SQLite.SQLiteOpenFlags.SharedCache;

		SQLiteAsyncConnection Database;

		private readonly UserService _userService = new UserService();

		/// <summary>
		/// Initializes a new instance of the <see cref="MyDbContext"/> class.
		/// </summary>
		public MyDbContext()
		{
		}

		/// <summary>
		/// Initializes the SQLite database connection and creates necessary tables if they do not exist.
		/// </summary>
		private async Task Init()
		{
			if (Database != null)
			{
				return;
			}

			var databaseFileExists = File.Exists(DatabasePath);
			if (!databaseFileExists)
			{
				Debug.WriteLine($"Database file does not exist at path: {DatabasePath}");
				Database = new SQLiteAsyncConnection(DatabasePath, Flags);
				await Database.CreateTableAsync<Event>();
				await Database.CreateTableAsync<User>();
				Debug.WriteLine($"Database created at path: {DatabasePath}");
			}
			else
			{
				Debug.WriteLine($"Database file exists at path: {DatabasePath}");
				Database = new SQLiteAsyncConnection(DatabasePath, Flags);
			}
		}

		/// <summary>
		/// Retrieves all events associated with a specific user ID.
		/// </summary>
		/// <param name="userId">The ID of the user.</param>
		/// <returns>A list of events.</returns>
		public async Task<List<Event>> GetEventsByUserId(string userId)
		{
			await Init();
			var events = await Database.Table<Event>()
							   .Where(e => e.UserId == userId)
							   .ToListAsync();

			return events;
		}




		/// <summary>
		/// Loads events for the current user and converts them to SchedulerAppointments.
		/// </summary>
		public virtual async Task LoadEventsForCurrentUser()
		{
			try
			{
				string userId = _userService.GetCurrentUserUid();
				if (string.IsNullOrEmpty(userId))
				{
					Debug.WriteLine("User ID is null or empty. Cannot load events.");
					return;
				}

				List<Event> events = await GetEventsByUserId(userId);
				if (events == null || events.Count == 0)
				{
					Debug.WriteLine("No events found for the specified user.");
					return;
				}

				List<SchedulerAppointment> Events = new List<SchedulerAppointment>();

				foreach (var ev in events)
				{
					var schedulerAppointment = new SchedulerAppointment
					{
						Subject = ev.ExerciseType,
						StartTime = ev.StartTime,
						EndTime = ev.EndTime,
						Id = ev.EventId
					};
					Events.Add(schedulerAppointment);
				}

				Debug.WriteLine($"Loaded {Events.Count} events for user ID: {userId}");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error loading events: {ex.Message}");
			}
		}

		/// <summary>
		/// Get event by ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<Event> GetById(int id)
		{
			await Init();
			return await Database.Table<Event>().Where(x => x.EventId == id).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Create Event
		/// </summary>
		/// <param name="newEvent"></param>
		/// <returns></returns>
		public async Task<bool> Create(Event newEvent)
		{
			try
			{
				await Init();

				if (Database == null)
				{
					Console.WriteLine("Error: Database is null.");
					return false;
				}

				await Database.InsertAsync(newEvent);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error saving event: {ex.Message}");

				Exception innerException = ex.InnerException;
				while (innerException != null)
				{
					Console.WriteLine($"Inner Exception: {innerException.Message}");
					innerException = innerException.InnerException;
				}

				if (ex.Message.Contains("format"))
				{
					Console.WriteLine("Error: Data format issue occurred.");
				}
				else
				{
					Console.WriteLine("Error: An unexpected error occurred.");
				}

				return false;
			}
		}

		/// <summary>
		/// Update an event
		/// </summary>
		/// <param name="updatedEvent"></param>
		/// <returns></returns>
		public async Task Update(Event updatedEvent)
		{
			await Init();
			await Database.UpdateAsync(updatedEvent);
		}


		/// <summary>
		/// Delete an event
		/// </summary>
		/// <param name="existingEvent"></param>
		/// <returns></returns>
		public async Task Delete(Event existingEvent)
		{
			await Init();
			await Database.DeleteAsync(existingEvent);
		}


		/// <summary>
		/// Get Events by their start date
		/// </summary>
		/// <param name="startDate"></param>
		/// <returns></returns>
		public async Task<List<Event>> GetEventsByStartDateAsync(DateTime startDate)
		{
			await Init();
			var events = await Database.Table<Event>().ToListAsync();
			return events.FindAll(e => e.StartTime >= startDate);
		}

		/// <summary>
		/// Get events within a date range
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public virtual async Task<List<Event>> GetEventsInRange(DateTime startDate, DateTime endDate)
		{
			await Init();
			var events = await Database.Table<Event>().ToListAsync();
			return events.FindAll(e => e.StartTime >= startDate && e.StartTime <= endDate);
		}

		/// <summary>
		/// Save Firebase User Id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task SaveUserId(string userId)
		{
			try
			{
				await Init();

				var user = new User { UserId = userId };
				await Database.InsertOrReplaceAsync(user);

				Debug.WriteLine($"User ID '{userId}' saved to SQLite database.");
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error saving user ID to SQLite database: {ex.Message}");
				throw;
			}
		}

		/// <summary>
		/// Get Events by time
		/// </summary>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<List<Event>> GetEventsByTimeAsync(DateTime startTime, DateTime endTime, string userId)
		{
			await Init();
			DateTime adjustedEndTime = endTime.Date.AddDays(1).AddTicks(-1);

			var events = await Database.Table<Event>()
									   .Where(e => e.StartTime >= startTime && e.StartTime <= adjustedEndTime && e.UserId == userId)
									   .ToListAsync();

			return events;
		}

	}
}
