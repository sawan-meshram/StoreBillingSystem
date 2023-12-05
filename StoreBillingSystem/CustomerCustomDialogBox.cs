using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using StoreBillingSystem.Entity;
using StoreBillingSystem.Events;
using StoreBillingSystem.Util;

namespace StoreBillingSystem
{
    public class CustomerCustomDialogBox : Form
    {

        public CustomerCustomDialogBox(DataGridView customerTable, IList<Customer> customers)
        {
            this.customers = customers;
            this.customerTable = customerTable;

            InitializeComponent();


        }

        private DataGridView customerTable;
        private IList<Customer> customers;
        //private BindingSource bindingSource;

        private Font labelFont = U.StoreLabelFont;

        private TextBox customerNameText;
        private TextBox phoneText;


        private string _customerNamePlaceHolder = U.ToTitleCase("Search Customer Name Here..");
        private string _phonePlaceHolder = U.ToTitleCase("Search Phone Number Here..");

        private void InitializeComponent()
        {
            Text = "Choose Customer";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1000, 600);
            BackColor = Color.Yellow;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 5,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 450F)); //row-2 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-3


            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350f)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F)); //black
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F)); //phone


            customerNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = _customerNamePlaceHolder,
                ForeColor = Color.Gray,
            };
            table.Controls.Add(customerNameText, 0, 0);

            phoneText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = _phonePlaceHolder,
                ForeColor = Color.Gray,
            };
            table.Controls.Add(phoneText, 2, 0);

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
            table.SetColumnSpan(customerTable, 5);

            //Row-9
            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0f)); //name

            Button okButton = new Button
            {
                Text = "OK",
                Dock = DockStyle.Fill,
                DialogResult = DialogResult.OK,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            CancelButton = cancelButton;

            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;

            table1.Controls.Add(cancelButton, 1, 0);
            table1.Controls.Add(okButton, 2, 0);

            table.Controls.Add(table1, 2, 3);



            Controls.Add(table);

            InitializeCustomerData();

            InitCustomerDialogFormEvent();
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
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
