using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices.Model
{
    public class UserLogin
    {
        public string? username { get; set; }
        public string? password { get; set; }

    }

    public class UserModel
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

    public class UserloginResponse
    {
        public bool? success { get; set; }
        public int? code { get; set; } = 0;
        public string? message { get; set; }
        public string? result { get; set; }

    }
    public class UserRerefreshPassword
    {
        public string? username { get; set; } = "";
        public string? oldpassword { get; set; } = "";

        public string? newpassword { get; set; } = "";
        public string? confirmpassword { get;set; } = "";

    }


}
