using InventoryNew.Entities;
using InventoryNew.Interfaces;
using Newtonsoft.Json;

namespace InventoryNew.Repositories
{
    class OrderRepository : IOrderRepository
    {
        public IList<Order> GetOrders()
        {
            var jsonString = ReadAllText("coding-assigment-orders.json");

            var orders = JsonConvert.DeserializeObject<Dictionary<string, Order>>(jsonString).Select(p =>
            new Order { Code = p.Key, Destination = p.Value.Destination, Priority = int.Parse(p.Key.Substring(p.Key.LastIndexOf('-') + 1)) }).ToList();

            return orders;
        }


        public static string ReadAllText(string path)
        {
            return File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), path));
        }
    }
}
