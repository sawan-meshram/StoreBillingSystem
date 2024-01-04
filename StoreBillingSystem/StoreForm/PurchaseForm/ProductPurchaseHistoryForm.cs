using System;
using System.Data;

using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using StoreBillingSystem.Database;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Util;
using StoreBillingSystem.Events;

namespace StoreBillingSystem.StoreForm.PurchaseForm
{
    public class ProductPurchaseHistoryForm : Form
    {
        public ProductPurchaseHistoryForm()
        {
            InitializeComponent();

            InitComponentEvent();

        }

        private List<DateTime> uniqueDates;
        private IDictionary<DateTime, IList<ProductPurchase>> datewiseMap;
        private IDictionary<long, Product> allProductMap;

        private IProductPurchaseDao productPurchaseDao;
        private IList<ProductPurchase> allProductPurchaseList;
        private IList<DateTime> sortedDates;

        private string _productNamePlaceHolder = U.ToTitleCase("Search Product Name Here..");


        private DataTable productDataTable;
        private TextBox productNameText;

        private Font labelFont = U.StoreLabelFont;
        private Font textBoxFont = U.StoreTextBoxFont;

        private Label totalPurchaseRecordLabel;
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
            Text = "Product Purchase History";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1100, 700);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 10,
                RowCount = 8,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 460F)); //row-4 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-5
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-6

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f)); //history purchase lbl
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200f)); //combo box
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
            table.Controls.Add(new Label
            {
                Text = "Search Product :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 1, 2);

            productNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = _productNamePlaceHolder,
                ForeColor = Color.Gray,
            };
            table.Controls.Add(productNameText, 2, 2);
            table.SetColumnSpan(productNameText, 2);
            //row-3 - blank

            //row-4
            historyTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };
            historyTable.RowHeadersDefaultCellStyle.Font = U.StoreLabelFont;

            historyTable.ColumnCount = 3;
            historyTable.Columns[0].Name = "SrNo.";
            historyTable.Columns[1].Name = "Product Id";
            historyTable.Columns[2].Name = "Product Name";

            historyTable.Columns[0].Width = 200;
            historyTable.Columns[1].Width = 200;
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

            table.Controls.Add(historyTable, 1, 4);
            table.SetColumnSpan(historyTable, table.ColumnCount - 2);



            //row-5
            Label purchaseRecordLabel = new Label
            {
                Text = "Purchase Record :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(purchaseRecordLabel, 1, 5);


            totalPurchaseRecordLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalPurchaseRecordLabel, 2, 5);


            //Row-6
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

            table.Controls.Add(table1, 1, 6);
            table.SetColumnSpan(table1, table.ColumnCount - 1);

            //row-7 //blank

            Controls.Add(table);
        }

        private void InitComponentEvent()
        {
            this.Load += Form_Load;

            productNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(productNameText, _productNamePlaceHolder);
            productNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(productNameText, _productNamePlaceHolder);
            productNameText.TextChanged += SearchCustomerTextBox_TextChanged;


            historyComboBox.SelectedValueChanged += HistoryComboBox_SelectedValueChanged;

            okButton.Click += OkButton_Click;
            viewButton.Click += ViewButton_Click;
            CancelButton = okButton;

            fromDate.ValueChanged += FromDate_ValueChanged;
            toDate.ValueChanged += ToDate_ValueChanged;

            searchButton.Click += SearchButton_Click;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            HideDateRange();
            historyComboBox.Items.Add("None");
            historyComboBox.Items.Add("All");
            historyComboBox.Items.Add("Date Range");

            historyComboBox.SelectedIndex = 0;

            productPurchaseDao = new ProductPurchaseDaoImpl(DatabaseManager.GetConnection());
            allProductPurchaseList = productPurchaseDao.ReadAll();

            allProductMap = new Dictionary<long, Product>();
            foreach(ProductPurchase purchase in allProductPurchaseList)
            {
                if(purchase.Product != null && !allProductMap.ContainsKey(purchase.Product.Id))
                {
                    allProductMap.Add(purchase.Product.Id, purchase.Product);
                }
            }

            uniqueDates = allProductPurchaseList.Select(x => x.PurchaseDateTime.Date).Distinct().ToList();
            datewiseMap = new Dictionary<DateTime, IList<ProductPurchase>>();

            foreach (DateTime date in uniqueDates)
            {
                datewiseMap.Add(date, allProductPurchaseList.Where(x => DateTime.Equals(x.PurchaseDateTime.Date, date)).ToList());
            }

            productDataTable = new DataTable();
            productDataTable.Columns.Add("SrNo.", typeof(int));
            productDataTable.Columns.Add("Product Id", typeof(long));
            productDataTable.Columns.Add("Name", typeof(string));

            historyTable.Columns[0].DataPropertyName = "SrNo.";
            historyTable.Columns[1].DataPropertyName = "Product Id";
            historyTable.Columns[2].DataPropertyName = "Name";

            historyTable.DataSource = productDataTable;

            ShowPurchaseRecordCount(0);
        }

        private void SearchCustomerTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBoxKeyEvent.CapitalizeText_TextChanged(productNameText);

            string searchTerm = productNameText.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == _productNamePlaceHolder.ToLower()) return;

            if (productDataTable.Rows.Count == 0) return;


            DataTable filterResults = productDataTable.Copy();
            filterResults.Clear();
            int srNo = 1;
            foreach (DataRow row in productDataTable.Rows)
            {
                if (row["Name"].ToString().ToLower().Contains(searchTerm))
                {

                    row["SrNo."] = srNo++;
                    filterResults.Rows.Add(row.ItemArray);
                }
            }

            historyTable.Rows.Clear();
            historyTable.DataSource = filterResults;
            ShowPurchaseRecordCount(filterResults.Rows.Count);
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
            string selectedValue = historyComboBox.SelectedItem.ToString();
            if (selectedValue == "All")
            {
                sortedDates = datewiseMap.Keys.OrderBy(date => date).ToList();
                //var sortedDatewiseMap = datewiseMap.OrderBy(kvp => kvp.Key).ToList();
                ShowProductPurchaseHistory();
            }
            else if (selectedValue == "Date Range")
            {
                sortedDates = datewiseMap.Keys.Where(date => fromDate.Value <= date && date <= toDate.Value)
                    .OrderBy(date => date)
                    .ToList();
                ShowProductPurchaseHistory();
            }
            else
            {
                sortedDates = null;
                Clear();
                MessageBox.Show("Search is not allow.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ShowProductPurchaseHistory()
        {
            productDataTable.Clear();
            historyTable.Rows.Clear();

            int srNo = 1;
            ISet<long> uniqueProductIds = new HashSet<long>();

            foreach (DateTime purchaseDate in sortedDates)
            {
                IList<ProductPurchase> productPurchases = datewiseMap[purchaseDate];

                foreach (ProductPurchase purchase in productPurchases)
                {
                    if (purchase.Product != null)
                    {
                        if (uniqueProductIds.Add(purchase.Product.Id))
                        {
                            productDataTable.Rows.Add(srNo, purchase.Product.Id, purchase.Product.Name);
                            srNo++;
                        }
                    }
                }
            }

            historyTable.DataSource = productDataTable;

            ShowPurchaseRecordCount(productDataTable.Rows.Count);

            if (sortedDates.Count == 0)
                MessageBox.Show("No records found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ToDate_ValueChanged(object sender, EventArgs e)
        {
            if (fromDate.Value > toDate.Value)
                fromDate.Value = toDate.Value;
        }

        private void FromDate_ValueChanged(object sender, EventArgs e)
        {
            if (fromDate.Value > toDate.Value)
                toDate.Value = fromDate.Value;
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            if (historyTable.SelectedRows[0].Index >= 0)
            {
                object[] row = productDataTable.Rows[historyTable.SelectedRows[0].Index].ItemArray;

                Product product = allProductMap[(long)row[1]];

                IList<ProductPurchase> selectedProductPurchases = new List<ProductPurchase>();

                foreach (DateTime purchaseDate in sortedDates)
                {
                    IList<ProductPurchase> productPurchases = datewiseMap[purchaseDate];

                    foreach (ProductPurchase purchase in productPurchases)
                    {
                        if (purchase.Product != null && purchase.Product.Id == product.Id)
                        {
                            selectedProductPurchases.Add(purchase);
                        }
                    }
                }
                new ProductPurchaseDisplayForm(product, selectedProductPurchases).ShowDialog();
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Clear()
        {
            TextBoxKeyEvent.BindPlaceholderToTextBox(productNameText, _productNamePlaceHolder, Color.Gray);

            ShowPurchaseRecordCount(0);
            historyTable.Rows.Clear();
        }

        private void ShowPurchaseRecordCount(int count)
        {
            totalPurchaseRecordLabel.Text = count.ToString();
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
