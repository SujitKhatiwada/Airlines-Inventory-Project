using InventoryNew.Entities;

namespace InventoryNew.Interfaces
{
    interface IScheduleRepository
    {
        IList<Schedule> GetSchedules();
    }
}
