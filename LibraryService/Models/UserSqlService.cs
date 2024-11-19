using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LibraryService.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace LibraryService.Models
{
    public class UserSqlService : IUserSqlService
    {
        public UserSqlService()
        {



        }
        public async Task<List<UserLoginModel>> getAsync(string sql)
        {
            var connstring = "Host=127.0.0.1;Username=postgres;Password=123456789;Database=user";
            await using var conn = new NpgsqlConnection(connstring);
            await conn.OpenAsync();

            List<UserLoginModel> userlogin = new List<UserLoginModel>();
            await using (var cmd = new NpgsqlCommand(sql, conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    userlogin.Add(new UserLoginModel()
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

                throw;
            }
        }
        public readonly string passwordHash = "#02091990#P@@Sw0rd_XXXX";
        public readonly string SaltKey = "@CS_!OUd0mS4k#02091990";
        public readonly string VIKey = "!@Oudomsak!@#$%^&*";

        public string Decrpt(string encryptedText)
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



        public class UserLoginModel
        {
            public string? username { get; set; }
            public string? password { get; set; }
            public string? first_name { get; set; }
            public string? last_name { get; set; }
            public string? section { get; set; }
            public string? role { get; set; }
            public string? last_login { get; set; }
            public string? expire_password { get; set; }

        }
    }
}
