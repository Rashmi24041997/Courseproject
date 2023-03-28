using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions;

[Serializable]
public class TeamNotFoundException : Exception
{
    public int Id { get; }

    public TeamNotFoundException()
    {
    }

    public TeamNotFoundException(int id)
    {
        Id = id;
    }

    public TeamNotFoundException(string? message) : base(message)
    {
    }

    public TeamNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TeamNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}