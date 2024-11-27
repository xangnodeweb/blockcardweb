using Abp.Extensions;
using Blazored.LocalStorage;
using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.IdentityModel.Tokens.Jwt;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class ResetPassword
    {
        [Inject] public ILocalStorageService LocalStorage { get; set; }
        [Inject] public IDialogService Dialog { get; set; }
        [Inject] public NavigationManager nav { get; set; }
        public UserSqlService userservice = new UserSqlService();
        public UserRerefreshPassword UserRefreshModel = new UserRerefreshPassword();    
        public string token { get; set; } = "";
        protected override async Task OnAfterRenderAsync(bool firstload)
        {

            if (firstload)
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                token = await LocalStorage.GetItemAsync<string>("token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    removetoken();
                }
                else
                {
                    var tokens = handler.ReadJwtToken(token);

                    if (tokens.ValidTo <= DateTime.Now.AddMinutes(-15))
                    {
                        removetoken();
                    }
                    else
                    {
                        UserRefreshModel.username = tokens.Claims.FirstOrDefault(x => x.Type == "username").Value;

                    }
                }
            }
        }
        public async Task refreshPassword()
        {
            if (string.IsNullOrWhiteSpace(UserRefreshModel.username))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(UserRefreshModel.oldpassword))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(UserRefreshModel.newpassword))
            {
                return;
            }


            if (string.IsNullOrWhiteSpace(UserRefreshModel.confirmpassword))
            {
                return;
            }


            var result = await userservice.refreshPassword(UserRefreshModel);

            if (result.success == true)
            {
                nav.NavigateTo("/", true);
            }
        }


        public async Task backtomain()
        {
            nav.NavigateTo("/", true);

        }

        public async Task removetoken()
        {
            await LocalStorage.RemoveItemAsync("token");
            nav.NavigateTo("/", true);


        }
    }
}
