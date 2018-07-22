using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegitrationAPI.Model
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }
    }
}
