using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegitrationAPI.Model
{
    public class ConfirmEmail
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
