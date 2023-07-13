using System;
using System.Threading.Tasks;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.ResultModels;

namespace AttendanceTracker.Core.Interfaces
{
	public interface IJobPositionService
	{
        Task<bool> AddJobPositionAsync(string name, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<JobPosition>> GetJobPositionsAsync(CancellationToken cancellationToken = default);
        Task<UpdateJobPositionStatusResult> UpdateJobPositionStatusAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateJobPositionAsync(int id, string jobPositionName, bool status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<JobPosition>> GetJobPositionsStatusTrueAsync(CancellationToken cancellationToken = default);
    }
}

