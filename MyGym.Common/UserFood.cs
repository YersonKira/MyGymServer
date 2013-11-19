using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGym.Common
{
    public class UserFood
    {
        public string Name { get; set; }
        public double Amount { get; set; }

        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Grease { get; set; }

        public string Grupo { get; set; }
    }
}
