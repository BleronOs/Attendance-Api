using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Interfaces
{
    public interface IDashBoardService
    {
        Task<int> GetEmployeeCountByManagerAsync(int managerId, CancellationToken cancellationToken = default);
        Task<int> GetPasiveEmployeeCountByManagerAsync(int managerId, CancellationToken cancellationToken = default);
        Task<int> GetCardStatusFalseByManagerAsync(int managerId, CancellationToken cancellationToken = default);
        Task<int> GetCardStatusTrueByManagerAsync(int managerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync(int managerId, DateTime dateTime, CancellationToken cancellationToken = default);

    }
}
