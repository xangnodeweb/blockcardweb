using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;

namespace BlockCardWeb.Components.Pages.ViewMain
{
    public partial class CustomViewMain
    {

        [Inject] public ILocalStorageService Localstorage { get; set; }

        [Parameter] public RenderFragment authorize { get; set; }
        [Parameter] public RenderFragment noauthorize { get; set; }
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
                    mainpage = 2;

                }

                await InvokeAsync(StateHasChanged);
            }

        }

    }
}
