namespace Dzshka2
{
    public class MovedEvent : IJournalEntry
    {
        public string ItemName { get; }
        public string FromShelf { get; }
        public int FromSlot { get; }
        public string ToShelf { get; }
        public int ToSlot { get; }

        public MovedEvent(string name, string fSh, int fSl, string tSh, int tSl)
        {
            ItemName = name;
            FromShelf = fSh;
            FromSlot = fSl;
            ToShelf = tSh;
            ToSlot = tSl;
        }

        public string ToLogLine()
        {
            return $"{ItemName}|{FromShelf}|{FromSlot}|{ToShelf}|{ToSlot}";
        }

        public string ToScreenLine()
        {
            return $"Перенос | с {FromShelf}:{FromSlot} на {ToShelf}:{ToSlot} | товар «{ItemName}»";
        }
    }
}