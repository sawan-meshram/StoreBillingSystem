using System;
using System.Drawing;
using System.Windows.Forms;

namespace StoreBillingSystem
{
    public class AddCustomer : Form
    {
        private Panel topPanel;
        private Panel bottomPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel centerPanel;

        public AddCustomer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Create the main form
            this.Text = "Customer Registration Form";
            this.Size = new Size(1366, 768);


            // Create panels for each region
            topPanel = new Panel();
            topPanel.BackColor = Color.LightBlue;
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;


            topPanel.Controls.Add(SetTop());

            bottomPanel = new Panel();
            bottomPanel.BackColor = Color.LightGreen;
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 50;

            leftPanel = new Panel();
            leftPanel.BackColor = Color.LightYellow;
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 100;

            rightPanel = new Panel();
            rightPanel.BackColor = Color.LightCoral;
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 100;

            centerPanel = new Panel();
            centerPanel.BackColor = Color.White;
            centerPanel.Dock = DockStyle.Fill;

            centerPanel.Controls.Add(SetCenter());

            // Add panels to the form
            this.Controls.Add(topPanel);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(leftPanel);

            this.Controls.Add(rightPanel);
            this.Controls.Add(centerPanel);
        }

        private FlowLayoutPanel SetTop()
        {
            FlowLayoutPanel flowPanel = new FlowLayoutPanel();
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.FlowDirection = FlowDirection.LeftToRight; // Flow from top to bottom

            Label title = new Label
            {
                Text = "Customer Registration",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,

                Padding = new Padding()
                {
                    Left = 30,
                    Top = 30
                },

                TextAlign = ContentAlignment.MiddleCenter
            };

            flowPanel.Controls.Add(title);

            return flowPanel;

        }

        private TextBox idTextBox;
        private TextBox nameTextBox;
        private TextBox addressTextBox;
        private TextBox phoneNumberTextBox;
        private Button registerButton;
        private Button clearButton;
        private DateTimePicker dateTimePicker;

        private TableLayoutPanel SetCenter()
        {
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            //tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tableLayoutPanel.Size = new Size(440, 480);
            tableLayoutPanel.Location = new Point(120, 100);
            tableLayoutPanel.BackColor = Color.Aquamarine;


            /*
            Label name = new Label
            {
                Text = "Name :",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(130, 100),
                ForeColor = Color.Black
                //Dock = DockStyle.Fill,
                //TextAlign = ContentAlignment.MiddleCenter
            };

            Label address =  new Label { 
                Text = "Address:", 
                Font = new Font("Arial", 14, FontStyle.Bold) ,
                Location = new Point(130, 150),
                ForeColor = Color.Black
            };

            Label phoneNumber = new Label
            {
                Text = "Phone Number:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(130, 200),
                ForeColor = Color.Black
            };

            tableLayoutPanel.Controls.Add(name);
            tableLayoutPanel.Controls.Add(address);
            tableLayoutPanel.Controls.Add(phoneNumber);
            */

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            //Registration date
            tableLayoutPanel.Controls.Add(
               new Label
               {
                   Text = "Register Date:",
                   Font = new Font("Arial", 14, FontStyle.Bold),
                   Dock = DockStyle.Fill,
                   ForeColor = Color.Black,
                   TextAlign = ContentAlignment.MiddleRight,
               }, 0, 0);

            dateTimePicker = new DateTimePicker
            {
                //CustomFormat = "yyyy-MM-dd HH:mm:ss",
                Format = DateTimePickerFormat.Short,
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 12),
                Margin = new Padding(10)
            };
            tableLayoutPanel.Controls.Add(dateTimePicker, 1, 0);

            //Customer Id
            tableLayoutPanel.Controls.Add(
                new Label
                {
                    Text = "Customer Id:",
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleRight,
                }, 0, 2);

            idTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                //Anchor = AnchorStyles.None,
                //TextAlign = HorizontalAlignment.Center,
                //BorderStyle = BorderStyle.Fixed3D,
                Font = new Font("Arial", 12),
                Margin = new Padding(10)
                //Padding = new Padding(20)
                //Height = 50
            };
            tableLayoutPanel.Controls.Add(idTextBox, 1, 2);


            //Name
            tableLayoutPanel.Controls.Add(
                new Label
                {
                    Text = "Name:",
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    Dock = DockStyle.Fill,
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleRight,
                }, 0, 4);

            nameTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                //Anchor = AnchorStyles.None,
                //TextAlign = HorizontalAlignment.Center,
                //BorderStyle = BorderStyle.Fixed3D,
                Font = new Font("Arial", 12),
                Margin = new Padding(10)
                //Padding = new Padding(20)
                //Height = 50
            };
            tableLayoutPanel.Controls.Add(nameTextBox, 1, 4);

            //Address
            tableLayoutPanel.Controls.Add(
                new Label 
                { 
                    Text = "Address:",
                    ForeColor = Color.Black,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight
                }, 0, 6);

            addressTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Size = new Size(200, 100),
                Multiline = true,
                Font = new Font("Arial", 12),
                Margin = new Padding(10)
                //Padding = new Padding(20)
            };
            tableLayoutPanel.Controls.Add(addressTextBox, 1, 6);

            //Phone Number
            tableLayoutPanel.Controls.Add(
                new Label 
                { 
                    Text = "Phone Number:",
                    ForeColor = Color.Black,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight
                }, 0, 8);

            phoneNumberTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 12),
                Margin = new Padding(10)
                //Padding = new Padding(20)
            };
            tableLayoutPanel.Controls.Add(phoneNumberTextBox, 1, 8);





            //Buttons
            clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.None,
                BackColor = Color.Blue,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Height = 40,
                Width = 100

            };
            //clearButton.Click += new EventHandler(ClearButton_Click);
            //tableLayoutPanel.Controls.Add(clearButton, 0, 6);

            registerButton = new Button
            {
                Text = "Register",
                Dock = DockStyle.None,
                BackColor = Color.Blue,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Height = 40,
                Width = 100

            };
            //registerButton.Click += new EventHandler(RegisterButton_Click);
            //tableLayoutPanel.Controls.Add(registerButton, 1, 6);

            FlowLayoutPanel flowLayout = new FlowLayoutPanel();
            flowLayout.Dock = DockStyle.Fill;
            flowLayout.FlowDirection = FlowDirection.LeftToRight;
            flowLayout.Controls.Add(clearButton);
            flowLayout.Controls.Add(registerButton);
            tableLayoutPanel.Controls.Add(flowLayout, 1, 10);

            return tableLayoutPanel;
        }
    }
}
