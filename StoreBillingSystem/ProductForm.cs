using System;
using System.Drawing;
using System.Windows.Forms;
using StoreBillingSystem.Database;
namespace StoreBillingSystem
{
    public class ProductForm : Form
    {
        public ProductForm()
        {
            InitComponents();

            FormClosed += Program.ProductForm_FormClosed;
            // Attach the FormClosed event handler.
            //FormClosed += ProductForm_FormClosing;
        }



        private Font labelFont = new Font("Arial", 11, FontStyle.Bold);
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
            bottomPanel.Height = 30;

            leftPanel = new Panel();
            leftPanel.BackColor = Color.LightYellow;
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 100;

            leftPanel.Controls.Add(LeftSidePanel());

            rightPanel = new Panel();
            rightPanel.BackColor = Color.LightCoral;
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 100;

            centerPanel = new Panel();
            centerPanel.BackColor = Color.White;
            centerPanel.Dock = DockStyle.Fill;
            centerPanel.Height = 610;

            centerPanel.Controls.Add(GetStockForm());

            this.Controls.Add(centerPanel);
            //this.Controls.Add(topPanel);

            this.Controls.Add(bottomPanel);
            this.Controls.Add(leftPanel);

            this.Controls.Add(rightPanel);

        }

        private Button categoryButton;
        private Button productTypeButton;
        private Button refreshButton;

