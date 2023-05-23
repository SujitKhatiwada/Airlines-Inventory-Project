using InventoryNew.Entities;
using InventoryNew.Interfaces;

namespace InventoryNew.Repositories
{
    class ScheduleRepository : IScheduleRepository
    {
        public IList<Schedule> GetSchedules()
        {
            var flightNo = 1;
            var day = 1;
            var schedules = new List<Schedule>();

            schedules.Add(new Schedule { FlightNumber = flightNo++, Departure = "YUL", Arrival = "YYZ", Day = day, IsLoaded = false });
            schedules.Add(new Schedule { FlightNumber = flightNo++, Departure = "YUL", Arrival = "YYC", Day = day, IsLoaded = false });
            schedules.Add(new Schedule { FlightNumber = flightNo++, Departure = "YUL", Arrival = "YVR", Day = day, IsLoaded = false });

            day++;
            schedules.Add(new Schedule { FlightNumber = flightNo++, Departure = "YUL", Arrival = "YYZ", Day = day, IsLoaded = false });
            schedules.Add(new Schedule { FlightNumber = flightNo++, Departure = "YUL", Arrival = "YYC", Day = day, IsLoaded = false });
            schedules.Add(new Schedule { FlightNumber = flightNo++, Departure = "YUL", Arrival = "YVR", Day = day, IsLoaded = false });

            return schedules;
        }
    }
}
