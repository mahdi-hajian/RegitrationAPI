using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegitrationAPI.Model
{
    public class UserTokenValidation
    {
        [Key()]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; private set; }
        public string Ip { get; set; }
        public DateTime LoginDateTime { get; private set; }
        public DateTime ExpireDateTime { get; set; }
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        public UserTokenValidation()
        {
            LoginDateTime = DateTime.Now;
        }
        public UserTokenValidation(DateTime expireDateTime, string ip) : this()
        {
            Ip = ip;
            ExpireDateTime = expireDateTime;
        }
    }
}
