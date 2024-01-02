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
namespace StoreBillingSystem.StoreForm.PurchaseForm
{
    public class PurchaseHistoryForm : Form
    {
        public PurchaseHistoryForm()
        {
            InitializeComponent();

            InitComponentEvent();

        }
        private IProductDao productDao;
        private IProductPurchaseDao productPurchaseDao;
        private IList<ProductPurchase> productPurchaseList;

        private Font labelFont = U.StoreLabelFont;
        private Font textBoxFont = U.StoreTextBoxFont;

        private Label totalPurchaseRecordLabel;
        private Label totalPurchaseAmountLabel;
        private Label lblFrom;
        private Label lblTo;

        private Button okButton;
        private Button viewButton;
        private Button deleteButton;
        private Button deleteAllButton;

        private ComboBox historyComboBox;
        private DataGridView historyTable;
        private DateTimePicker fromDate;
        private DateTimePicker ToDate;

        private void InitializeComponent()
        {
            Text = "Product History";

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
                Text = "History Purchase :",
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
                Enabled = false,
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

            ToDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
                Enabled = false
            };
            table.Controls.Add(ToDate, 8, 0);

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

            historyTable.ColumnCount = 3;
            historyTable.Columns[0].Name = "Sr. No.";
            historyTable.Columns[1].Name = "Purchase Date";
            historyTable.Columns[2].Name = "Purchase Total";

            historyTable.Columns[0].Width = 100;
            historyTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            historyTable.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            historyTable.Columns[2].DefaultCellStyle.Format = "N";


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
            table.SetColumnSpan(historyTable, table.ColumnCount-1);



            //row-3
            Label purchaseRecordLabel = new Label
            {
                Text = "Purchase Record :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(purchaseRecordLabel, 1, 3);


            totalPurchaseRecordLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalPurchaseRecordLabel, 2, 3);

            Label totalLabel = new Label
            {
                Text = "Total Purchase Amt. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalLabel, 5, 3);
            table.SetColumnSpan(totalLabel, 2);

            totalPurchaseAmountLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalPurchaseAmountLabel, 7, 3);
            table.SetColumnSpan(totalPurchaseAmountLabel, 2);


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
            table.SetColumnSpan(table1, table.ColumnCount-1);

            //row-5
            //blank

            Controls.Add(table);
        }

        private void InitComponentEvent()
        {
            this.Load += Form_Load;

            historyComboBox.SelectedValueChanged += HistoryComboBox_SelectedValueChanged;

            okButton.Click += OkButton_Click;

            CancelButton = okButton;
        }

        private void HistoryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedValue = historyComboBox.SelectedItem.ToString();
            if(selectedValue == "Date Range")
            {
                ShowDateRange();
            }
            else
            {
                HideDateRange();
            }
        }

        private void ShowDateRange()
        {
            lblTo.Show();
            lblFrom.Show();
            fromDate.Show();
            ToDate.Show();
        }

        private void HideDateRange()
        {
            lblTo.Hide();
            lblFrom.Hide();
            fromDate.Hide();
            ToDate.Hide();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            HideDateRange();
            historyComboBox.Items.Add("None");
            historyComboBox.Items.Add("All");
            historyComboBox.Items.Add("Date Range");

            historyComboBox.SelectedIndex = 0;

            productPurchaseDao = new ProductPurchaseDaoImpl(DatabaseManager.GetConnection());
            productPurchaseList = productPurchaseDao.ReadAll();

            List<DateTime> uniqueDates = productPurchaseList.Select(x => x.PurchaseDateTime.Date).Distinct().ToList();
            Dictionary<DateTime, IList<ProductPurchase>> datewiseMap = new Dictionary<DateTime, IList<ProductPurchase>>();

            foreach(DateTime date in uniqueDates)
            {
                datewiseMap.Add(date, productPurchaseList.Where(x => DateTime.Equals(x.PurchaseDateTime.Date, date)).ToList());
                
                Console.WriteLine(date);
            }

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
