namespace InventoryNew.Entities
{
    public class Order
    {
        //use auto properties
        public int Priority { get; set; }
        public string Code { get; set; }
        public string Destination { get; set; }
        public Schedule Schedule { get; set; }

        public bool IsNotLoaded()
        {
            return Schedule == null;
        }
    }
}
