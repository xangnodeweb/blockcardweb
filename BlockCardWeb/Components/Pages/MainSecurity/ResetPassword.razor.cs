using Abp.Extensions;
using Blazored.LocalStorage;
using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class ResetPassword
    {
        [Inject] public ILocalStorageService LocalStorage { get; set; }
        [Inject] public IDialogService Dialog { get; set; }
        [Inject] public NavigationManager nav { get; set; }
        public UserSqlService userservice = new UserSqlService();
        public UserRerefreshPassword UserRefreshModel = new UserRerefreshPassword();
        public MudTextField<string> refoldpassword = new MudTextField<string>();
        public MudTextField<string> refnewpassword = new MudTextField<string>();
        public MudTextField<string> refconfirmpassword = new MudTextField<string>();
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
                refoldpassword.Error = true;
                refoldpassword.ErrorText = "ກະລຸນາປ້ອນລະຫັດຜ່ານ";
                StateHasChanged();
                return;
            }

            if (string.IsNullOrWhiteSpace(UserRefreshModel.newpassword))
            {

                refnewpassword.Error = true;
                refnewpassword.ErrorText = "ກະລຸນາປ້ອນລະຫັດຜ່ານໃໝ່";
                StateHasChanged();
                return;
            }


            if (string.IsNullOrWhiteSpace(UserRefreshModel.confirmpassword))
            {
                refconfirmpassword.Error = true;
                refconfirmpassword.ErrorText = "ກະລຸນາປ້ອນລະຫັດຢືນຢັນ";
                return;
            }

            if (UserRefreshModel.oldpassword == UserRefreshModel.newpassword)
            {
                DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = "ກະລຸນາປ້ອນລະຫັດໃໝ່ ຕ່າງກັບລະຫັດຜ່ານເກົ່າທີ່ທ່ານໃຊ້" };
                DialogResult dialogresult = await Dialog.Show<DialogVoucher>("dialog option", dialogparameter, new DialogOptions() { NoHeader = true }).Result;
                return;
            }

            if (UserRefreshModel.newpassword != UserRefreshModel.confirmpassword)
            {
                DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = "ກະລຸນາປ້ອນລະຫັດໃໝ່ ແລະ ລະຫັດຢືນຢັນໃຫ້ຕົງກັນ", ["optionicon"] = 0 };
                DialogResult dialogresult = await Dialog.Show<DialogVoucher>("dialog option", dialogparameter, new DialogOptions() { NoHeader = true }).Result;

                return;
            }
            //if (UserRefreshModel.oldpassword == UserRefreshModel.confirmpassword)
            //{
            //    DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = "ກະລຸນາຢືນຢັນລະຫັດໃໝ່ ໃຫ້ຖືກກັບລະຫັດຜ່ານທີ່ຕ້ອງການປ່ຽນ", ["optionicon"] = 0 };
            //    DialogResult dialogresult = await Dialog.Show<DialogVoucher>("dialog option", dialogparameter, new DialogOptions() { NoHeader = true }).Result;
            //    return;
            //}



            var result = await userservice.refreshPassword(UserRefreshModel);

            if (result.success == true)
            {
                DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = "ປ່ຽນລະຫັດຜ່ານສຳເລັດ", ["optionicon"] = 1 };
                DialogResult dialogresult = await Dialog.Show<DialogVoucher>("dialog option", dialogparameter, new DialogOptions() { NoHeader = true }).Result;
                if (dialogresult.Canceled)
                {
                    nav.NavigateTo("/", true);
                }
                else
                {
                    nav.NavigateTo("/", true);
                }
             
            }
            else
            {
                if (result.success == false && result.code == 2)
                {
                    DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = "ລະຫັດຜ່ານທີ່ປ້ອນບໍ່ຕົງກັບລະຜ່ານທີ່ທ່ານໃຊ້" };
                    Dialog.Show<DialogVoucher>("dialog option", dialogparameter, new DialogOptions() { NoHeader = true });
                }
                else
                {
                    DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = "ປ່ຽນລະຫັດຜ່ານບໍ່ສຳເລັດ" };
                    Dialog.Show<DialogVoucher>("dialog option", dialogparameter , new DialogOptions() { NoHeader = true });
                }


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
