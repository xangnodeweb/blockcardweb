using LibraryServices.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Npgsql;
using System.Text;
using System;
using System.Xml;
using NPOI.HSSF.Record;
using iTextSharp.text;
using Microsoft.AspNetCore.Http.Headers;
using NPOI.SS.UserModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using iTextSharp.text.pdf.qrcode;
using static iTextSharp.text.pdf.PdfSigGenericPkcs;

namespace BlockCardWeb.Components.Export
{
    public class UserService
    {
        private readonly string url = "http://10.30.6.120:8081/srv_uvc.asmx";
        public readonly string urlblockcard = "http://172.28.236.57:8080/services/UvcServices";

        public async Task<DefaultReponse<List<Supplier>>> getSupplier()
        {
            DefaultReponse<List<Supplier>> response = new DefaultReponse<List<Supplier>>();
            try
            {
                var sql = "select * from uvc_supplier_card";
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";
                List<Supplier> model = new List<Supplier>();
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        model.Add(new Supplier() { supplier_id = Convert.ToInt32(reader["supplier_id"]), supplier_name = reader["supplier_name"].ToString() });
                    }
                }
                response.success = true;
                response.result = model;
                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                response.message = ex.Message;
                response.code = 1;
                response.result = new List<Supplier>();
                return response;
            }
        }
        public async Task<DefaultReponse<List<Province>>> getProvince()
        {
            DefaultReponse<List<Province>> response = new DefaultReponse<List<Province>>();
            try
            {
                var sql = "select * from uvc_province";
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";
                List<Province> model = new List<Province>();
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        model.Add(new Province { provinceid = Convert.ToInt32(reader["id"]), provincename = reader["province"].ToString() });
                    }
                }
                response.success = true;
                response.result = model;
                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                response.message = ex.Message;
                response.code = 1;
                response.result = new List<Province>();
                return response;
            }
        }
        public async Task<DefaultReponse<QueryVoucherResponse>> Queryvoucher(string bs)
        {
            DefaultReponse<QueryVoucherResponse> response = new DefaultReponse<QueryVoucherResponse>();

            try
            {
                if (string.IsNullOrWhiteSpace(bs))
                {
                    return response;
                }
                XmlDocument docxml = new XmlDocument();
                var bodyxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">" + " <soap12:Body>\r\n    <qryVoucher xmlns=\"http://tempuri.org/\">" + $"<BS>{bs}</BS>" + "  </qryVoucher>  </soap12:Body></soap12:Envelope>";


                HttpClient httpclient = new HttpClient();
                HttpContent content = new StringContent(bodyxml, Encoding.UTF8, "text/xml");
                httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/soap+xml;charset=utf-8");
                HttpResponseMessage responseclient = await httpclient.PostAsync(url, content);
                var model = responseclient.Content.ReadAsStringAsync().Result;

                docxml.LoadXml(model);
                var jsontext = JsonConvert.SerializeXmlNode(docxml);
                var datajson = JObject.Parse(jsontext);
                var data = datajson["soap:Envelope"]["soap:Body"]["qryVoucherResponse"];
                var dataresult = data != null ? data["qryVoucherResult"] : null;
                QueryVoucherResponse queryvouchermodel = new QueryVoucherResponse();

                if (dataresult != null)
                {
                    if (dataresult["ResultCode"] != null)
                    {
                        queryvouchermodel.ResultCode = dataresult["ResultCode"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.ResultCode = "";
                    }

                    if (dataresult["ResultDesc"] != null)
                    {
                        queryvouchermodel.ResultDesc = dataresult["ResultDesc"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.ResultDesc = "";
                    }

                    if (dataresult["SerialNo"] != null)
                    {
                        queryvouchermodel.SerialNo = dataresult["SerialNo"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.SerialNo = "";
                    }
                    if (dataresult["FaceValue"] != null)
                    {
                        queryvouchermodel.FaceValue = dataresult["FaceValue"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.FaceValue = "";
                    }

                    if (dataresult["HotCardFlag"] != null)
                    {
                        queryvouchermodel.HotCardFlag = dataresult["HotCardFlag"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.HotCardFlag = "";
                    }

                    if (dataresult["RechargeNumber"] != null)
                    {
                        queryvouchermodel.RechargeNumber = dataresult["RechargeNumber"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.RechargeNumber = "";
                    }

                    if (dataresult["CardStartDate"] != null)
                    {
                        queryvouchermodel.CardStartDate = dataresult["CardStartDate"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.CardStartDate = "";
                    }

                    if (dataresult["CardStopDate"] != null)
                    {
                        queryvouchermodel.CardStopDate = dataresult["CardStopDate"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.CardStopDate = "";
                    }
                    if (dataresult["HotCardFlagDesc"] != null)
                    {
                        queryvouchermodel.HotCardFlagDesc = dataresult["HotCardFlagDesc"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.HotCardFlagDesc = "";
                    }
                    if (dataresult["TradeTime"] != null)
                    {
                        queryvouchermodel.TradeTime = dataresult["TradeTime"].ToString();
                    }
                    else
                    {
                        queryvouchermodel.TradeTime = "";
                    }
                }
                response.result = queryvouchermodel;
                response.success = true;
                response.code = 0;
                response.message = "QUERY_SUCCESS";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                response.code = 1;
                return response;
            }
        }
        public async Task<DefaultReponse<BlockCardVoucherrequest>> SaveBlockCardVoucher(BlockCardVoucherrequest request)
        {
            DefaultReponse<BlockCardVoucherrequest> response = new DefaultReponse<BlockCardVoucherrequest>();
            try
            {
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";
                await using var conn = new NpgsqlConnection(connstring);

                await conn.OpenAsync();

                var sql = "insert into uvc_block_card" +
                    " (bs_old, facevalue , expire_date , bs_new , msisdn , supplier_name , province , create_time , remark , create_user ) " +
                    "values(@bs_old , @facevalue , @expire_date , @bs_new , @msisdn , @supplier_name , @province , @create_time , @remark , @create_user )";

                await using (var cmd = new NpgsqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("bs_old", request.SerialNo);
                    cmd.Parameters.AddWithValue("facevalue", request.SerialNo);
                    cmd.Parameters.AddWithValue("expire_date", request.SerialNo);
                    cmd.Parameters.AddWithValue("bs_new", request.SerialNo);
                    cmd.Parameters.AddWithValue("msisdn", request.SerialNo);
                    cmd.Parameters.AddWithValue("supplier_name", request.SerialNo);
                    cmd.Parameters.AddWithValue("province", request.SerialNo);
                    cmd.Parameters.AddWithValue("create_time", request.SerialNo);
                    cmd.Parameters.AddWithValue("remark", request.SerialNo);
                    cmd.Parameters.AddWithValue("create_user", request.SerialNo);
                    await cmd.ExecuteNonQueryAsync();
                }
                return response;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return response;
            }
        }

        public async Task<DefaultReponse<BlockCardStatusResponse>> ModifyVoucher(string serialNo)
        {
            DefaultReponse<BlockCardStatusResponse> response = new DefaultReponse<BlockCardStatusResponse>();

            try
            {
                XmlDocument docxml = new XmlDocument();
                if (string.IsNullOrWhiteSpace(serialNo))
                {

                    response.code = 1;
                    response.message = "NOT_FOUND_SerialNo";
                    response.result = null;
                    return response;
                }

            
                


               // var bodyxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:uvc=\"http://www.huawei.com/bme/cbsinterface/uvcservices\" xmlns:uvc1=\"http://www.huawei.com/bme/cbsinterface/uvcheader\">" + "    <soapenv:Header/>  <soapenv:Body><uvc:ModifyVoucherLockRequestMsg><RequestHeader> <uvc1:Version> 1 </uvc1:Version>                        <uvc1:BusinessCode> CBS_test </uvc1:BusinessCode>" + "<uvc1:MessageSeq>${=(new java.text.SimpleDateFormat(\"yyyyMMddHHmmss\")).format(new Date())}${=(int)(Math.random() * 1000)}</uvc1:MessageSeq>                    <uvc1:AccessSecurity>  <uvc1:LoginSystemCode> APIGEEAPI </uvc1:LoginSystemCode>         <uvc1:Password> cdVOUWF+57KsMd57vH8D3H+ykq4CbeLtc8wCapSScPhjazQDDuTrFUP4sDBpyX+q</uvc1:Password>   <uvc1:RemoteIP>?</uvc1:RemoteIP></uvc1:AccessSecurity>  </RequestHeader>   <ModifyVoucherLockRequest> " + $"<uvc:SerialNoList>{serialNo}</uvc:SerialNoList>     <uvc:OperationType>4</uvc:OperationType>                 <uvc:OperationReason> sds </uvc:OperationReason>  </ModifyVoucherLockRequest> </uvc:ModifyVoucherLockRequestMsg>  </soapenv:Body></soapenv:Envelope>";
              var bodyxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>     <soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:uvc=\"http://www.huawei.com/bme/cbsinterface/uvcservices\" xmlns:uvc1=\"http://www.huawei.com/bme/cbsinterface/uvcheader\">\r\n" + "<soapenv:Header/>  <soapenv:Body><uvc:ModifyVoucherLockRequestMsg><RequestHeader> <uvc1:Version> 1 </uvc1:Version>" + " < !--Optional:--> <uvc1:BusinessCode> UVC_test </uvc1:BusinessCode>     <uvc1:MessageSeq>${=(new java.text.SimpleDateFormat(\"yyyyMMddHHmmss\")).format(new Date())}${=(int)(Math.random() * 1000)}</uvc1:MessageSeq>   < !--Optional:--><uvc1:AccessSecurity>  <uvc1:LoginSystemCode> APIGEEAPI </uvc1:LoginSystemCode>\r\n" + "<uvc1:Password> cdVOUWF+57KsMd57vH8D3H+ykq4CbeLtc8wCapSScPhjazQDDuTrFUP4sDBpyX+q</uvc1:Password>   <uvc1:RemoteIP>?</uvc1:RemoteIP></uvc1:AccessSecurity>  </RequestHeader>   <ModifyVoucherLockRequest>       <uvc:SerialNoList>220927000000087</uvc:SerialNoList><uvc:OperationType>1</uvc:OperationType>< !--Optional:--><uvc:OperationReason> sds </uvc:OperationReason>< !--Optional:--></ModifyVoucherLockRequest></uvc:ModifyVoucherLockRequestMsg></soapenv:Body></soapenv:Envelope>"; 


                HttpClient httpclient = new HttpClient();
                HttpContent content = new StringContent(bodyxml, Encoding.UTF8, "text/xml");

                httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text/xml;charset=utf-8");
                HttpResponseMessage responseclient = await httpclient.PostAsync(urlblockcard, content);
                var model = responseclient.Content.ReadAsStringAsync().Result;

                docxml.LoadXml(model);
                var jsontext = JsonConvert.SerializeXmlNode(docxml);
                var datajson = JObject.Parse(jsontext);
                var data = datajson["soap:Envelope"]["soap:Body"]["qryVoucherResponse"];
                var dataresult = data != null ? data["qryVoucherResult"] : null;
                QueryVoucherResponse queryvouchermodel = new QueryVoucherResponse();

                response.success = true;


                return response;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return response;
            }
        }

    }
}
