namespace Courseproject.Common.Model;

public class Job : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<Employee> Employees { get; set; } = default!;
}
