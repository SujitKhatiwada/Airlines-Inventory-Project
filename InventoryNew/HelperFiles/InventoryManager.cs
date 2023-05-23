using InventoryNew.Entities;
using InventoryNew.Repositories;

namespace InventoryNew.HelperFiles
{
    public class InventoryManager
    {
        public IList<Order> Orders { get; private set; }
        public IList<Flight> FlightsScheduled { get; private set; }
        public IList<Schedule> Schedules { get; private set; }


        public InventoryManager()
        {
            FlightsScheduled = new List<Flight>();
            Schedules = new ScheduleRepository().GetSchedules();
        }

        public void ProcessFlightScheduleMenuUserChoice(int userChoice)
        {
            if (userChoice > 0 && userChoice <= Schedules.Count)
            {
                var selectedSchedule = Schedules.FirstOrDefault(s => !s.IsLoaded && s.FlightNumber == userChoice);
                if (selectedSchedule != null)
                {
                    var scheduledFlight = new Flight(20, selectedSchedule);
                    FlightsScheduled.Add(scheduledFlight);
                    FlightsScheduled = FlightsScheduled.OrderBy(f => f.Schedule.FlightNumber).ToList();
                    DisplayScheduleLoadedMessage(selectedSchedule);
                }
            }
        }

        public void DisplayScheduleLoadedMessage(Schedule schedule)
        {
            var menu = new Menu()
            {
                Items = new List<string>()
            {
                $"{LoadedMessage(schedule)}"
            }
            };

            new InformationManager().DisplayAndRead(menu);
        }

        public void DisplayLoadedSchedules()
        {
            var menu = new Menu()
            {
                Header = "\n======= Loaded schedules =======\n"
            };

            foreach (var flight in FlightsScheduled)
            {
                menu.Items.Add(flight.FlightSchedule());
            }

            new InformationManager().DisplayAndRead(menu);
        }

        public void DisplayFlightItineraries()
        {
            LoadOrdersInFlights();

            var menu = new Menu()
            {
                Header = "\n======= Flight itineraries =======\n"
            };

            foreach (var order in Orders)
            {
                menu.Items.Add(Itinerary(order));
            }

            new InformationManager().DisplayAndRead(menu);
        }

        private void LoadOrdersInFlights()
        {
            Orders = new OrderRepository().GetOrders().OrderBy(o => o.Priority).ToList();

            foreach (var schedule in Schedules)
            {
                if (schedule.IsLoaded)
                {
                    var loadedFlights = FlightsScheduled.Where(f => f.Schedule == schedule).ToList();

                    foreach (var flight in loadedFlights)
                    {
                        var flightOrders = Orders.Where(o => o.IsNotLoaded() && o.Destination == schedule.Arrival).Take(flight.Capacity).Select(o => { o.Schedule = schedule; return o; }).ToList();
                        flight.Orders = flightOrders;
                    }
                }
            }
        }

        public Menu GetFlightScheduleMenu()
        {
            var menu = new Menu
            {
                Header = "Choose a schedule to load"
            };

            foreach (var item in Schedules)
            {
                if (!item.IsLoaded)
                {
                    menu.Items.Add(item.ToString());
                }
            }

            menu.ExitValue = Schedules.Count + 1;
            menu.Items.Add($"{menu.ExitValue}. Main menu");

            return menu;
        }

        private static string LoadedMessage(Schedule schedule)
        {
            return $"Schedule {schedule.FlightNumber} loaded";
        }

        private static string Itinerary(Order order)
        {
            return order.IsNotLoaded() ? $"order: {order.Code}, flightNumber: not scheduled"
                : $"order: {order.Code}, flightNumber: {order.Schedule.FlightNumber}, departure: {order.Schedule.Departure}, arrival: {order.Schedule.Arrival}, day: {order.Schedule.Day}";
        }
    }
}
