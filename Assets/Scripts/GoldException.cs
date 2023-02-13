using System;
using System.Runtime.Serialization;

[Serializable]
public class GoldException : Exception
{
    public GoldException()
    {
    }

    public GoldException(string message) : base(message)
    {
    }

    public GoldException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected GoldException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}