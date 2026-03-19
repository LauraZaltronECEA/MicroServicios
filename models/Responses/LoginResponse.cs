using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models.Responses
{
    public class LoginResponse
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }

        public bool Estado { get; set; }

    }
}
