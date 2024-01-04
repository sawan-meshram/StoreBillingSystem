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
namespace StoreBillingSystem.StoreForm.PurchaseForm
{
    public class ProductPurchaseDisplayForm : Form
    {

        public ProductPurchaseDisplayForm(Product product, IList<ProductPurchase> datewisePurchasesList)
        {
            this.product = product;
            this.datewisePurchasesList = datewisePurchasesList;
            InitializeComponent();
            InitComponentEvent();
        }

        private IList<ProductPurchase> datewisePurchasesList;
        private Product product;

        private Font textBoxFont = U.StoreTextBoxFont;

        private Label totalPurchaseRecordLabel;
        private Label totalPurchaseAmountLabel;

        private Button okButton;
        private DataGridView historyTable;


        private void InitializeComponent()
        {
            Text = $"Product history for \"{ product.Name}\"";


            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1150, 620);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 5,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 490F)); //row-2 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-3

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15f)); //blank
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200f)); //history purchase lbl
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f)); //history purchase lbl
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 460f)); //history purchase lbl

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150f)); //history purchase lbl
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200f)); //history purchase lbl



            //row-0 //blank
            //row-1
            historyTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };
            historyTable.RowHeadersDefaultCellStyle.Font = U.StoreLabelFont;

            historyTable.ColumnCount = 10;
            historyTable.Columns[0].Name = "Sr. No.";
            historyTable.Columns[1].Name = "Purchase Date";
            historyTable.Columns[2].Name = "Qty";
            historyTable.Columns[3].Name = "Price";
            historyTable.Columns[4].Name = "Purchase SGST";
            historyTable.Columns[5].Name = "Purchase CGST";
            historyTable.Columns[6].Name = "Mfg Date";
            historyTable.Columns[7].Name = "Exp Date";
            historyTable.Columns[8].Name = "Batch Number";
            historyTable.Columns[9].Name = "Total";


            historyTable.Columns[0].Width = 60;
            historyTable.Columns[1].Width = 120;
            historyTable.Columns[2].Width = 100;
            historyTable.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            historyTable.Columns[4].Width = 100;
            historyTable.Columns[5].Width = 100;
            historyTable.Columns[6].Width = 100;
            historyTable.Columns[7].Width = 100;
            historyTable.Columns[8].Width = 120;
            historyTable.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            historyTable.Columns[2].DefaultCellStyle.Format = "N";
            historyTable.Columns[3].DefaultCellStyle.Format = "C3";
            historyTable.Columns[4].DefaultCellStyle.Format = "P";
            historyTable.Columns[5].DefaultCellStyle.Format = "P";
            historyTable.Columns[9].DefaultCellStyle.Format = "C3";


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

            table.Controls.Add(historyTable, 1, 1);
            table.SetColumnSpan(historyTable, table.ColumnCount - 2);

            //row-3
            Label purchaseRecordLabel = new Label
            {
                Text = "Purchase Record :",
                Font = textBoxFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(purchaseRecordLabel, 1, 2);


            totalPurchaseRecordLabel = new Label
            {
                Font = textBoxFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalPurchaseRecordLabel, 2, 2);

            Label totalLabel = new Label
            {
                Text = "Total Purchase Amt. :",
                Font = textBoxFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalLabel, 4, 2);

            totalPurchaseAmountLabel = new Label
            {
                Font = textBoxFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalPurchaseAmountLabel, 5, 2);



            okButton = new Button
            {
                Text = "Ok",
                //Dock = DockStyle.Fill,
                //DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.None,
                Width = 100,
                Height = 35,
                Font = textBoxFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            table.Controls.Add(okButton, 3, 3);


            //row-4
            //blank
            Controls.Add(table);
        }


        private void InitComponentEvent()
        {
            this.Load += Form_Load;

            okButton.Click += OkButton_Click;

            CancelButton = okButton;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            //productDao = new ProductDaoImpl(DatabaseManager.GetConnection());

            if (datewisePurchasesList != null)
            {
                int srNo = 1;
                double totalAmount = 0;
                foreach (ProductPurchase purchase in datewisePurchasesList)
                {
                    double total = purchase.PurchasePrice * purchase.Qty;

                    historyTable.Rows.Add(srNo, purchase.PurchaseDateTime.ToString("dd MMM yyyy"), purchase.Qty, purchase.PurchasePrice,
                        purchase.PurchaseCGSTInPercent, purchase.PurchaseSGSTInPercent,
                        string.IsNullOrEmpty(purchase.MfgDate) ? "" : DateTime.ParseExact(purchase.MfgDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("MMM yyy"),
                        string.IsNullOrEmpty(purchase.ExpDate) ? "" : DateTime.ParseExact(purchase.ExpDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("MMM yyy"),
                        string.IsNullOrEmpty(purchase.BatchNumber) ? "" : purchase.BatchNumber,
                        total);

                    srNo++;
                    totalAmount += total;
                }
                ShowPurchaseRecordCount(datewisePurchasesList.Count);
                ShowTotalPurchaseAmount(totalAmount);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ShowPurchaseRecordCount(int count)
        {
            totalPurchaseRecordLabel.Text = count.ToString();
        }

        public void ShowTotalPurchaseAmount(double totalAmount)
        {
            totalPurchaseAmountLabel.Text = totalAmount.ToString("C3");
        }

    }
}
