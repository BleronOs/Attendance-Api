using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.ResultModels;

namespace AttendanceTracker.Core.Interfaces
{
	public interface IManagerService
	{
        Task<IReadOnlyList<Manager>> GetManagersAsync(CancellationToken cancellationToken = default);
        Task<bool> AddManagersAsync(int employeeID,bool status ,CancellationToken cancellationToken = default);
        Task<bool> UpdateManagerStatusToFalseAsync(int id, CancellationToken cancellationToken);
        Task<bool> UpdateManagersAsync(int id, int employeeID,bool status ,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Manager>> GetEmployeeManagmentAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Manager>> GetManagersWithStatusActiveAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Manager>> GetManagersWithStatusPassiveAsync(CancellationToken cancellationToken = default);

    }
}

