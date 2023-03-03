using System;
using System.Diagnostics;
using System.Runtime.Serialization;

[Serializable]
public class ResourceException : Exception
{
    public ResourceException()
    {
    }

    public ResourceException(string message) : base(message)
    {
        Debug.Print("Resource Exception: " + message);
    }

    public ResourceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ResourceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}