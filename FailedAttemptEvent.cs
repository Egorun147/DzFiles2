namespace Dzshka2
{
    public class FailedAttemptEvent : IJournalEntry
    {
        public string Reason { get; }

        public FailedAttemptEvent(string reason)
        {
            Reason = reason;
        }

        public string ToLogLine()
        {
            return Reason;
        }

        public string ToScreenLine()
        {
            return $"Неудача | {Reason}";
        }
    }
}