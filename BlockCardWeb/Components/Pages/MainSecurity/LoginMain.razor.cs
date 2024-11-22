using Blazored.LocalStorage;
using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using MudBlazor;
using System.Security.Cryptography;
using System.Text;
using static LibraryServices.UserSqlService;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class LoginMain
    {
        [Inject] public ILocalStorageService localStorage { get; set; }
        [Inject] public NavigationManager nav { get; set; }
        public UserModel usermodel = new UserModel();
        public UserLogin userrequest = new UserLogin();

        public MudTextField<string> refuser = new MudTextField<string>();
        public MudTextField<string> refpass = new MudTextField<string>();


        InputType passwordinput = InputType.Password;
        public bool? isshow = false;
        public string passwordinputicon = Icons.Material.Filled.VisibilityOff;

        public UserSqlService userSqlService = new UserSqlService();
        public void showpassword()
        {
            if (isshow == true)
            {
                isshow = false;
                passwordinput = InputType.Password;
                passwordinputicon = Icons.Material.Filled.VisibilityOff;
            }
            else
            {
                isshow = true;
                passwordinput = InputType.Text;
                passwordinputicon = Icons.Material.Filled.Visibility;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await userSqlService.getUser();
            //await userSqlService.getAsync("");


        }

        public async Task onLogin()
        {
            if (userrequest != null)
            {

                if (string.IsNullOrWhiteSpace(userrequest.username))
                {
                    refuser.Error = true;
                    refuser.ErrorText = "please enter value username";
                    refuser.FocusAsync();
                    return;
                }
                if (string.IsNullOrWhiteSpace(userrequest.password))
                {
                    refpass.Error = true;
                    refpass.ErrorText = "please enter value password";
                    refpass.FocusAsync();

                    return;
                }

            }



            //var results = Decrpt(usermodel.username);

            // GENARATE PASSWORDHASH = 

            //var passwordhash = userSqlService.Encrypt(usermodel.password);
            //UserModel body = new UserModel();

            //body.username = usermodel.username;
            //body.password = passwordhash;
            //body.first_name = usermodel.username;
            //body.last_name = passwordhash;
            //body.section = "";
            //body.last_login = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //body.section = "";
            //body.role = "";
            //body.expire_password = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
            //await userSqlService.createuser(body);

           var resultlogin = await userSqlService.Loginuser(userrequest);

            if (resultlogin.success == true)
            {
                if (!string.IsNullOrWhiteSpace(resultlogin.result))
                {
                    await localStorage.SetItemAsStringAsync("token", resultlogin.result);
                    nav.NavigateTo("/", true);
                }
            }

        }
        public async Task password(string password)
        {
            var passwords = userSqlService.Decrpt(password);

        }


        public readonly string passwordHash = "#02091990#P@@Sw0rd_XXXX";
        public readonly string SaltKey = "@CS_!OUd0mS4k#02091990";
        //public readonly string VIKey = "!@Oudomsak!@#$%^&";
        public readonly string VIKey = "!@Oudomsak!@#$%^";
        public string passs = "OCS@123";
        //public string passs = "t4hEBRugM8m2dVbsYFfaRA==";
        public string Decrpt(string encryptedText)
        {
            try
            {

                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(passwordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);

                //var kybyte = keyBytes;
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
                //var keygetbytestring = Encoding.ASCII.GetBytes(VIKey);
                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception)
            {
                throw;
            }

        }



    }
}
