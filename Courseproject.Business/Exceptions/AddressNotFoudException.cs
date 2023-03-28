using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions;

[Serializable]
public class AddressNotFoudException : Exception
{
    public int Id { get; }

    public AddressNotFoudException()
    {
    }

    public AddressNotFoudException(int id)
    {
        Id = id;
    }

    public AddressNotFoudException(string? message) : base(message)
    {
    }

    public AddressNotFoudException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected AddressNotFoudException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}