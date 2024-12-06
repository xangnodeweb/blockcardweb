using Microsoft.AspNetCore.Components;
using LibraryServices.Model;
using Microsoft.AspNetCore.Connections.Features;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MudBlazor;
using System.Diagnostics;
using Castle.Facilities.TypedFactory.Internal;
using LibraryServices;
using BlockCardWeb.Components.Export;

namespace BlockCardWeb.Components.Pages.MainSecurity
{
    public partial class QueryVoucher
    {
        public QueryVoucherResponse queryvoucher = new QueryVoucherResponse();
        [Inject] public IDialogService Dialog { get; set; }
        public string? bs { get; set; }
        public string url = "http://10.30.6.120:8081/srv_uvc.asmx";
        public bool btnstatus { get; set; } = false;
        public MudNumericField<string> refbs = new MudNumericField<string>();
        public bool? loading { get; set; } = false;

        public UserSqlService userSqlService = new UserSqlService();
        public UserService userService = new UserService();
        public List<Supplier> suppliermodel = new List<Supplier>();
        public List<Province> provincemodel = new List<Province>();
        protected override async Task OnInitializedAsync()
        {


        }

        public async Task QueryVoucherBs()
        {
            try
            {

                loading = true;
                StateHasChanged();
                if (string.IsNullOrWhiteSpace(bs))
                {
                    loading = false;
                    refbs.Error = true;
                    refbs.ErrorText = "ກະລຸນາລະບຸໝາຍເລກ BS";
                    refbs.FocusAsync();
                    StateHasChanged();
                    return;
                }
                var bsquery = "";
                var faceValue = "";
                var expireDate = "";
                //var bs = "/UVC/BlockCard/?bs=" + bsquery + "&faceValue=" + faceValue + "&expireDate=" + expireDate;
                //srv_uvc srvu = new srv_uvc();
                //var res = srvu.Query_BS(bs);
                XmlDocument docxml = new XmlDocument();
                var bodyxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">" + " <soap12:Body>\r\n    <qryVoucher xmlns=\"http://tempuri.org/\">" + $"<BS>{bs}</BS>" + "  </qryVoucher>  </soap12:Body></soap12:Envelope>";


                HttpClient httpclient = new HttpClient();
                HttpContent content = new StringContent(bodyxml, Encoding.UTF8, "text/xml");
                httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/soap+xml;charset=utf-8");
                HttpResponseMessage response = await httpclient.PostAsync(url, content);
                var model = response.Content.ReadAsStringAsync().Result;

                docxml.LoadXml(model);
                var jsontext = JsonConvert.SerializeXmlNode(docxml);
                var datajson = JObject.Parse(jsontext);
                var data = datajson["soap:Envelope"]["soap:Body"]["qryVoucherResponse"];
                var dataresult = data != null ? data["qryVoucherResult"] : null;

                if (dataresult != null)
                {
                    if (dataresult["ResultCode"].ToString() == "0")
                    {


                        if (dataresult["ResultCode"] != null)
                        {
                            queryvoucher.ResultCode = dataresult["ResultCode"].ToString();
                        }
                        else
                        {
                            queryvoucher.ResultCode = "";
                        }

                        if (dataresult["ResultDesc"] != null)
                        {
                            queryvoucher.ResultDesc = dataresult["ResultDesc"].ToString();
                        }
                        else
                        {
                            queryvoucher.ResultDesc = "";
                        }




                        if (dataresult["SerialNo"] != null)
                        {
                            queryvoucher.SerialNo = dataresult["SerialNo"].ToString();
                        }
                        else
                        {
                            queryvoucher.SerialNo = "";
                        }

                        if (dataresult["FaceValue"] != null)
                        {
                            queryvoucher.FaceValue = dataresult["FaceValue"].ToString();
                        }
                        else
                        {
                            queryvoucher.FaceValue = "";
                        }

                        if (dataresult["HotCardFlag"] != null)
                        {
                            queryvoucher.HotCardFlag = dataresult["HotCardFlag"].ToString();
                        }
                        else
                        {
                            queryvoucher.HotCardFlag = "";
                        }



                        if (dataresult["RechargeNumber"] != null)
                        {
                            queryvoucher.RechargeNumber = dataresult["RechargeNumber"].ToString();
                        }
                        else
                        {
                            queryvoucher.RechargeNumber = "";
                        }



                        if (dataresult["CardStartDate"] != null)
                        {
                            queryvoucher.CardStartDate = dataresult["CardStartDate"].ToString();
                        }
                        else
                        {
                            queryvoucher.CardStartDate = "";
                        }

                        if (dataresult["CardStopDate"] != null)
                        {
                            queryvoucher.CardStopDate = dataresult["CardStopDate"].ToString();
                        }
                        else
                        {
                            queryvoucher.CardStopDate = "";
                        }



                        if (dataresult["HotCardFlagDesc"] != null)
                        {
                            queryvoucher.HotCardFlagDesc = dataresult["HotCardFlagDesc"].ToString();
                        }
                        else
                        {
                            queryvoucher.HotCardFlagDesc = "";
                        }

                        if (dataresult["TradeTime"] != null)
                        {
                            queryvoucher.TradeTime = dataresult["TradeTime"].ToString();
                        }
                        else
                        {
                            queryvoucher.TradeTime = "";
                        }

                        btnstatus = true;
                    }
                    else if (dataresult["ResultCode"].ToString() == "107010623")
                    {
                        btnstatus = false;
                        DialogParameters dialog = new DialogParameters() { ["contentstring"] = $"ບັດບໍ່ມີໃນລະບົບ" };
                        Dialog.Show<DialogVoucher>("custom dialog", dialog, new MudBlazor.DialogOptions { NoHeader = true });

                    }
                    else
                    {
                        queryvoucher = new QueryVoucherResponse();
                        btnstatus = false;
                        var message = dataresult["ResultDesc"].ToString() != null ? dataresult["ResultDesc"].ToString() : "";

                        DialogParameters dialog = new DialogParameters() { ["contentstring"] = $"{message}" };
                        Dialog.Show<DialogVoucher>("custom dialog", dialog, new MudBlazor.DialogOptions { NoHeader = true });

                    }


                }
                else
                {
                    btnstatus = false;
                    queryvoucher = new QueryVoucherResponse();
                    DialogParameters dialog = new DialogParameters() { ["contentstring"] = "" };
                    Dialog.Show<DialogVoucher>("custom dialog", dialog, new MudBlazor.DialogOptions { NoHeader = true });
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                DialogParameters dialog = new DialogParameters() { ["contentstring"] = $"{ex}" };
                Dialog.Show<DialogVoucher>("custom dialog", dialog, new MudBlazor.DialogOptions { NoHeader = true });
                btnstatus = false;
            }

            loading = false;
            StateHasChanged();
        }

        public async Task onBlockCardDialog()
        {
            if (queryvoucher != null)
            {
                if (string.IsNullOrWhiteSpace(queryvoucher.SerialNo))
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(queryvoucher.FaceValue))
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(queryvoucher.CardStopDate))
                {
                    return;
                }
            }


