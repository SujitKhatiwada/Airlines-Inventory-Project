using System;
using System.Linq;
using System.Web;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using DocumentFormat.OpenXml;

// Written By: Sujit Khatiwada , 20023-05-12
// Tool Used: Visual Studio Code 
namespace Exercise
{
    public class Exercise
    {
        private static readonly int FLIGHT_INVENTORY_CAPACITY = 2;
        public static void Main(string[] args)
        {

            var flightDetails = new FlightRepository().GetFlightSchedules().OrderBy(x => x.Day).ToList();

            //list out the loaded flight schedule on the console
            ListFlightSchedules(flightDetails);

            //Generate flight itineraries by scheduling a batch of orders.
            GetFlightItineraries(flightDetails);
        }

        private static void ListFlightSchedules(List<Flight> flightDetails)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("User Story 1: Loaded Flight Schedules.");
            Console.WriteLine();
            foreach (var arrivingFlight in flightDetails)
            {
                Console.WriteLine(arrivingFlight.ToString());
            }
        }


        /// <summary>
        /// Flight Iteneraries 
        /// </summary>
        private static void GetFlightItineraries(List<Flight> flightDetails)
        {
            Console.WriteLine();
            Console.WriteLine("User Story 2: Generated flight itenaries by scheduling a batch of orders.");
            //O(NlogN)
            var orders = new OrderRepository().GetOrderInfoFromJSON().OrderBy(x =>x.Service);
            Console.WriteLine();
            foreach (var order in orders)
            {
                //O(N)*O(F) F-Flights
                var seletedFlight = flightDetails.FirstOrDefault(x => !x.Loaded && order.Destination == x.FlightCode);
                var res = "";
                if (seletedFlight != null)
                {
                    if (seletedFlight.Orders != null && seletedFlight.Orders.Count == (FLIGHT_INVENTORY_CAPACITY - 1))
                    {
                        seletedFlight.Loaded = true;
                    }
                    seletedFlight.Orders.Add(order);
                    res = $"order: {order.OrderName}, flightNumber: {seletedFlight.FlightNumber}, departure: {seletedFlight.DepartureFlightCode}, arrival: {seletedFlight.FlightCode}, day: {seletedFlight.Day}";
                }
                else
                {
                    res = $"order: {order.OrderName}, flightNumber: not scheduled";
                }
                Console.WriteLine(res);
            }
        }

        #region Helper Class

        /// <summary>
        /// Flight information 
        /// </summary>
        public class Flight
        {
            public Flight(int flightNumber, string flightCity, string flightCode, string departureCity, string departureFlightCode, int day, bool loaded)
            {
                FlightNumber = flightNumber;
                FlightCity = flightCity;
                FlightCode = flightCode;
                DepartureCity = departureCity;
                DepartureFlightCode = departureFlightCode;
                Day = day;
                Loaded = loaded;
                Orders = new List<Order>();
            }
            public int FlightNumber { get; set; }
            public int Day { get; set; }
            public bool Loaded { get; set; }
            public string FlightCity { get; set; }
            public string FlightCode { get; set; }
            public string DepartureCity { get; set; }
            public string DepartureFlightCode { get; set; }
            public IList<Order> Orders { get; set; }

            public int OrdersCount { get; set; } = 0;

            //Not respons
            public override string ToString()
            {
                return $"Flight: {FlightNumber}, Departure: {DepartureFlightCode},  Arrival: {FlightCode}, Day: {Day}";
            }
        }

        public class Order
        {
            [JsonPropertyName("destination")]
            public string Destination { get; set; }

            [JsonPropertyName("service")]
            public string Service { get; set; }

            public string OrderName { get; set; }

            public int Priority { get; set; }
        }

        //public Enum Service : int
        //{
        //    [StringValue("a")]
        //    SameDay = 1;
        //    NextDay = 2;
        //    Regular = 3;
        //}

        #endregion

        #region Repositories

