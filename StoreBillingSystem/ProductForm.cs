﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Mono.Data.Sqlite;

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

        private ICategoryDao categoryDao;
        private IProductTypeDao productTypeDao;
        private IProductDao productDao;
        private IProductSellingDao productSellingDao;
        private IProductPurchaseDao productPurchaseDao;

        private IList<Category> categoryList;
        private IList<ProductType> productTypeList;

        private void InitComponentsData()
        {
            SqliteConnection connection = DatabaseManager.GetConnection();

            productDao = new ProductDaoImpl(connection);

            //Set New product Id
            productIdText.Text = productDao.GetNewProductId().ToString();

            productSellingDao = new ProductSellingDaoImpl(connection);
            productPurchaseDao = new ProductPurchaseDaoImpl(connection);

            categoryDao = new CategoryDaoImpl(connection);
            categoryList = categoryDao.ReadAll();

            productTypeDao = new ProductTypeDaoImpl(connection);
            productTypeList = productTypeDao.ReadAll();

            BindCategoryTypeToComboBox();
            BindProductTypeToComboBox();

            //to retrive the selected Category
            /*
            Category category = (Category)categoryTypesComboBox.SelectedItem;
            string name = category.Name;
            int id = category.Id;
            */
        }

        private Font labelFont = new Font("Arial", 11, FontStyle.Bold);
        private Font textfieldFont = new Font("Arial", 11);

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

        private CheckBox allowMfgExpBatchCheckBox;
        private Label separator_1;
        private Label separator_2;
        private Label separator_3;

        private string productPlaceHolder = Util.U.ToTitleCase("Enter product here...");
        private string pricePlaceHolder = "0.00";
        private string qtyPercentPlaceHolder = "0.0";
        private string batchNumberPlaceHolder = Util.U.ToTitleCase("Enter batch number here...");


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
                ColumnCount = 9, //7
                RowCount = 20,
                AutoScroll = true
            };


            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-1 //Id, purchase date
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-2 // name
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-3 //category
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-4 //type
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-5 Horizontal Separator
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-6 //qty, purchase price
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-7 //purchase gst
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-8
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-9
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-10
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-11
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-12 Horizontal Separator
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-13 //GST
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-14 //Allow Mfg & Exp
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-14 //Mfg & Exp
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-15 //batch num
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-16 Horizontal Separato
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-17 //add & clear button

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F)); //name
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
                Margin = new Padding(5),
                ReadOnly = true
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
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
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
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(productTypesComboBox, 2, 4);

            //Row-5
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
            table.Controls.Add(separator_1, 0, 5);

            //Row-6
            //Product Qty
            table.Controls.Add(new Label
            {
                Text = "Qty. :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 6);

            qtyText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = qtyPercentPlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(qtyText, 2, 6);

            //Purchase Price
            table.Controls.Add(new Label
            {
                Text = "Purchase Price :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 6);

            purchasePriceText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(purchasePriceText, 6, 6);

            //Row-7
            //purchase CGst (%)
            table.Controls.Add(new Label
            {
                Text = "Purchase CGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 7);

            purchaseCGstPercentText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = qtyPercentPlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(purchaseCGstPercentText, 2, 7);

            //purchase SGst (%)
            table.Controls.Add(new Label
            {
                Text = "Purchase SGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 7);

            purchaseSGstPercentText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = qtyPercentPlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(purchaseSGstPercentText, 6, 7);

           
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = pricePlaceHolder,
                ForeColor = Color.Gray
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
                Text = qtyPercentPlaceHolder,
                ForeColor = Color.Gray
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
                Text = qtyPercentPlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(sgstText, 6, 13);

            //Row-14
            table.Controls.Add(new Label
            {
                Text = "Allow Mfg_Exp_Batch :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 14);

            allowMfgExpBatchCheckBox = new CheckBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
            };
            table.Controls.Add(allowMfgExpBatchCheckBox, 2, 14);

            //Row-15
            //Manufacture date
            table.Controls.Add(new Label
            {
                Text = "Mfg. Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 15);

            manufactureDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
                Enabled = false
            };
            table.Controls.Add(manufactureDate, 2, 15);

            //Expiry date
            table.Controls.Add(new Label
            {
                Text = "Exp. Date :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 4, 15);

            expiryDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
                Enabled = false,
            };
            table.Controls.Add(expiryDate, 6, 15);

            //Row-16
            //Batch Number
            table.Controls.Add(new Label
            {
                Text = "Batch Number :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 16);

            batchNumberText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                ForeColor = Color.Gray,
                Margin = new Padding(5),
                Text = batchNumberPlaceHolder,
                //ReadOnly = true,
                Enabled = false
            };

            table.Controls.Add(batchNumberText, 2, 16);

            //Row-17
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
            table.Controls.Add(separator_3, 0, 17);

            //Row-18
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
            table.Controls.Add(clearProductButton, 3, 18);

            Button addProductButton = new Button
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            table.Controls.Add(addProductButton, 4, 18);

            //Row-18 - Blank Row


            table.SetColumnSpan(productNameText, 2);
            table.SetColumnSpan(categoryTypesComboBox, 2);
            table.SetColumnSpan(separator_1, 8);
            table.SetColumnSpan(separator_2, 8);
            table.SetColumnSpan(separator_3, 8);

            //mainPanel.Controls.Add(table);


            box.Controls.Add(table);


            InitAddProductFormEvent();

            //Button event
            clearProductButton.Click += (sender, e) => ClearProductForm();
            addProductButton.Click += (sender, e) => AddProduct();

            return box;
        }

        private void InitAddProductFormEvent()
        {
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



            //Capitalise text on text box
            productNameText.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(productNameText);
            batchNumberText.KeyPress += TextBoxKeyEvent.UppercaseTextBox_KeyPress;

            // placeholder on text box
            productNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(productNameText, productPlaceHolder);
            productNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(productNameText, productPlaceHolder);

            batchNumberText.Enter += (sender, e) =>
            {
                TextBoxKeyEvent.PlaceHolderText_GotFocus(batchNumberText, batchNumberPlaceHolder);
                TextBoxKeyEvent.ReadOnlyTextBox_GotFocus(batchNumberText, Color.LightGray);
            };

            batchNumberText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(batchNumberText, batchNumberPlaceHolder);

            purchasePriceText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(purchasePriceText, pricePlaceHolder);
            purchasePriceText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(purchasePriceText, pricePlaceHolder);

            sellingPriceText_1.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_1, pricePlaceHolder);
            sellingPriceText_1.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_1, pricePlaceHolder);
            sellingPriceText_2.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_2, pricePlaceHolder);
            sellingPriceText_2.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_2, pricePlaceHolder);
            sellingPriceText_3.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_3, pricePlaceHolder);
            sellingPriceText_3.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_3, pricePlaceHolder);
            sellingPriceText_4.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_4, pricePlaceHolder);
            sellingPriceText_4.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_4, pricePlaceHolder);

            discountText_1.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_1, pricePlaceHolder);
            discountText_1.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_1, pricePlaceHolder);
            discountText_2.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_2, pricePlaceHolder);
            discountText_2.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_2, pricePlaceHolder);
            discountText_3.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_3, pricePlaceHolder);
            discountText_3.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_3, pricePlaceHolder);
            discountText_4.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_4, pricePlaceHolder);
            discountText_4.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_4, pricePlaceHolder);

            qtyText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(qtyText, qtyPercentPlaceHolder);
            qtyText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(qtyText, qtyPercentPlaceHolder);
            cgstText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(cgstText, qtyPercentPlaceHolder);
            cgstText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(cgstText, qtyPercentPlaceHolder);
            sgstText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sgstText, qtyPercentPlaceHolder);
            sgstText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sgstText, qtyPercentPlaceHolder);
            purchaseCGstPercentText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(purchaseCGstPercentText, qtyPercentPlaceHolder);
            purchaseCGstPercentText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(purchaseCGstPercentText, qtyPercentPlaceHolder);
            purchaseSGstPercentText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(purchaseSGstPercentText, qtyPercentPlaceHolder);
            purchaseSGstPercentText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(purchaseSGstPercentText, qtyPercentPlaceHolder);

            //Enable & Disable field
            allowMfgExpBatchCheckBox.CheckedChanged += AllowMfgExpBatchCheckBox_CheckedChanged;

            productIdText.Enter += (sender, e) => TextBoxKeyEvent.ReadOnlyTextBox_GotFocus(productIdText, Color.LightGray);
        }


        private void AllowMfgExpBatchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            manufactureDate.Enabled = allowMfgExpBatchCheckBox.Checked;
            expiryDate.Enabled = allowMfgExpBatchCheckBox.Checked;
            batchNumberText.Enabled = allowMfgExpBatchCheckBox.Checked;
        }


        private void AddProduct()
        {
            Console.WriteLine("Inside form");

            string productId = productIdText.Text.Trim();
            string productName = productNameText.Text.Trim();

            Console.WriteLine("productName :" + productName);
            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Product Name can be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(productDao.IsRecordExists(productName))
            {
                MessageBox.Show("Given product name is duplicate.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string batchNumber = batchNumberText.Text.Trim();
            DateTime mfgDate = manufactureDate.Value;
            DateTime expDate = expiryDate.Value;

            if (allowMfgExpBatchCheckBox.Checked)
            {
                if (string.IsNullOrWhiteSpace(batchNumber))
                {
                    MessageBox.Show("Batch Number can be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (mfgDate.CompareTo(expDate) != -1)
                {
                    MessageBox.Show("Manufacture date must be less than to expiry date.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            float qty = float.Parse(qtyText.Text.Trim());
            double purchasePrice = double.Parse(purchasePriceText.Text.Trim());

            if(qty == 0 && purchasePrice > 0)
            {
                MessageBox.Show("If no qty provided, then provide either qty or set purchase price to '0' or empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (qty > 0 && purchasePrice == 0)
            {
                MessageBox.Show("If no purchase price provided, then provide either purchase price or set qty to '0' or empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            double sellPriceA = double.Parse(sellingPriceText_1.Text.Trim());
            double sellPriceB = double.Parse(sellingPriceText_2.Text.Trim());
            double sellPriceC = double.Parse(sellingPriceText_3.Text.Trim());
            double sellPriceD = double.Parse(sellingPriceText_4.Text.Trim());

            double discountA = double.Parse(discountText_1.Text.Trim());
            double discountB = double.Parse(discountText_2.Text.Trim());
            double discountC = double.Parse(discountText_3.Text.Trim());
            double discountD = double.Parse(discountText_4.Text.Trim());

            if ((qty == 0 && purchasePrice == 0) && 
                (sellPriceA > 0 || sellPriceB > 0 || sellPriceC > 0 || sellPriceD > 0 || discountA > 0 || discountB > 0 || discountC > 0 || discountD > 0))
            {
                MessageBox.Show("If no qty & purchase price provided, then can't set selling & discount prices.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            float sellingCGST = float.Parse(cgstText.Text.Trim());
            float sellingSGST = float.Parse(sgstText.Text.Trim());

            if (sellPriceA != 0) {
                if(discountA != 0 && discountA >= sellPriceA) {
                    MessageBox.Show("Discount price 'A' can't be greater than or equal to selling price 'A'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if (sellPriceA < purchasePrice){
                    MessageBox.Show("Selling price 'A' can't be less than purchase price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if((sellPriceA - discountA) - purchasePrice < 0) {
                    MessageBox.Show("Verify selling & discount price 'A' to avoid losses compared to the purchase price. [Note=> Loss = (Sell Price - Discount Price) - Purchase Price]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }   
            }
            if (discountB != 0) {
                if (sellPriceB != 0 && discountB >= sellPriceB) {
                    MessageBox.Show("Discount price 'B' can't be greater than or equal to selling price 'B'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if (sellPriceB < purchasePrice) {
                    MessageBox.Show("Selling price 'B' can't be less than purchase price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if ((sellPriceB - discountB) - purchasePrice < 0) {
                    MessageBox.Show("Verify selling & discount price 'B' to avoid losses compared to the purchase price. [Note=> Loss = (Sell Price - Discount Price) - Purchase Price]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (discountC != 0) {
                if (sellPriceC != 0 && discountC >= sellPriceC) {
                    MessageBox.Show("Discount price 'C' can't be greater than or equal to selling price 'C'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if (sellPriceC < purchasePrice) {
                    MessageBox.Show("Selling price 'C' can't be less than purchase price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if ((sellPriceC - discountC) - purchasePrice < 0) {
                    MessageBox.Show("Verify selling & discount price 'C' to avoid losses compared to the purchase price. [Note=> Loss = (Sell Price - Discount Price) - Purchase Price]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (discountD != 0) {
                if (sellPriceD != 0 && discountD >= sellPriceD) {
                    MessageBox.Show("Discount price 'D' can't be greater than or equal to selling price 'D'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if (discountD < purchasePrice) {
                    MessageBox.Show("Selling price 'D' can't be less than purchase price.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                } else if ((discountD - sellPriceD) - purchasePrice < 0) {
                    MessageBox.Show("Verify selling & discount price 'D' to avoid losses compared to the purchase price. [Note=> Loss = (Sell Price - Discount Price) - Purchase Price]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }


            var category = (Category)categoryTypesComboBox.SelectedItem;
            var productType = (ProductType)productTypesComboBox.SelectedItem;


            var purchaseDateTime = purchaseDate.Value;
            float purchaseCGST = float.Parse(purchaseCGstPercentText.Text.Trim());
            float purchaseSGST = float.Parse(purchaseSGstPercentText.Text.Trim());

            Product product = new Product(long.Parse(productId), productName, category, productType, qty);
            ProductPurchase purchase = new ProductPurchase(product, qty, purchasePrice, purchaseCGST, purchaseSGST, Util.U.ToDateTime(purchaseDateTime));
            if (allowMfgExpBatchCheckBox.Checked)
            {
                purchase.MfgExpBatch(Util.U.ToDate(mfgDate), Util.U.ToDate(expDate), batchNumber);
            }

            ProductSelling selling = new ProductSelling(product, sellPriceA, discountA, sellPriceB, discountB, sellPriceC, discountC, sellPriceD, discountD, sellingCGST, sellingSGST);

            if (productDao.Insert(product) && productSellingDao.Insert(selling) && productPurchaseDao.Insert(purchase))
            {
                MessageBox.Show("Product added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearProductForm();
                return;
            }
            else
            {
                MessageBox.Show("Something occur while insertion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindPlaceholderToTextBox(TextBox textBox, string placeholderText, Color placeholderTextColor)
        {
            textBox.Text = placeholderText;
            textBox.ForeColor = placeholderTextColor;
        }

        private void ClearProductForm()
        {
            productIdText.Text = productDao.GetNewProductId().ToString();

            BindPlaceholderToTextBox(productNameText, productPlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(qtyText, qtyPercentPlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(purchasePriceText, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(purchaseCGstPercentText, qtyPercentPlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(purchaseSGstPercentText, qtyPercentPlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(sellingPriceText_1, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(discountText_1, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(sellingPriceText_2, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(discountText_2, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(sellingPriceText_3, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(discountText_3, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(sellingPriceText_4, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(discountText_4, pricePlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(cgstText, qtyPercentPlaceHolder, Color.Gray);
            BindPlaceholderToTextBox(sgstText, qtyPercentPlaceHolder, Color.Gray);

            allowMfgExpBatchCheckBox.Checked = false;

            categoryTypesComboBox.SelectedIndex = 0;
            productTypesComboBox.SelectedIndex = 0;

            BindPlaceholderToTextBox(batchNumberText, batchNumberPlaceHolder, Color.Gray);

            expiryDate.Value = DateTime.Now;
            manufactureDate.Value = DateTime.Now;
            purchaseDate.Value = DateTime.Now;
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
            //categoryTable.DataSource = categoryList;
            BindCategoryTypeToDataGridView();

            table.Controls.Add(categoryTable, 0, 3);

            table.Controls.Add(footerPanel, 1, 4);


            table.SetColumnSpan(categoryTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            form.Controls.Add(table);

            //add event
            addButton.Click += AddCategoryButton_Click;

            // Attach the CellFormatting event handler
            //categoryTable.CellFormatting += CategoryTable_CellFormatting;

            categoryNameField.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(categoryNameField);

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
            //Binf list with datagridview
            productTypeTable.Columns[0].DataPropertyName = "Id";
            productTypeTable.Columns[1].DataPropertyName = "Abbr";
            productTypeTable.Columns[2].DataPropertyName = "Name";

            //productTypeTable.DataSource = productTypeList;
            BindProductTypeToDataGridView();

            table.Controls.Add(productTypeTable, 0, 3);

            table.Controls.Add(footerPanel, 1, 4);


            table.SetColumnSpan(productTypeTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            form.Controls.Add(table);

            // Attach event handlers
            addButton.Click += AddProductTypeButton_Click;

            // Attach the CellFormatting event handler
            //productTypeTable.CellFormatting += ProductTypeTable_CellFormatting;

            typeFullNameField.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(typeFullNameField);
            typeAbbrNameField.TextChanged += (sender, e) => TextBoxKeyEvent.UpperText_TextChanged(typeAbbrNameField);
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
                //Datagridview
                BindCategoryTypeToDataGridView();

                //checkbox
                BindCategoryTypeToComboBox();

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
                BindProductTypeToDataGridView();

                //Combobox
                BindProductTypeToComboBox();

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

        private void BindCategoryTypeToDataGridView()
        {
            categoryTable.DataSource = null;
            categoryTable.DataSource = categoryList;
        }

        private void BindCategoryTypeToComboBox()
        {
            categoryTypesComboBox.DataSource = null;
            categoryTypesComboBox.DataSource = categoryList;
            categoryTypesComboBox.DisplayMember = "Name";
            categoryTypesComboBox.ValueMember = "Id";
        }

        private void BindProductTypeToDataGridView()
        {
            productTypeTable.DataSource = null;
            productTypeTable.DataSource = productTypeList;
        }

        private void BindProductTypeToComboBox()
        {
            productTypesComboBox.DataSource = null;
            productTypesComboBox.DataSource = productTypeList;
            productTypesComboBox.DisplayMember = "Name";
            productTypesComboBox.ValueMember = "Id";
        }


    }
}
