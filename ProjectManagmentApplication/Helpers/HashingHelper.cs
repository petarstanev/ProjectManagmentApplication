using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProjectManagmentApplication.Helpers
{
    public static class HashingHelper
    {

        public static string HashPassword(string password)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = Encoding.ASCII.GetBytes(password);
            byte[] resultBytes = sha.ComputeHash(dataBytes);
            
            return Encoding.ASCII.GetString(resultBytes);
        }
    }
}