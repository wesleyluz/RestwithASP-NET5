using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Repository.UserRep
{
    public interface IUserRepository
    {
        public User ValitateCredential(UserVO user);
        public User ValitateCredential(string username);

        public bool RevokeToken(string username);
        public User RefreshUserInfo(User user);
    }
}
