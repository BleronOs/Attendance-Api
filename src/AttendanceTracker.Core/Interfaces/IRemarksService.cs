using System;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Interfaces
{
	public interface IRemarksService
	{
        Task<IReadOnlyList<Remarks>> GetRemarksAsync(int id,CancellationToken cancellationToken = default);
        Task<bool> AddRemarksAsync(int employeeId, string notes, CancellationToken cancellationToken = default);
    }
}

