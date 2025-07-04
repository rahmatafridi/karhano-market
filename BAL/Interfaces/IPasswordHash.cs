using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IPasswordHash
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashpassword, string password);
    }
}
