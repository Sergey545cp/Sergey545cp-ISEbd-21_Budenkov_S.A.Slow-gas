namespace SlowGas.Models.Exceptions
{
    public class NullListException : Exception
    {
        public NullListException() : base("The returned list is null") { }
        public NullListException(string message) : base(message) { }
    }
}