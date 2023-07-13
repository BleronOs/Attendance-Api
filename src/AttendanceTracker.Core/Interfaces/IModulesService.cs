using System;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Interfaces
{
	public interface IModulesService
    {
        Task<IReadOnlyList<Modules>> GetModulesAsync(CancellationToken cancellationToken = default);

    }
}

