using System;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Interfaces
{
	public interface ICheckService
	{
        Task<IReadOnlyList<Check>> GetChecksAsync(CancellationToken cancellationToken = default);
        Task<bool> AddCheckAsync(DateTime checkDate, int cardId, string? adminId, string? note, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetTodaysChecksAsync(DateTime dateTime, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetTodaysChecksCalculatedAsync(DateTime dateTime, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetChecksByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetTodayChecksByEmployeeIdAsync(DateTime dateTime, int employeeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetEmployeeChecksByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetEmployeeChecksByEmployeeIdAndSpecificDateTimeAsync(DateTime dateTime, int employeeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetTodayEmployeeChecksByEmployeeIdAsync(DateTime dateTime, int employeeId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Check>> GetWeekChecksByEmployeeIdAsync(DateTime dateTime, int employeeId, CancellationToken cancellationToken = default);
        Task<bool> GetEmployeeinChecksListAsync(int employeeId, CancellationToken cancellationToken = default);
    }
}

