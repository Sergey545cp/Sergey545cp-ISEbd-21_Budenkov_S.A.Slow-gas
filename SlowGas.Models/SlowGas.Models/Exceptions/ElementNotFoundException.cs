namespace SlowGas.Models.Exceptions
{
    public class ElementNotFoundException : Exception
    {
        public string Value { get; }

        public ElementNotFoundException(string value)
            : base($"Element not found: {value}")
        {
            Value = value;
        }
    }
}