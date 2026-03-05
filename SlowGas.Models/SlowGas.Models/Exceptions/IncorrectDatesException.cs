namespace SlowGas.Models.Exceptions
{
    public class IncorrectDatesException : Exception
    {
        public IncorrectDatesException(DateTime start, DateTime end)
            : base($"Start date {start} must be before end date {end}") { }
    }
}