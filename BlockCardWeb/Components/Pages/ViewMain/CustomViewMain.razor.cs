using Blazored.LocalStorage;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;

namespace BlockCardWeb.Components.Pages.ViewMain
{
    public partial class CustomViewMain
    {

        [Inject] public ILocalStorageService Localstorage { get; set; }
        [Inject] public IJSRuntime js { get;set; }
        [Parameter] public RenderFragment authorize { get; set; }
        [Parameter] public RenderFragment noauthorize { get; set; }
        [Parameter] public EventCallback<UserClaim> username { get; set; }
        public UserClaim userclaim = new UserClaim();
        public int? mainpage { get; set; } = 0;
        public string? token { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstload)
        {

            if (firstload)
            {
                mainpage = 0;
                StateHasChanged();

                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();

               token = await Localstorage.GetItemAsync<string>("token");


                if (string.IsNullOrWhiteSpace(token))
                {
                    mainpage = 1;
                }
                else
                {
                    checkToken();
                    mainpage = 2;

                }

                await InvokeAsync(StateHasChanged);
            }

        }
        public async Task checkToken()
        {
            try
            {
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();

                if (string.IsNullOrWhiteSpace(token))
                {
                    mainpage = 1;
                    await Localstorage.RemoveItemAsync("token");
                }
                else
                {
                    var tokens = hand.ReadJwtToken(token);
                    if (tokens.ValidTo <= DateTime.Now.AddMinutes(-15))
                    {
                        mainpage = 1;
                        await Localstorage.RemoveItemAsync("token");
                    }
                    else
                    {
                        userclaim.username = tokens.Claims.FirstOrDefault(x => x.Type == "username").Value;
                        userclaim.firstname = tokens.Claims.FirstOrDefault(x => x.Type == "firstname").Value;
                        userclaim.lastname = tokens.Claims.FirstOrDefault(x => x.Type == "lastname").Value;
                        userclaim.role = tokens.Claims.FirstOrDefault(x => x.Type == "role").Value;
                        userclaim.section = tokens.Claims.FirstOrDefault(x => x.Type == "section").Value;
                        await username.InvokeAsync(userclaim);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task logout()
        {
            await Localstorage.RemoveItemAsync("token");
            mainpage = 1;
            await InvokeAsync(StateHasChanged);

        }

    }
}
