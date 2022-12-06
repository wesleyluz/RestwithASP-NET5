using RestWithASPNET.Data.VO;
using RestWithASPNET.Model;

namespace RestWithASPNET.Repository.UserRep
{
    public interface IUserRepository
    {
        User ValitateCredential(UserVO user);
    }
}
