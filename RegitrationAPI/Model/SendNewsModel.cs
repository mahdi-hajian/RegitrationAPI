using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegitrationAPI.Model
{
    public class SendNewsModel
    {
        public string Category { get; set; }
        public string NewsSubject { get; set; }
        public string Content { get; set; }
    }
}
