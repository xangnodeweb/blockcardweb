
using Microsoft.AspNetCore.Components;

namespace BlockCardWeb.Components.Pages
{
    public partial class MainPagebar
    {

        [Inject] public NavigationManager nav { get; set; }

        public int? pathmain = 0;
       

        protected override async Task OnInitializedAsync()
        {

            var path = nav.Uri;
            var paths = nav.BaseUri.Split('/');
            if (paths[3] == "")
            {
                pathmain = 0;
            }
            await InvokeAsync(StateHasChanged);
        }


    }
}
