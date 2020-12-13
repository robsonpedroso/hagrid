using Hagrid.Core.Domain.Contracts.Infrastructure.Repositories;
using Hagrid.Core.Domain.Contracts.Services;
using Hagrid.Core.Domain.Entities;
using Hagrid.Infra.Contracts.Repository;
using Hagrid.Infra.Utils;
using System;
using System.Collections.Generic;
using VO = Hagrid.Core.Domain.ValueObjects;

namespace Hagrid.Core.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IPermissionRepository permissionRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IAccountRoleRepository accountRoleRepository;
        private readonly IResourceRepository resourceRepository;
        private readonly IRestrictionRepository restrictionRepository;

        public RoleService(
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IAccountRoleRepository accountRoleRepository,
            IResourceRepository resourceRepository,
            IRestrictionRepository restrictionRepository)
        {
            this.permissionRepository = permissionRepository;
            this.roleRepository = roleRepository;
            this.accountRoleRepository = accountRoleRepository;
            this.resourceRepository = resourceRepository;
            this.restrictionRepository = restrictionRepository;
        }

        public Role Save(Role role)
        {
            var roleOld = roleRepository.Get(role.Code, role.StoreCode);

            if (!roleOld.IsNull() && !role.Permissions.IsNull())
            {
                permissionRepository.Delete(roleOld.Permissions);
            }

            if (!roleOld.IsNull() && !role.AccountRoles.IsNull())
            {
                accountRoleRepository.Delete(roleOld.AccountRoles);
            }

            if (!roleOld.IsNull() && !role.Restrictions.IsNull())
            {
                restrictionRepository.Delete(roleOld.Restrictions);
            }

            if (!role.Permissions.IsNull())
            {
                role.Permissions.ForEach(p =>
                {
                    p.RoleCode = role.Code;
                    p.SaveDate = DateTime.Now;
                    p.UpdateDate = DateTime.Now;

                    permissionRepository.Save(p);
                });
            }

            if (!role.AccountRoles.IsNull())
            {
                role.AccountRoles.ForEach(a =>
                {
                    a.RoleCode = role.Code;
                    a.SaveDate = DateTime.Now;
                    a.UpdateDate = DateTime.Now;

                    accountRoleRepository.Save(a);
                });
            }

            if (!role.Restrictions.IsNull())
            {
                role.Restrictions.ForEach(a =>
                {
                    a.RoleCode = role.Code;
                    a.SaveDate = DateTime.Now;
                    a.UpdateDate = DateTime.Now;

                    restrictionRepository.Save(a);
                });
            }

            return roleRepository.Save(role);
        }

        public void SaveAll(Guid storeCode)
        {
            var resource = resourceRepository.GetApplicationAccess();
            resource.ForEach(r =>
            {
                var _role = new Role()
                {
                    Code = Guid.NewGuid(),
                    Status = true,
                    UpdateDate = DateTime.Now,
                    SaveDate = DateTime.Now,
                    StoreCode = storeCode,
                    Name = string.Format("{0} {1}", VO.Config.PermissionNameRole, r.Application.Name),
                    Description = "Grupo de Acesso padrão para acesso dos administradores a novas lojas",
                    Type = Enums.RoleType.StoreAdmin,
                    Permissions = new List<Permission>() {
                        new Permission()
                        {
                            Code = Guid.NewGuid(),
                            Operations = Enums.Operations.View,
                            ResourceCode = r.Code,
                            SaveDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            Status = true
                        }
                    }
                };

                _role = roleRepository.Save(_role);
            });
        }

        #region "  IDomainService  "

        public List<IRepository> GetRepositories()
        {
            return new List<IRepository>() {
                permissionRepository,
                roleRepository,
                accountRoleRepository,
                restrictionRepository
            };
        }

        #endregion
    }
}
