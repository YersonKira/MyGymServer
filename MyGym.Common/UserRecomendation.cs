using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGym.Common
{
    public class UserRecomendation
    {
        public string Name { get; set; }
        public string Preparation { get; set; }
        public IEnumerable<UserFood> Ingredients { get; set; }
    }
}
