using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices.Model
{
    public class DefaultReponse<T>
    {
        public bool? success { get; set; } = false;
        public string? message { get; set; } = "";
        public int? code = 0;
        public T? result { get; set; }
    }
}
