using System;
using System.Collections.Generic;
using System.Linq;
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

}
