using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedirecotorService
{
    public static class Helpers
    {

        public static double GetDoubleFromString(this string value, double defaultValue)
        {
            double convertedDouble = defaultValue;
            if (!string.IsNullOrEmpty(value)) 
                 convertedDouble = double.TryParse(value, out double converted) ? converted : defaultValue;

            return convertedDouble;
        }
    }
}
