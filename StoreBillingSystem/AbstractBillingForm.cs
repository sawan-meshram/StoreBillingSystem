using System;
using System.Drawing;
using System.Windows.Forms;
using StoreBillingSystem.Util;

namespace StoreBillingSystem
{
    public class AbstractBillingForm : Form
    {
        private Font labelFont = new Font("Arial", 12, FontStyle.Bold);
        private Font textfieldFont = new Font("Arial", 12);


        private Panel topPanel;
        private Panel topPanel_1;
        private Panel bottomPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel centerPanel;

        public AbstractBillingForm()
        {
            InitComponets();

        }

        /// <summary>
        /// Main Form
        /// </summary>
        private void InitComponets()
        {
            // Create panels for each region
            topPanel = new Panel();
            topPanel.BackColor = Color.LightBlue;
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;


            topPanel.Controls.Add(GetHeaderForm());

            bottomPanel = new Panel();
            bottomPanel.BackColor = Color.LightGreen;
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 50;

            leftPanel = new Panel();
            leftPanel.BackColor = Color.LightYellow;
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 100;

            rightPanel = new Panel();
            rightPanel.BackColor = Color.LightCoral;
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 100;

            centerPanel = new Panel();
            centerPanel.BackColor = Color.White;
            centerPanel.Dock = DockStyle.Fill;


            centerPanel.Controls.Add(GetBillingForm());

            // Create panels for each region
            topPanel_1 = new Panel();
            topPanel_1.BackColor = Color.Yellow;
            topPanel_1.Dock = DockStyle.Top;
            topPanel_1.Height = 100;
            topPanel_1.Controls.Add(GetProductForm());

            // Add panels to the form
            this.Controls.Add(topPanel_1);

            this.Controls.Add(topPanel);

            this.Controls.Add(bottomPanel);
            this.Controls.Add(leftPanel);

            this.Controls.Add(rightPanel);
            this.Controls.Add(centerPanel);

            bottomPanel.Controls.Add(GetFooterForm());


        }


