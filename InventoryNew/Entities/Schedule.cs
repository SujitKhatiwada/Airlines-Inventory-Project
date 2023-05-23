namespace InventoryNew.Entities
{
    public class Schedule
    {
        public int FlightNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int Day { get; set; }
        public bool IsLoaded { get; set; }

        public override string ToString()
        {
            //use string interpolation
            return $"{FlightNumber}. Departure: {Departure},  Arrival: {Arrival}, Day: {Day}";
        }
    }
}
