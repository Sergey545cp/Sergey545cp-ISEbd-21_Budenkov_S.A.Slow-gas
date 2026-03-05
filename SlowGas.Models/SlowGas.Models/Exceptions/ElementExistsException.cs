namespace SlowGas.Models.Exceptions
{
    public class ElementExistsException : Exception
    {
        public string ParamName { get; }
        public string ParamValue { get; }

        public ElementExistsException(string paramName, string paramValue)
            : base($"Element with {paramName}={paramValue} already exists")
        {
            ParamName = paramName;
            ParamValue = paramValue;
        }
    }
}