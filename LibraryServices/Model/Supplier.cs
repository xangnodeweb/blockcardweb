using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryServices.Model
{
    public class Supplier
    {
        //[JsonPropertyName("supplier_id")]
        public int supplier_id { get; set; }
        //[JsonPropertyName("supplier_name")]
        public string supplier_name { get; set; }
    }
}
