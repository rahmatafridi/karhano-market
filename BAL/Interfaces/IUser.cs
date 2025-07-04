using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IUser
    {
        List<UserVM> GetUsers();

        bool AddUser(UserVM user);
        UserVM GetUserById(Guid Id);
        UserVM GetUserByEmail(string email);
        bool UpdateUser(UserVM user);
        bool DeleteUser(Guid Id);


    }
}
