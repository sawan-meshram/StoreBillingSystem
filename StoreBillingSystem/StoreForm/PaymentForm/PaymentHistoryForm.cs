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
    public class PaymentHistoryForm : Form
    {
        public PaymentHistoryForm()
        {
            InitializeComponent();
            InitComponentEvent();
        }

        private IDictionary<DateTime, IList<Payment>> datewiseMap;
        private IList<Payment> paymentList;
        private IList<DateTime> sortedDates;
        private IList<DateTime> uniqueDates;
        private IList<Payment> historyPaymentList;

        private IPaymentDao paymentDao;


        private Font labelFont = U.StoreLabelFont;
        private Font textBoxFont = U.StoreTextBoxFont;

        private Label totalPaymentRecordLabel;
        private Label totalPaidAmountLabel;
        private Label totalBalanceAmountLabel;
        private Label lblFrom;
        private Label lblTo;

        private Button okButton;
        private Button resetButton;

        private Button searchButton;

        private ComboBox historyComboBox;
        private ComboBox paymentStatusComboBox;
        private ComboBox paymentModeComboBox;

        private DataGridView historyTable;
        private DateTimePicker fromDate;
        private DateTimePicker toDate;

        private void InitializeComponent()
        {
            Text = "Payment History";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1100, 670);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 11,
                RowCount = 7,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 460F)); //row-2 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-3

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f)); //history purchase lbl
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f)); //payment status
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f)); //payment mode
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f)); //search button
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f)); //from
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f)); //to
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 35f)); //blank

            //row-0
            table.Controls.Add(new Label
            {
                Text = "Payment History :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 1, 0);

            table.Controls.Add(new Label
            {
                Text = "Payment Status :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 2, 0);

            table.Controls.Add(new Label
            {
                Text = "Payment Mode :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 3, 0);

            lblFrom = new Label
            {
                Text = "From :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            };
            table.Controls.Add(lblFrom, 6, 0);

            lblTo = new Label
            {
                Text = "To :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            };
            table.Controls.Add(lblTo, 8, 0);

            //row-1
            historyComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(historyComboBox, 1, 1);

            paymentStatusComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(paymentStatusComboBox, 2, 1);

            paymentModeComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(paymentModeComboBox, 3, 1);

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
            table.Controls.Add(searchButton, 4, 1);

            fromDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(fromDate, 6, 1);

            toDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(toDate, 8, 1);

            //row-2 - blank

            //row-3
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
            historyTable.Columns[1].Name = "Paid Date";
            historyTable.Columns[2].Name = "Paid Amt.";
            historyTable.Columns[3].Name = "Payment Mode";
            historyTable.Columns[4].Name = "Status";
            historyTable.Columns[5].Name = "Balance Date";
            historyTable.Columns[6].Name = "Balance Amt.";


            historyTable.Columns[0].Width = 90;
            historyTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            historyTable.Columns[2].Width = 200;
            historyTable.Columns[3].Width = 150;
            historyTable.Columns[4].Width = 80;
            historyTable.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            historyTable.Columns[6].Width = 200;

            //historyTable.Columns[1]
            //historyTable.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //historyTable.Columns[1].DataPropertyName = "PaidDate";
            historyTable.Columns[2].DataPropertyName = "PaidAmount";
            historyTable.Columns[3].DataPropertyName = "PaymentMode";
            historyTable.Columns[4].DataPropertyName = "Status";
            //historyTable.Columns[5].DataPropertyName = "BalancePaidDate";
            historyTable.Columns[6].DataPropertyName = "BalanceAmount";


            historyTable.Columns[2].DefaultCellStyle.Format = "C3";
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

            table.Controls.Add(historyTable, 1, 3);
            table.SetColumnSpan(historyTable, table.ColumnCount - 2);

            //row-4
            Label paymentRecordLabel = new Label
            {
                Text = "Payment Record :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(paymentRecordLabel, 1, 4);


            totalPaymentRecordLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalPaymentRecordLabel, 2, 4);

            Label totalPaidLabel = new Label
            {
                Text = "Total Paid Amt. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalPaidLabel, 5, 4);
            //table.SetColumnSpan(totalPaidLabel, 2);

            totalPaidAmountLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalPaidAmountLabel, 6, 4);
            //table.SetColumnSpan(totalPaidAmountLabel, 2);

            Label totalBalanceLabel = new Label
            {
                Text = "Total Bal. Amt. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalBalanceLabel, 7, 4);

            totalBalanceAmountLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalBalanceAmountLabel, 8, 4);

            //Row-5
            okButton = new Button
            {
                Text = "Ok",
                //Dock = DockStyle.Fill,
                //DialogResult = DialogResult.OK,
                Width = 80,
                Height = 35,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            resetButton = new Button
            {
                Text = "Reset",
                //DialogResult = DialogResult.Cancel,
                //Dock = DockStyle.Fill,
                Width = 80,
                Height = 35,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

           
            CancelButton = okButton;
            table.Controls.Add(resetButton, 4, 5);
            table.Controls.Add(okButton, 5, 5);

            //row-5
            //blank

            Controls.Add(table);
        }


        private void InitComponentEvent()
        {
            this.Load += Form_Load;

            historyComboBox.SelectedValueChanged += HistoryComboBox_SelectedValueChanged;

            okButton.Click += OkButton_Click;
            resetButton.Click += ResetButton_Click;
            CancelButton = okButton;

            fromDate.ValueChanged += FromDate_ValueChanged;
            toDate.ValueChanged += ToDate_ValueChanged;

            searchButton.Click += SearchButton_Click;
            historyTable.DataBindingComplete += HistoryTable_DataBindingComplete;

        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Clear();
            HideDateRange();
            historyComboBox.SelectedIndex = 0;
            paymentModeComboBox.SelectedIndex = 0;
            paymentStatusComboBox.SelectedIndex = 0;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            datewiseMap = new Dictionary<DateTime, IList<Payment>>();
            historyPaymentList = new List<Payment>();

            HideDateRange();
            historyComboBox.Items.Add("None");
            historyComboBox.Items.Add("All");
            historyComboBox.Items.Add("Date Range");

            paymentModeComboBox.Items.Add(string.Empty);
            foreach (PaymentMode value in Enum.GetValues(typeof(PaymentMode)))
            {
                paymentModeComboBox.Items.Add(value);
            }

            paymentStatusComboBox.Items.Add(string.Empty);
            foreach (BillingStatus value in Enum.GetValues(typeof(BillingStatus)))
            {
                paymentStatusComboBox.Items.Add(value);
            }

            historyComboBox.SelectedIndex = 0;
            paymentModeComboBox.SelectedIndex = 0;
            paymentStatusComboBox.SelectedIndex = 0;

            paymentDao = new PaymentDaoImpl(DatabaseManager.GetConnection());
            paymentList = paymentDao.ReadAll();

            uniqueDates = paymentList.Select(x => x.PaidDateTime.Date).Distinct().ToList();

            foreach (DateTime date in uniqueDates)
            {
                datewiseMap.Add(date, paymentList.Where(x => DateTime.Equals(x.PaidDateTime.Date, date)).ToList());
            }


            ShowPaymentRecordCount(0);
            ShowTotalPaidAmount(0);
            ShowTotalBalanceAmount(0);
        }

        private void BindPaymentToDataGridView()
        {
            historyTable.DataSource = null;
            historyTable.DataSource = historyPaymentList;
        }

        private void HistoryTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            long srNo = 1;

            foreach (DataGridViewRow row in historyTable.Rows)
            {
                // Set the value for Customer.Name column
                if (row.DataBoundItem is Payment payment)
                {
                    row.Cells[0].Value = srNo++;
                    row.Cells[1].Value = payment.PaidDateTime.ToString("yyyy-MM-dd, hh:mm:ss tt");

                    if (!string.IsNullOrEmpty(payment.BalancePaidDate))
                    {
                        DateTime date = DateTime.ParseExact(payment.BalancePaidDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        row.Cells[5].Value = date.ToString("yyyy-MM-dd, hh:mm:ss tt");
                    }
                }
            }
        }

        private void HistoryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedValue = historyComboBox.SelectedItem.ToString();
            if (selectedValue == "Date Range")
            {
                ShowDateRange();
            }
            else
            {
                HideDateRange();
            }
            Clear();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            historyPaymentList.Clear();

            string selectedValue = historyComboBox.SelectedItem.ToString();
            //Console.WriteLine("selectedValue ::" + selectedValue);

            if (selectedValue == "All")
            {
                sortedDates = datewiseMap.Keys.OrderBy(date => date).ToList();
                //var sortedDatewiseMap = datewiseMap.OrderBy(kvp => kvp.Key).ToList();
                ShowPaymentHistory();
            }
            else if (selectedValue == "Date Range")
            {
                sortedDates = datewiseMap.Keys.Where(date => fromDate.Value <= date && date <= toDate.Value)
                    .OrderBy(date => date)
                    .ToList();
                ShowPaymentHistory();
            }
            else
            {
                sortedDates = null;
                Clear();
                MessageBox.Show("Search is not allow.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ShowPaymentHistory()
        {

            double totalPaidAmt = 0, totalBalanceAmt = 0;
            foreach (DateTime billDate in sortedDates)
            {
                IList<Payment> payments = datewiseMap[billDate];

                foreach (Payment payment in payments)
                {
                    string mode = paymentModeComboBox.SelectedItem.ToString();
                    string status = paymentStatusComboBox.SelectedItem.ToString();
                    if(string.IsNullOrEmpty(mode) && string.IsNullOrEmpty(status))
                    {
                        historyPaymentList.Add(payment);

                        totalPaidAmt += payment.PaidAmount;
                        totalBalanceAmt += payment.BalanceAmount;
                    }
                    else if (!string.IsNullOrEmpty(mode) && string.IsNullOrEmpty(status))
                    {
                        PaymentMode paymentMode = (PaymentMode)Enum.Parse(typeof(PaymentMode), mode);
                        if(payment.PaymentMode == paymentMode)
                        {
                            historyPaymentList.Add(payment);

                            totalPaidAmt += payment.PaidAmount;
                            totalBalanceAmt += payment.BalanceAmount;
                        }
                    }
                    else if (string.IsNullOrEmpty(mode) && !string.IsNullOrEmpty(status))
                    {
                        BillingStatus billingStatus = (BillingStatus)Enum.Parse(typeof(BillingStatus), status);
                        if (payment.Status == billingStatus)
                        {
                            historyPaymentList.Add(payment);

                            totalPaidAmt += payment.PaidAmount;
                            totalBalanceAmt += payment.BalanceAmount;
                        }
                    }
                    else
                    {
                        PaymentMode paymentMode = (PaymentMode)Enum.Parse(typeof(PaymentMode), mode);
                        BillingStatus billingStatus = (BillingStatus)Enum.Parse(typeof(BillingStatus), status);
                        if (payment.PaymentMode == paymentMode && payment.Status == billingStatus)
                        {
                            historyPaymentList.Add(payment);

                            totalPaidAmt += payment.PaidAmount;
                            totalBalanceAmt += payment.BalanceAmount;
                        }
                    }
                }

            }

            BindPaymentToDataGridView();

            ShowPaymentRecordCount(historyPaymentList.Count);
            ShowTotalPaidAmount(totalPaidAmt);
            ShowTotalBalanceAmount(totalBalanceAmt);

            if (historyPaymentList.Count == 0)
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
            ShowPaymentRecordCount(0);
            ShowTotalPaidAmount(0);
            ShowTotalBalanceAmount(0);
            historyPaymentList.Clear();
            historyTable.Rows.Clear();
        }

        private void ShowPaymentRecordCount(int count)
        {
            totalPaymentRecordLabel.Text = count.ToString();
        }

        public void ShowTotalPaidAmount(double totalAmount)
        {
            totalPaidAmountLabel.Text = totalAmount.ToString("C3");
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
