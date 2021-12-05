using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

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

        public static bool ValidateDateTime(string data)
        {
            return DateTime.TryParseExact(data, "HH:mm", CultureInfo.InvariantCulture,DateTimeStyles.None, out _);
        }
    }
}
