namespace InventoryNew.Entities
{
    public class Menu
    {
        public string Header { get; set; }
        public IList<string> Items { get; set; }
        public int ExitValue { get; set; }

        public Menu()
        {
            Items = new List<string>();
        }
    }
}
