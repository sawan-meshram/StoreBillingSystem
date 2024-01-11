using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using StoreBillingSystem.Database;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;
namespace StoreBillingSystem.StoreForm.PaymentForm
{
    public class PaymentBalanceHistoryForm : Form
    {
        public PaymentBalanceHistoryForm()
        {
            InitializeComponent();
            InitComponentEvent();
        }

        private IDictionary<int, IList<Payment>> customerPaymentMap;
        private IDictionary<int, Customer> customerMap;
        private IList<Payment> paymentList;
        private IList<Customer> historyBalanceCustomerList;
        private IList<Customer> sortedCustomers;

        private IPaymentDao paymentDao;

        private Font labelFont = U.StoreLabelFont;
        private Font textBoxFont = U.StoreTextBoxFont;

        private Label totalBalanceRecordLabel;
        private Label totalBalanceAmountLabel;
        private Label lblFrom;
        private Label lblTo;

        private Button okButton;
        private Button viewButton;
        private Button deleteButton;
        private Button deleteAllButton;
        private Button searchButton;

        private ComboBox historyComboBox;
        private DataGridView historyTable;
        private DateTimePicker fromDate;
        private DateTimePicker toDate;

        private void InitializeComponent()
        {
            Text = "Balance Payment History";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1100, 650);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 10,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 460F)); //row-2 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-3

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f)); //history purchase lbl
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180f)); //combo box
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70f)); //from
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180f)); //from date
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70f)); //to
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180f)); //to date

            //row-0
            table.Controls.Add(new Label
            {
                Text = "Balance History :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 1, 0);

            historyComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(historyComboBox, 2, 0);

            searchButton = new Button
            {
                Text = "Search",
                //Dock = DockStyle.Fill,
                Anchor = AnchorStyles.None,
                //DialogResult = DialogResult.OK,
                Width = 80,
                Height = 40,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)

            };
            table.Controls.Add(searchButton, 3, 0);

            lblFrom = new Label
            {
                Text = "From :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(lblFrom, 4, 0);

            fromDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(fromDate, 5, 0);

            lblTo = new Label
            {
                Text = "To :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            table.Controls.Add(lblTo, 7, 0);

            toDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(toDate, 8, 0);

            //row-1 - blank

            //row-2
            historyTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };
            historyTable.RowHeadersDefaultCellStyle.Font = U.StoreLabelFont;

            historyTable.ColumnCount = 7;
            historyTable.Columns[0].Name = "Sr. No.";
            historyTable.Columns[1].Name = "Customer Name";
            historyTable.Columns[2].Name = "Phone";
            historyTable.Columns[3].Name = "Customer Since";
            historyTable.Columns[4].Name = "Bill Balance";
            historyTable.Columns[5].Name = "Last Date Balance";
            historyTable.Columns[6].Name = "Total Bal Amt";


            historyTable.Columns[0].Width = 90;
            historyTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            historyTable.Columns[2].Width = 120;
            historyTable.Columns[3].Width = 140;
            historyTable.Columns[4].Width = 90;
            historyTable.Columns[5].Width = 180;
            historyTable.Columns[6].Width = 200;
            //historyTable.Columns[1]
            //historyTable.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //historyTable.Columns[1].DataPropertyName = "BillingDateTime";
            historyTable.Columns[1].DataPropertyName = "Name";
            historyTable.Columns[2].DataPropertyName = "PhoneNumber";

            historyTable.Columns[6].DefaultCellStyle.Format = "C3";


            historyTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; //data display in center
            historyTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //header display in center

            historyTable.ReadOnly = true;
            historyTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            historyTable.MultiSelect = false;


            foreach (DataGridViewColumn column in historyTable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            historyTable.AllowUserToAddRows = false;
            historyTable.AutoGenerateColumns = false;
            historyTable.RowHeadersVisible = false;
            historyTable.AllowUserToResizeRows = false;
            historyTable.AllowUserToResizeColumns = false;

            table.Controls.Add(historyTable, 1, 2);
            table.SetColumnSpan(historyTable, table.ColumnCount - 2);



            //row-3
            Label recordLabel = new Label
            {
                Text = "Customer Record :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(recordLabel, 1, 3);


            totalBalanceRecordLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalBalanceRecordLabel, 2, 3);

            Label totalLabel = new Label
            {
                Text = "Total Bal Amt. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalLabel, 5, 3);
            table.SetColumnSpan(totalLabel, 2);

            totalBalanceAmountLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalBalanceAmountLabel, 7, 3);
            table.SetColumnSpan(totalBalanceAmountLabel, 2);


            //Row-4
            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360)); //name

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
            deleteAllButton = new Button
            {
                Text = "Delete All",
                //DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            CancelButton = okButton;



            table1.Controls.Add(deleteAllButton, 1, 0);
            table1.Controls.Add(deleteButton, 2, 0);
            table1.Controls.Add(viewButton, 3, 0);
            table1.Controls.Add(okButton, 4, 0);

            table.Controls.Add(table1, 1, 4);
            table.SetColumnSpan(table1, table.ColumnCount - 1);

            //row-5
            //blank

            Controls.Add(table);
        }

        private void InitComponentEvent()
        {
            this.Load += Form_Load;

            historyComboBox.SelectedValueChanged += HistoryComboBox_SelectedValueChanged;

            okButton.Click += OkButton_Click;
            //viewButton.Click += ViewButton_Click;
            CancelButton = okButton;

            fromDate.ValueChanged += FromDate_ValueChanged;
            toDate.ValueChanged += ToDate_ValueChanged;

            searchButton.Click += SearchButton_Click;
            historyTable.DataBindingComplete += HistoryTable_DataBindingComplete;

        }


        private void Form_Load(object sender, EventArgs e)
        {
            HideDateRange();
            historyComboBox.Items.Add("None");
            historyComboBox.Items.Add("All");
            historyComboBox.Items.Add("Customer Since");
            historyComboBox.Items.Add("Last Date Balance");

            historyComboBox.SelectedIndex = 0;


            customerPaymentMap = new Dictionary<int, IList<Payment>>();
            customerMap = new Dictionary<int, Customer>();
            historyBalanceCustomerList = new List<Customer>();

            paymentDao = new PaymentDaoImpl(DatabaseManager.GetConnection());
            paymentList = paymentDao.ReadAllByBalance();

            foreach (Payment payment in paymentList)
            {

                IList<Payment> payments;
                if (customerPaymentMap.ContainsKey(payment.Billing.Customer.Id))
                {
                    payments = customerPaymentMap[payment.Billing.Customer.Id];
                }
                else
                {
                    payments = new List<Payment>();
                    //if customer id is not found on map
                    customerMap.Add(payment.Billing.Customer.Id, payment.Billing.Customer);
                }
                payments.Add(payment);
                customerPaymentMap[payment.Billing.Customer.Id] = payments;
            }

            BindcCustomerToDataGridView();

            ShowCustomerRecordCount(0);
            ShowTotalBalanceAmount(0);
        }

        private void BindcCustomerToDataGridView()
        {
            historyTable.DataSource = null;
            historyTable.DataSource = historyBalanceCustomerList;
        }

        private double GetCustomerTotalBalanceAmount(int customerId)
        {
            double total = 0;
            foreach(Payment payment in customerPaymentMap[customerId])
            {
                total += payment.BalanceAmount;
            }
            return total;
        }

        private DateTime GetCustomerLastPaidOrBalanceDate(int customerId)
        {
            IList<Payment> payments = customerPaymentMap[customerId];

            string date = payments[payments.Count - 1].BalancePaidDate;
            if (string.IsNullOrEmpty(date))
            {
                return payments[payments.Count - 1].PaidDateTime;
            }

            return DateTime.ParseExact(payments[payments.Count - 1].BalancePaidDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        private int GetCustomerNumberOfBillsForBalance(int customerId)
        {
            return customerPaymentMap[customerId].Count;
        }

        private void HistoryTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            long srNo = 1;
            foreach (DataGridViewRow row in historyTable.Rows)
            {
                // Set the value for Customer.Name column
                if (row.DataBoundItem is Customer customer)
                {
                    row.Cells[0].Value = srNo++;
                    row.Cells[3].Value = customer.RegisterDateTime.ToString("dd MMM yyyy");
                    row.Cells[4].Value = GetCustomerNumberOfBillsForBalance(customer.Id);
                    row.Cells[5].Value = GetCustomerLastPaidOrBalanceDate(customer.Id).ToString("dd MMM yyyy, hh:mm:ss tt");
                    row.Cells[6].Value = GetCustomerTotalBalanceAmount(customer.Id);
                }
            }
        }

        private void HistoryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedValue = historyComboBox.SelectedItem.ToString();
            if(selectedValue == "All" || selectedValue == "None")
            {
                HideDateRange();
            }
            else
            {
                ShowDateRange();
            }
            Clear();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            historyBalanceCustomerList.Clear();

            string selectedValue = historyComboBox.SelectedItem.ToString();

            if (selectedValue == "All")
            {
                sortedCustomers = customerMap.Values
                    .OrderBy(customer => customer.RegisterDateTime)
                    .ToList();

                //var sortedDatewiseMap = datewiseMap.OrderBy(kvp => kvp.Key).ToList();
                ShowBalanceHistory();
            }
            else if (selectedValue == "Customer Since")
            {
                sortedCustomers = customerMap.Values
                    .Where(customer => fromDate.Value <= customer.RegisterDateTime && customer.RegisterDateTime <= toDate.Value)
                    .OrderBy(customer => customer.RegisterDateTime)
                    .ToList();
                ShowBalanceHistory();
            }
            else if (selectedValue == "Last Date Balance")
            {
                sortedCustomers = customerMap.Values
                    .Where(customer => fromDate.Value <= GetCustomerLastPaidOrBalanceDate(customer.Id).Date && GetCustomerLastPaidOrBalanceDate(customer.Id).Date <= toDate.Value)
                    .OrderBy(customer => GetCustomerLastPaidOrBalanceDate(customer.Id).Date)
                    .ToList();
                ShowBalanceHistory();
            }
            else
            {
                sortedCustomers = null;
                Clear();
                MessageBox.Show("Search is not allow.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void ShowBalanceHistory()
        {
            double totalAmount = 0;
            foreach (Customer customer in sortedCustomers)
            {
                totalAmount += GetCustomerTotalBalanceAmount(customer.Id);
                historyBalanceCustomerList.Add(customer);
            }

            BindcCustomerToDataGridView();

            ShowCustomerRecordCount(historyBalanceCustomerList.Count);
            ShowTotalBalanceAmount(totalAmount);

            if (historyBalanceCustomerList.Count == 0)
                MessageBox.Show("No records found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void FromDate_ValueChanged(object sender, EventArgs e)
        {
            if (fromDate.Value > toDate.Value)
                toDate.Value = fromDate.Value;
        }


        private void ToDate_ValueChanged(object sender, EventArgs e)
        {
            if (fromDate.Value > toDate.Value)
                fromDate.Value = toDate.Value;
        }
        private void Clear()
        {
            ShowCustomerRecordCount(0);
            ShowTotalBalanceAmount(0);
            historyTable.Rows.Clear();
        }

        private void ShowCustomerRecordCount(int count)
        {
            totalBalanceRecordLabel.Text = count.ToString();
        }

        public void ShowTotalBalanceAmount(double totalAmount)
        {
            totalBalanceAmountLabel.Text = totalAmount.ToString("C3");
        }

        private void ShowDateRange()
        {
            lblTo.Show();
            lblFrom.Show();
            fromDate.Show();
            toDate.Show();

            fromDate.Value = DateTime.Today;
            toDate.Value = DateTime.Today;
            //fromDate.MaxDate = fromDate.Value;
            //toDate.MinDate = toDate.Value;
        }

        private void HideDateRange()
        {
            lblTo.Hide();
            lblFrom.Hide();
            fromDate.Hide();
            toDate.Hide();
        }
    }
}
