using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using StoreBillingSystem.Entity;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Database;
using StoreBillingSystem.Events;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.StoreForm.CustomerForm
{
    public class CustomerInsertForm : Form
    {
        public CustomerInsertForm()
        {
            InitializeComponent();

            InitComponentsData();
        }

        private ICustomerDao customerDao;
        private List<string> customerNames;
        private List<string> phones;


        private AutoCompleteStringCollection customerNameAutoSuggestion;
        private AutoCompleteStringCollection phoneAutoSuggestion;

        private void InitComponentsData()
        {

            customerDao = new CustomerDaoImpl(DatabaseManager.GetConnection());

            //Set New product Id
            idTextBox.Text = customerDao.GetNewCustomerId().ToString();

            customerNameAutoSuggestion = new AutoCompleteStringCollection();
            customerNames = (List<string>)customerDao.CustomerNames();

            //phoneAutoSuggestion = new AutoCompleteStringCollection();
            //phones = (List<string>)customerDao.Phones();

            BindAutoSuggestionToCustomerNameTextBox();
            //BindAutoSuggestionToPhoneTextBox();

        }

        private Font labelFont = Util.U.StoreLabelFont;
        private Font textfieldFont = Util.U.StoreTextBoxFont;

        private TextBox idTextBox;
        private TextBox nameTextBox;
        private TextBox addressTextBox;
        private TextBox phoneNumberTextBox;
        private DateTimePicker registerDateTimePicker;

        private string customerNamePlaceHolder = Util.U.ToTitleCase("Enter full name here...");
        private string customerPhonePlaceHolder = Util.U.ToTitleCase("Enter 10 digit mobile number ..");
        private string customerAddressPlaceHolder = Util.U.ToTitleCase("Enter address here...");

        private void InitializeComponent()
        {
            Text = "Register Customer";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(560, 530);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;



            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                BackColor = U.StoreDialogBackColor,
                ColumnCount = 4,
                RowCount = 12
            };


            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));

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
                ReadOnly = true,
                BackColor = Color.White
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
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Dock = DockStyle.None,
                BackColor = Color.Blue,
                Font = labelFont,
                ForeColor = Color.White,
                Height = 40,
                Width = 100
            };

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
            flowLayout.Controls.Add(cancelButton); 
            flowLayout.Controls.Add(clearButton);
            flowLayout.Controls.Add(registerButton);
            table.Controls.Add(flowLayout, 2, 10);


            InitAddCustomerFormEvent();

            registerButton.Click += (sender, e) => RegisterForm();
            clearButton.Click += (sender, e) => ClearForm();
            cancelButton.Click+= (sender, e) => CancelForm();

            this.CancelButton = cancelButton;
            Controls.Add(table);
        }

        private void CancelForm()
        {
            this.Close();
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

                MessageBox.Show($"Phone is already taken for customer '{customerDao.Read(long.Parse(phone)).Name}'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            Customer customer = new Customer(int.Parse(id), name, address, long.Parse(phone), Util.U.ToDateTime(registerDateTime));
            if (customerDao.Insert(customer))
            {
                MessageBox.Show("Customer added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customerNameAutoSuggestion.Add(customer.Name);
                //phoneAutoSuggestion.Add(customer.PhoneNumber.ToString());
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
            phoneNumberTextBox.KeyPress += PhoneTextBox_KeyPress;

            nameTextBox.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(nameTextBox, customerNamePlaceHolder);
            nameTextBox.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(nameTextBox, customerNamePlaceHolder);

            addressTextBox.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(addressTextBox, customerAddressPlaceHolder);
            addressTextBox.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(addressTextBox, customerAddressPlaceHolder);

            phoneNumberTextBox.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(phoneNumberTextBox, customerPhonePlaceHolder);
            phoneNumberTextBox.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(phoneNumberTextBox, customerPhonePlaceHolder);

            idTextBox.Enter += (sender, e) => TextBoxKeyEvent.ReadOnlyTextBox_GotFocus(idTextBox, Color.LightGray);

        }

        private void PhoneTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, decimal point, and the backspace key
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Ignore the input
            }
            if (phoneNumberTextBox.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancel the key press
                //MessageBox.Show("Please enter a valid phone number with exactly 10 digits.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
