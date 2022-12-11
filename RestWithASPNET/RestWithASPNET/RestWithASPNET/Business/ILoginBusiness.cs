using RestWithASPNET.Data.VO;

namespace RestWithASPNET.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidadeteCredentials(UserVO user);
        TokenVO ValidadeteCredentials(TokenVO token);

        public bool RevokeToken(string username);
    }
}
