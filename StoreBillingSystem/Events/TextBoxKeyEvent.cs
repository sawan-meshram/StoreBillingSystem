using System;
using System.Windows.Forms;

namespace StoreBillingSystem.Events
{
    public class TextBoxKeyEvent
    {
        private TextBoxKeyEvent()
        {
        }

        /// <summary>
        /// It allows only numberic numbers on text box while key press.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public static void NumbericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, decimal point, and the backspace key
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Ignore the input
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && ((TextBox)sender).Text.Contains("."))
            {
                e.Handled = true; // Ignore the input
            }
        }

        /// <summary>
        /// It allow only Uppercases character on the text box while key press.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public static void UppercaseTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }
    }
}
