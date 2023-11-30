using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using StoreBillingSystem.Entity;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Database;
using StoreBillingSystem.Events;

namespace StoreBillingSystem
{
    public class AddCustomer : Form
    {
        private Panel topPanel;
        private Panel bottomPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel centerPanel;

        private Font labelFont = Util.U.StoreLabelFont;
        private Font textfieldFont = Util.U.StoreTextBoxFont;

        public AddCustomer()
        {
            InitializeComponent();

            //closed the database
            FormClosed += Program.ProductForm_FormClosed;
            InitComponentsData();
        }

        private ICustomerDao customerDao;
        private List<string> customerNames;
        private List<string> phones;


        private AutoCompleteStringCollection customerNameAutoSuggestion;
        private AutoCompleteStringCollection phoneAutoSuggestion;

        private void InitComponentsData()
        {
            SqliteConnection connection = DatabaseManager.GetConnection();

            customerDao = new CustomerDaoImpl(connection);

            //Set New product Id
            idTextBox.Text = customerDao.GetNewCustomerId().ToString();

            customerNameAutoSuggestion = new AutoCompleteStringCollection();
            customerNames = (List<string>)customerDao.CustomerNames();


            BindAutoSuggestionToCustomerNameTextBox();
            BindAutoSuggestionToPhoneTextBox();

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
                Font = Util.U.StoreTitleFont,
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
        //private Button registerButton;
        //private Button clearButton;
        private DateTimePicker registerDateTimePicker;

        private string customerNamePlaceHolder = Util.U.ToTitleCase("Enter full name here...");
        private string customerPhonePlaceHolder = Util.U.ToTitleCase("Enter 10 digit mobile number ..");
        private string customerAddressPlaceHolder = Util.U.ToTitleCase("Enter address here...");

        private TableLayoutPanel SetCenter()
        {
            TableLayoutPanel table = new TableLayoutPanel 
            {
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Size = new Size(540, 480),
                Location = new Point(120, 100),
                BackColor = Color.Aquamarine,
                ColumnCount = 3
            };

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

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            //Registration date
            table.Controls.Add(new Label
            {
               Text = "Register Date:",
               Font = labelFont,
               Dock = DockStyle.Fill,
               ForeColor = Color.Black,
               TextAlign = ContentAlignment.MiddleRight,
            }, 0, 0);

            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
            }, 1, 0); ;

            registerDateTimePicker = new DateTimePicker
            {
                //CustomFormat = "yyyy-MM-dd HH:mm:ss",
                Format = DateTimePickerFormat.Short,
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(10)
            };
            table.Controls.Add(registerDateTimePicker, 2, 0);

            //Customer Id
            table.Controls.Add(new Label
            {
                Text = "Customer Id:",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 2);

            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
            }, 1, 2);


            idTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                //Anchor = AnchorStyles.None,
                //TextAlign = HorizontalAlignment.Center,
                //BorderStyle = BorderStyle.Fixed3D,
                Font = labelFont,
                Margin = new Padding(10),
                ReadOnly = true
                //Padding = new Padding(20)
                //Height = 50
            };
            table.Controls.Add(idTextBox, 2, 2);


            //Name
            table.Controls.Add(new Label
            {
                Text = "Name:",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 4);

            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
            }, 1, 4);

            nameTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                //Anchor = AnchorStyles.None,
                //TextAlign = HorizontalAlignment.Center,
                //BorderStyle = BorderStyle.Fixed3D,
                Font = textfieldFont,
                Margin = new Padding(10),
                Text = customerNamePlaceHolder,
                ForeColor = Color.Gray
                //Padding = new Padding(20)
                //Height = 50
            };
            table.Controls.Add(nameTextBox, 2, 4);

            //Address
            table.Controls.Add(new Label 
            { 
                Text = "Address:",
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                Font = labelFont,
                TextAlign = ContentAlignment.MiddleRight
            }, 0, 6);

            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
            }, 1, 6);

            addressTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Size = new Size(200, 100),
                Multiline = true,
                Font = textfieldFont,
                Margin = new Padding(10),
                Text = customerAddressPlaceHolder,
                ForeColor = Color.Gray
                //Padding = new Padding(20)
            };
            table.Controls.Add(addressTextBox, 2, 6);

            //Phone Number
            table.Controls.Add(new Label 
            { 
                Text = "Phone Number:",
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                Font = labelFont,
                TextAlign = ContentAlignment.MiddleRight
            }, 0, 8);

            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
            }, 1, 8);

            phoneNumberTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(10),
                Text = customerPhonePlaceHolder,
                ForeColor = Color.Gray
                //Padding = new Padding(20)
            };
            table.Controls.Add(phoneNumberTextBox, 2, 8);


            //Buttons
            Button clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.None,
                BackColor = Color.Blue,
                Font = labelFont,
                ForeColor = Color.White,
                Height = 40,
                Width = 100

            };
            //clearButton.Click += new EventHandler(ClearButton_Click);
            //tableLayoutPanel.Controls.Add(clearButton, 0, 6);

            Button registerButton = new Button
            {
                Text = "Register",
                Dock = DockStyle.None,
                BackColor = Color.Blue,
                Font = labelFont,
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
            table.Controls.Add(flowLayout, 2, 10);


            InitAddCustomerFormEvent();

            registerButton.Click += (sender, e) => RegisterForm();
            clearButton.Click += (sender, e) => ClearForm();

            return table;
        }

        private void RegisterForm()
        {
            string id = idTextBox.Text.Trim();
            string name = nameTextBox.Text.Trim();
            string address = addressTextBox.Text.Trim();
            string phone = phoneNumberTextBox.Text.Trim();
            var registerDateTime = registerDateTimePicker.Value;

            if (name.ToLower() == customerNamePlaceHolder.ToLower()) name = string.Empty;
            if (address.ToLower() == customerAddressPlaceHolder.ToLower()) address = string.Empty;
            if (phone.ToLower() == customerPhonePlaceHolder.ToLower()) phone = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Customer name can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Address can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Phone number can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (customerDao.IsRecordExists(long.Parse(phone)))
            {
                MessageBox.Show("Phone is already exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Customer customer = new Customer(int.Parse(id), name, address, long.Parse(phone), Util.U.ToDateTime(registerDateTime));
            if (customerDao.Insert(customer))
            {
                MessageBox.Show("Customer added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customerNameAutoSuggestion.Add(customer.Name);
                phoneAutoSuggestion.Add(customer.PhoneNumber.ToString());
                ClearForm();
            }
            else
            {
                MessageBox.Show("Something occur while insertion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            idTextBox.Text = customerDao.GetNewCustomerId().ToString();

            TextBoxKeyEvent.BindPlaceholderToTextBox(nameTextBox, customerNamePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(addressTextBox, customerAddressPlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(phoneNumberTextBox, customerPhonePlaceHolder, Color.Gray);

            registerDateTimePicker.Value = DateTime.Now;

        }

        private void InitAddCustomerFormEvent()
        {
            nameTextBox.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(nameTextBox);
            addressTextBox.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(addressTextBox);
            phoneNumberTextBox.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;

            nameTextBox.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(nameTextBox, customerNamePlaceHolder);
            nameTextBox.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(nameTextBox, customerNamePlaceHolder);

            addressTextBox.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(addressTextBox, customerAddressPlaceHolder);
            addressTextBox.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(addressTextBox, customerAddressPlaceHolder);

            phoneNumberTextBox.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(phoneNumberTextBox, customerPhonePlaceHolder);
            phoneNumberTextBox.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(phoneNumberTextBox, customerPhonePlaceHolder);
        }

        private void BindAutoSuggestionToCustomerNameTextBox()
        {
            customerNameAutoSuggestion.AddRange(customerNames.ToArray());

            nameTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            nameTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            nameTextBox.AutoCompleteCustomSource = customerNameAutoSuggestion;
        }

        private void BindAutoSuggestionToPhoneTextBox()
        {
            phoneAutoSuggestion.AddRange(phones.ToArray());

            phoneNumberTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            phoneNumberTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            phoneNumberTextBox.AutoCompleteCustomSource = phoneAutoSuggestion;
        }

    }
}
