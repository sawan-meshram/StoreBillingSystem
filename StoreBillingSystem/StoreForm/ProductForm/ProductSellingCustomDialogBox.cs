using System;
using System.Drawing;
using System.Windows.Forms;

using StoreBillingSystem.Entity;
namespace StoreBillingSystem.StoreForm.ProductForm
{
    public class CustomProductSellingDialogBox : Form
    {
        public CustomProductSellingDialogBox(DataGridView productSellingTable, ProductSelling productSelling)
        {
            this.productSellingTable = productSellingTable;
            this.productSelling = productSelling;

            InitializeComponent();

            InitProductSellingData();
        }

        private DataGridView productSellingTable;
        private ProductSelling productSelling;

        private Font labelFont = Util.U.StoreLabelFont;

        private void InitializeComponent()
        {
            Text = productSelling.Product.Name;

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(850, 430);
            BackColor = Util.U.StoreDialogBackColor;
            this.AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 11,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-4
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); //row-5
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); //row-6
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-7 //Blank
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 115F)); //row-8 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-9


            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F)); //name

            //Row-0
            table.Controls.Add(
                new Label { Text = "Product Id :", Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleRight,},
                0, 0);
            table.Controls.Add(
                new Label { Text = productSelling.Product.Id.ToString(), Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(50,0,0,0), },
                1, 0);

            //Row-1
            table.Controls.Add(
                new Label { Text = "Product Name :", Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleRight, },
                0, 1);
            table.Controls.Add(
                new Label { Text = productSelling.Product.Name, Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(50, 0, 0, 0), },
                1, 1);

            //Row-2
            table.Controls.Add(
                new Label { Text = "Category :", Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleRight, },
                0, 2);
            table.Controls.Add(
                new Label { Text = productSelling.Product.Category.Name, Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(50, 0, 0, 0), },
                1, 2);

            //Row-3
            table.Controls.Add(
                new Label { Text = "Total Qty :", Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleRight, },
                0, 3);

            table.Controls.Add(
                new Label { Text = productSelling.Product.TotalQty.ToString(), Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(50, 0, 0, 0), },
                1, 3);

            //Row-4
            table.Controls.Add(
                new Label { Text = "Product Type :", Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleRight, },
                0, 4);

            table.Controls.Add(
                new Label { Text = productSelling.Product.ProductType.Name, Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(50, 0, 0, 0), },
                1, 4);

            //Row-5
            table.Controls.Add(
                new Label { Text = "CGST (%) :", Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleRight, },
                0, 5);

            table.Controls.Add(
                new Label { Text = productSelling.CGSTInPercent.ToString(), Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(50, 0, 0, 0), },
                1, 5);

            //Row-6
            table.Controls.Add(
                new Label { Text = "SGST (%) :", Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleRight, },
                0, 6);
            table.Controls.Add(
                new Label { Text = productSelling.SGSTInPercent.ToString(), Font = labelFont, Dock = DockStyle.Fill, ForeColor = Color.Black, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(50, 0, 0, 0), },
                1, 6);

            //Row-7
            //blank

            //Row-8
            /*productSellingTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
            };
            */
            productSellingTable.RowHeadersDefaultCellStyle.Font = Util.U.StoreLabelFont;
            productSellingTable.ColumnCount = 7;
            productSellingTable.Columns[0].Name = "Selling Type";
            productSellingTable.Columns[1].Name = "Selling (Rs.)";
            productSellingTable.Columns[2].Name = "Discount (Rs.)";
            productSellingTable.Columns[3].Name = "Total Selling (Rs.)";
            productSellingTable.Columns[4].Name = "GST (%)";
            productSellingTable.Columns[5].Name = "GST (Rs.)";
            productSellingTable.Columns[6].Name = "Total (Rs.)";


            productSellingTable.Columns[0].Width = 120;
            productSellingTable.Columns[1].Width = 120;
            productSellingTable.Columns[2].Width = 120;
            productSellingTable.Columns[3].Width = 120;
            productSellingTable.Columns[4].Width = 120;
            productSellingTable.Columns[5].Width = 120;
            productSellingTable.Columns[6].Width = 120;

            productSellingTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //data display in center
            productSellingTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //header display in center

            productSellingTable.ReadOnly = true;
            productSellingTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productSellingTable.MultiSelect = false;


            //productSellingTable.Columns[0].Width = 140;
            //productSellingTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn column in productSellingTable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            productSellingTable.AllowUserToAddRows = false;
            productSellingTable.AutoGenerateColumns = false;
            productSellingTable.RowHeadersVisible = false;
            productSellingTable.AllowUserToResizeRows = false;
            productSellingTable.AllowUserToResizeColumns = false;

            //to select only row at a time
            //productSellingTable.SelectionChanged += ProductSellingTable_SelectionChanged;


            table.Controls.Add(productSellingTable, 0, 8);
            table.SetColumnSpan(productSellingTable, 3);

            //Row-9
            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200f)); //name

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

            table.Controls.Add(table1, 1, 9);


            Controls.Add(table);

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

        private void InitProductSellingData()
        {
            float gstInPercent = productSelling.GetTotalGSTInPercent();

            AddRow("A", productSelling.SellingPrice_A, productSelling.DiscountPrice_A, gstInPercent);
            AddRow("B", productSelling.SellingPrice_B, productSelling.DiscountPrice_B, gstInPercent);
            AddRow("C", productSelling.SellingPrice_C, productSelling.DiscountPrice_C, gstInPercent);
            AddRow("D", productSelling.SellingPrice_D, productSelling.DiscountPrice_D, gstInPercent);
        }

        private void AddRow(string type, double sellingPrice, double discountPrice, float gstInPercent) 
        {
            double totalSelling = productSelling.GetTotalTaxableSellingPrice(sellingPrice, discountPrice);
            double gstInPrice = productSelling.GetTotalTaxableSellingGSTInPrice(sellingPrice, discountPrice);
            double total = productSelling.GetTotalSellingPrice(sellingPrice, discountPrice);

            productSellingTable.Rows.Add(type, sellingPrice, discountPrice, totalSelling, gstInPercent, gstInPrice, total);
        }

        private void ProductSellingTable_SelectionChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < productSellingTable.SelectedRows.Count-1; i++){
                productSellingTable.SelectedRows[0].Selected = false;
            }
        }
    }
}
