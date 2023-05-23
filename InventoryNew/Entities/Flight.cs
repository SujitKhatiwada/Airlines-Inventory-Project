namespace InventoryNew.Entities
{
    public class Flight
    {
        //use auto properties
        public int Capacity { get; private set; }
        public Schedule Schedule { get; private set; }
        public IList<Order> Orders { get; set; }

        public Flight(int capacity, Schedule schedule)
        {
            Capacity = capacity;
            schedule.IsLoaded = true;
            Schedule = schedule;
            Orders = new List<Order>();
        }

        public string FlightSchedule()
        {
            //use string interpolation
            return $"Flight: {Schedule.FlightNumber}, departure: {Schedule.Departure}, arrival: {Schedule.Arrival}, day: {Schedule.Day}";
        }
    }
}
