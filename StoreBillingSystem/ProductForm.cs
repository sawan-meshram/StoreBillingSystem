using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using StoreBillingSystem.Entity;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Database;
using StoreBillingSystem.Events;

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
            InitComponentsData();
        }

        private void InitComponentsData()
        {
            categoryDao = new CategoryDaoImpl(DatabaseManager.GetConnection());
            categoryList = categoryDao.ReadAll();

            productTypeDao = new ProductTypeDaoImpl(DatabaseManager.GetConnection());
            productTypeList = productTypeDao.ReadAll();

            categoryTypesComboBox.DataSource = categoryList;
            categoryTypesComboBox.DisplayMember = "Name";
            categoryTypesComboBox.ValueMember = "Id";


            productTypesComboBox.DataSource = productTypeList;
            productTypesComboBox.DisplayMember = "Abbr";
            productTypesComboBox.ValueMember = "Id";

            //to retrive the selected Category
            /*
            Category category = (Category)categoryTypesComboBox.SelectedItem;
            string name = category.Name;
            int id = category.Id;
            */           
        }

        private ICategoryDao categoryDao;
        private IProductTypeDao productTypeDao;

        private IList<Category> categoryList;
        private IList<ProductType> productTypeList;


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
        private ComboBox categoryTypesComboBox;
        private ComboBox productTypesComboBox;
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

        private Label separator_1;
        private Label separator_2;
        private Label separator_3;

        private string productPlaceHolder = Util.U.ToTitleCase("Enter product here...");
        private string pricePlaceHolder = "0.00";
        private string batchNumberPlaceHolder = Util.U.ToTitleCase("Enter batch number");


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
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                //
                //
                //Margin = new Padding(50),
                //BackColor = Color.YellowGreen,
                ColumnCount = 9, //7
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
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F)); //asterisks
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F)); //space

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F)); //asterisks
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
            table.Controls.Add(productIdText, 2, 1);

            //Purchase date
            table.Controls.Add(new Label
            {
                Text = "Purchase Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 1);

            purchaseDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(purchaseDate, 6, 1);

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

            //mandatory field
            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.BottomCenter,
            }, 1, 2);

            productNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = productPlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(productNameText, 2, 2);

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

            //mandatory field
            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.BottomCenter,
            }, 1, 3);

            categoryTypesComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(categoryTypesComboBox, 2, 3);

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

            //mandatory field
            table.Controls.Add(new Label
            {
                Text = "*",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.BottomCenter,
            }, 1, 4);

            productTypesComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            table.Controls.Add(productTypesComboBox, 2, 4);

            //Product Qty
            table.Controls.Add(new Label
            {
                Text = "Qty. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 4);

            qtyText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = "0"
            };
            table.Controls.Add(qtyText, 6, 4);

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
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(purchasePriceText, 2, 5);

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
                Margin = new Padding(5),
                Text = "0"
            };
            table.Controls.Add(purchaseCGstPercentText, 2, 6);

            //purchase SGst (%)
            table.Controls.Add(new Label
            {
                Text = "Purchase SGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 6);

            purchaseSGstPercentText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = "0"
            };
            table.Controls.Add(purchaseSGstPercentText, 6, 6);

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
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(sellingPriceText_1, 2, 8);

            //Discount Price A
            table.Controls.Add(new Label
            {
                Text = "Discount - A (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 8);

            discountText_1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(discountText_1, 6, 8);


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
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(sellingPriceText_2, 2, 9);

            //Discount Price B
            table.Controls.Add(new Label
            {
                Text = "Discount - B (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 9);

            discountText_2 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(discountText_2, 6, 9);

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
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(sellingPriceText_3, 2, 10);

            //Discount Price C
            table.Controls.Add(new Label
            {
                Text = "Discount - C (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 10);

            discountText_3 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(discountText_3, 6, 10);

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
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(sellingPriceText_4, 2, 11);

            //Discount Price D
            table.Controls.Add(new Label
            {
                Text = "Discount - D (Rs.):",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 11);

            discountText_4 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = "0.00"
            };
            table.Controls.Add(discountText_4, 6, 11);

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
                Margin = new Padding(5),
                Text = "0"
            };
            table.Controls.Add(cgstText, 2, 13);

            //SGST
            table.Controls.Add(new Label
            {
                Text = "SGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 13);

            sgstText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = "0"
            };
            table.Controls.Add(sgstText, 6, 13);

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
            table.Controls.Add(manufactureDate, 2, 14);

            //Expiry date
            table.Controls.Add(new Label
            {
                Text = "Exp. Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 14);

            expiryDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
            };
            table.Controls.Add(expiryDate, 6, 14);

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
                ForeColor = Color.Gray,
                Margin = new Padding(5),
                Text = batchNumberPlaceHolder
            };

            table.Controls.Add(batchNumberText, 2, 15);

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
            Button clearProductButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            table.Controls.Add(clearProductButton, 3, 17);

            Button addProductButton = new Button
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            table.Controls.Add(addProductButton, 4, 17);

            //Row-18 - Blank Row


            table.SetColumnSpan(productNameText, 2);
            table.SetColumnSpan(categoryTypesComboBox, 2);
            table.SetColumnSpan(separator_1, 8);
            table.SetColumnSpan(separator_2, 8);
            table.SetColumnSpan(separator_3, 8);

            //mainPanel.Controls.Add(table);


            box.Controls.Add(table);


            //Numeric text on text box
            qtyText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            purchasePriceText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            sellingPriceText_1.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            sellingPriceText_2.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            sellingPriceText_3.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            sellingPriceText_4.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            discountText_1.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            discountText_2.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            discountText_3.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            discountText_4.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            cgstText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            sgstText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            purchaseCGstPercentText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;
            purchaseSGstPercentText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;


            //Button event
            addProductButton.Click += AddProductButton_Click;


            //Capitalise text on text box
            productNameText.TextChanged += ProductNameText_TextChanged;
            batchNumberText.TextChanged += BatchNumberText_TextChanged;

            // placeholder on text box
            productNameText.Enter += ProductNameText_GotFocus;
            productNameText.Leave += ProductNameText_LostFocus;

            batchNumberText.Enter += BatchNumberText_GotFocus;
            batchNumberText.Leave += BatchNumberText_LostFocus;
            return box;
        }

        private void ProductNameText_GotFocus(object sender, EventArgs e)
        {
            if (productNameText.Text.ToLower() == productPlaceHolder.ToLower())
            {
                productNameText.Text = "";
                productNameText.ForeColor = Color.Black;
            }
        }

        private void ProductNameText_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(productNameText.Text))
            {
                productNameText.Text = productPlaceHolder;
                productNameText.ForeColor = Color.Gray;
            }
        }

        private void BatchNumberText_GotFocus(object sender, EventArgs e)
        {
            // Clear the placeholder text when the TextBox gets focus
            if (batchNumberText.Text.ToLower() == batchNumberPlaceHolder.ToLower())
            {
                batchNumberText.Text = string.Empty;
                batchNumberText.ForeColor = Color.Black; // Set text color to default
            }
        }

        private void BatchNumberText_LostFocus(object sender, EventArgs e)
        {
            // Set the placeholder text when the TextBox loses focus and is empty
            if (string.IsNullOrWhiteSpace(batchNumberText.Text))
            {
                batchNumberText.Text = batchNumberPlaceHolder;
                batchNumberText.ForeColor = Color.Gray; // Set text color to placeholder color
            }
        }

        private void AddProductButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Inside form");

            string productId = productIdText.Text.Trim();
            string productName = productNameText.Text.Trim();
            Console.WriteLine("productName :" + productName);
            if (string.IsNullOrEmpty(productName))
            {
            }

            var category = (Category)categoryTypesComboBox.SelectedItem;
            var productType = (ProductType)productTypesComboBox.SelectedItem;

            var purchaseDateTime = purchaseDate.Value;
            purchasePriceText.Text.Trim();

            var mfgDate = manufactureDate.Value;
            var expDate = expiryDate.Value;

            qtyText.Text.Trim();
            purchasePriceText.Text.Trim();
            sellingPriceText_1.Text.Trim();
            sellingPriceText_2.Text.Trim();
            sellingPriceText_3.Text.Trim();
            sellingPriceText_4.Text.Trim();

            discountText_1.Text.Trim();
            discountText_2.Text.Trim();
            discountText_3.Text.Trim();
            discountText_4.Text.Trim();
            cgstText.Text.Trim();
            sgstText.Text.Trim();
            purchaseCGstPercentText.Text.Trim();
            purchaseSGstPercentText.Text.Trim();
            batchNumberText.Text.Trim();

        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            CategoryForm().ShowDialog();
        }

        private void ProductTypeButton_Click(object sender, EventArgs e)
        {
            ProductTypeForm().ShowDialog();
        }

        private DataGridView categoryTable;
        private TextBox categoryNameField;
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

            categoryNameField = new TextBox
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


            categoryTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                //Size = new Size(1160, 454),
            };
            categoryTable.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            categoryTable.ColumnCount = 2;
            categoryTable.Columns[0].Name = "Category Id";
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

            //Binf list with datagridview
            categoryTable.Columns[0].DataPropertyName = "Id";
            categoryTable.Columns[1].DataPropertyName = "Name";
            categoryTable.DataSource = categoryList;

            table.Controls.Add(categoryTable, 0, 3);

            table.Controls.Add(footerPanel, 1, 4);


            table.SetColumnSpan(categoryTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            form.Controls.Add(table);

            //add event
            addButton.Click += AddCategoryButton_Click;

            // Attach the CellFormatting event handler
            //categoryTable.CellFormatting += CategoryTable_CellFormatting;

            categoryNameField.TextChanged += CategoryNameField_TextChanged;
            return form;
        }

        private DataGridView productTypeTable;
        private TextBox typeFullNameField;
        private TextBox typeAbbrNameField;
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
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
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

            typeFullNameField = new TextBox
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

            typeAbbrNameField = new TextBox
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


            productTypeTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                //Size = new Size(1160, 454),
            };
            productTypeTable.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            productTypeTable.ColumnCount = 3;
            productTypeTable.Columns[0].Name = "Type Id";
            productTypeTable.Columns[1].Name = "Type Abbrivation";
            productTypeTable.Columns[2].Name = "Type Name";

            productTypeTable.Columns[0].Width = 100;
            productTypeTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            productTypeTable.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            foreach (DataGridViewColumn column in productTypeTable.Columns)
            {
                //column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //categoryTable.Rows.Add("1", "Daal", "KG", "1", "100.00", "5", "0", "100.00");

            productTypeTable.AllowUserToAddRows = false;
            productTypeTable.AutoGenerateColumns = false;
            productTypeTable.RowHeadersVisible = false;

            table.Controls.Add(productTypeTable, 0, 3);

            table.Controls.Add(footerPanel, 1, 4);


            table.SetColumnSpan(productTypeTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            form.Controls.Add(table);

            // Attach event handlers
            addButton.Click += AddProductTypeButton_Click;

            // Attach the CellFormatting event handler
            //productTypeTable.CellFormatting += ProductTypeTable_CellFormatting;

            typeFullNameField.TextChanged += TypeFullNameField_TextChanged;
            typeAbbrNameField.TextChanged += TypeAbbrNameField_TextChanged;
            typeAbbrNameField.KeyPress += TextBoxKeyEvent.UppercaseTextBox_KeyPress;
            return form;
        }


        //private int categorySrNo = 1;
        private int productTypeSrNo = 1;
        /*
        private void UpdateCategorySrNoColumn()
        {
            Console.WriteLine("Inside function :" + categoryTable.Rows.Count);
            for (int i = 0; i < categoryList.Count; i++) 
            {
                categoryTable.Rows[i].Cells[0].Value = i + 1;
            }
        }
        */

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(categoryNameField.Text))
            {
                // Validation failed, display a message box
                MessageBox.Show("All fields required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = categoryNameField.Text.Trim();
            Category category = new Category(name);
            if (!categoryDao.IsRecordExists(name))
            {
                bool inserted = categoryDao.Insert(category);

                if (!inserted)
                {
                    MessageBox.Show("Something occur while insertion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                categoryList.Add(category);
                //categoryTable.Rows.Add(category.Id, category.Name);
                categoryTable.DataSource = null;
                categoryTable.DataSource = categoryList;

                //categorySrNo++;
                categoryNameField.Clear();
                MessageBox.Show("Insert successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Already exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void AddProductTypeButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(typeFullNameField.Text) || string.IsNullOrWhiteSpace(typeAbbrNameField.Text))
            {
                // Validation failed, display a message box
                MessageBox.Show("All fields required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            string name = typeFullNameField.Text.Trim();
            string abbr = typeAbbrNameField.Text.Trim();

            ProductType productType = new ProductType(name, abbr);

            if(!productTypeDao.IsRecordExists(name, abbr))
            {
                bool inserted = productTypeDao.Insert(productType);
                if (!inserted)
                {
                    MessageBox.Show("Something occur while insertion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                productTypeList.Add(productType);
                //productTypeTable.Rows.Add(productType.Id, productType.Abbr, productType.Name);
                productTypeTable.DataSource = null;
                productTypeTable.DataSource = productTypeList;

                //categorySrNo++;
                typeFullNameField.Clear();
                typeAbbrNameField.Clear();
                MessageBox.Show("Insert successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Already exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void CategoryTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell is in the first column (index 0) and not a header cell
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // Set the value to the current row index plus the initial serial number
                e.Value = e.RowIndex+1;
                e.FormattingApplied = true;
            }
        }

        private void ProductTypeTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell is in the first column (index 0) and not a header cell
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // Set the value to the current row index plus the initial serial number
                e.Value = e.RowIndex + productTypeSrNo;
                e.FormattingApplied = true;
            }
        }


        private void ProductNameText_TextChanged(object sender, EventArgs e)
        {
            productNameText.Text = CapitalizeEachWord(productNameText.Text);
            productNameText.SelectionStart = productNameText.Text.Length; // Set the cursor at the end
        }

        private void TypeAbbrNameField_TextChanged(object sender, EventArgs e)
        {
            typeAbbrNameField.Text = typeAbbrNameField.Text.ToUpper();
            typeAbbrNameField.SelectionStart = typeAbbrNameField.Text.Length; // Set the cursor at the end
        }


        private void BatchNumberText_TextChanged(object sender, EventArgs e)
        {
            batchNumberText.Text = batchNumberText.Text.ToUpper();
            batchNumberText.SelectionStart = batchNumberText.Text.Length; // Set the cursor at the end
        }

      

        private void TypeFullNameField_TextChanged(object sender, EventArgs e)
        {
            typeFullNameField.Text = CapitalizeEachWord(typeFullNameField.Text);
            typeFullNameField.SelectionStart = typeFullNameField.Text.Length; // Set the cursor at the end
        }

        private void CategoryNameField_TextChanged(object sender, EventArgs e)
        {
            categoryNameField.Text = CapitalizeEachWord(categoryNameField.Text);
            categoryNameField.SelectionStart = categoryNameField.Text.Length; // Set the cursor at the end
        }

        private string CapitalizeEachWord(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return Util.U.ToTitleCase(input);
            }

            return input;
        }

    }
}
