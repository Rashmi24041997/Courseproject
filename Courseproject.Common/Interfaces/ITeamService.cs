using Courseproject.Common.Dtos.Teams;

namespace Courseproject.Common.Interfaces;

public interface ITeamService
{
    Task<int> CreateTeamAsync(TeamCreate teamCreate);
    Task UpdateTeamAsync(TeamUpdate teamUpdate);
    Task<List<TeamGet>> GetTeamsAsync();
    Task<TeamGet> GetTeamAsync(int id);
    Task DeleteTeamAsync(TeamDelete teamDelete);
}
