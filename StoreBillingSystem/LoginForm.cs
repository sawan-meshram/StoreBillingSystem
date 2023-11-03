using System;
using System.Windows.Forms;

namespace StoreBillingSystem
{
    public class LoginForm : Form
    {

        private TableLayoutPanel tableLayoutPanel;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button loginButton;


        public LoginForm()
        {
            InitComponents();
        }

        private void InitComponents()
        {
            tableLayoutPanel = new TableLayoutPanel();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            loginButton = new Button();

            // Form setup
            this.Text = "Login Form";
            this.Size = new System.Drawing.Size(800, 450);
            this.Controls.Add(tableLayoutPanel);

            // TableLayoutPanel configuration
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = 3;
            //tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));


            // Username Label and TextBox
            tableLayoutPanel.Controls.Add(new Label { Text = "Username:" }, 0, 0);
            usernameTextBox.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(usernameTextBox, 1, 0);

            // Password Label and TextBox
            tableLayoutPanel.Controls.Add(new Label { Text = "Password:" }, 0, 1);
            passwordTextBox.Dock = DockStyle.Fill;
            passwordTextBox.PasswordChar = '*'; // Set password character for the password field
            tableLayoutPanel.Controls.Add(passwordTextBox, 1, 1);

            // Login Button
            loginButton.Text = "Login";
            loginButton.Dock = DockStyle.Fill;
            loginButton.Click += new EventHandler(loginButton_Click); // Event handler for button click
            tableLayoutPanel.Controls.Add(loginButton, 0, 2);
            tableLayoutPanel.SetColumnSpan(loginButton, 2);

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            // Perform a basic authentication check
            if (username == "admin" && password == "password")
            {
                // Successful login
                MessageBox.Show("Login successful");
                // Add code to open a new form or perform further actions upon successful login
            }
            else
            {
                // Failed login
                MessageBox.Show("Invalid username or password");
            }
        }


    }

}

