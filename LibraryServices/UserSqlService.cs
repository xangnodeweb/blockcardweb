using LibraryServices.Model;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices
{
    public class UserSqlService : IUserSqlService
    {
        public async Task<List<UserModel>> getAsync(string sql)
        {
            var connstring = "Host=127.0.0.1;Username=postgres;Password=123456789;Database=user";
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
                return userlogin;
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
                await using (var cmd = new NpgsqlCommand(sql , conn))
                {
                    cmd.Parameters.AddWithValue("username", request.username);
                    cmd.Parameters.AddWithValue("password", request.password);
                    cmd.Parameters.AddWithValue("first_name", request.first_name);
                    cmd.Parameters.AddWithValue("last_name", request.last_name);
                    cmd.Parameters.AddWithValue("section", request.section);
                    cmd.Parameters.AddWithValue("role", request.role);
                    cmd.Parameters.AddWithValue("last_login", DateTime.ParseExact(request.last_login , "yyyy-MM-dd HH:mm:ss" , CultureInfo.InvariantCulture));
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

        public async Task createUser(UserModel request)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<UserloginResponse> Loginuser(UserLogin request)
        {
            try
            {
                UserloginResponse response = new UserloginResponse();
                var sql = "select * from loginuser";
                List<UserModel> resultuser = await getAsync(sql);

                if (resultuser.Count > 0)
                {

                    var user = resultuser.FirstOrDefault(x => x.username == request.username);
                    if (user != null)
                    {
                        var ispassword = Decrpt(user.password);

                        if (ispassword == request.password)
                        {
                            var token = await genarateToken(user);
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                response.code = 0;
                                response.message = "login_success";
                                response.success = true;
                                response.result = token;
                            }
                        }
                        else
                        {
                            response.code = 1;
                            response.message = "username and password incorrent.";
                            response.success = false;
                        }
                    }
                }


                return response;
            }
            catch (Exception)
            {
                throw;
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
                    new Claim("lastname" , usermodel.last_name.ToString())
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


    }
}
