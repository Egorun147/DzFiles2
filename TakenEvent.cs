namespace Dzshka2
{
    public class TakenEvent : IJournalEntry
    {
        public string ItemName { get; }
        public int Slot { get; }
        public string ShelfName { get; }

        public TakenEvent(string name, int slot, string shelf)
        {
            ItemName = name;
            Slot = slot;
            ShelfName = shelf;
        }

        public string ToLogLine()
        {
            return $"{ItemName}|{Slot}|{ShelfName}";
        }

        public string ToScreenLine()
        {
            return $"Изъятие | полка {ShelfName} | слот {Slot} | товар «{ItemName}»";
        }
    }
}