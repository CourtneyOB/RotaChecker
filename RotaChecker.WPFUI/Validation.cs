using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.WPFUI
{
    public static class Validation
    {
        public static bool ValidateInteger(string data)
        {
            //discards out parameter
            return int.TryParse(data, out _);
        }

        public static bool ValidateDouble(string data)
        {
            return double.TryParse(data, out _);
        }
    }
}
