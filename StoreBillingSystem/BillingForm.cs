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
using StoreBillingSystem.Util;

namespace StoreBillingSystem
{
    public class BillingForm : Form
    {
        private Font labelFont = new Font("Arial", 12, FontStyle.Bold);
        private Font textfieldFont = new Font("Arial", 12);


        private Panel topPanel;
        private Panel topPanel_1;
        private Panel bottomPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel centerPanel;


        public BillingForm()
        {
            //for disable header bar
            //ControlBox = false;
            //FormBorderStyle = FormBorderStyle.None;
            // Create the main form
            this.Text = "Billing";
            this.Size = new Size(1366, 768);

            InitComponets();

            InitComponentsData();
        }

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

            // Set the KeyPreview property of the form to true
            this.KeyPreview = true;

            //add event to form
            this.KeyDown += BillingForm_KeyDown;

        }

        private void BillingForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for the Ctrl+Alt+C key combination
            if (e.Control && e.Alt && e.KeyCode == Keys.C)
            {
                // Show the custom search form
                PreparedCustomerDialogBoxToSearch();
            }
        }

        private AutoCompleteStringCollection customerNameAutoSuggestion;
        private AutoCompleteStringCollection customerMobileNumberAutoSuggestion;

        private AutoCompleteStringCollection productNameAutoSuggestion;
        private List<string> customers;
        private List<string> customerMobiles;

        private List<string> productNames;

        private IProductDao productDao;
        private IProductSellingDao productSellingDao;
        private ICustomerDao customerDao;
        private void InitComponentsData()
        {
            SqliteConnection connection = DatabaseManager.GetConnection();

            productDao = new ProductDaoImpl(connection);
            productSellingDao = new ProductSellingDaoImpl(connection);
            customerDao = new CustomerDaoImpl(connection);

            productNameAutoSuggestion = new AutoCompleteStringCollection();
            productNames = (List<string>)productDao.ProductNames();

            customerNameAutoSuggestion = new AutoCompleteStringCollection();
            customers = (List<string>)customerDao.CustomerNames();

            customerMobileNumberAutoSuggestion = new AutoCompleteStringCollection();
            customerMobiles = (List<string>)customerDao.Phones();

            BindAutoSuggestionToCustomerNameTextBox();
            BindAutoSuggestionToMobileNumberTextBox();
            BindAutoSuggestionToProductNameTextBox();

        }


        private string customerNamePlaceHolder = U.ToTitleCase("Enter customer name ...");
        private string mobileNumberPlaceHolder = U.ToTitleCase("Enter mobile number ...");

        private TextBox billingNumText;
        private TextBox customerNameText;
        private TextBox mobileNumberText;
        private DateTimePicker dateTimePicker;

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
                Margin = new Padding(5)
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


            dateTimePicker = new DateTimePicker
            {
                //CustomFormat = "yyyy-MM-dd HH:mm:ss",
                Format = DateTimePickerFormat.Short,
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(dateTimePicker, 6, 0);

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

            panel.SetColumnSpan(customerNameText, 2);

            InitBillingHeaderFormEvent();
            return panel;

        }

        private void InitBillingHeaderFormEvent()
        {
            customerNameText.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(customerNameText);
            mobileNumberText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;


            customerNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(customerNameText, customerNamePlaceHolder);
            customerNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(customerNameText, customerNamePlaceHolder);

            mobileNumberText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(mobileNumberText, mobileNumberPlaceHolder);
            mobileNumberText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(mobileNumberText, mobileNumberPlaceHolder);


            customerNameText.KeyDown += CustomerNameText_KeyDown;
            mobileNumberText.KeyDown += MobileNumberText_KeyDown;
        }

       

        private DataGridView customerTable;
        private Customer _customer;
        private void CustomerNameText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchByCustomerName(customerNameText.Text.Trim());
            }
        }

        private void MobileNumberText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchByMobileNumber(mobileNumberText.Text.Trim());
            }
        }

        private void SearchByCustomerName(string customerName)
        {
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                IList<Customer> _customers = customerDao.Read(customerName);
                if(_customers.Count > 1)
                {
                    PreparedCustomerDialogBoxToSearch();
                }
                else if (_customers.Count == 1)
                {
                    _customer = _customers[0];
                    LoadCustomerDataToTextBox(_customer);
                }
                else MessageBox.Show("Customer name is not found.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Enter customer name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void SearchByMobileNumber(string mobileNumber)
        {
            if (!string.IsNullOrWhiteSpace(mobileNumber))
            {
                Customer customer = customerDao.Read(long.Parse(mobileNumber));
                if (customer != null)
                {
                    _customer = customer;
                    LoadCustomerDataToTextBox(_customer);
                }
                else MessageBox.Show("Customer mobile number is not found.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Enter mobile number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void PreparedCustomerDialogBoxToSearch()
        {
            customerTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
            };

            // Open the dialog box when Enter key is pressed
            CustomerCustomDialogBox customDialogBox = new CustomerCustomDialogBox(customerTable, customerDao.ReadAll());
            if (customDialogBox.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("Ok");

                DataGridViewRow selectedRow = customerTable.SelectedRows[0];
                string customerId = selectedRow.Cells["Id"].Value.ToString();
                _customer = customerDao.Read(int.Parse(customerId));
                Console.WriteLine("Customer :" + _customer);

                LoadCustomerDataToTextBox(_customer);
            }
            else
            {
                // Handle Cancel button logic
                Console.WriteLine("Cancel");
                _customer = null;
            }
        }

        private void LoadCustomerDataToTextBox(Customer customer)
        {
            customerNameText.Text = customer.Name;
            customerNameText.ForeColor = Color.Black;
            mobileNumberText.Text = customer.PhoneNumber.ToString();
            mobileNumberText.ForeColor = Color.Black;
        }


        private void BindAutoSuggestionToCustomerNameTextBox()
        {
            customerNameAutoSuggestion.AddRange(customers.ToArray());

            customerNameText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            customerNameText.AutoCompleteSource = AutoCompleteSource.CustomSource;
            customerNameText.AutoCompleteCustomSource = customerNameAutoSuggestion;
        }

        private void BindAutoSuggestionToMobileNumberTextBox()
        {
            customerMobileNumberAutoSuggestion.AddRange(customerMobiles.ToArray());

            mobileNumberText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            mobileNumberText.AutoCompleteSource = AutoCompleteSource.CustomSource;
            mobileNumberText.AutoCompleteCustomSource = customerMobileNumberAutoSuggestion;
        }

        private void BindAutoSuggestionToProductNameTextBox()
        {
            productNameAutoSuggestion.AddRange(productNames.ToArray());

            productNameText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            productNameText.AutoCompleteSource = AutoCompleteSource.CustomSource;
            productNameText.AutoCompleteCustomSource = productNameAutoSuggestion;
        }

       

        private List<BillingItem> billingItems = new List<BillingItem>();

        private string pricePlaceHolder = "0.00";
        private string qtyPercentPlaceHolder = "0.0";
        private string productPlaceHolder = Util.U.ToTitleCase("Enter product here...");
        private string searchCodePlaceHolder = Util.U.ToTitleCase("Search product code...");



        private TextBox productNameText;
        private TextBox productCodeText;
        private TextBox askingPriceText;
        private TextBox qtyText;
        private ComboBox productTypes;

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
            Button clearProductButton = new Button
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
            panel.Controls.Add(askingPriceText,5, 1);

            Button addProductButton = new Button
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

            InitAddProductFormEvent();

            clearProductButton.Click += (sender, e) => ClearProductForm();
            addProductButton.Click += (sender, e) => AddProductForm();
            return box;
        }

        private void InitAddProductFormEvent()
        {

            productNameText.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(productNameText);

            productNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(productNameText, productPlaceHolder);
            productNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(productNameText, productPlaceHolder);

            productCodeText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(productCodeText, searchCodePlaceHolder);
            productCodeText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(productCodeText, searchCodePlaceHolder);

            qtyText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(qtyText, qtyPercentPlaceHolder);
            qtyText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(qtyText, qtyPercentPlaceHolder);

            askingPriceText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(askingPriceText, pricePlaceHolder);
            askingPriceText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(askingPriceText, pricePlaceHolder);

            qtyText.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            askingPriceText.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            productCodeText.KeyPress += TextBoxKeyEvent.NumbericTextBox_KeyPress;

            productNameText.KeyDown += ProductNameTextBox_KeyDown;
            productCodeText.KeyDown += ProductCodeTextBox_KeyDown;
            qtyText.KeyDown += QtyTextBox_KeyDown;
            askingPriceText.KeyDown += AskingPriceTextBox_KeyDown;

        }

        private void AddProductForm()
        {
            if(string.IsNullOrWhiteSpace(productNameText.Text) || productNameText.Text == productPlaceHolder)
            {
                MessageBox.Show("Product Name can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrWhiteSpace(productCodeText.Text) || productCodeText.Text == searchCodePlaceHolder)
            {
                MessageBox.Show("Product code can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrWhiteSpace(qtyText.Text) || qtyText.Text == qtyPercentPlaceHolder)
            {
                MessageBox.Show("Qty can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrWhiteSpace(askingPriceText.Text) || askingPriceText.Text == pricePlaceHolder)
            {
                MessageBox.Show("Asking price can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrWhiteSpace(productTypes.SelectedItem.ToString()))
            {
                MessageBox.Show("Product type can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string type = productTypes.SelectedItem.ToString();
            Console.WriteLine("product ty :" + type);

            if (_billingItem != null)
            {
                _billingItem.AskQty = float.Parse(qtyText.Text.Trim());
                _billingItem.AskAmount = double.Parse(askingPriceText.Text.Trim());
                _billingItem.SrNo = billingItems.Count + 1;

                double totalAmt = _billingItem.SellingPrice * _billingItem.AskQty;
                double totalDiscount = _billingItem.DiscountPrice * _billingItem.AskQty;
                double total = totalAmt - totalDiscount;
                //add to billing table
                /*
                billingTable.Rows.Add(_billingItem.SrNo, _billingItem.ProductSelling.Product.Name, _billingItem.ProductSelling.Product.ProductType.Abbr,
                    _billingItem.AskQty, 
                    _billingItem.SellingPrice.ToString("C2"), 
                    _billingItem.TotalAmount.ToString("C2"),
                    _billingItem.ProductSelling.GetTotalGSTInPercent().ToString("P"), 
                    _billingItem.TotalDiscount.ToString("C2"), 
                    _billingItem.Total.ToString("C2"));
                */
                billingTable.Rows.Add(_billingItem.SrNo, _billingItem.ProductSelling.Product.Name, _billingItem.ProductSelling.Product.ProductType.Abbr,
                    _billingItem.AskQty,
                    _billingItem.SellingPrice,
                    _billingItem.TotalAmount,
                    _billingItem.ProductSelling.GetTotalGSTInPercent(),
                    _billingItem.TotalDiscount,
                    _billingItem.Total);


                //add to List collection
                billingItems.Add(_billingItem);



                //clear after add to billing table
                ClearProductForm();
                _billingItem = null;
                UpdateTotalAmountTable();
            }
        }

        private void UpdateTotalAmountTable() 
        {
            double totalAmt = 0, totalDiscount = 0, total = 0;
            foreach(BillingItem item in billingItems)
            {
                totalAmt += item.TotalAmount;
                totalDiscount += item.TotalDiscount;
                total += item.Total;
            }
            /*
            amtTable.Rows[0].Cells[5].Value = totalAmt.ToString("C2");
            amtTable.Rows[0].Cells[6].Value = 0.ToString("C2");
            amtTable.Rows[0].Cells[7].Value = totalDiscount.ToString("C2");
            amtTable.Rows[0].Cells[8].Value = total.ToString("C2");
            */
            amtTable.Rows[0].Cells[5].Value = totalAmt;
            amtTable.Rows[0].Cells[6].Value = 0;
            amtTable.Rows[0].Cells[7].Value = totalDiscount;
            amtTable.Rows[0].Cells[8].Value = total;
        }


        private void ClearProductForm()
        {
            //productCodeText.Text = "";
            productTypes.Items.Clear();

            TextBoxKeyEvent.BindPlaceholderToTextBox(customerNameText, customerNamePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(mobileNumberText, mobileNumberPlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(productNameText, productPlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(productCodeText, searchCodePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(qtyText, qtyPercentPlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(askingPriceText, pricePlaceHolder, Color.Gray);
        }

        private void ProductNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Open the dialog box when Enter key is pressed
                SearchByProductName(productNameText.Text.Trim());
            }
        }

        private void ProductCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Open the dialog box when Enter key is pressed
                SearchByProductId(productCodeText.Text.Trim());
            }
        }
        private void QtyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //calculate asking price
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(qtyText.Text.Trim())) return;

                //if qty contains placeholder value
                if (qtyText.Text.Trim() == qtyPercentPlaceHolder) return;

                if (_billingItem != null)
                {
                    askingPriceText.Text = (float.Parse(qtyText.Text.Trim()) * (_billingItem.SellingPrice - _billingItem.DiscountPrice)).ToString();
                    //TextBoxKeyEvent.BindPlaceholderToTextBox(productNameText, productPlaceHolder, Color.Gray);
                    askingPriceText.ForeColor = Color.Black;
                }
            }
        }

        private void AskingPriceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //calculate asking qty
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(askingPriceText.Text.Trim())) return;
                if (askingPriceText.Text.Trim() == pricePlaceHolder) return;
                if (_billingItem != null && !string.IsNullOrWhiteSpace(qtyText.Text.Trim()))
                {
                    qtyText.Text = (double.Parse(askingPriceText.Text.Trim()) / (_billingItem.SellingPrice - _billingItem.DiscountPrice)).ToString();
                    //TextBoxKeyEvent.BindPlaceholderToTextBox(productNameText, productPlaceHolder, Color.Gray);
                    qtyText.ForeColor = Color.Black;
                }
            }
        }


        private void SearchByProductName(string productName)
        {
            if (!string.IsNullOrWhiteSpace(productName))
            {
                Product product = productDao.Read(productName);
                if (product != null) PreparedProductItemForBilling(product);
                else MessageBox.Show("Product name is not found.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Enter product name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void SearchByProductId(string productId)
        {
            if (!string.IsNullOrWhiteSpace(productId))
            {
                Product product = productDao.Read(long.Parse(productId));
                if (product != null) PreparedProductItemForBilling(product);
                else MessageBox.Show("Product id is not found.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Enter product id.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private DataGridView productSellingTable;
        private BillingItem _billingItem;
        private void PreparedProductItemForBilling(Product product)
        {
            Console.WriteLine("product ::" + product);
            productTypes.Items.Add(product.ProductType.Abbr);
            productTypes.SelectedIndex = 0;

            if (productNameText.Text == productPlaceHolder)
            {
                productNameText.Text = product.Name;
                productNameText.ForeColor = Color.Black;
            }

            if (productCodeText.Text == searchCodePlaceHolder)
            {
                productCodeText.Text = product.Id.ToString();
                productCodeText.ForeColor = Color.Black;
            }

            ProductSelling selling = productSellingDao.Read(product);

            productSellingTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
            };

            ProductSellingCustomDialogBox sellingCustomDialog = new ProductSellingCustomDialogBox(productSellingTable, selling);
            if (sellingCustomDialog.ShowDialog() == DialogResult.OK)
            {
                // Handle OK button logic
                Console.WriteLine("Ok");
                DataGridViewRow selectedRow = productSellingTable.SelectedRows[0];
                string type = selectedRow.Cells["Selling Type"].Value.ToString();

                SellingType sellingType = 0;
                if (type == SellingType.A.ToString()) sellingType = SellingType.A;
                else if (type == SellingType.B.ToString()) sellingType = SellingType.B;
                else if (type == SellingType.C.ToString()) sellingType = SellingType.C;
                else if (type == SellingType.D.ToString()) sellingType = SellingType.D;

                _billingItem = new BillingItem(selling, sellingType);
                /*
                productSellingTable.Columns[0].Name = "Selling Type";
                productSellingTable.Columns[1].Name = "Selling (Rs.)";
                productSellingTable.Columns[2].Name = "Discount (Rs.)";
                productSellingTable.Columns[3].Name = "Total Selling (Rs.)";
                productSellingTable.Columns[4].Name = "GST (%)";
                productSellingTable.Columns[5].Name = "GST (Rs.)";
                productSellingTable.Columns[6].Name = "Total (Rs.)";
                */
            }
            else
            {
                // Handle Cancel button logic
                Console.WriteLine("Cancel");
                _billingItem = null;
            }


        }


        private DataGridView billingTable;
        private DataGridView amtTable;
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
                Location= new Point(100, 180),
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

            amtTable.Rows.Add("", "Total (Rs.)", "", "", "", 0.00.ToString("C2"), 0.00.ToString("C2"), 0.00.ToString("C2"), 0.00.ToString("C2"));
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
            return panel;
        }


    }
}
