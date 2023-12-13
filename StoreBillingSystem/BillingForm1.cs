using System;
using System.Drawing;
using System.Drawing.Printing;
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
    public class BillingForm1 : AbstractBillingForm
    {

        private List<BillingItem> billingItems = new List<BillingItem>();

        private AutoCompleteStringCollection customerNameAutoSuggestion;
        private AutoCompleteStringCollection customerMobileNumberAutoSuggestion;

        private AutoCompleteStringCollection productNameAutoSuggestion;
        private List<string> customers;
        private List<string> customerMobiles;

        private List<string> productNames;


        private IProductDao productDao;
        private IProductSellingDao productSellingDao;
        private ICustomerDao customerDao;
        private IBillingDateDao billingDateDao;
        private IBillingDao billingDao;
        private IBillingDetailsDao billingDetailsDao;
        private IPaymentDao paymentDao;
        private DataGridView customerTable;
        private Customer _customer;
        private BillingDate _billingDate;

        private ContextMenuStrip mouseMenu;
        private bool _onUpdateItem = false;

        private DataGridView productSellingTable;
        private BillingItem _billingItem;
        private BillingDetails _billingDetails;

        public BillingForm1()
        {
            //for disable header bar
            //ControlBox = false;
            //FormBorderStyle = FormBorderStyle.None;
            // Create the main form
            this.Text = "Billing";
            //this.Size = new Size(1366, 768);

            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            this.MinimumSize = new Size(1366, 768); // Set your minimum size
            this.MaximumSize = new Size(1366, 768); // Set your maximum size

            // Set the KeyPreview property of the form to true
            this.KeyPreview = true;

            //add event to form
            this.KeyDown += BillingForm_KeyDown;


            InitBillingHeaderFormEvent();
            InitAddProductFormEvent();
            InitBillingTableEvent();
            InitFooterFormEvent();


            InitComponentsData();
        }

        /// <summary>
        /// Inits the components data from database to form.
        /// </summary>
        private void InitComponentsData()
        {
            SqliteConnection connection = DatabaseManager.GetConnection();

            billingDateDao = new BillingDateDaoImpl(connection);
            billingDao = new BillingDaoImpl(connection);
            billingDetailsDao = new BillingDetailsDaoImpl(connection);
            paymentDao = new PaymentDaoImpl(connection);
            
            BillingDateChanged();

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
        /// <summary>
        /// Billings the date changed then Set Billing Number to form.
        /// </summary>
        private void BillingDateChanged()
        {
            DateTime billDate = billDateTimePicker.Value;
            _billingDate = new BillingDate(U.ToDate(billDate));

            if (!billingDateDao.IsRecordExists(_billingDate.BillDate))
            {
                billingDateDao.Insert(_billingDate);
            }
            else
            {
                _billingDate = billingDateDao.Read(_billingDate.BillDate);
            }

            //Set Billing Number
            billingNumText.Text = billingDao.GetNewBillingNumber(_billingDate).ToString();
        }

        /// <summary>
        /// Binds the auto suggestion to customer name text box.
        /// </summary>
        private void BindAutoSuggestionToCustomerNameTextBox()
        {
            customerNameAutoSuggestion.AddRange(customers.ToArray());

            customerNameText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            customerNameText.AutoCompleteSource = AutoCompleteSource.CustomSource;
            customerNameText.AutoCompleteCustomSource = customerNameAutoSuggestion;
        }

        /// <summary>
        /// Binds the auto suggestion to mobile number text box.
        /// </summary>
        private void BindAutoSuggestionToMobileNumberTextBox()
        {
            customerMobileNumberAutoSuggestion.AddRange(customerMobiles.ToArray());

            mobileNumberText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            mobileNumberText.AutoCompleteSource = AutoCompleteSource.CustomSource;
            mobileNumberText.AutoCompleteCustomSource = customerMobileNumberAutoSuggestion;
        }

        /// <summary>
        /// Binds the auto suggestion to product name text box.
        /// </summary>
        private void BindAutoSuggestionToProductNameTextBox()
        {
            productNameAutoSuggestion.AddRange(productNames.ToArray());

            productNameText.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            productNameText.AutoCompleteSource = AutoCompleteSource.CustomSource;
            productNameText.AutoCompleteCustomSource = productNameAutoSuggestion;
        }

 
        /*
         * =========================================       
         * Customer Form
         * =========================================        
         */

        /// <summary>
        /// Inits the billing header or top form event.
        /// </summary>
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

            billDateTimePicker.ValueChanged += BillDateTimePicker_ValueChanged;

            clearCustomerButton.Click += (sender, e) => ClearCustomerForm();
        }

        private void BillDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            // Handle the ValueChanged event when billing date changed
            BillingDateChanged();
        }

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
                if (_customers.Count > 1)
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

        private void ClearCustomerForm()
        {
            TextBoxKeyEvent.BindPlaceholderToTextBox(customerNameText, customerNamePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(mobileNumberText, mobileNumberPlaceHolder, Color.Gray);
        }

        //=================================================

        /*
         * ================================================
         * Product Form
         * ================================================        
         */

        /// <summary>
        /// Inits the add product form event.
        /// </summary>
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

            clearProductButton.Click += (sender, e) => ClearProductForm();
            addProductButton.Click += (sender, e) => AddProductForm();
        }

        private void ClearProductForm()
        {
            if (_onUpdateItem)
            {
                productNameText.ReadOnly = false;
                productCodeText.ReadOnly = false;
                _onUpdateItem = false;
            }


            //productCodeText.Text = "";
            productTypes.Items.Clear();

            TextBoxKeyEvent.BindPlaceholderToTextBox(productNameText, productPlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(productCodeText, searchCodePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(qtyText, qtyPercentPlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(askingPriceText, pricePlaceHolder, Color.Gray);
        }

        private void AddProductForm()
        {
            if (string.IsNullOrWhiteSpace(productNameText.Text) || productNameText.Text == productPlaceHolder)
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
                //_billingItem.SrNo = billingItems.Count + 1;

                //double totalAmt = _billingItem.SellingPrice * _billingItem.AskQty;
                //double totalDiscount = _billingItem.DiscountPrice * _billingItem.AskQty;
                //double total = totalAmt - totalDiscount;
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
                //_billingItem.SrNo

                //When update Item
                if (_onUpdateItem)
                {
                    billingTable.Rows[billingTable.CurrentCell.RowIndex].Cells[3].Value = _billingItem.AskQty;
                    billingTable.Rows[billingTable.CurrentCell.RowIndex].Cells[4].Value = _billingItem.SellingPrice;
                    billingTable.Rows[billingTable.CurrentCell.RowIndex].Cells[5].Value = _billingItem.TotalAmount;
                    billingTable.Rows[billingTable.CurrentCell.RowIndex].Cells[6].Value = _billingItem.ProductSelling.GetTotalGSTInPercent();
                    billingTable.Rows[billingTable.CurrentCell.RowIndex].Cells[7].Value = _billingItem.TotalDiscount;
                    billingTable.Rows[billingTable.CurrentCell.RowIndex].Cells[8].Value = _billingItem.Total;

                    productNameText.ReadOnly = false;
                    productCodeText.ReadOnly = false;
                    _onUpdateItem = false;
                }
                else
                {
                    //When new added Item
                    billingTable.Rows.Add("", _billingItem.ProductSelling.Product.Name, _billingItem.ProductSelling.Product.ProductType.Abbr,
                        _billingItem.AskQty,
                        _billingItem.SellingPrice,
                        _billingItem.TotalAmount,
                        _billingItem.ProductSelling.GetTotalGSTInPercent(),
                        _billingItem.TotalDiscount,
                        _billingItem.Total);

                    //add to List collection
                    billingItems.Add(_billingItem);
                }



                //clear after add to billing table
                ClearProductForm();
                _billingItem = null;
                UpdateTotalAmountTable();
            }
        }

        private void UpdateTotalAmountTable()
        {
            double totalAmt = 0, totalDiscount = 0, total = 0;
            foreach (BillingItem item in billingItems)
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

        //==========================================================

        /*
         * ==================================================       
         * Billing Table
         * ==================================================        
         */

        /// <summary>
        /// Inits the billing totat display event.
        /// </summary>
        private void InitBillingTableEvent()
        {
            billingTable.CellMouseUp += BillingTable_CellMouseUp;
            billingTable.CellFormatting += BillingTable_CellFormatting;
        }

        private void BillingTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell is in the first column (index 0) and not a header cell
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // Set the value to the current row index plus the initial serial number
                e.Value = e.RowIndex + 1;
                e.FormattingApplied = true;
            }
        }

        private void BillingTable_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (mouseMenu == null)
                {
                    // Select the row that was clicked
                    billingTable.CurrentCell = billingTable.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    PreparedBillingTableMouseMenu();
                    mouseMenu.Show(billingTable, e.Location);
                }
                mouseMenu = null;
            }
        }

        private void PreparedBillingTableMouseMenu()
        {
            mouseMenu = new ContextMenuStrip();
            mouseMenu.Items.Add("Update Item", null, OnUpdateItem);
            mouseMenu.Items.Add("Remove Item", null, OnRemoveItem);
            mouseMenu.Items.Add("Remove All Item", null, OnRemoveAllItem);

        }


        private void OnUpdateItem(object sender, EventArgs e)
        {

            _billingItem = billingItems[billingTable.CurrentCell.RowIndex];

            productNameText.Text = _billingItem.ProductSelling.Product.Name;
            productNameText.ForeColor = Color.Black;


            productTypes.Items.Clear();
            productTypes.Items.Add(_billingItem.ProductSelling.Product.ProductType.Abbr);
            productTypes.SelectedIndex = 0;


            productCodeText.Text = _billingItem.ProductSelling.Product.Id.ToString();
            productCodeText.ForeColor = Color.Black;

            qtyText.Text = _billingItem.AskQty.ToString();
            qtyText.ForeColor = Color.Black;

            askingPriceText.Text = _billingItem.AskAmount.ToString();
            askingPriceText.ForeColor = Color.Black;

            productNameText.ReadOnly = true;
            productNameText.BackColor = Color.White;
            productCodeText.ReadOnly = true;
            productCodeText.BackColor = Color.White;

            _onUpdateItem = true;
        }

        private void OnRemoveItem(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Do you want to remove the selected Item with Sr No.:{billingTable.CurrentCell.RowIndex + 1}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                billingItems.RemoveAt(billingTable.CurrentCell.RowIndex);
                // Delete the row from the DataGridView
                billingTable.Rows.RemoveAt(billingTable.CurrentCell.RowIndex);
                UpdateTotalAmountTable();
            }

        }

        private void OnRemoveAllItem(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Do you want to remove all Items?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                billingTable.Rows.Clear();
                billingItems.Clear();
                UpdateTotalAmountTable();
            }

        }
        //==========================================================

        /*
         * ==================================================       
         * Whole Form Event
         * ==================================================        
         */


        private void BillingForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Check for the Ctrl+Alt+C key combination
            if (e.Control && e.Alt && e.KeyCode == Keys.C)
            {
                // Show the custom search form
                PreparedCustomerDialogBoxToSearch();
            }
        }

        //==========================================================

        /*
         * ==================================================
         *  Footer Button Event
         * ==================================================
         */        
                
        /// <summary>
        /// Inits the footer or bottom form event.
        /// </summary>
        private void InitFooterFormEvent()
        {
            clearBillingButton.Click += (sender, e) => ClearAll();
            saveBillingButton.Click += (sender, e) => SaveBilling();
            newBillingButton.Click += (sender, e) => ClearAll();
            printBillingButton.Click += (sender, e) => PrintBilling();
        }


        private bool ValidationOnBillingForm()
        {
            if (string.IsNullOrWhiteSpace(customerNameText.Text) || customerNameText.Text == customerNamePlaceHolder)
            {
                MessageBox.Show("Customer name can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (string.IsNullOrWhiteSpace(mobileNumberText.Text) || mobileNumberText.Text == mobileNumberPlaceHolder)
            {
                MessageBox.Show("Customer mobile number can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (billingTable.Rows.Count <= 0)
            {
                MessageBox.Show("Billing items can't be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void SaveBilling()
        {
            //DialogResult result = MessageBox.Show($"Do you want to save this bill?", "Save Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (result == DialogResult.Yes)
            {
                if (!ValidationOnBillingForm()) return;

                double grossAmt = double.Parse(amtTable.Rows[0].Cells[5].Value.ToString());
                double gstPrice = double.Parse(amtTable.Rows[0].Cells[6].Value.ToString());
                double discountPrice = double.Parse(amtTable.Rows[0].Cells[7].Value.ToString());
                double netAmt = double.Parse(amtTable.Rows[0].Cells[8].Value.ToString());


                Billing billing = new Billing
                {
                    BillingDate = _billingDate,
                    Customer = _customer,
                    BillingNumber = long.Parse(billingNumText.Text),
                    BillingDateTime = U.ToDateTime(billDateTimePicker.Value),
                    GrossAmount = grossAmt,
                    GSTPrice = gstPrice,
                    DiscountPrice = discountPrice,
                    NetAmount = netAmt
                };

                _billingDetails = new BillingDetails
                {
                    Billing = billing,
                    Items = new List<Item>()
                };

                for(int i = 0; i < billingItems.Count; i++)
                {
                    _billingDetails.Items.Add(new Item
                    {
                        SrNum = i+1, //long.Parse(billingTable.Rows[i].Cells[0].Value.ToString())
                        Product = billingItems[i].ProductSelling.Product,
                        Qty = billingItems[i].AskQty,
                        Price = billingItems[i].SellingPrice,
                        GrossAmount = billingItems[i].TotalAmount,
                        GSTPercent = billingItems[i].ProductSelling.GetTotalGSTInPercent(),
                        DiscountPrice = billingItems[i].TotalDiscount,
                        NetAmount = billingItems[i].Total
                    }
                    );

                }
                Payment payment = new Payment(billing);


                CustomPaymentDialogBox customPaymentDialogBox = new CustomPaymentDialogBox(payment);
                if (customPaymentDialogBox.ShowDialog() == DialogResult.OK)
                {

                    Console.WriteLine(payment);
                    /*
                    //Write code to insert into database
                    bool isBillingInsert = billingDao.Insert(billing);
                    bool isBillingDetailsInsert = billingDetailsDao.Insert(_billingDetails);
                    bool isPaymentInsert = paymentDao.Insert(payment);


                    if (isBillingInsert && isBillingDetailsInsert && isPaymentInsert)
                    {
                        MessageBox.Show("Save successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAll();
                    }
                    */
                }
                else
                {
                    _billingDetails = null;
                }


            }
        }

        private void ClearAll()
        {
            ClearCustomerForm();
            ClearProductForm();

            //Items clear
            billingItems.Clear();
            billingTable.Rows.Clear();
            TotalAmountTableChangedToDefault();

            //Set to Current Time & Bill Number
            billDateTimePicker.Value = DateTime.Now;
            BillingDateChanged();
        }

        private void PrintBilling()
        {
            if (!ValidationOnBillingForm()) return;

            SaveBilling();

            //MessageBox.Show("No implementation added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            // Create a PrintPreviewDialog to preview the printout
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Set up the document layout for printing
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font normalFont = new Font("Arial", 12);

            int yPos = 20;

            // Print customer information
            e.Graphics.DrawString("Customer Name: " + _billingDetails.Billing.Customer.Name, headerFont, Brushes.Black, 20, yPos);
            yPos += 30;
            e.Graphics.DrawString("Customer Address: " + _billingDetails.Billing.Customer.Address, normalFont, Brushes.Black, 20, yPos);
            yPos += 40;

            // Print billing items
            foreach (Item item in _billingDetails.Items)
            {
                string line = $"{item.Product.Name} - {item.Qty} x {item.Price}";
                e.Graphics.DrawString(line, normalFont, Brushes.Black, 20, yPos);
                yPos += 20;
            }

            // Print total amount
            yPos += 10;
            e.Graphics.DrawString("Total Amount: " + _billingDetails.Billing.NetAmount.ToString("C2"), headerFont, Brushes.Black, 20, yPos);
        }
    }
}
