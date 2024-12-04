using Abp.Extensions;
using Blazored.LocalStorage;
using BlockCardWeb.Components.Export;
using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class DialogBlockCard
    {
        [Inject] public ILocalStorageService LocalStorage { get; set; }
        [Inject] public IDialogService DialogShow { get; set; }
        [CascadingParameter] public MudDialogInstance Dialog { get; set; }
        [Inject] public NavigationManager nav { get; set; }
        [Parameter] public string messagecontent { get; set; }
        [Parameter] public QueryVoucherResponse queryvouchermodel { get; set; }

        //public UserSqlService userSqlService = new UserSqlService();
        public UserService userService = new UserService();
        public List<Supplier> suppliermodel = new List<Supplier>();
        public List<Province> provincemodel = new List<Province>();
        public BlockCardVoucherrequest blockcardrequest = new BlockCardVoucherrequest();
        public string? token { get; set; } = "";
        protected override async Task OnAfterRenderAsync(bool firstload)
        {
            if (firstload)
            {
                await Task.WhenAll(getSupplier(), getProvince(), geToken());

                if (queryvouchermodel != null)
                {
                    if (!string.IsNullOrWhiteSpace(queryvouchermodel.SerialNo))
                    {

                        blockcardrequest.SerialNo = queryvouchermodel.SerialNo;
                        blockcardrequest.FaceValue = queryvouchermodel.FaceValue;
                        blockcardrequest.CardStopDate = queryvouchermodel.CardStopDate;
                        if (suppliermodel.Count > 0)
                        {

                        }
                    }
                }
            }
            await InvokeAsync(StateHasChanged);

        }


        public async Task getSupplier()
        {
            var sql = "select * from uvc_supplier_card";
            try
            {
                //var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                //List<Supplier> modes = new List<Supplier>();
                //Console.WriteLine(modes);
                //await using var conn = new NpgsqlConnection(connstring);
                //await conn.OpenAsync();

                //await using (var cmd = new NpgsqlCommand(sql, conn))
                //await using (var reader = await cmd.ExecuteReaderAsync())
                //{
                //    while (await reader.ReadAsync())
                //    {
                //        modes.Add(new Supplier { supplier_id = Convert.ToInt32(reader["supplier_id"]), supplier_name = reader["supplier_name"].ToString() });
                //    }
                //}

                var modes = await userService.getSupplier();
                if (modes.result.Count > 0)
                {
                    suppliermodel = modes.result.OrderBy(x => Convert.ToInt32(x.supplier_id)).ToList();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex != null)
                {
                    Console.WriteLine(ex);
                }
            }

        }
        public async Task getProvince()
        {
            var sql = "select * from uvc_province";
            try
            {
                //var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                //List<Province> modes = new List<Province>();
                //Console.WriteLine(modes);
                //await using var conn = new NpgsqlConnection(connstring);
                //await conn.OpenAsync();

                //await using (var cmd = new NpgsqlCommand(sql, conn))
                //await using (var reader = await cmd.ExecuteReaderAsync())
                //{
                //    while (await reader.ReadAsync())
                //    {
                //        modes.Add(new Province { provinceid = Convert.ToInt32(reader["id"]), provincename = reader["province"].ToString() });
                //    }
                //}
                var model = await userService.getProvince();
                if (model.result.Count > 0)
                {
                    provincemodel = model.result.OrderBy(x => Convert.ToInt32(x.provinceid)).ToList();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex != null)
                {
                    Console.WriteLine(ex);
                }
            }

        }



        public async Task onsaveBlockcard()
        {

            if (blockcardrequest == null)
            {
                return;
            }
            blockcardrequest.createtime = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
            if (string.IsNullOrWhiteSpace(blockcardrequest.SerialNo))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.FaceValue))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(blockcardrequest.CardStopDate))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(blockcardrequest.msisdn))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.bs_new))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.createtime))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.createuser))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.suppliername))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.provincename))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.remark))
            {
                return;
            }
            var query_bs_new = await userService.Queryvoucher(blockcardrequest.bs_new); // query new_bs HotCardFlag == 0 Operaion Success && HotcardFlagDesc == Activated  

            // query
            if (query_bs_new.success == true && query_bs_new.result != null) // check status voucher then HotCardFlag 0 bs_new == success if true next to send modify Voucher 1 == unclock voucher || Voucher 4 == lock voucher
            {
                if (query_bs_new.result.HotCardFlag != "0")
                {
                    Dialog.Close();
                    DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = "" };
                    DialogShow.Show<DialogVoucher>("custom option dialog", dialogparameter, new DialogOptions() { NoHeader = true });
                    return;
                }
            
                var serialNo = "";
                var resultblockcardlock = await userService.ModifyVoucher(blockcardrequest.bs_new);


                //var resultblockcard = await userService.SaveBlockCardVoucher(blockcardrequest);

            }


        }
        public async Task ValueSupplier(string value)
        {
            if (value != null)
            {
                blockcardrequest.suppliername = value;
            }
            await InvokeAsync(StateHasChanged);
        }
        public async Task ValueProvince(string value)
        {
            if (value != null)
            {
                blockcardrequest.provincename = value;
            }
            await InvokeAsync(StateHasChanged);
        }
        public async Task ValueRemark(string value)
        {
            if (value != null)
            {
                blockcardrequest.remark = value;
            }
            await InvokeAsync(StateHasChanged);
        }
        public async Task geToken()
        {
            try
            {
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                token = await LocalStorage.GetItemAsync<string>("token");
                if (string.IsNullOrWhiteSpace(token))
                {
                    nav.NavigateTo("/", true);
                }
                else
                {
                    var tokenss = hand.ReadJwtToken(token);

                    if (tokenss.ValidTo <= DateTime.Now.AddMinutes(-15))
                    {
                        nav.NavigateTo("/", true);
                    }
                    else
                    {
                        blockcardrequest.createuser = tokenss.Claims.FirstOrDefault(x => x.Type == "username").Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public async Task Close()
        {
            Dialog.Close();
        }

    }
}
