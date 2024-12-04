using LibraryServices.Model;

namespace LibraryServices
{
    public interface IUserSqlService
    {
        string Decrpt(string encryptedText);
        string Encrypt(string sInputText);
        Task<DefaultReponse<List<UserModel>>> getAsync(string sql);
        Task<List<Province>> getProvince(string sql);
        Task<VoucherReportReponse> getQueryVoucherReport(string sql);
        Task<List<Supplier>> getsupplierddialog(string sql);
        Task<List<Supplier>> getSupplierTolist(string sql);
        Task getUser();
        Task<UserloginResponse> Loginuser(UserLogin request);
        Task<DefaultReponse<UserModel>> refreshPassword(UserRerefreshPassword request);
    }
}