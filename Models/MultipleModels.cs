using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAuth.Models
{
    public class MultipleModels
    {
        public List<MovieUserInfo> MovieModels { get; set; }
        public User UserModel { get; set; }
    }
}
