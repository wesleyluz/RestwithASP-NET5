using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;
using RestWithASPNET.Model.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RestWithASPNET.Repository.UserRep
{
    public class UserRepository : IUserRepository
    {
        private readonly MySqlContext _context;


        public UserRepository(MySqlContext context)
        {
            _context = context;

        }

        public User ValitateCredential(string username)
        {
            return _context.Users.SingleOrDefault(u => (u.UserName == username));
        }
        public User ValitateCredential(UserVO user)
        {
            var pass = ComputeHash(user.Password, new SHA256CryptoServiceProvider());
            return _context.Users.FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == pass));
        }
        public bool RevokeToken(string username)
        {
            var user = _context.Users.SingleOrDefault(u => (u.UserName == username));
            if (user == null) return false;
            user.RefreshToken = null;
            _context.SaveChanges();
            return true;
        }
        public User RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(u => u.Id.Equals(user.Id))) return null; 

            var result = _context.Users.SingleOrDefault(p => p.Id.Equals(user.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;

        }

        private string ComputeHash(string input, SHA256CryptoServiceProvider algorithm)
        {
            Byte[] inputsBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputsBytes);
            return BitConverter.ToString(hashedBytes);

        }

       
    }
}
