namespace SlowGas.Models.Exceptions
{
    public class StorageException : Exception
    {
        public StorageException(Exception innerException)
            : base($"Error in storage: {innerException.Message}", innerException) { }
    }
}