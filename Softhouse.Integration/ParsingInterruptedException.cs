using System;
using System.Runtime.Serialization;

namespace Softhouse.Integration
{
  [Serializable]
  public class ParsingInterruptedException : Exception
  {
    public ParsingInterruptedException()
    {
    }

    public ParsingInterruptedException(string message) : base(message)
    {
    }

    public ParsingInterruptedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ParsingInterruptedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}