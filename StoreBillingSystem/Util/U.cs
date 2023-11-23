using System;
using System.Globalization;

namespace StoreBillingSystem.Util
{
    public class U
    {
        private U()
        {
        }

        //Convert string to capitalise except the upper case
        public static string ToTitleCase(string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }
    }
}
