using Courseproject.Common.Dtos.Employee;
using Courseproject.Common.Model;

namespace Courseproject.Common.Interfaces;

public interface IEmployeeService
{
    Task<int> CreateEmployeeAsync(EmployeeCreate employeeCreate);
    Task UpdateEmployeeAsync(EmployeeUpdate employeeUpdate);
    Task UpdateProfilePhotoAsync(ProfilePhotoUpdate profilePhotoUpdate);
    Task<List<EmployeeList>> GetEmployeesAsnyc(EmployeeFilter employeeFilter);
    Task<EmployeeDetails> GetEmployeeAsync(int id);
    Task DeleteEmployeeAsync(EmployeeDelete employeeDelete);
}
