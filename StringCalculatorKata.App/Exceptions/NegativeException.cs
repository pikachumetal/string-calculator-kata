using System;

namespace InitialKata.App.Exceptions
{
    public class NegativeException : Exception
    {
        public NegativeException(string numbers) : base($"negatives not allowed - [{numbers}]") { }
    }
}
