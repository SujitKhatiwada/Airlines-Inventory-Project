using static Exercise.Exercise;

namespace Exercise
{
    public interface IFlightRepository
    {
        IList<Flight> GetFlightSchedules();
        Dictionary<string, List<Flight>> GetFlightSchedule();
    }
}