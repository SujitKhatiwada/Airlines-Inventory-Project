using InventoryNew.Entities;

namespace InventoryNew.Interfaces
{
    public interface IOrderRepository
    {
        IList<Order> GetOrders();
    }
}
