using Courseproject.Common.Dtos.Address;
using Courseproject.Common.Dtos.Job;
using Courseproject.Common.Dtos.Teams;

namespace Courseproject.Common.Dtos.Employee;

public record EmployeeDetails(int Id, string FirstName, string LastName, AddressGet Address, JobGet Job, List<TeamGet> Teams, string? ProfilePhotoPath);