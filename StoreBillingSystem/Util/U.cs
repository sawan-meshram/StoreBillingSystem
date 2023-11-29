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

        /// <summary>
        /// Return DateTime in 'yyyy-MM-dd HH:mm:ss' format.
        /// </summary>
        /// <returns>The date time.</returns>
        /// <param name="dateTime">Date time.</param>
        public static string ToDateTime(DateTime dateTime)
        {
            return dateTime != null ? dateTime.ToString("yyyy-MM-dd HH:mm:ss") : null;
        }

        /// <summary>
        /// Return DateTime in 'yyyy-MM-dd' format.
        /// </summary>
        /// <returns>The date.</returns>
        /// <param name="dateTime">Date time.</param>
        public static string ToDate(DateTime dateTime)
        {
            return dateTime != null ? dateTime.ToString("yyyy-MM-dd") : null;
        }
    }
}
