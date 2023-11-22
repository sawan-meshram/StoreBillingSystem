using System;

using System.Windows.Forms;
namespace StoreBillingSystem
{
    public class CustomerRegistrationForm : Form
    {

        private TableLayoutPanel tableLayoutPanel;
        private Panel centerPanel;

        private TextBox nameTextBox;
        private TextBox addressTextBox;
        private TextBox phoneNumberTextBox;
        private Button registerButton;
        private Button clearButton;

        public CustomerRegistrationForm()
        {
            InitComponents();

            // Open the form in full-screen mode

            //this.WindowState = FormWindowState.Maximized;
        }

        private void InitComponents()
        {
            // Set up form properties
            this.Text = "Customer Registration Form";
            this.Size = new System.Drawing.Size(1366, 768);
            this.ForeColor = System.Drawing.Color.Black;

            // Create a panel to hold the TableLayoutPanel
            centerPanel = new Panel();
            centerPanel.Dock = DockStyle.Fill;
            centerPanel.BackColor = System.Drawing.Color.LightGray;
            this.Controls.Add(centerPanel);

            // Initialize controls
            tableLayoutPanel = new TableLayoutPanel();


            nameTextBox = new TextBox();
            addressTextBox = new TextBox();
            phoneNumberTextBox = new TextBox();
            //registerButton = new Button();

            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.RowCount = 11;

            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.BackColor = System.Drawing.Color.White;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));


            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F)); //top
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //title
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //name
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); //address
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //phone number
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //button


            Label title = new Label
            {
                Text = "Customer Registration",
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold)
            };

            tableLayoutPanel.Controls.Add(title, 1, 1);
            tableLayoutPanel.SetColumnSpan(title, 2);


            tableLayoutPanel.Controls.Add(
                new Label
                {
                    Text = "Name:",
                    Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                }, 1, 3);

            nameTextBox.Dock = DockStyle.Fill;
            nameTextBox.Height = 50;
            tableLayoutPanel.Controls.Add(nameTextBox, 2, 3);

            tableLayoutPanel.Controls.Add(new Label { Text = "Address:", Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold) }, 1, 5);
            addressTextBox.Dock = DockStyle.Fill;
            //addressTextBox.Height = 60;
            //addressTextBox.Size = new System.Drawing.Size(200, 100);
            addressTextBox.Multiline = true;
            tableLayoutPanel.Controls.Add(addressTextBox, 2, 5);

            tableLayoutPanel.Controls.Add(new Label { Text = "Phone Number:", Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold) }, 1, 7);
            phoneNumberTextBox.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(phoneNumberTextBox, 2, 7);


            clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.None,
                BackColor = System.Drawing.Color.Blue,
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Height = 40,
                Width = 200

            };
            clearButton.Click += new EventHandler(ClearButton_Click);
            tableLayoutPanel.Controls.Add(clearButton, 1, 9);

            registerButton = new Button
            {
                Text = "Register",
                Dock = DockStyle.None,
                BackColor = System.Drawing.Color.Blue,
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Height = 40,
                Width = 200

            };
            registerButton.Click += RegisterButton_Click;
            tableLayoutPanel.Controls.Add(registerButton, 2, 9);
            //tableLayoutPanel.SetColumnSpan(registerButton, 2);




            centerPanel.Controls.Add(tableLayoutPanel);
            this.Controls.Add(centerPanel);


        }

        private void ClearButton_Click(object sender, EventArgs e)
        {

        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string address = addressTextBox.Text;
            string phoneNumber = phoneNumberTextBox.Text;

            MessageBox.Show("Registered:\nName: " + name + "\nAddress: " + address + "\nPhone Number: " + phoneNumber);
        }
    }
}
