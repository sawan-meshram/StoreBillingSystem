using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using StoreBillingSystem.Database;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Events;
using StoreBillingSystem.Util;

namespace StoreBillingSystem
{
    public class ProductDisplayForm : Form
    {
        public ProductDisplayForm()
        {
            productDao = new ProductDaoImpl(DatabaseManager.GetConnection());
            products = productDao.ReadAll();

            customerTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };

            InitializeComponent();
            InitializeCustomerData();

            InitCustomerDialogFormEvent();
        }

       
        private IProductDao productDao;

        private DataGridView customerTable;
        private IList<Product> products;
        private Product _product;
        //private BindingSource bindingSource;

        private Font labelFont = U.StoreLabelFont;
        private Font textBoxFont = U.StoreTextBoxFont;

        private Label totalCustomerLabel;
        private TextBox productNameText;

        private Button okButton;
        private Button viewButton;
        private Button deleteButton;
        private Button updateButton;
        private Button clearButton;
        private ComboBox categoryTypes;

        public bool IsCustomerModified { get; private set; }

        private string _customerNamePlaceHolder = U.ToTitleCase("Search Customer Name Here..");
        private string _phonePlaceHolder = U.ToTitleCase("Search Phone Number Here..");

        private void InitializeComponent()
        {
            Text = "Customer Details";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1150, 650);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 460F)); //row-2 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-3

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350f)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F)); //black
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F)); //phone
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F)); //phone


            //row-0
            table.Controls.Add(new Label
            {
                Text = "Search Product :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 0);

            productNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = _customerNamePlaceHolder,
                ForeColor = Color.Gray,
            };
            table.Controls.Add(productNameText, 1, 0);

            table.Controls.Add(new Label
            {
                Text = "Select Category :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 0);


            categoryTypes = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(categoryTypes, 4, 0);


            clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                DialogResult = DialogResult.OK,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            table.Controls.Add(clearButton, 5, 0);

            //row-1
            //blank

            //row-2
            /*
            customerTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };
            */

            customerTable.RowHeadersDefaultCellStyle.Font = U.StoreLabelFont;
            customerTable.ColumnCount = 6;
            customerTable.Columns[0].Name = "Id";
            customerTable.Columns[1].Name = "Name";
            customerTable.Columns[2].Name = "Phone";
            customerTable.Columns[3].Name = "Address";
            customerTable.Columns[4].Name = "Registration Date";
            customerTable.Columns[5].Name = "Update Date";


            customerTable.Columns[0].Width = 100;
            customerTable.Columns[1].Width = 250;
            customerTable.Columns[2].Width = 150;
            customerTable.Columns[3].Width = 330;
            //customerTable.Columns[4].Width = 140;
            customerTable.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            customerTable.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            customerTable.Columns[0].DataPropertyName = "Id";
            customerTable.Columns[1].DataPropertyName = "Name";
            customerTable.Columns[2].DataPropertyName = "PhoneNumber";
            customerTable.Columns[3].DataPropertyName = "Address";
            customerTable.Columns[4].DataPropertyName = "RegisterDate";
            customerTable.Columns[5].DataPropertyName = "UpdateDate";


            customerTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; //data display in center
            customerTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //header display in center

            customerTable.ReadOnly = true;
            customerTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customerTable.MultiSelect = false;


            //productSellingTable.Columns[0].Width = 140;
            //productSellingTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn column in customerTable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            customerTable.AllowUserToAddRows = false;
            customerTable.AutoGenerateColumns = false;
            customerTable.RowHeadersVisible = false;
            customerTable.AllowUserToResizeRows = false;
            customerTable.AllowUserToResizeColumns = false;

            //to select only row at a time
            //productSellingTable.SelectionChanged += ProductSellingTable_SelectionChanged;


            table.Controls.Add(customerTable, 0, 2);
            table.SetColumnSpan(customerTable, table.ColumnCount);

            //row-3
            Label totalCustomerShowLabel = new Label
            {
                Text = "Total Customer :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalCustomerShowLabel, 3, 3);
            table.SetColumnSpan(totalCustomerShowLabel, 2);

            totalCustomerLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalCustomerLabel, 5, 3);

            //Row-4
            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 290)); //name

            okButton = new Button
            {
                Text = "Ok",
                Dock = DockStyle.Fill,
                //DialogResult = DialogResult.OK,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            viewButton = new Button
            {
                Text = "View",
                //DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            deleteButton = new Button
            {
                Text = "Delete",
                //DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            updateButton = new Button
            {
                Text = "Update",
                //DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            CancelButton = okButton;



            table1.Controls.Add(updateButton, 1, 0);
            table1.Controls.Add(deleteButton, 2, 0);
            table1.Controls.Add(viewButton, 3, 0);
            table1.Controls.Add(okButton, 4, 0);

            table.Controls.Add(table1, 1, 4);
            table.SetColumnSpan(table1, 4);

            //row-5
            //blank

            Controls.Add(table);


        }

        private void InitializeCustomerData()
        {
            /*
            // Create a BindingSource and bind it to the List
            bindingSource = new BindingSource();
            bindingSource.DataSource = customers;

            // Set the DataGridView DataSource to the BindingSource
            customerTable.DataSource = bindingSource;
            */
            customerTable.DataSource = null;
            customerTable.DataSource = customers;
            UpdateCustomerCountAtLabel(customers.Count);
        }

        private void UpdateCustomerCountAtLabel(int count)
        {
            totalCustomerLabel.Text = count.ToString();
        }


        private void SearchCustomerTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBoxKeyEvent.CapitalizeText_TextChanged(customerNameText);

            string searchTerm = customerNameText.Text.ToLower();
            Console.WriteLine(searchTerm);
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == _customerNamePlaceHolder.ToLower()) return;

            List<Customer> filteredList = customers
                .Where(customer => customer.Name.ToLower().Contains(searchTerm))
                .ToList();

            // Update the BindingSource with the filtered data
            //bindingSource.DataSource = filteredList;

            customerTable.DataSource = null;
            customerTable.DataSource = filteredList;
            UpdateCustomerCountAtLabel(filteredList.Count);
        }
        private void SearchPhoneTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = phoneText.Text;
            Console.WriteLine(searchTerm);
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == _phonePlaceHolder.ToLower()) return;

            List<Customer> filteredList = customers
                .Where(customer => customer.PhoneNumber.ToString().Contains(searchTerm))
                .ToList();

            // Update the BindingSource with the filtered data
            //bindingSource.DataSource = filteredList;

            customerTable.DataSource = null;
            customerTable.DataSource = filteredList;
            UpdateCustomerCountAtLabel(filteredList.Count);
        }


        private void InitCustomerDialogFormEvent()
        {

            customerNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(customerNameText, _customerNamePlaceHolder);
            customerNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(customerNameText, _customerNamePlaceHolder);

            phoneText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;

            phoneText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(phoneText, _phonePlaceHolder);
            phoneText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(phoneText, _phonePlaceHolder);

            customerNameText.TextChanged += SearchCustomerTextBox_TextChanged;
            phoneText.TextChanged += SearchPhoneTextBox_TextChanged;

            okButton.Click += OkButton_Click;
            viewButton.Click += ViewButton_Click;
            deleteButton.Click += DeleteButton_Click;
            updateButton.Click += UpdateButton_Click;
            clearButton.Click += ClearButton_Click;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            TextBoxKeyEvent.BindPlaceholderToTextBox(customerNameText, _customerNamePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(phoneText, _phonePlaceHolder, Color.Gray);
            InitializeCustomerData();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.OK;
            Close();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Do you want to remove the selected Customer with its ID is :{customerTable.CurrentRow.Cells[0].Value}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (customerDao.Delete(Int32.Parse(customerTable.CurrentRow.Cells[0].Value.ToString())))
                {
                    // Delete customer records from list that its bind with datagridview datasource
                    customers.RemoveAt(customerTable.CurrentCell.RowIndex);

                    // Delete the row from the DataGridView
                    customerTable.Rows.RemoveAt(customerTable.CurrentCell.RowIndex);

                    //update totalCustomer count
                    UpdateCustomerCountAtLabel(customers.Count);
                }
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {

            _customer = customers[customerTable.CurrentCell.RowIndex];
            CustomerForm(false, true).ShowDialog();
            //DialogResult = DialogResult.Cancel;
            //Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            _customer = customers[customerTable.CurrentCell.RowIndex];
            CustomerForm(true, false).ShowDialog();

        }

        private Form CustomerForm(bool forUpdateCustomer, bool forViewCustomer)
        {
            Form form = new Form();
            form.HelpButton = true; // Display a help button on the form
            form.FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            form.MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            form.MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            form.StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            form.Size = new Size(500, 550);
            form.BackColor = U.StoreDialogBackColor;

            if (forUpdateCustomer) form.Text = "Update Customer Details";
            else if (forViewCustomer) form.Text = "Customer Details";

            form.Controls.Add(UpdateViewForm(form, forUpdateCustomer, forViewCustomer));


            InitUpdateViewCustomerData();

            return form;
        }

        private void InitUpdateViewCustomerData()
        {
            _idTextBox.Text = _customer.Id.ToString();
            _nameTextBox.Text = _customer.Name;
            _addressTextBox.Text = _customer.Address;
            _phoneNumberTextBox.Text = _customer.PhoneNumber.ToString();
            _registerDateTime.Text = _customer.RegisterDate;

            if (_updateDateTime != null) _updateDateTime.Text = string.IsNullOrWhiteSpace(_customer.UpdateDate) ? "" : _customer.UpdateDate;
        }


        private TextBox _idTextBox;
        private TextBox _nameTextBox;
        private TextBox _addressTextBox;
        private TextBox _phoneNumberTextBox;
        private TextBox _registerDateTime;
        private TextBox _updateDateTime;
        private DateTimePicker _updateDateTimePicker;

        private TableLayoutPanel UpdateViewForm(Form form, bool forUpdateCustomer, bool forViewCustomer)
        {
            Label registerDateLabel = new Label
            {
                Text = "Register Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label updateDateLabel = new Label
            {
                Text = "Update Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label customerIdLabel = new Label
            {
                Text = "Customer Id :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label nameLabel = new Label
            {
                Text = "Full Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label addressLabel = new Label
            {
                Text = "Address :",
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                Font = labelFont,
                TextAlign = ContentAlignment.MiddleRight
            };

            Label phoneLabel = new Label
            {
                Text = "Phone Number :",
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                Font = labelFont,
                TextAlign = ContentAlignment.MiddleRight
            };


            _idTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(10),
                ReadOnly = true,
                BackColor = Color.White
            };

            _nameTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
            };

            _addressTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Size = new Size(200, 100),
                Multiline = true,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
            };

            _phoneNumberTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
            };

            _registerDateTime = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
                ReadOnly = true,
                BackColor = Color.White
            };



            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Aquamarine,
                ColumnCount = 3,
                RowCount = 9
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 325F));

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));


            table.Controls.Add(customerIdLabel, 0, 0);
            table.Controls.Add(_idTextBox, 1, 0);

            table.Controls.Add(nameLabel, 0, 1);
            table.Controls.Add(_nameTextBox, 1, 1);

            table.Controls.Add(addressLabel, 0, 2);
            table.Controls.Add(_addressTextBox, 1, 2);

            table.Controls.Add(phoneLabel, 0, 3);
            table.Controls.Add(_phoneNumberTextBox, 1, 3);

            table.Controls.Add(registerDateLabel, 0, 4);
            table.Controls.Add(_registerDateTime, 1, 4);


            FlowLayoutPanel flowLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight
            };

            //To show Update Form
            if (forUpdateCustomer)
            {
                _updateDateTimePicker = new DateTimePicker
                {
                    //CustomFormat = "yyyy-MM-dd HH:mm:ss",
                    Format = DateTimePickerFormat.Short,
                    Dock = DockStyle.Fill,
                    Font = labelFont,
                    Margin = new Padding(10)
                };

                table.Controls.Add(updateDateLabel, 0, 5);
                table.Controls.Add(_updateDateTimePicker, 1, 5);

                Button _cancelButton = new Button
                {
                    Text = "Cancel",
                    Dock = DockStyle.None,
                    BackColor = Color.Blue,
                    Font = labelFont,
                    ForeColor = Color.White,
                    Height = 40,
                    Width = 100

                };

                Button _updateButton = new Button
                {
                    Text = "Update",
                    Dock = DockStyle.None,
                    BackColor = Color.Blue,
                    Font = labelFont,
                    ForeColor = Color.White,
                    Height = 40,
                    Width = 100

                };

                flowLayout.Controls.Add(_cancelButton);
                flowLayout.Controls.Add(_updateButton);

                _cancelButton.Click += (sender, e) => form.Close();
                _updateButton.Click += (sender, e) => UpdateCustomerFormEvent(form); ;

                form.CancelButton = _cancelButton;
            }
            //To show View Form
            else if (forViewCustomer)
            {
                _updateDateTime = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Font = U.StoreTextBoxFont,
                    Margin = new Padding(10),
                    ForeColor = Color.Black
                };

                table.Controls.Add(updateDateLabel, 0, 5);
                table.Controls.Add(_updateDateTime, 1, 5);

                Button _okButton = new Button
                {
                    Text = "Ok",
                    Dock = DockStyle.None,
                    BackColor = Color.Blue,
                    Font = labelFont,
                    ForeColor = Color.White,
                    Height = 40,
                    Width = 100

                };


                _nameTextBox.ReadOnly = true;
                _addressTextBox.ReadOnly = true;
                _phoneNumberTextBox.ReadOnly = true;
                _updateDateTime.ReadOnly = true;

                _nameTextBox.BackColor = Color.White;
                _addressTextBox.BackColor = Color.White;
                _phoneNumberTextBox.BackColor = Color.White;
                _updateDateTime.BackColor = Color.White;



                flowLayout.Controls.Add(_okButton);
                flowLayout.Anchor = AnchorStyles.None; //to show button on center
                //Ok button event
                _okButton.Click += (sender, e) => form.Close();

                form.CancelButton = _okButton;
            }

            _nameTextBox.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(_nameTextBox);
            _addressTextBox.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(_addressTextBox);
            _phoneNumberTextBox.KeyPress += PhoneTextBox_KeyPress;

            table.Controls.Add(flowLayout, 1, 7);

            return table;
        }

        private void UpdateCustomerFormEvent(Form form)
        {
            string name = _nameTextBox.Text.Trim();
            string address = _addressTextBox.Text.Trim();
            string phone = _phoneNumberTextBox.Text.Trim();


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
            if (_customer.Name == name && _customer.Address == address && _customer.PhoneNumber == long.Parse(phone))
            {
                MessageBox.Show("There is nothing to update.", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_customer.PhoneNumber != long.Parse(phone))
            {
                if (customerDao.IsRecordExists(long.Parse(phone)))
                {
                    MessageBox.Show("Phone number is already exist.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (_customer.Name != name && _customer.PhoneNumber != long.Parse(phone))
            {
                if (customerDao.IsRecordExists(name, long.Parse(phone)))
                {
                    MessageBox.Show("Name & Phone number already exist.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            Customer customer = new Customer(_customer.Id, name, address, long.Parse(phone), _customer.RegisterDate, U.ToDateTime(_updateDateTimePicker.Value));
            if (customerDao.Update(customer))
            {
                MessageBox.Show("Update successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                customers[customerTable.CurrentCell.RowIndex] = customer;
                form.Close();
            }
            else
            {
                MessageBox.Show("Update failed.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void PhoneTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, decimal point, and the backspace key
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Ignore the input
            }
            if (_phoneNumberTextBox.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Cancel the key press
                MessageBox.Show("Please enter a valid phone number with exactly 10 digits.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
