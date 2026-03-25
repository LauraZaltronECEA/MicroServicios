using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services.Handlers
{
    public class EncriptHandler
    {
        public static string Encode(string text) { 
            var textBytes = Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(textBytes);
        }

        public static string Decode(string text) 
        {
            var textBytes = System.Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(textBytes);
        }

    }
}
