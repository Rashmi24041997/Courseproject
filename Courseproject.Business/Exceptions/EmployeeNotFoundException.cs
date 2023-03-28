using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions;

[Serializable]
public class EmployeeNotFoundException : Exception
{
    public int Id { get; }

    public EmployeeNotFoundException()
    {
    }

    public EmployeeNotFoundException(int id)
    {
        Id = id;
    }

    public EmployeeNotFoundException(string? message) : base(message)
    {
    }

    public EmployeeNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EmployeeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}