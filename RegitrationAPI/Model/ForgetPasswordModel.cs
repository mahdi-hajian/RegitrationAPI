using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegitrationAPI.Model
{
    public class ForgetPasswordModel
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
