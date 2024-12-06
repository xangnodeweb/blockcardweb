using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class DialogVoucher
    {
        [CascadingParameter] public MudDialogInstance muddialog { get; set; }
        [Parameter] public string? contentstring { get;set; }
        [Parameter] public int? optionicon { get; set; }    

        public void cancle() => muddialog.Close();



    }
}
