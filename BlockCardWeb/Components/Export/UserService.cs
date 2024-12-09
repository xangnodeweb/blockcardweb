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
using static iTextSharp.text.pdf.PdfSigGenericPkcs;
using System.Net.Http.Headers;

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
                    if (dataresult["ResultCode"].ToString() == "0")
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
                    else if(dataresult["ResultCode"].ToString() == "107010623")
                    {
                        response.code = 2;
                        response.message = "NOT_FOUND_VOUCHER_CARD";
                        return response;
                    }
                    else
                    {
                        response.code = 1;

                   
                        response.code = int.Parse(dataresult["ResultCode"].ToString()); 
                        response.message = dataresult["ResultDesc"].ToString();
                        return response;
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
                response.message = ex.Message;
                return response;
            }
        }
        public async Task<DefaultReponse<BlockCardVoucherrequest>> SaveBlockCardVoucher(BlockCardVoucherrequest request)
        {
            DefaultReponse<BlockCardVoucherrequest> response = new DefaultReponse<BlockCardVoucherrequest>();
            try
            {
                //var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";
                var connstring = "Host=127.0.0.1;Username=postgres;Password=123456789;Database=user";
                await using var conn = new NpgsqlConnection(connstring);

                await conn.OpenAsync();

                //var sql = "insert into uvc_block_card" +
                //    " (bs_old, facevalue , expire_date , bs_new , msisdn , supplier_name , province , create_time , remark , create_user ) " +
                //    "values(@bs_old , @facevalue , @expire_date , @bs_new , @msisdn , @supplier_name , @province , @create_time , @remark , @create_user )";

                var sql = "insert into uvc_block_card" +
               " (bs_old, facevalue , expire_date , bs_new , msisdn , supplier_name , province , create_time , remark , create_user ) " +
               $"values( '{request.SerialNo}' ,'{request.FaceValue}' ,'{request.CardStopDate}' , '{request.bs_new}', '{request.msisdn}' , '{request.suppliername}', '{request.provincename}' , '{request.createtime}' ,'{request.remark}' , '{request.createuser}' )";
                await using (var cmd = new NpgsqlCommand(sql, conn))
                {

                    //cmd.Parameters.AddWithValue("bs_old", request.SerialNo);
                    //cmd.Parameters.AddWithValue("facevalue", request.FaceValue);
                    //cmd.Parameters.AddWithValue("expire_date", request.CardStopDate);
                    //cmd.Parameters.AddWithValue("bs_new", request.bs_new);
                    //cmd.Parameters.AddWithValue("msisdn", request.msisdn);
                    //cmd.Parameters.AddWithValue("supplier_name", request.suppliername);
                    //cmd.Parameters.AddWithValue("province", request.provincename);
                    //cmd.Parameters.AddWithValue("create_time", request.createtime);
                    //cmd.Parameters.AddWithValue("remark", request.remark);
                    //cmd.Parameters.AddWithValue("create_user", request.createuser);
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
            BlockCardStatusResponse blockcardresponse = new BlockCardStatusResponse();
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



                Random randoms = new Random();
                var msgseq = $"{randoms.Next(0, 99999999)}" + $"{randoms.Next(0, 999999)}";
                Console.WriteLine(msgseq);
                //${=(new java.text.SimpleDateFormat(\"yyyyMMddHHmmss\")).format(new Date())}${=(int)(Math.random() * 1000)}

                var bodyxml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:uvc=\"http://www.huawei.com/bme/cbsinterface/uvcservices\" xmlns:uvc1=\"http://www.huawei.com/bme/cbsinterface/uvcheader\"> <soapenv:Header/><soapenv:Body><uvc:ModifyVoucherLockRequestMsg><RequestHeader><uvc1:Version>1</uvc1:Version><uvc1:BusinessCode>UVC_test</uvc1:BusinessCode>" + $"<uvc1:MessageSeq> {msgseq} </uvc1:MessageSeq><uvc1:AccessSecurity><uvc1:LoginSystemCode>APIGEEAPI</uvc1:LoginSystemCode><uvc1:Password>cdVOUWF+57KsMd57vH8D3H+ykq4CbeLtc8wCapSScPhjazQDDuTrFUP4sDBpyX+q</uvc1:Password><uvc1:RemoteIP>?</uvc1:RemoteIP></uvc1:AccessSecurity></RequestHeader><ModifyVoucherLockRequest> " + $"<uvc:SerialNoList>{serialNo}</uvc:SerialNoList>     <uvc:OperationType>4</uvc:OperationType>                <uvc:OperationReason>sds</uvc:OperationReason></ModifyVoucherLockRequest></uvc:ModifyVoucherLockRequestMsg></soapenv:Body></soapenv:Envelope>";

                HttpClient httpclient = new HttpClient();
                HttpContent content = new StringContent(bodyxml, Encoding.UTF8, "text/xml");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, urlblockcard);

                request.Content = new StringContent(bodyxml, Encoding.UTF8, "text/xml");
                request.Content.Headers.Clear();
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                request.Headers.Clear();

                var requestbody = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(urlblockcard),
                    Content = content
                };
                httpclient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text/xml;charset=utf-8");

                var resultresponse = await httpclient.SendAsync(requestbody);
                //var status =  resultresponse.EnsureSuccessStatusCode;
                if (!resultresponse.IsSuccessStatusCode)
                {
                    response.code = 1;
                    response.message = "BLOCKCARD_FAILED";
                    response.result = null;
                    return response;
                }
                var resultread = await resultresponse.Content.ReadAsStringAsync();

                docxml.LoadXml(resultread);
                var jsontext = JsonConvert.SerializeXmlNode(docxml);
                var datajson = JObject.Parse(jsontext);


                var data = datajson["soapenv:Envelope"]["soapenv:Body"]["uvc:ModifyVoucherLockResultMsg"];
                var dataresult = data != null ? data["ResultHeader"] : null;

                if (dataresult != null)
                {
                    blockcardresponse.Version = dataresult["uvc1:Version"].ToString();
                    blockcardresponse.ResultCode = dataresult["uvc1:ResultCode"].ToString();
                    blockcardresponse.ResultDesc = dataresult["uvc1:ResultDesc"].ToString();
                }
                if (blockcardresponse != null)
                {
                    response.result = blockcardresponse;
                    response.success = true;
                    return response;
                }
                response.message = "CANNOT_BLOCKCARD_FAILED";
                return response;

            }
            catch (Exception ex)
            {
                response.message = "CANNOT_BLOCKCARD_FAILED";
                Console.WriteLine(ex);
                return response;
            }
        }

    }
}