        /// <summary>
        /// Obtain flight details for given days
        /// </summary>
        class FlightRepository : IFlightRepository
        {
            public IList<Flight> GetFlightSchedules()
            {
                var flightNo = 1;
                var day = 1;
                var flights = new List<Flight>
                {
                    //constructor overloading
                    new Flight(flightNo++, "Toronto", "YYZ", "Montreal", "YUL", day, false),
                    new Flight(flightNo++, "Calgary", "YYC", "Montreal", "YUL", day, false),
                    new Flight(flightNo++, "Vancouver", "YVR", "Montreal", "YUL", day, false)
                };

                day++;
                flights.Add(new Flight(flightNo++, "Toronto", "YYZ", "Montreal", "YUL", day, false));
                flights.Add(new Flight(flightNo++, "Calgary", "YYC", "Montreal", "YUL", day, false));
                flights.Add(new Flight(flightNo++, "Vancouver", "YVR", "Montreal", "YUL", day, false));

                return flights;
            }

            public Dictionary<string, List<Flight>> GetFlightSchedule() 
            {
                var flightNo = 1;
                var day = 1;
                var flights = new List<Flight>
                {
                    //constructor overloading
                    new Flight(flightNo++, "Toronto", "YYZ", "Montreal", "YUL", day, false),
                    new Flight(flightNo++, "Calgary", "YYC", "Montreal", "YUL", day, false),
                    new Flight(flightNo++, "Vancouver", "YVR", "Montreal", "YUL", day, false)
                };

                day++;
                flights.Add(new Flight(flightNo++, "Toronto", "YYZ", "Montreal", "YUL", day, false));
                flights.Add(new Flight(flightNo++, "Calgary", "YYC", "Montreal", "YUL", day, false));
                flights.Add(new Flight(flightNo++, "Vancouver", "YVR", "Montreal", "YUL", day, false));


                var flightDict = new Dictionary<string, List<Flight>>();
                foreach (Flight flight in flights)
                {
                    if (!flightDict.ContainsKey(flight.FlightCode))
                    {
                        flightDict.Add(flight.FlightCode, new List<Flight>());
                    }
                    flightDict[flight.FlightCode].Add(flight);
                }
                return flightDict;
            }
        }

        /// <summary>
        /// Obtain list of orders from JSON file provided
        /// </summary>
        class OrderRepository : IOrderRepository
        {
            public IList<Order> GetOrderInfoFromJSON()
            {
                //using (StreamReader r = new StreamReader())
                //{
                    try
                    {
                        //string json = r.ReadToEnd();
                        string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "coding-assigment-orders-part-two.json"));
                        var order = JsonConvert.DeserializeObject<Dictionary<string, Order>>(json)?.Select(p =>
                                    new Order { OrderName = p.Key, Service = p.Value.Service, Destination = p.Value.Destination, Priority = int.Parse(p.Key.Substring(p.Key.LastIndexOf('-') + 1)) })?.ToList();
                        //Order By priority 
                        return order?.OrderBy(x => x.Priority)?.ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("But problem deserealizing the JSON file");
                        Console.WriteLine($"{ex.Message}");
                        return null;
                    }

                //}
            }
        }

        #endregion
    }

    public Enum Service : int
    {
        [StringValue("a")]
        SameDay = 1,

        NextDay = 2,
        Regular = 3
    }
}

//Dictionary Way

//O(1)
// Dictionary<Key,Value>
//Key: Flight Destination,
//Value: List of flights ordered by Day

//var flightDict = new FlightRepository().GetFlightSchedule();
//foreach (var order in orders)
//{
//    Flight seletedFlight = null;
//    var res = "";
//    if (flightDict.ContainsKey(order.Destination))
//    {
//        seletedFlight = flightDict[order.Destination][0];
//        seletedFlight.OrdersCount++;
//        if (seletedFlight.OrdersCount == FLIGHT_INVENTORY_CAPACITY)
//        {
//            flightDict[order.Destination].RemoveAt(0); // Removes first item O(n)  = number of inner list flights in each key
//            if (flightDict[order.Destination].Count == 0)
//            {
//                flightDict.Remove(order.Destination);
//            }
//        }
//        res = $"order: {order.OrderName}, flightNumber: {seletedFlight.FlightNumber}, departure: {seletedFlight.DepartureFlightCode}, arrival: {seletedFlight.FlightCode}, day: {seletedFlight.Day}";
//    }
//    else 
//    {
//        res = $"order: {order.OrderName}, flightNumber: not scheduled";
//    }
//    Console.WriteLine(res);
//}





