using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.ResultModels;
using AttendanceTracker.Core.Specifications;

namespace AttendanceTracker.Core.Services
{
	public class ModulesAccessService:IModulesAccessService
	{
		private readonly IAsyncRepository<ModulesAccess> _modulesAccessRepository;
		public ModulesAccessService(IAsyncRepository<ModulesAccess> modulesAccessRepository)
		{
			_modulesAccessRepository = modulesAccessRepository;

        }
        public async Task<bool> AddModuleAccessAsync(string role, int module, CancellationToken cancellationToken = default)
        {
            var created = await _modulesAccessRepository.AddAsync(new ModulesAccess
            {
                RoleId = role,
                ModuleId = module,
                HasAccess = true
            }, cancellationToken);

            return created;
        }
        public async Task<IReadOnlyList<ModulesAccess>> GetModulesAccessesAsync(CancellationToken cancellationToken = default)
        {
            var modulesAccesses = await _modulesAccessRepository.ListAllAsync();
            return modulesAccesses;
        }

        public async Task<IReadOnlyList<ModulesAccess>> GetModulesAccessForRoleAsync(string roleId,CancellationToken cancellationToken = default)
        {
            var getRoleSpecification = new GetModuleAccessTrueWithRoleSpecificaions(roleId);
            
            return await _modulesAccessRepository.ListAsync(getRoleSpecification);
        }


        public async Task<bool> UpdateInsertModuleAccessAsync(string roleid,int modulid, CancellationToken cancellationToken = default)
        {
            //var getIfExist = new findIfExistRowModulesAccessSpecifications(roleid,modulid);
            //var getModuleAccess = await _modulesAccessRepository.FirstOrDefaultAsync(getIfExist, cancellationToken);

            //if (getModuleAccess == null)
            //{
            //    AddModuleAccessAsync(roleid,modulid, cancellationToken);
            //    return true;
            //}


            //if(getModuleAccess != null && getModuleAccess.HasAccess == false)
            //{

            //    getModuleAccess.HasAccess = true;
            //    await _modulesAccessRepository.UpdateAsync(getModuleAccess);
            //    return true;
            //}
            //if (getModuleAccess != null && getModuleAccess.HasAccess == true)
            //{
            //    getModuleAccess.HasAccess = false;
            //    await _modulesAccessRepository.UpdateAsync(getModuleAccess);
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            var getModuleAccess = await _modulesAccessRepository.FirstOrDefaultAsync( new findIfExistRowModulesAccessSpecifications(roleid, modulid), cancellationToken);
            if (getModuleAccess == null)
            {
                await AddModuleAccessAsync(roleid, modulid, cancellationToken);
                return true;
            }

            getModuleAccess.HasAccess = !getModuleAccess.HasAccess;
            await _modulesAccessRepository.UpdateAsync(getModuleAccess);
            return true;
        }

    }
}

