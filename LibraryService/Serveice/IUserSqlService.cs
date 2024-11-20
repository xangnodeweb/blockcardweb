using static LibraryService.Serveice.UserSqlService;

namespace LibraryService.Serveice
{
    public interface IUserSqlService
    {
        Task getUser();
        Task<List<UserLoginModel>> getAsync(string sql);
        string Decrpt(string encryptText);
        string Encrypt(string sInputText);
        Task Loginuser(UserLoginModel request);
    }
}