
namespace SchedulerService.Domain.ScheduledTaskAggregate.ValueObjects
{
    public record ScheduledTime
    {
        public DateTime Value { get; }

        public ScheduledTime(DateTime value)
        {

            Value = NormalizeToMinutes(value);
        }

        private static DateTime NormalizeToMinutes(DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, DateTimeKind.Utc);

        public static implicit operator ScheduledTime(DateTime value)
            => new(value);

        public static implicit operator DateTime(ScheduledTime value)
            => value.Value;
    }
}
