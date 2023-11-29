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

        /// <summary>
        /// It allows tp got a focus on text box that exist places holder text.
        /// </summary>
        /// <param name="textBox">Text box.</param>
        /// <param name="placeHolderMsg">Place holder message.</param>
        public static void PlaceHolderText_GotFocus(TextBox textBox, string placeHolderMsg)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (textBox.Text.ToLower() == placeHolderMsg.ToLower())
            {
                textBox.Text = "";
                textBox.ForeColor = System.Drawing.Color.Black; // Set text color to default
            }
        }

        /// <summary>
        /// It allows to loss a focus on text box and put places holder text on it's empty.
        /// </summary>
        /// <param name="textBox">Text box.</param>
        /// <param name="placeHolderMsg">Place holder message.</param>
        public static void PlaceHolderText_LostFocus(TextBox textBox, string placeHolderMsg)
        {
            // Set the placeholder text when the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeHolderMsg;
                textBox.ForeColor = System.Drawing.Color.Gray; // Set text color to placeholder color
            }
        }

        /// <summary>
        /// Capitalizes the text to textbox when text changed on it.
        /// </summary>
        /// <param name="textBox">Text box.</param>
        public static void CapitalizeText_TextChanged(TextBox textBox)
        {
            textBox.Text = CapitalizeEachWord(textBox.Text);
            textBox.SelectionStart = textBox.Text.Length; // Set the cursor at the end
        }

        /// <summary>
        /// Uppers the text to textbox when text changed on it.
        /// </summary>
        /// <param name="textBox">Text box.</param>
        public static void UpperText_TextChanged(TextBox textBox)
        {
            textBox.Text = textBox.Text.ToUpper();
            textBox.SelectionStart = textBox.Text.Length; // Set the cursor at the end
        }

        private static string CapitalizeEachWord(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return Util.U.ToTitleCase(input);
            }

            return input;
        }


        public static void ReadOnlyTextBox_GotFocus(TextBox textBox, System.Drawing.Color backgroundColor)
        {
            textBox.BackColor = backgroundColor;
        }
    }
}
