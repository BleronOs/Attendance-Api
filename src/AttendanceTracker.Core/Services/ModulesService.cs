using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Interfaces;
namespace AttendanceTracker.Core.Services
{
	public class ModulesService:IModulesService
	{
		private readonly IAsyncRepository<Modules> _modulesRepository;
		public ModulesService(IAsyncRepository<Modules> modulesRepository)
		{
			_modulesRepository = modulesRepository;
		}
        public async Task<IReadOnlyList<Modules>> GetModulesAsync(CancellationToken cancellationToken = default)
        {
            var modulesList = await _modulesRepository.ListAllAsync();
            return modulesList;
        }
    }
}

