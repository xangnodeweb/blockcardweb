using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class LoginMain
    {
        [Inject] public ILocalStorageService localStorage { get; set; }
        [Inject] public NavigationManager nav { get; set; }



        InputType passwordinput = InputType.Password;
        public bool? isshow = false;
        public string passwordinputicon = Icons.Material.Filled.VisibilityOff;


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


    }
}
