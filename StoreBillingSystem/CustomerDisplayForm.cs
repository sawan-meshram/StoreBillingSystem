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
    public class CustomerDisplayForm : Form
    {
        public CustomerDisplayForm()
        {
            ICustomerDao customerDao = new CustomerDaoImpl(DatabaseManager.GetConnection());
            customers = customerDao.ReadAll();

            InitializeComponent();
            InitializeCustomerData();

            InitCustomerDialogFormEvent();
        }

        private DataGridView customerTable;
        private IList<Customer> customers;
        //private BindingSource bindingSource;

        private Font labelFont = U.StoreLabelFont;

        private TextBox customerNameText;
        private TextBox phoneText;
        private Button okButton;
        private Button viewButton;
        private Button deleteButton;
        private Button updateButton;

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
            Size = new Size(1000, 650);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
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


            table.Controls.Add(new Label 
            { 
                Text = "Search :", 
                Font = labelFont, 
                Dock = DockStyle.Fill, 
                ForeColor = Color.Black, 
                TextAlign = ContentAlignment.MiddleRight, 
            }, 0, 0);

            customerNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = _customerNamePlaceHolder,
                ForeColor = Color.Gray,
            };
            table.Controls.Add(customerNameText, 1, 0);

            phoneText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = _phonePlaceHolder,
                ForeColor = Color.Gray,
            };
            table.Controls.Add(phoneText, 3, 0);

            Button clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                DialogResult = DialogResult.OK,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            table.Controls.Add(clearButton, 4, 0);

            customerTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };

            customerTable.RowHeadersDefaultCellStyle.Font = U.StoreLabelFont;
            customerTable.ColumnCount = 5;
            customerTable.Columns[0].Name = "Id";
            customerTable.Columns[1].Name = "Name";
            customerTable.Columns[2].Name = "Phone";
            customerTable.Columns[3].Name = "Address";
            customerTable.Columns[4].Name = "Registration Date";


            customerTable.Columns[0].Width = 100;
            customerTable.Columns[1].Width = 250;
            customerTable.Columns[2].Width = 150;
            customerTable.Columns[3].Width = 330;
            //customerTable.Columns[4].Width = 140;
            customerTable.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            customerTable.Columns[0].DataPropertyName = "Id";
            customerTable.Columns[1].DataPropertyName = "Name";
            customerTable.Columns[2].DataPropertyName = "PhoneNumber";
            customerTable.Columns[3].DataPropertyName = "Address";
            customerTable.Columns[4].DataPropertyName = "RegisterDate";


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

            //Row-9
            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220)); //name

            okButton = new Button
            {
                Text = "Ok",
                Dock = DockStyle.Fill,
                DialogResult = DialogResult.OK,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            viewButton = new Button
            {
                Text = "View",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            deleteButton = new Button
            {
                Text = "Delete",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            updateButton = new Button
            {
                Text = "Update",
                DialogResult = DialogResult.Cancel,
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

        }


        private void searchCustomerTextBox_TextChanged(object sender, EventArgs e)
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

        }
        private void searchPhoneTextBox_TextChanged(object sender, EventArgs e)
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

        }


        private void InitCustomerDialogFormEvent()
        {

            customerNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(customerNameText, _customerNamePlaceHolder);
            customerNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(customerNameText, _customerNamePlaceHolder);

            phoneText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;

            phoneText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(phoneText, _phonePlaceHolder);
            phoneText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(phoneText, _phonePlaceHolder);

            customerNameText.TextChanged += searchCustomerTextBox_TextChanged;
            phoneText.TextChanged += searchPhoneTextBox_TextChanged;

            okButton.Click += OkButton_Click;
            viewButton.Click += ViewButton_Click;
            deleteButton.Click += DeleteButton_Click;
            updateButton.Click += UpdateButton_Click;
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.OK;
            Close();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
            //Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
        }
    }
}
