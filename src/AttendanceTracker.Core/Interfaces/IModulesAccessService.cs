using System;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Interfaces
{
	public interface IModulesAccessService
	{
        Task<bool> AddModuleAccessAsync(string role, int module,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ModulesAccess>> GetModulesAccessesAsync(CancellationToken cancellationToken = default);
        Task<bool> UpdateInsertModuleAccessAsync(string roleid, int modulid, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ModulesAccess>> GetModulesAccessForRoleAsync(string roleId, CancellationToken cancellationToken = default);

    }
}

