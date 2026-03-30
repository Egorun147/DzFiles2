namespace Dzshka2
{
    public class PlacedEvent : IJournalEntry
    {
        public string ItemName { get; }
        public int Slot { get; }
        public string ShelfName { get; }

        public PlacedEvent(string itemName, int slot, string shelfName)
        {
            ItemName = itemName;
            Slot = slot;
            ShelfName = shelfName;
        }

        public string ToLogLine()
        {
            return $"{ItemName}|{Slot}|{ShelfName}";
        }

        public string ToScreenLine()
        {
            return $"Размещение | полка {ShelfName} | слот {Slot} | товар «{ItemName}»";
        }

        public static PlacedEvent FromLogLine(string line)
        {
            string[] p = line.Split('|');
            return new PlacedEvent(p[0], int.Parse(p[1]), p[2]);
        }
    }
}