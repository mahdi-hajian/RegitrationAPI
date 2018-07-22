using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RegitrationAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Mahdi { get; set; }
        public IList<UserTokenValidation> UserTokenValidations { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