        protected Button clearBillingButton;
        protected Button printBillingButton;
        protected Button saveBillingButton;
        protected Button newBillingButton;
        /// <summary>
        /// Gets the footer form.
        /// </summary>
        /// <returns>The footer form.</returns>
        private Panel GetFooterForm()
        {

            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //Size = new Size(1100, 90),
                //Location = new Point(0, 0),
                BackColor = Color.MintCream,
                RowCount = 2,
                ColumnCount = 6
            };

            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F)); //row-1
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F)); //col-0
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //col-0
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //col-0
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //col-0
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //col-0

            clearBillingButton = new Button
            {
                Text = "&Clear All",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(clearBillingButton, 1, 0);

            printBillingButton = new Button
            {
                Text = "&Print",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(printBillingButton, 2, 0);


            saveBillingButton = new Button
            {
                Text = "&Save",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(saveBillingButton, 3, 0);


            newBillingButton = new Button
            {
                Text = "&New",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(newBillingButton, 4, 0);

            return panel;
        }


        protected string customerNamePlaceHolder = U.ToTitleCase("Enter customer name ...");
        protected string mobileNumberPlaceHolder = U.ToTitleCase("Enter mobile number ...");

        protected TextBox billingNumText;
        protected TextBox customerNameText;
        protected TextBox mobileNumberText;
        protected DateTimePicker billDateTimePicker;
        protected Button clearCustomerButton;
        /// <summary>
        /// Gets the header form.
        /// </summary>
        /// <returns>The header form.</returns>
        private Panel GetHeaderForm()
        {

            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //Size = new Size(1100, 90),
                //Location = new Point(0, 0),
                //BackColor = Color.Aquamarine
                ColumnCount = 8,
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F)); //col-0
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F)); //col-1
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); //col-2
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); //col-3
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F)); //col-4
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); //col-5
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F)); //col-6

            //Billing Number
            panel.Controls.Add(new Label
            {
                Text = "Billing No. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 0);

            billingNumText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                BackColor = Color.White,
                ReadOnly = true
            };
            panel.Controls.Add(billingNumText, 1, 0);

            panel.Controls.Add(new Label
            {
                Text = "Billing Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 5, 0);


            billDateTimePicker = new DateTimePicker
            {
                //CustomFormat = "yyyy-MM-dd HH:mm:ss",
                Format = DateTimePickerFormat.Short,
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(billDateTimePicker, 6, 0);

            //Customer Name
            panel.Controls.Add(new Label
            {
                Text = "Customer Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 1);

            customerNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = customerNamePlaceHolder,
                ForeColor = Color.Gray,
            };
            panel.Controls.Add(customerNameText, 1, 1);

            panel.Controls.Add(new Label
            {
                Text = "Mobile Number :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 1);

            mobileNumberText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = mobileNumberPlaceHolder,
                ForeColor = Color.Gray,
            };
            panel.Controls.Add(mobileNumberText, 4, 1);

            clearCustomerButton = new Button
            {
                Text = "Clear",
                //Dock = DockStyle.Right,
                Width = 100,
                Height = 27,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5),
            };
            panel.Controls.Add(clearCustomerButton, 5, 1);

            panel.SetColumnSpan(customerNameText, 2);

            return panel;
        }


        protected string pricePlaceHolder = "0.00";
        protected string qtyPercentPlaceHolder = "0.0";
        protected string productPlaceHolder = Util.U.ToTitleCase("Enter product here...");
        protected string searchCodePlaceHolder = Util.U.ToTitleCase("Search product code...");

        protected TextBox productNameText;
        protected TextBox productCodeText;
        protected TextBox askingPriceText;
        protected TextBox qtyText;
        protected ComboBox productTypes;
        protected Button clearProductButton;
        protected Button addProductButton;

        /// <summary>
        /// Gets the product form.
        /// </summary>
        /// <returns>The product form.</returns>
        private GroupBox GetProductForm()
        {
            GroupBox box = new GroupBox
            {
                Text = "Product Billing",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.Blue,
                //Margin = new Padding(0, 50, 0, 0),
            };

            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //Size = new Size(1100, 50),
                //Location = new Point(0, 0),
                BackColor = Color.YellowGreen,
                ColumnCount = 8
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); //name
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); //types
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); //qty
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); //asking price
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //Button

            //Product Name
            panel.Controls.Add(new Label
            {
                Text = "Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 0);

            productNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = productPlaceHolder,
                ForeColor = Color.Gray
            };
            panel.Controls.Add(productNameText, 1, 0);
            panel.SetColumnSpan(productNameText, 3);

            // Product Search
            panel.Controls.Add(new Label
            {
                Text = "Search Code :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 0);

            productCodeText = new TextBox
            {
                Text = searchCodePlaceHolder,
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(productCodeText, 5, 0);

            //Clear Button
            clearProductButton = new Button
            {
                Text = "&Clear",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(clearProductButton, 6, 0);


            //Product Types
            panel.Controls.Add(new Label
            {
                Text = "Types :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 1);

            productTypes = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panel.Controls.Add(productTypes, 1, 1);


            //Product Qty
            panel.Controls.Add(new Label
            {
                Text = "Qty. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 2, 1);

            qtyText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = qtyPercentPlaceHolder,
                ForeColor = Color.Gray
            };
            panel.Controls.Add(qtyText, 3, 1);

            //Asking Price
            panel.Controls.Add(new Label
            {
                Text = "Asking Price :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 1);

            askingPriceText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
            };
            panel.Controls.Add(askingPriceText, 5, 1);

            addProductButton = new Button
            {
                Text = "&Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(addProductButton, 6, 1);
            box.Controls.Add(panel);

            return box;
        }

        protected DataGridView billingTable;
        protected DataGridView amtTable;
        /// <summary>
        /// Gets the billing form at Center for Total display.
        /// </summary>
        /// <returns>The billing form.</returns>
        private Panel GetBillingForm()
        {
            Panel panel = new Panel
            {
                //FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                BackColor = Color.Brown,
                //Size = new Size(1000, 500),
                //Location = new Point(55, 155),

            };


            // Create a DataGridView
            billingTable = new DataGridView
            {
                Dock = DockStyle.None,
                BackgroundColor = Color.Cyan,
                Margin = new Padding(0),
                Location = new Point(100, 180),
                //Left = 100,
                //Top = 180,
                Size = new Size(1160, 454),
                //Font = textfieldFont,
                //Width = 1160, //based on left & right panel width
            };
            billingTable.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            //dataGridView.ColumnHeadersDefaultCellStyle.Font = textfieldFont;

            //Console.WriteLine(dataGridView.Location.X + " " + dataGridView.Location.Y);
            // Create columns for the DataGridView
            billingTable.ColumnCount = 9;
            billingTable.Columns[0].Name = "Sr.No.";
            billingTable.Columns[1].Name = "Product Name";
            billingTable.Columns[2].Name = "Type";
            billingTable.Columns[3].Name = "Qty.";
            billingTable.Columns[4].Name = "Rate";
            billingTable.Columns[5].Name = "Amount";
            billingTable.Columns[6].Name = "GST(%)";
            billingTable.Columns[7].Name = "Discount(Rs.)";
            billingTable.Columns[8].Name = "Total";


            billingTable.Columns[0].Width = 80;
            billingTable.Columns[1].Width = 400;
            billingTable.Columns[2].Width = 80;
            billingTable.Columns[3].Width = 70;
            billingTable.Columns[4].Width = 80;
            billingTable.Columns[5].Width = 100;
            billingTable.Columns[6].Width = 100;
            billingTable.Columns[7].Width = 100;
            billingTable.Columns[8].Width = 150;

            billingTable.Columns[4].DefaultCellStyle.Format = "C2";
            billingTable.Columns[5].DefaultCellStyle.Format = "C2";
            billingTable.Columns[6].DefaultCellStyle.Format = "P";
            billingTable.Columns[7].DefaultCellStyle.Format = "C2";
            billingTable.Columns[8].DefaultCellStyle.Format = "C2";
            foreach (DataGridViewColumn column in billingTable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //billingTable.Rows.Add("1", "Daal", "KG", "1", "100.00", "5", "0", "100.00");
            // Sample data to populate the DataGridView
            //string[] row1 = { "1", "John Doe", "30" };
            //string[] row2 = { "2", "Jane Smith", "25" };
            //string[] row3 = { "3", "Alice Johnson", "35" };

            // Add rows to the DataGridView
            //dataGridView.Rows.Add(row1);
            //dataGridView.Rows.Add(row2);
            //dataGridView.Rows.Add(row3);
            //dataGridView.BringToFront();

            //dataGridView.Visible = true;
            billingTable.AllowUserToAddRows = false;
            billingTable.AutoGenerateColumns = false;
            billingTable.RowHeadersVisible = false;
            billingTable.AllowUserToResizeRows = false;
            billingTable.AllowUserToResizeColumns = false;
            billingTable.ReadOnly = true;
            billingTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            billingTable.MultiSelect = false;

            panel.Controls.Add(billingTable);
            //Console.WriteLine(panel.Location.X + " " + panel.Location.Y);
            //Console.WriteLine(panel.Size);

            // Create a DataGridView
            amtTable = new DataGridView
            {
                Dock = DockStyle.None,
                BackgroundColor = Color.Blue,
                Margin = new Padding(0),
                Location = new Point(100, 630),
                //Left = 100,
                //Top = 180,
                Size = new Size(1160, 25),
                Font = labelFont,
                //Width = 1160, //based on left & right panel width
            };

            amtTable.ColumnCount = 9;
            amtTable.Columns[0].Width = 80;
            amtTable.Columns[1].Width = 400;
            amtTable.Columns[2].Width = 80;
            amtTable.Columns[3].Width = 70;
            amtTable.Columns[4].Width = 80;
            amtTable.Columns[5].Width = 100;
            amtTable.Columns[6].Width = 100;
            amtTable.Columns[7].Width = 100;
            amtTable.Columns[8].Width = 150;

            amtTable.Columns[5].DefaultCellStyle.Format = "C2";
            amtTable.Columns[6].DefaultCellStyle.Format = "C2";
            amtTable.Columns[7].DefaultCellStyle.Format = "C2";
            amtTable.Columns[8].DefaultCellStyle.Format = "C2";

            amtTable.Rows[0].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Replace "Arial" with your desired font

            //amtTable.Rows.Add("", "Total (Rs.)", "", "", "", 0.00.ToString("C2"), 0.00.ToString("C2"), 0.00.ToString("C2"), 0.00.ToString("C2"));
            amtTable.ColumnHeadersVisible = false;
            amtTable.AllowUserToAddRows = false;
            amtTable.AutoGenerateColumns = false;
            amtTable.RowHeadersVisible = false;

            amtTable.AllowUserToResizeRows = false;
            amtTable.AllowUserToResizeColumns = false;
            amtTable.ReadOnly = true;
            amtTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            amtTable.MultiSelect = false;


            //amtTable.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10);
            //amtTable.ColumnHeadersDefaultCellStyle.Font = textfieldFont;



            panel.Controls.Add(amtTable);

            //PreparedBillingTableMouseMenu();
            TotalAmountTableChangedToDefault();

            return panel;
        }

        protected void TotalAmountTableChangedToDefault()
        {
            amtTable.Rows.Clear();
            amtTable.Rows.Add("", "Total (Rs.)", "", "", "", 0.00.ToString("C2"), 0.00.ToString("C2"), 0.00.ToString("C2"), 0.00.ToString("C2"));
        }

    }
}
