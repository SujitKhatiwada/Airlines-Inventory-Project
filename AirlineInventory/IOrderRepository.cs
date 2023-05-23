using static Exercise.Exercise;

namespace Exercise
{
    public interface IOrderRepository
    {
        IList<Order> GetOrderInfoFromJSON();
    }
}