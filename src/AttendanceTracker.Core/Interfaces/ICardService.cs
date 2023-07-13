using System;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Interfaces
{
	public interface ICardService
	{
        Task<IReadOnlyList<Card>> GetCardsAsync(CancellationToken cancellationToken = default);
        Task<bool> AddCardsAsync(string cardId, bool stat, int employeeId, string notes, CancellationToken cancellationToken = default);
        Task<bool> UpdateCardStatusFalseAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateCardAsync(int id, string cardRef, bool stat, int employeeId, string notes, CancellationToken cancellationToken);
        Task<Card> GetActiveCardByEmployeeIdAsync(int employeeId, bool isReadonly, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Card>> GetCardWithStatusFalseAsync(CancellationToken cancellationToken = default);
        Task<bool> UpdateCardStatusTrueAsync(int id, int employeeId, string cardRef, bool status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Card>> GetCardByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<bool> UpdateCardStatusIfEmployeeIsActiveAsync(int id, int employeeId,bool statusi ,string cardRefId,CancellationToken cancellationToken);
        Task<bool> UpdateCardWhenEmployeeIsPassiveAsync(int employeeId, CancellationToken cancellationToken);
        }
}
