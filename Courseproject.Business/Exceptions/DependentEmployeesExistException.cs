using Courseproject.Common.Model;
using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions;

[Serializable]
public class DependentEmployeesExistException : Exception
{
    public List<Employee> Employees { get; }

    public DependentEmployeesExistException()
    {
    }

    public DependentEmployeesExistException(List<Employee> employees)
    {
        Employees = employees;
    }

    public DependentEmployeesExistException(string? message) : base(message)
    {
    }

    public DependentEmployeesExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DependentEmployeesExistException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}