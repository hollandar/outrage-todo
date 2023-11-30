using System.Runtime.Serialization;

namespace ConsoleApp1.Settings
{
    [Serializable]
    internal class UnknownStandardException : Exception
    {
        public UnknownStandardException()
        {
        }

        public UnknownStandardException(string? message) : base(message)
        {
        }

        public UnknownStandardException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnknownStandardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}