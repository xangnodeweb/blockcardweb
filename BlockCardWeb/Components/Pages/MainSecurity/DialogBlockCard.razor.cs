using Abp.Extensions;
using Blazored.LocalStorage;
using BlockCardWeb.Components.Export;
using LibraryServices;
using LibraryServices.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        [Inject] public IJSRuntime js { get; set; }
        [CascadingParameter] public MudDialogInstance Dialog { get; set; }
        [Inject] public NavigationManager nav { get; set; }
        [Parameter] public string messagecontent { get; set; }
        [Parameter] public QueryVoucherResponse queryvouchermodel { get; set; }

        //public UserSqlService userSqlService = new UserSqlService();
        public UserService userService = new UserService();
        public List<Supplier> suppliermodel = new List<Supplier>();
        public List<Province> provincemodel = new List<Province>();
        public BlockCardVoucherrequest blockcardrequest = new BlockCardVoucherrequest();

        public MudNumericField<string> refbs_new = new MudNumericField<string>();
        public MudNumericField<string> refphone = new MudNumericField<string>();
        public MudSelect<string> refsupplier = new MudSelect<string>();
        public MudSelect<string> refProvince = new MudSelect<string>();

        public string? token { get; set; } = "";
        public bool? blockcardload { get; set; } = false;

        protected override async Task OnAfterRenderAsync(bool firstload)
        {
            if (firstload)
            {
                isload(true);
                await Task.WhenAll(getSupplier(), getProvince(), geToken());

                if (queryvouchermodel != null)
                {
                    if (!string.IsNullOrWhiteSpace(queryvouchermodel.SerialNo))
                    {

                        blockcardrequest.SerialNo = queryvouchermodel.SerialNo;
                        blockcardrequest.FaceValue = queryvouchermodel.FaceValue;
                        blockcardrequest.CardStopDate = queryvouchermodel.CardStopDate;
                        blockcardrequest.remark = "Pin Lost";
                        if (suppliermodel.Count > 0)
                        {

                        }
                    }
                }
            }
            blockcardload = false;
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
            isload(true);

            if (blockcardrequest == null)
            {
                isload(false);
                return;
            }
            blockcardrequest.createtime = $"{DateTime.Now.ToString("yyyy-MM-dd")}";
            if (string.IsNullOrWhiteSpace(blockcardrequest.SerialNo))
            {
                isload(false);
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.FaceValue))
            {
                isload(false);
                return;
            }
            if (string.IsNullOrWhiteSpace(blockcardrequest.CardStopDate))
            {
                isload(false);
                return;
            }
            if (string.IsNullOrWhiteSpace(blockcardrequest.bs_new))
            {
                isload(false);
                refbs_new.Error = true;
                refbs_new.ErrorText = "ກະລຸນາປ້ອນເລກທີ B/S ປ່ຽນແທນ";
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.msisdn))
            {
                isload(false);
                refphone.Error = true;
                refphone.ErrorText = "ກະລຸນາປ້ອນໝາຍເລກຕິດຕໍ່ລູກຄ້າ";
                return;
            }



            if (string.IsNullOrWhiteSpace(blockcardrequest.createtime))
            {
                isload(false);
                return;
            }

            if (string.IsNullOrWhiteSpace(blockcardrequest.createuser))
            {
                isload(false);
                return;
            }

            if (blockcardrequest.suppliername == "0")
            {
                isload(false);
                refsupplier.Error = true;
                refsupplier.ErrorText = "ກະລຸນາເລືອກຜູ້ຜະລິດບັດ";

                return;
            }

            if (blockcardrequest.provincename == "0")
            {
                isload(false);
                refProvince.Error = true;
                refProvince.ErrorText = "ກະລຸນາປ້ອນແຂວງ";
                return;
            }

            //if (string.IsNullOrWhiteSpace(blockcardrequest.remark))
            //{


            //}     
            blockcardrequest.remark = "Pin Lost";
            var query_bs_new = await userService.Queryvoucher(blockcardrequest.bs_new); // query new_bs HotCardFlag == 0 Operaion Success && HotcardFlagDesc == Activated  

            // query
            if (query_bs_new.success == true && query_bs_new.result != null) // check status voucher then HotCardFlag 0 bs_new == success if true next to send modify Voucher 1 == unclock voucher || Voucher 4 == lock voucher
            {
                if (query_bs_new.result.HotCardFlag != "0")
                {
                    Dialog.Close();
                    var statuscard = await checkstatuscard(query_bs_new.result);
                    DialogParameters dialogparameter = new DialogParameters() { ["contentstring"] = statuscard };
                    DialogShow.Show<DialogVoucher>("custom option dialog", dialogparameter, new DialogOptions() { NoHeader = true });
                    return;
                }

                var serialNo = "";
                //await js.InvokeAsync<string>("sendblock");

                var resultblockcardlock = await userService.ModifyVoucher(blockcardrequest.bs_new);
                if (resultblockcardlock.success == true)
                {
                    Dialog.Close("1"); // Close == 1 dialog block success
                }
                else
                {
                    Dialog.Close("0"); // Close == 0 dialog blockcard failed
                }

                //var resultblockcard = await userService.SaveBlockCardVoucher(blockcardrequest);

            }
            isload(false);
            StateHasChanged();
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
        public async Task<string> checkstatuscard(QueryVoucherResponse request)
        {
            var status = "";
            if (request.HotCardFlag == "4")
            {
                status = "ບັດນີ້ບໍ່ສາມາດນຳໃຊ້ໄດ້ ເນື່ອງຈາກມີການ BLOCK";
            }
            else if (request.HotCardFlag == "1")
            {
                status = "ບັດນີ້ຖືກນຳໃຊ້ແລ້ວ";
            }
            else if (request.HotCardFlag == "7")
            {
                status = "ບັດນີ້ໝົດອາຍຸການນຳໃຊ້";
            }
            else if (request.HotCardFlag == "5")
            {
                status = "ບັດນີ້ຍັງບໍ່ທັນ Active ໃນລະບົບ UVC";
            }


            return status;
        }
        public async Task isload(bool status)
        {
            blockcardload = status;
            await InvokeAsync(StateHasChanged);
        }
        public async Task Close()
        {
            Dialog.Close();
        }

    }
}
