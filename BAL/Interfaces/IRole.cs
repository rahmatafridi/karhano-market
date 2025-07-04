using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IRole
    {
        List<RoleVM> GetRoles();
        RoleVM GetRoleById(Guid id);
        bool AddRole (RoleVM role); 
        bool UpdateRole(RoleVM role);
        bool DeleteRole(Guid Id);

    }
}
