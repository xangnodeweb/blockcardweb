using LibraryServices.Model;

namespace LibraryServices
{
    public interface IUserSqlService
    {
        string Decrpt(string encryptedText);
        string Encrypt(string sInputText);
        Task<List<UserModel>> getAsync(string sql);
        Task getUser();
        Task<UserloginResponse> Loginuser(UserLogin request);
    }
}