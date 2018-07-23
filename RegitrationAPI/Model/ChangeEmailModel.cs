using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegitrationAPI.Model
{
    public class ChangeEmailModel
    {
        public string NewEmail { get; set; }
        public string UserId { get; set; }
    }
}
