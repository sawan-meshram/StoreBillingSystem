using System;
using System.Drawing;
using System.Windows.Forms;
namespace StoreBillingSystem
{
    public class ProductForm : Form
    {
        public ProductForm()
        {
            InitComponents();

        }

        private Font labelFont = new Font("Arial", 12, FontStyle.Bold);
        private Font textfieldFont = new Font("Arial", 12);

        private Panel topPanel;

        private Panel bottomPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel centerPanel;

        private void InitComponents()
        {
            this.Text = "Product Filling";
            this.Size = new Size(1366, 768);



            // Create panels for each region
            topPanel = new Panel();
            topPanel.BackColor = Color.LightBlue;
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 80;


            //topPanel.Controls.Add(GetHeaderForm());

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
            centerPanel.Dock = DockStyle.Top;
            centerPanel.Height = 610;

            centerPanel.Controls.Add(GetStockForm());

            this.Controls.Add(centerPanel);
            this.Controls.Add(topPanel);

            this.Controls.Add(bottomPanel);
            this.Controls.Add(leftPanel);

            this.Controls.Add(rightPanel);

        }

        private TextBox productIdText;
        private TextBox productNameText;
        private ComboBox categoryTypes;
        private ComboBox productTypes;
        private TextBox qtyText;
        private DateTimePicker purchaseDate;
        private TextBox purchasePriceText;
        private TextBox sellingPriceText;
        private TextBox discountText;
        private TextBox gstText;
        private TextBox purchaseGstPercentText;
        private TextBox purchaseGstPriceText;
        private DateTimePicker manufactureDate;
        private DateTimePicker expiryDate;
        private Button addProductButton;
        private Button clearProductButton;


        private GroupBox GetStockForm()
        {
            GroupBox box = new GroupBox
            {
                Text = "Product Filling",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Blue,
                BackColor = Color.YellowGreen,


                //Size = new Size(1100, 500),
                //Left = 100,
                //Top = 200,
                //Location = new Point(100,80)
                //Margin = new Padding(0, 50, 0, 0),
            };
            //Console.WriteLine(box.Location.X + " " + box.Location.Y);

            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //
                //
                //Margin = new Padding(50),
                //BackColor = Color.YellowGreen,
                ColumnCount = 6,
                RowCount = 12,
            };


            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));


            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); //name
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); //space

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F)); //qty
            //panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            //panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); //asking price
            //panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //Button

            //Row-1
            //product Id
            panel.Controls.Add(new Label
            {
                Text = "Product Id :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 1);

            productIdText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(productIdText, 1, 1);

            //Purchase date
            panel.Controls.Add(new Label
            {
                Text = "Purchase Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 1);

            purchaseDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            panel.Controls.Add(purchaseDate, 4, 1);

            //Row-2
            //Product Name
            panel.Controls.Add(new Label
            {
                Text = "Product Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 2);

            productNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(productNameText, 1, 2);
            panel.SetColumnSpan(productNameText, 3);

            //Row-3
            //Category Types
            panel.Controls.Add(new Label
            {
                Text = "Category :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 3);

            categoryTypes = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(categoryTypes, 1, 3);
            panel.SetColumnSpan(categoryTypes, 3);

            //Row-4
            // Product Type
            panel.Controls.Add(new Label
            {
                Text = "Type :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 4);

            productTypes = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(productTypes, 1, 4);

            //Product Qty
            panel.Controls.Add(new Label
            {
                Text = "Qty. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 4);

            qtyText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(qtyText, 4, 4);

            //Row-5
            //Purchase Price
            panel.Controls.Add(new Label
            {
                Text = "Purchase Price :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 5);

            purchasePriceText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(purchasePriceText, 1, 5);

            //Selling Price
            panel.Controls.Add(new Label
            {
                Text = "Selling Price :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 5);

            sellingPriceText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(sellingPriceText, 4, 5);

            //Row-6
            //purchase Gst (%)
            panel.Controls.Add(new Label
            {
                Text = "Purchase GST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 6);

            purchaseGstPercentText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(purchaseGstPercentText, 1, 6);

            //Purchase GST Amount
            panel.Controls.Add(new Label
            {
                Text = "GST (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 6);

            purchaseGstPriceText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(purchaseGstPriceText, 4, 6);


            //Row-7
            //GST
            panel.Controls.Add(new Label
            {
                Text = "GST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 7);

            gstText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(gstText, 1, 7);


            //Discount
            panel.Controls.Add(new Label
            {
                Text = "Discount (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 7);

            discountText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(discountText, 4, 7);

            //Row-8
            //Manufacture date
            panel.Controls.Add(new Label
            {
                Text = "Mfg. Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 8);

            manufactureDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            panel.Controls.Add(manufactureDate, 1, 8);

            //Expiry date
            panel.Controls.Add(new Label
            {
                Text = "Exp. Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 8);

            expiryDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            panel.Controls.Add(expiryDate, 4, 8);

            //Row-9


            //Row-10
            //Clear Button
            clearProductButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(clearProductButton, 2, 10);

            addProductButton = new Button
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(addProductButton, 3, 10);
            box.Controls.Add(panel);

            return box;
        }
    }
}
