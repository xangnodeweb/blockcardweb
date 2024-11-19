
using static LibraryService.Models.UserSqlService;

namespace LibraryService.Models
{
    public interface IUserSqlService
    {
        Task getUser();
        Task<List<UserLoginModel>> getAsync(string sql);
        string Decrpt(string encryptText);
    }
}