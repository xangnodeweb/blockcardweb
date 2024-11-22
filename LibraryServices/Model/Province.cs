using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryServices.Model
{
    public class Province
    {
        /// <summary>
        /// id Properties Name is provinceid
        /// </summary>
        [JsonPropertyName("id")]
        public int provinceid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("province")]
        public string provincename { get; set; }
    }
}
