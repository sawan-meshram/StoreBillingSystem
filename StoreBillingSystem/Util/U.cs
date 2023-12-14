using System;
using System.Globalization;
using System.Drawing;

namespace StoreBillingSystem.Util
{
    public class U
    {
        private U()
        {

        }

        public static readonly Font StoreLabelFont = new Font("Arial", 11, FontStyle.Bold);
        public static readonly Font StoreTextBoxFont = new Font("Arial", 11);
        public static readonly Font StoreTitleFont = new Font("Arial", 16, FontStyle.Bold);

        public static readonly Color StoreMainBackColor = Color.Aquamarine;
        public static readonly Color StoreDialogBackColor = Color.Bisque;

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

        public static double CalculateQty(double askedPrice, double soldPrice)
        {
            return askedPrice / soldPrice;
        }

        public static double CalculateAskingPrice(double askedQty, double soldPrice)
        {
            return askedQty * soldPrice;
        }
    }
}
