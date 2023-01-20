using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAuth.Models
{
    public class User
    {
        public string UserName { get; set; }

        [Key]
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }

        public string Password { get; set; }
    }
}