            DialogParameters dialogparameter = new DialogParameters() { ["messagecontent"] = "", ["queryvouchermodel"] = queryvoucher };
            DialogResult result = await Dialog.Show<DialogBlockCard>("custom option dialog", dialogparameter, new DialogOptions() { NoHeader = true }).Result;
            if (result.Data != null)
            {
                if (result.Data == "1")
                {
                    var resultmodel = await userService.Queryvoucher(queryvoucher.SerialNo);
                    if (resultmodel.success == true)
                    {
                        queryvoucher = resultmodel.result;
                    }
                    DialogParameters dialog = new DialogParameters() { ["contentstring"] = "Block Card ສຳເລັດ", ["optionicon"] = 1 };
                    Dialog.Show<DialogVoucher>("custom dialog", dialog, new MudBlazor.DialogOptions { NoHeader = true });

                }
                else
                {
                    DialogParameters dialog = new DialogParameters() { ["contentstring"] = "Block Card ສຳເລັດບໍ່ສຳເລັດ" };
                    Dialog.Show<DialogVoucher>("custom dialog", dialog, new MudBlazor.DialogOptions { NoHeader = true });
                }


            }
            StateHasChanged();
        }

        public async Task<string> checkflied(JToken value)
        {
            try
            {
                if (!value.Any())
                {
                    return "";
                }
                else
                {

                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "";

        }
        public async Task cleardata()
        {
            queryvoucher = new QueryVoucherResponse();
            btnstatus = false;
            bs = "";
            refbs.FocusAsync();

            StateHasChanged();
        }

    }
}