        private Panel LeftSidePanel()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); //row-3 //blank
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-4
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-5

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //space

           

            categoryButton = new Button
            {
                Text = "Category",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(2)
            };

            GroupBox categoryBox = new GroupBox
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 8, FontStyle.Bold),
                ForeColor = Color.Blue,
                //BackColor = Color.YellowGreen,
            };

            categoryBox.Controls.Add(categoryButton);
            table.Controls.Add(categoryBox, 0, 1);

            productTypeButton = new Button
            {
                Text = "Type",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(2)
            };
            GroupBox productTypeBox = new GroupBox
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 8, FontStyle.Bold),
                ForeColor = Color.Blue,
                //BackColor = Color.YellowGreen,
            };

            productTypeBox.Controls.Add(productTypeButton);
            table.Controls.Add(productTypeBox, 0, 2);

            refreshButton = new Button
            {
                Text = "Refresh",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5),

            };
            table.Controls.Add(refreshButton, 0, 4);


            categoryButton.Click += CategoryButton_Click;
            productTypeButton.Click += ProductTypeButton_Click;
            return table;
        }

        private TextBox productIdText;
        private TextBox productNameText;
        private ComboBox categoryTypes;
        private ComboBox productTypes;
        private TextBox qtyText;
        private DateTimePicker purchaseDate;
        private TextBox purchasePriceText;
        private TextBox sellingPriceText_1;
        private TextBox sellingPriceText_2;
        private TextBox sellingPriceText_3;
        private TextBox sellingPriceText_4;

        private TextBox discountText_1;
        private TextBox discountText_2;
        private TextBox discountText_3;
        private TextBox discountText_4;
        private TextBox cgstText;
        private TextBox sgstText;
        private TextBox purchaseCGstPercentText;
        private TextBox purchaseSGstPercentText;
        //private TextBox purchaseGstPriceText;
        private TextBox batchNumberText;

        private DateTimePicker manufactureDate;
        private DateTimePicker expiryDate;
        private Button addProductButton;
        private Button clearProductButton;
        private Label separator_1;
        private Label separator_2;
        private Label separator_3;

        private GroupBox GetStockForm()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
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

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //
                //
                //Margin = new Padding(50),
                //BackColor = Color.YellowGreen,
                ColumnCount = 7,
                RowCount = 19,
                AutoScroll = true
            };


            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-4
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-5
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-6
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); //row-7 Horizontal Separator
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-8
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-9
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-10
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-11
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); //row-12 Horizontal Separator
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-13
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-14
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-15
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); //row-16 Horizontal Separato
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-17

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); //space

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F)); //qty
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F)); //extra cols
            //panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            //panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); //asking price
            //panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F)); //Button

            //Row-1
            //product Id
            table.Controls.Add(new Label
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
            table.Controls.Add(productIdText, 1, 1);

            //Purchase date
            table.Controls.Add(new Label
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
            table.Controls.Add(purchaseDate, 4, 1);

            //Row-2
            //Product Name
            table.Controls.Add(new Label
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
            table.Controls.Add(productNameText, 1, 2);

            //Row-3
            //Category Types
            table.Controls.Add(new Label
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
            table.Controls.Add(categoryTypes, 1, 3);

            //Row-4
            // Product Type
            table.Controls.Add(new Label
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
            table.Controls.Add(productTypes, 1, 4);

            //Product Qty
            table.Controls.Add(new Label
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
            table.Controls.Add(qtyText, 4, 4);

            //Row-5
            //Purchase Price
            table.Controls.Add(new Label
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
            table.Controls.Add(purchasePriceText, 1, 5);

            //Row-6
            //purchase CGst (%)
            table.Controls.Add(new Label
            {
                Text = "Purchase CGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 6);

            purchaseCGstPercentText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(purchaseCGstPercentText, 1, 6);

            //purchase SGst (%)
            table.Controls.Add(new Label
            {
                Text = "Purchase SGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 6);

            purchaseSGstPercentText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(purchaseSGstPercentText, 4, 6);

            //Row-7
            //Horizontal Separator
            separator_1 = new Label
            {
                AutoSize = false,
                Height = 2,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.Fixed3D,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(separator_1, 0, 7);

            //Row-8
            //Selling Price A
            table.Controls.Add(new Label
            {
                Text = "Selling Price - A (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 8);

            sellingPriceText_1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(sellingPriceText_1, 1, 8);

            //Discount Price A
            table.Controls.Add(new Label
            {
                Text = "Discount - A (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 8);

            discountText_1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(discountText_1, 4, 8);


            //Row-9
            //Selling Price B
            table.Controls.Add(new Label
            {
                Text = "Selling Price - B (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 9);

            sellingPriceText_2 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(sellingPriceText_2, 1, 9);

            //Discount Price B
            table.Controls.Add(new Label
            {
                Text = "Discount - B (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 9);

            discountText_2 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(discountText_2, 4, 9);

            //Row-10
            //Selling Price C
            table.Controls.Add(new Label
            {
                Text = "Selling Price - C (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 10);

            sellingPriceText_3 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(sellingPriceText_3, 1, 10);

            //Discount Price C
            table.Controls.Add(new Label
            {
                Text = "Discount - C (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 10);

            discountText_3 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(discountText_3, 4, 10);

            //Row-11
            //Selling Price D
            table.Controls.Add(new Label
            {
                Text = "Selling Price - D (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 11);

            sellingPriceText_4 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(sellingPriceText_4, 1, 11);

            //Discount Price D
            table.Controls.Add(new Label
            {
                Text = "Discount - D (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 11);

            discountText_4 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(discountText_4, 4, 11);

            //Row-12
            //Horizontal Separator
            separator_2 = new Label
            {
                AutoSize = false,
                Height = 2,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.Fixed3D,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(separator_2, 0, 12);

            //Row-13
            //CGST
            table.Controls.Add(new Label
            {
                Text = "CGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 13);

            cgstText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(cgstText, 1, 13);

            //SGST
            table.Controls.Add(new Label
            {
                Text = "SGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 13);

            sgstText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(sgstText, 4, 13);

            //Row-14
            //Manufacture date
            table.Controls.Add(new Label
            {
                Text = "Mfg. Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 14);

            manufactureDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(manufactureDate, 1, 14);

            //Expiry date
            table.Controls.Add(new Label
            {
                Text = "Exp. Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 14);

            expiryDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(expiryDate, 4, 14);

            //Row-15
            //Batch Number
            table.Controls.Add(new Label
            {
                Text = "Batch Number :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 15);

            batchNumberText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(batchNumberText, 1, 15);

            //Row-16
            //Horizontal Separator
            separator_3 = new Label
            {
                AutoSize = false,
                Height = 2,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.Fixed3D,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(separator_3, 0, 16);

            //Row-17
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
            table.Controls.Add(clearProductButton, 2, 17);

            addProductButton = new Button
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            table.Controls.Add(addProductButton, 3, 17);

            //Row-18 - Blank Row


            table.SetColumnSpan(productNameText, 3);
            table.SetColumnSpan(categoryTypes, 3);
            table.SetColumnSpan(separator_1, 6);
            table.SetColumnSpan(separator_2, 6);
            table.SetColumnSpan(separator_3, 6);

            //mainPanel.Controls.Add(table);


            box.Controls.Add(table);

            return box;
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            CategoryForm().ShowDialog();
        }

        private void ProductTypeButton_Click(object sender, EventArgs e)
        {
            ProductTypeForm().ShowDialog();
        }

        private Form CategoryForm()
        {
            Form form = new Form 
            {
                Text = "Add Category",
                HelpButton = true, // Display a help button on the form
                FormBorderStyle = FormBorderStyle.FixedDialog, // Define the border style of the form to a dialog box.
                MaximizeBox = false, // Set the MaximizeBox to false to remove the maximize box.
                MinimizeBox = false, // Set the MinimizeBox to false to remove the minimize box.
                StartPosition = FormStartPosition.CenterScreen, // Set the start position of the form to the center of the screen.
                Size = new Size(600, 600),
                BackColor = Color.Yellow,
            };

            TableLayoutPanel footerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                //BackColor = Color.Lime,
            };

            // Create two buttons to use as the accept and cancel buttons.
            /*
            Button okButton = new Button
            {
                Text = "Ok",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };*/
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button deleteButton = new Button
            {
                Text = "Delete",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button updateButton = new Button
            {
                Text = "Update",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            footerPanel.Controls.Add(cancelButton, 0, 0);
            footerPanel.Controls.Add(deleteButton, 1, 0);
            footerPanel.Controls.Add(updateButton, 2, 0);
            //footerPanel.Controls.Add(okButton, 1, 0);


            // Set the accept button of the form to button1.
            //form.AcceptButton = okButton;
            // Set the cancel button of the form to button2.
            form.CancelButton = cancelButton;

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 380F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-4


            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 450F)); //name

            table.Controls.Add(new Label
            {
                Text = "Category Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 0);

            TextBox categoryNameField = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(categoryNameField, 1, 0);

            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            Button addButton = new Button
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            table1.Controls.Add(clearButton, 0, 0);
            table1.Controls.Add(addButton, 1, 0);

            table.Controls.Add(table1, 1, 1);


            DataGridView categoryTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                //Size = new Size(1160, 454),
            };
            categoryTable.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            categoryTable.ColumnCount = 2;
            categoryTable.Columns[0].Name = "Sr.No.";
            categoryTable.Columns[1].Name = "Category Name";

            categoryTable.Columns[0].Width = 140;
            categoryTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn column in categoryTable.Columns)
            {
                //column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //categoryTable.Rows.Add("1", "Daal", "KG", "1", "100.00", "5", "0", "100.00");

            categoryTable.AllowUserToAddRows = false;
            categoryTable.AutoGenerateColumns = false;
            categoryTable.RowHeadersVisible = false;

            table.Controls.Add(categoryTable, 0, 3);

            table.Controls.Add(footerPanel, 1, 4);


            table.SetColumnSpan(categoryTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            form.Controls.Add(table);
            return form;
        }

        private Form ProductTypeForm()
        {
            Form form = new Form
            {
                Text = "Add Product Type",
                HelpButton = true, // Display a help button on the form
                FormBorderStyle = FormBorderStyle.FixedDialog, // Define the border style of the form to a dialog box.
                MaximizeBox = false, // Set the MaximizeBox to false to remove the maximize box.
                MinimizeBox = false, // Set the MinimizeBox to false to remove the minimize box.
                StartPosition = FormStartPosition.CenterScreen, // Set the start position of the form to the center of the screen.
                Size = new Size(600, 600),
                BackColor = Color.Yellow,
            };

            TableLayoutPanel footerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                //BackColor = Color.Lime,
            };

            // Create two buttons to use as the accept and cancel buttons.
            /*
            Button okButton = new Button
            {
                Text = "Ok",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };*/
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button deleteButton = new Button
            {
                Text = "Delete",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button updateButton = new Button
            {
                Text = "Update",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            footerPanel.Controls.Add(cancelButton, 0, 0);
            footerPanel.Controls.Add(deleteButton, 1, 0);
            footerPanel.Controls.Add(updateButton, 2, 0);
            //footerPanel.Controls.Add(okButton, 1, 0);


            // Set the accept button of the form to button1.
            //form.AcceptButton = okButton;
            // Set the cancel button of the form to button2.
            form.CancelButton = cancelButton;

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 380F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-4


            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 450F)); //name

            table.Controls.Add(new Label
            {
                Text = "Type Full Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 0);

            TextBox typeFullNameField = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(typeFullNameField, 1, 0);

            table.Controls.Add(new Label
            {
                Text = "Type Abbr. Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 1);

            TextBox typeAbbrNameField = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(typeAbbrNameField, 1, 1);

            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            Button addButton = new Button
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };

            table1.Controls.Add(clearButton, 0, 0);
            table1.Controls.Add(addButton, 1, 0);

            table.Controls.Add(table1, 1, 2);


            DataGridView categoryTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                //Size = new Size(1160, 454),
            };
            categoryTable.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            categoryTable.ColumnCount = 3;
            categoryTable.Columns[0].Name = "Sr.No.";
            categoryTable.Columns[1].Name = "Type Abbrivation";
            categoryTable.Columns[2].Name = "Type Name";

            categoryTable.Columns[0].Width = 100;
            categoryTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            categoryTable.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (DataGridViewColumn column in categoryTable.Columns)
            {
                //column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //categoryTable.Rows.Add("1", "Daal", "KG", "1", "100.00", "5", "0", "100.00");

            categoryTable.AllowUserToAddRows = false;
            categoryTable.AutoGenerateColumns = false;
            categoryTable.RowHeadersVisible = false;

            table.Controls.Add(categoryTable, 0, 3);

            table.Controls.Add(footerPanel, 1, 4);


            table.SetColumnSpan(categoryTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            form.Controls.Add(table);
            return form;
        }
    }
}
