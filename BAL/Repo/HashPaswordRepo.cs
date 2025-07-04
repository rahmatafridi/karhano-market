using BAL.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repo
{
    public class HashPaswordRepo : IPasswordHash
    {



        public string HashPassword(string password)
        {
            var passwordhash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordhash;
        }
        public bool VerifyPassword(string hashpassword, string password)
        {
            bool isVerify = BCrypt.Net.BCrypt.Verify(password, hashpassword);
            return isVerify;
        }
    }
}
