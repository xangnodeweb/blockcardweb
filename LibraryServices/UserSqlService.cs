using LibraryServices.Model;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices
{
    public class UserSqlService : IUserSqlService
    {
        public async Task<DefaultReponse<List<UserModel>>> getAsync(string sql)
        {
            DefaultReponse<List<UserModel>> response = new DefaultReponse<List<UserModel>>();
            try
            {
                //var connstring = "Host=127.0.0.1;Username=postgres;Password=123456789;Database=user";
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                List<UserModel> userlogin = new List<UserModel>();
                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        userlogin.Add(new UserModel()
                        {
                            //username = reader.GetString(0) , 
                            //expire_password = reader.GetString(0),
                            //password = reader.GetString(0),
                            //first_name = reader.GetString(0),
                            //last_name = reader.GetString(0),
                            //role = reader.GetString(0),
                            //section = reader.GetString(0),
                            username = reader["username"].ToString(),
                            expire_password = reader["expire_password"].ToString(),
                            password = reader["password"].ToString(),
                            first_name = reader["first_name"].ToString(),
                            last_name = reader["last_name"].ToString(),
                            role = reader["role"].ToString(),
                            section = reader["section"].ToString(),
                            last_login = reader["last_login"].ToString()
                        });
                    }
                    if (userlogin.Count > 0)
                    {
                        response.success = true;
                        response.result = userlogin;
                    }
                    else
                    {
                        response.success = false;
                        response.message = "NOT_FOUND_USER";
                        response.code = 1;
                        response.result = new List<UserModel>();
                    }

                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "CANNOT_QUERY_USER";
                response.code = 3;
                response.result = new List<UserModel>();
                return response;
            }
        }

        public async Task createuser(UserModel request)
        {

            try
            {
                var connstring = "Host=127.0.0.1;Username=postgres;Password=123456789;Database=user";

                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                var sql = "insert into loginuser (username , password, first_name , last_name , section , role , last_login , expire_password) values(@username , @password, @first_name, @last_name , @section, @role , @last_login , @expire);";
                await using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("username", request.username);
                    cmd.Parameters.AddWithValue("password", request.password);
                    cmd.Parameters.AddWithValue("first_name", request.first_name);
                    cmd.Parameters.AddWithValue("last_name", request.last_name);
                    cmd.Parameters.AddWithValue("section", request.section);
                    cmd.Parameters.AddWithValue("role", request.role);
                    cmd.Parameters.AddWithValue("last_login", DateTime.ParseExact(request.last_login, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("expire", DateTime.ParseExact(request.expire_password, "yyyy-MM-dd", CultureInfo.InvariantCulture));
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task getUser()
        {
            try
            {
                var connstring = "Host=127.0.0.1;Username=postgres;Password=123456789;Database=user";

                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                //await using (var cmd = new NpgsqlCommand("insert into userlogin (username , password)  value(@user)" , conn))
                //{
                //    cmd.Parameters.AddWithValue("user", "xang1234123123");
                //    await cmd.ExecuteNonQueryAsync();
                //}
                await using (var cmd = new NpgsqlCommand("select * from userlogin", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                        Console.WriteLine(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task<List<Supplier>> getSupplierTolist(string sql)
        {
            try
            {
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                List<Supplier> modes = new List<Supplier>();
                Console.WriteLine(modes);
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        modes.Add(new Supplier { supplier_id = Convert.ToInt32(reader["supplier_id"]), supplier_name = reader["supplier_name"].ToString() });
                    }
                }
                return modes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex != null)
                {
                    Console.WriteLine(ex);
                }
            }
            return new List<Supplier>();
        }

        public async Task<List<Province>> getProvince(string sql)
        {
            try
            {
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                List<Province> modes = new List<Province>();
                Console.WriteLine(modes);
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        modes.Add(new Province { provinceid = Convert.ToInt32(reader["id"]), provincename = reader["province"].ToString() });
                    }
                }
                return modes;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<VoucherReportReponse> getQueryVoucherReport(string sql)
        {
            VoucherReportReponse voucherreponse = new VoucherReportReponse();
            try
            {
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                List<BlockCardReponse> modes = new List<BlockCardReponse>();

                Console.WriteLine(modes);
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var modelblockcard = new BlockCardReponse()
                        {
                            bs_old = reader["bs_old"].ToString(),
                            facevalue = reader["facevalue"].ToString(),
                            expire_date = reader["expire_date"].ToString(),
                            bs_new = reader["bs_new"].ToString(),
                            msisdn = reader["msisdn"].ToString(),
                            supplier_name = reader["supplier_name"].ToString(),
                            province = reader["province"].ToString(),
                            create_time = reader["create_time"].ToString(),
                            remark = reader["remark"].ToString(),
                            create_user = reader["create_user"].ToString()
                        };

                        modes.Add(new BlockCardReponse
                        {
                            bs_old = reader["bs_old"].ToString(),
                            facevalue = reader["facevalue"].ToString(),
                            expire_date = reader["expire_date"].ToString(),
                            bs_new = reader["bs_new"].ToString(),
                            msisdn = reader["msisdn"].ToString(),
                            supplier_name = reader["supplier_name"].ToString(),
                            province = reader["province"].ToString(),
                            create_time = reader["create_time"].ToString(),
                            remark = reader["remark"].ToString(),
                            create_user = reader["create_user"].ToString()
                        });
                    }
                }
                voucherreponse.result.success = true;
                voucherreponse.result.message = "";
                voucherreponse.result.result = modes;
                return voucherreponse;

            }
            catch (Exception ex)
            {
                voucherreponse.result.message = "CANNOT_GET_Voucher";
                voucherreponse.result.code = 0;
                return voucherreponse;
            }
        }
        public async Task<bool> updateUser(UserModel request)
        {
            try
            {
                var connstring = "Host=127.0.0.1;Username=postgres;Password=123456789;Database=user";


                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();
                var password = DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");
                var sql = $"update loginuser  set password='{request.password}' , expire_password='{password}' where username='{request.username}' AND first_name='{request.first_name}'";
                await using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    //cmd.Parameters.AddWithValue("password", request.password);
                    await cmd.ExecuteNonQueryAsync();
                }



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;

        }

        public async Task<List<Supplier>> getsupplierddialog(string sql)
        {
            try
            {
                var connstring = "Host=172.28.17.243;Username=postgres;Password=12345678;Database=UVC_BlockCard";

                List<Supplier> modes = new List<Supplier>();
                Console.WriteLine(modes);
                await using var conn = new NpgsqlConnection(connstring);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand(sql, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        modes.Add(new Supplier { supplier_id = Convert.ToInt32(reader["supplier_id"]), supplier_name = reader["supplier_name"].ToString() });
                    }
                }
                return modes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (ex != null)
                {
                    Console.WriteLine(ex);
                }
            }
            return new List<Supplier>();
        }
        public async Task<UserloginResponse> Loginuser(UserLogin request)
        {

                UserloginResponse response = new UserloginResponse();
            try
            {
                //var sql = "select * from loginuser";
                var sql = $"select * from UVC_LOGIN where username='{request.username}'";

                DefaultReponse<List<UserModel>> resultuser = await getAsync(sql);

                if (resultuser.success == true && resultuser.result.Count > 0)
                {

                    var user = resultuser.result.FirstOrDefault(x => x.username == request.username);
                    if (user != null)
                    {
                        var ispassword = Decrpt(user.password);
                        // PASSWORD have two option lb string ToLower and ToUpperCase
                        if (ispassword.ToLower() == request.password.ToLower())
                        {
                            var token = await genarateToken(user);
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                response.message = "login_success";
                                response.success = true;
                                response.result = token;
                            }
                        }
                        else
                        {
                            response.code = 2;
                            response.message = "username and password incorrent.";
                        }
                    }
                    else
                    {
                        response.code = 1;
                        response.message = "NOT_FOUND_USER";
                        response.result = null;
                    }
                }
                else
                {
                    if (resultuser.code == 1 && resultuser.success == false)
                    {
                        response.result = null;
                        response.code = 1;
                        response.message = "NOT_FOUND_USER";
                    }
                    else
                    {
                        response.message = "LOGIN_FAILED";
                   
                    }
                }



                return response;
            }
            catch (Exception ex)
            {
                response.code = 3;
                response.message = "CANNOT_LOGIN_USER";
                return response;
            }
        }


        public async Task<DefaultReponse<UserModel>> refreshPassword(UserRerefreshPassword request)
        {
            DefaultReponse<UserModel> userreponse = new DefaultReponse<UserModel>();
            try
            {
                UserModel usermodel = new UserModel();

                var sql = $"select * from loginuser where username='{request.username}'";
                var user = await getAsync(sql);

                if (user.result.Count == 0)
                {
                    return await usercheckresponse(false, "NOT_FOUND_USER", 1, null);

                }
                var userrefresh = user.result.FirstOrDefault();

                if (userrefresh != null)
                {
                    usermodel = userrefresh;

                    var password = Decrpt(userrefresh.password);

                    if (password != null)
                    {
                        if (request.oldpassword != password)
                        {
                            return await usercheckresponse(false, "OLD_PASSWORD_INCURRENT", 1, null);
                        }
                        var passwordbyte = Encrypt(request.newpassword);
                        if (passwordbyte != null)
                        {
                            usermodel.password = passwordbyte;
                            var updateuser = await updateUser(usermodel);
                            if (updateuser)
                            {
                                return await usercheckresponse(true, "UPDATE_PASSWORD_SUCESS", 0, usermodel);
                            }
                        }
                    }
                }
                return await usercheckresponse(false, "UPDATE_PASSWORD_FAILED", 0, null);
            }
            catch (Exception ex)
            {
                return await usercheckresponse(false, ex.Message, 0, null);
            }
        }

        // create genarate or decrypt password
        public readonly string passwordHash = "#02091990#P@@Sw0rd_XXXX";
        public readonly string SaltKey = "@CS_!OUd0mS4k#02091990";
        public readonly string VIKey = "!@Oudomsak!@#$%^";

        public string Decrpt(string encryptedText)
        {

            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(passwordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);

                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Encrypt(string sInputText)
        {
            try
            {
                byte[] plainTextbytes = Encoding.UTF8.GetBytes(sInputText);
                byte[] keyBytes = new Rfc2898DeriveBytes(passwordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);

                var symetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                var encryptor = symetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                byte[] cipherTextBytes;
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextbytes, 0, plainTextbytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //

        public async Task<string> genarateToken(UserModel usermodel)
        {
            try
            {
                var cliam = new[]
                {
                    new Claim("username" , usermodel.username.ToString()),
                    new Claim("firstname" , usermodel.first_name.ToString()),
                    new Claim("role" , usermodel.role.ToString()),
                    new Claim("lastname" , usermodel.last_name.ToString()),
                    new Claim("section" , usermodel.section.ToString())
                };
                var key_srcret = "this is secret key custom genarate and write token type base64";

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key_srcret));
                var cred = new SigningCredentials(key, SecurityAlgorithms.Aes256CbcHmacSha512);

                var token = new JwtSecurityToken(
                    claims: cliam,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: cred
                    );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<DefaultReponse<UserModel>> usercheckresponse(bool status, string message, int code, UserModel model)
        {
            DefaultReponse<UserModel> response = new DefaultReponse<UserModel>();
            try
            {
                response.success = status;
                response.message = message;
                response.code = code;
                response.result = model;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            response.result = null;
            return response;
        }



    }
}
