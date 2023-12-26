using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using StoreBillingSystem.Database;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Events;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.StoreForm.ProductForm
{
    public class ProductDisplayForm : Form
    {
        public ProductDisplayForm()
        {
            InitializeComponent();
            InitializeProductData();

            InitProductFormEvent();
        }


        private IProductDao productDao;
        private ICategoryDao categoryDao;
        private IProductTypeDao productTypeDao;

        private DataGridView productTable;
        private IList<Product> productList;
        private IList<Category> categoryList;
        private IList<ProductType> productTypeList;

        private Product _product;
        //private BindingSource bindingSource;

        private Font labelFont = U.StoreLabelFont;
        private Font textBoxFont = U.StoreTextBoxFont;

        private Label totalProductLabel;
        private TextBox productNameText;

        private Button okButton;
        private Button viewButton;
        private Button deleteButton;
        private Button updateButton;
        private Button clearButton;
        private ComboBox categoryTypesComboBox;

        public bool IsCustomerModified { get; private set; }

        private string _productNamePlaceHolder = U.ToTitleCase("Search Product Name Here..");
        private string qtyPercentPlaceHolder = 0.0F.ToString("N");

        private void InitializeComponent()
        {
            Text = "Product Details";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1150, 650);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 6,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 460F)); //row-2 //DataGridView
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-3

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400f)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F)); //black
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F)); //phone
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F)); //phone


            //row-0
            table.Controls.Add(new Label
            {
                Text = "Search Product :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 0);

            productNameText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = _productNamePlaceHolder,
                ForeColor = Color.Gray,
            };
            table.Controls.Add(productNameText, 1, 0);

            table.Controls.Add(new Label
            {
                Text = "Select Category :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 3, 0);


            categoryTypesComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = textBoxFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(categoryTypesComboBox, 4, 0);


            clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                DialogResult = DialogResult.OK,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            table.Controls.Add(clearButton, 5, 0);

            //row-1
            //blank

            //row-2

            productTable = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };
            productTable.RowHeadersDefaultCellStyle.Font = U.StoreLabelFont;

            productTable.ColumnCount = 6;
            productTable.Columns[0].Name = "Id";
            productTable.Columns[1].Name = "Name";
            productTable.Columns[2].Name = "Category";
            productTable.Columns[3].Name = "Product Type Full";
            productTable.Columns[4].Name = "Product Type Abbr";
            productTable.Columns[5].Name = "Total Qty";


            productTable.Columns[0].Width = 100;
            productTable.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            productTable.Columns[2].Width = 250;
            productTable.Columns[3].Width = 120;
            productTable.Columns[4].Width = 120;
            productTable.Columns[5].Width = 150;


            productTable.Columns[0].DataPropertyName = "Id";
            productTable.Columns[1].DataPropertyName = "Name";
            productTable.Columns[5].DataPropertyName = "TotalQty";

            productTable.Columns[5].DefaultCellStyle.Format = "N";


            productTable.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; //data display in center
            productTable.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //header display in center

            productTable.ReadOnly = true;
            productTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productTable.MultiSelect = false;


            foreach (DataGridViewColumn column in productTable.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            productTable.AllowUserToAddRows = false;
            productTable.AutoGenerateColumns = false;
            productTable.RowHeadersVisible = false;
            productTable.AllowUserToResizeRows = false;
            productTable.AllowUserToResizeColumns = false;

            table.Controls.Add(productTable, 0, 2);
            table.SetColumnSpan(productTable, table.ColumnCount);

            //row-3
            Label totalProductShowLabel = new Label
            {
                Text = "Total Product :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalProductShowLabel, 3, 3);
            table.SetColumnSpan(totalProductShowLabel, 2);

            totalProductLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalProductLabel, 5, 3);

            //Row-4
            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 290)); //name

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
            updateButton = new Button
            {
                Text = "Edit",
                //DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            CancelButton = okButton;



            table1.Controls.Add(updateButton, 1, 0);
            table1.Controls.Add(deleteButton, 2, 0);
            table1.Controls.Add(viewButton, 3, 0);
            table1.Controls.Add(okButton, 4, 0);

            table.Controls.Add(table1, 1, 4);
            table.SetColumnSpan(table1, 4);

            //row-5
            //blank

            Controls.Add(table);


        }



        private void ProductTable_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in productTable.Rows)
            {
                // Set the value for Category.Name column
                if (row.DataBoundItem is Product product)
                {
                    row.Cells[2].Value = product.Category?.Name;
                    row.Cells[3].Value = product.ProductType?.Name;
                    row.Cells[4].Value = product.ProductType?.Abbr;
                }
            }
        }


        private void BindProductToDataGridView()
        {
            productTable.DataSource = null;
            productTable.DataSource = productList;
        }
        private void InitializeProductData()
        {

            productDao = new ProductDaoImpl(DatabaseManager.GetConnection());
            productList = new List<Product>(productDao.ReadAll());

            BindProductToDataGridView();
            UpdateProductCountAtLabel(productList.Count);


            categoryDao = new CategoryDaoImpl(DatabaseManager.GetConnection());
           
            categoryList = categoryDao.ReadAll(true);

            IList<Category> categories = categoryList.ToList();
            categories.Insert(0, new Category { Id = 0, Name = "" });

            categoryTypesComboBox.DataSource = null;
            categoryTypesComboBox.DataSource = categories;
            categoryTypesComboBox.DisplayMember = "Name";
            categoryTypesComboBox.ValueMember = "Id";

            productTypeDao = new ProductTypeDaoImpl(DatabaseManager.GetConnection());
            productTypeList = productTypeDao.ReadAll(true);
        }

        private void UpdateProductCountAtLabel(int count)
        {
            totalProductLabel.Text = count.ToString();
        }

        private void InitProductFormEvent()
        {

            productNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(productNameText, _productNamePlaceHolder);
            productNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(productNameText, _productNamePlaceHolder);


            productNameText.TextChanged += SearchCustomerTextBox_TextChanged;
            categoryTypesComboBox.SelectedIndexChanged += CategoryTypesComboBox_SelectedIndexChanged;
            productTable.DataBindingComplete += ProductTable_DataBindingComplete;


            okButton.Click += OkButton_Click;
            viewButton.Click += ViewButton_Click;
            deleteButton.Click += DeleteButton_Click;
            updateButton.Click += UpdateButton_Click;
            clearButton.Click += ClearButton_Click;
        }

        private void SearchCustomerTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBoxKeyEvent.CapitalizeText_TextChanged(productNameText);

            string searchTerm = productNameText.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm == _productNamePlaceHolder.ToLower()) return;

            List<Product> filteredList = productList
                .Where(product => product.Name.ToLower().Contains(searchTerm))
                .ToList();

            // Update the BindingSource with the filtered data

            productTable.DataSource = null;
            productTable.DataSource = filteredList;
            UpdateProductCountAtLabel(filteredList.Count);
        }



        private void CategoryTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = (Category)categoryTypesComboBox.SelectedItem;

            List<Product> filteredList = productList
                .Where(product => product.Category?.Id == category.Id || category.Id == 0)
                .ToList();

            // Update the BindingSource with the filtered data

            productTable.DataSource = null;
            productTable.DataSource = filteredList;
            UpdateProductCountAtLabel(filteredList.Count);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            TextBoxKeyEvent.BindPlaceholderToTextBox(productNameText, _productNamePlaceHolder, Color.Gray);
            categoryTypesComboBox.SelectedIndex = 0;
            InitializeProductData();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.OK;
            Close();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (productTable.Rows.Count == 0) return;
            DialogResult result = MessageBox.Show($"Do you want to remove the selected Product with its ID is :{productTable.CurrentRow.Cells[0].Value}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (productDao.Delete(long.Parse(productTable.CurrentRow.Cells[0].Value.ToString())))
                {
                    // Delete product records from list that its bind with datagridview datasource
                    productList.RemoveAt(productTable.CurrentCell.RowIndex);

                    // Delete the row from the DataGridView
                    productTable.Rows.RemoveAt(productTable.CurrentCell.RowIndex);

                    //update totalCustomer count
                    UpdateProductCountAtLabel(productList.Count);
                }
            }
        }


        private void ViewButton_Click(object sender, EventArgs e)
        {

            _product = productList[productTable.CurrentCell.RowIndex];
            ProductForm(false, true).ShowDialog();
            //DialogResult = DialogResult.Cancel;
            //Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            _product = productList[productTable.CurrentCell.RowIndex];
            ProductForm(true, false).ShowDialog();

        }

        private Form ProductForm(bool forUpdateCustomer, bool forViewCustomer)
        {
            Form form = new Form();
            form.HelpButton = true; // Display a help button on the form
            form.FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            form.MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            form.MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            form.StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            form.Size = new Size(600, 450);
            form.BackColor = U.StoreDialogBackColor;

            if (forUpdateCustomer) form.Text = "Update Product Details";
            else if (forViewCustomer) form.Text = "Product Details";

            form.Controls.Add(UpdateViewForm(form, forUpdateCustomer, forViewCustomer));


            InitUpdateViewProductData(forUpdateCustomer, forViewCustomer);

            return form;
        }

        private void InitUpdateViewProductData(bool forUpdateProduct, bool forViewProduct)
        {
            _idTextBox.Text = _product.Id.ToString();
            _nameTextBox.Text = _product.Name;
            _qtyTextBox.Text = _product.TotalQty.ToString("N");
            _typeAbbrTextBox.Text = _product.ProductType?.Abbr ?? "";

            if (forViewProduct)
            {
                _categoryTextBox.Text = _product.Category?.Name ?? "";
                _typeNameTextBox.Text = _product.ProductType?.Name ?? "";
            }
            else if (forUpdateProduct)
            {
                BindCategoryTypeToComboBox();
                BindProductTypeToComboBox();

                if(float.Parse(_qtyTextBox.Text) == 0)
                {
                    TextBoxKeyEvent.BindPlaceholderToTextBox(_qtyTextBox, qtyPercentPlaceHolder, Color.Gray);
                }


                int categoryIndex = categoryList.IndexOf(categoryList.FirstOrDefault(category => _product.Category?.Id == category.Id));
                _categoryComboBox.SelectedIndex = categoryIndex;

                int productTypeIndex = productTypeList.IndexOf(productTypeList.FirstOrDefault(productType => _product.ProductType?.Id == productType.Id));
                _typeNameComboBox.SelectedIndex = productTypeIndex;

                _typeNameComboBox.SelectedIndexChanged += TypeNameComboBox_SelectedIndexChanged;
            }
        }

        private void TypeNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var productType = (ProductType)_typeNameComboBox.SelectedItem;
            _typeAbbrTextBox.Text = productType.Abbr;
        }

        private void BindCategoryTypeToComboBox()
        {
            _categoryComboBox.DataSource = null;
            _categoryComboBox.DataSource = categoryList;
            _categoryComboBox.DisplayMember = "Name";
            _categoryComboBox.ValueMember = "Id";
        }


        private void BindProductTypeToComboBox()
        {
            _typeNameComboBox.DataSource = null;
            _typeNameComboBox.DataSource = productTypeList;
            _typeNameComboBox.DisplayMember = "Name";
            _typeNameComboBox.ValueMember = "Id";
        }

        private TextBox _idTextBox;
        private TextBox _nameTextBox;
        private TextBox _categoryTextBox;
        private TextBox _typeNameTextBox;
        private TextBox _typeAbbrTextBox;
        private TextBox _qtyTextBox;
        private ComboBox _categoryComboBox;
        private ComboBox _typeNameComboBox;
        private TableLayoutPanel UpdateViewForm(Form form, bool forUpdateProduct, bool forViewProduct)
        {

            Label productIdLabel = new Label
            {
                Text = "Product Id :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label nameLabel = new Label
            {
                Text = "Product Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label categoryLabel = new Label
            {
                Text = "Category :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label typeNameLabel = new Label
            {
                Text = "Product Type Name :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            };

            Label typeAbbrLabel = new Label
            {
                Text = "Product Type Abbr :",
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                Font = labelFont,
                TextAlign = ContentAlignment.MiddleRight
            };

            Label qtyLabel = new Label
            {
                Text = "Total Qty :",
                ForeColor = Color.Black,
                Dock = DockStyle.Fill,
                Font = labelFont,
                TextAlign = ContentAlignment.MiddleRight
            };


            _idTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
                ReadOnly = true,
                BackColor = Color.White
            };

            _nameTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
            };

            _typeAbbrTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
                ReadOnly = true,
                BackColor = Color.White
            };

            _qtyTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = U.StoreTextBoxFont,
                Margin = new Padding(10),
            };


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Aquamarine,
                ColumnCount = 4,
                RowCount = 9
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 325F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            table.Controls.Add(productIdLabel, 0, 0);
            table.Controls.Add(_idTextBox, 1, 0);

            table.Controls.Add(nameLabel, 0, 1);
            table.Controls.Add(_nameTextBox, 1, 1);
            table.SetColumnSpan(_nameTextBox, 2);

            table.Controls.Add(categoryLabel, 0, 2);
            table.Controls.Add(typeNameLabel, 0, 3);

            table.Controls.Add(typeAbbrLabel, 0, 4);
            table.Controls.Add(_typeAbbrTextBox, 1, 4);

            table.Controls.Add(qtyLabel, 0, 5);
            table.Controls.Add(_qtyTextBox, 1, 5);



            FlowLayoutPanel flowLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
            };

            //To show Update Form
            if (forUpdateProduct)
            {
                _categoryComboBox = new ComboBox
                {
                    Dock = DockStyle.Fill,
                    Font = textBoxFont,
                    Margin = new Padding(10),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                _typeNameComboBox = new ComboBox
                {
                    Dock = DockStyle.Fill,
                    Font = textBoxFont,
                    Margin = new Padding(10),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                table.Controls.Add(_categoryComboBox, 1, 2);
                table.Controls.Add(_typeNameComboBox, 1, 3);

                Button _cancelButton = new Button
                {
                    Text = "Cancel",
                    Dock = DockStyle.None,
                    BackColor = Color.Blue,
                    Font = labelFont,
                    ForeColor = Color.White,
                    Height = 40,
                    Width = 100

                };

                Button _updateButton = new Button
                {
                    Text = "Update",
                    Dock = DockStyle.None,
                    BackColor = Color.Blue,
                    Font = labelFont,
                    ForeColor = Color.White,
                    Height = 40,
                    Width = 100

                };

                flowLayout.Controls.Add(_cancelButton);
                flowLayout.Controls.Add(_updateButton);

                _cancelButton.Click += (sender, e) => form.Close();
                _updateButton.Click += (sender, e) => UpdateProductFormEvent(form); ;

                form.CancelButton = _cancelButton;
            }
            //To show View Form
            else if (forViewProduct)
            {
                _categoryTextBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Font = U.StoreTextBoxFont,
                    Margin = new Padding(10),
                };

                _typeNameTextBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Font = U.StoreTextBoxFont,
                    Margin = new Padding(10),
                };

                table.Controls.Add(_categoryTextBox, 1, 2);
                table.Controls.Add(_typeNameTextBox, 1, 3);


                Button _okButton = new Button
                {
                    Text = "Ok",
                    Dock = DockStyle.None,
                    BackColor = Color.Blue,
                    Font = labelFont,
                    ForeColor = Color.White,
                    Height = 40,
                    Width = 100
                };

                _nameTextBox.ReadOnly = true;
                _categoryTextBox.ReadOnly = true;
                _typeNameTextBox.ReadOnly = true;
                _qtyTextBox.ReadOnly = true;

                _nameTextBox.BackColor = Color.White;
                _categoryTextBox.BackColor = Color.White;
                _typeNameTextBox.BackColor = Color.White;
                _qtyTextBox.BackColor = Color.White;



                flowLayout.Controls.Add(_okButton);
                flowLayout.Anchor = AnchorStyles.None; //to show button on center
                //Ok button event
                _okButton.Click += (sender, e) => form.Close();

                form.CancelButton = _okButton;
            }

            table.Controls.Add(flowLayout, 1, 7);

            _nameTextBox.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(_nameTextBox);
            _qtyTextBox.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;

            _qtyTextBox.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(_qtyTextBox, qtyPercentPlaceHolder);
            _qtyTextBox.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(_qtyTextBox, qtyPercentPlaceHolder);

            return table;
        }

        private void UpdateProductFormEvent(Form form)
        {
            string name = _nameTextBox.Text.Trim();
            string qty = _qtyTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Product name can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrWhiteSpace(qty))
            {
                MessageBox.Show("Total quantity number can't be empty or null.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var productType = (ProductType)_typeNameComboBox.SelectedItem;
            var category = (Category)_categoryComboBox.SelectedItem;

            if (_product.Name == name && _product.Category?.Id == category?.Id && _product.ProductType?.Id == productType?.Id && _product.TotalQty == float.Parse(qty))
            {
                MessageBox.Show("There is nothing to update.", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_product.Name != name)
            {
                if (productDao.IsRecordExists(name))
                {
                    MessageBox.Show("Product name is already exist.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            Product product = new Product(_product.Id, name, category, productType, float.Parse(qty));

            /*
            foreach (DataGridViewRow row in productTable.Rows)
            {
                // Set the value for Category.Name column
                if (row.DataBoundItem is Product product)
                {
                    row.Cells[2].Value = product.Category?.Name;
                    row.Cells[3].Value = product.ProductType?.Name;
                    row.Cells[4].Value = product.ProductType?.Abbr;
                }
            }
            */

            if (productDao.Update(product))
            {
                MessageBox.Show("Update successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                productList[productTable.CurrentCell.RowIndex] = product;

                DataGridViewRow row = productTable.Rows[productTable.CurrentCell.RowIndex];
                if (row.DataBoundItem is Product p)
                {
                    row.Cells[2].Value = p.Category?.Name;
                    row.Cells[3].Value = p.ProductType?.Name;
                    row.Cells[4].Value = p.ProductType?.Abbr;
                }
                // Or used below statement to update on table
                //BindProductToDataGridView();
                form.Close();
            }
            else
            {
                MessageBox.Show("Update failed.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

    }
}
