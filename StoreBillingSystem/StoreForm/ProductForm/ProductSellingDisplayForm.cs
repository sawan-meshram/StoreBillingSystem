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
    public class ProductSellingDisplayForm : Form
    {
        public ProductSellingDisplayForm()
        {
            InitializeComponent();
            InitProductSellingDisplayFormEvent();
            InitializeProductData();
        }

        private IProductDao productDao;
        private ICategoryDao categoryDao;
        private IProductSellingDao productSellingDao;

        private IList<Product> productList;
        private IList<Category> categoryList;
        private IList<ProductSelling> productSellingList;

        private Product _product;
        private ProductSelling _productSelling;

        private Font labelFont = U.StoreLabelFont;
        private Font textfieldFont = Util.U.StoreTextBoxFont;

        private string _productNamePlaceHolder = U.ToTitleCase("Search Product Name Here..");
        private string _pricePlaceHolder = "0.00";

        private ComboBox categoryTypesComboBox;
        private DataGridView productTable;

        private Label totalProductLabel;
        private Label separator_1;

        private Button clearButton;
        private Button okButton;
        private Button viewButton;
        private Button deleteButton;
        private Button updateButton;
        private Button clearSellingButton;
        private Button saveButton;

        private TextBox productNameText;
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

        private void InitializeComponent()
        {
            Text = "Product & Selling Details";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(1200, 650);
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
                Font = labelFont,
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
            productTable = SetProductTable();
            table.Controls.Add(productTable, 0, 2);
            table.SetColumnSpan(productTable, 4);

            //Selling Forms
            TableLayoutPanel table1 = SetProductSellingForm();

            Panel panelWithBorder = new Panel();
            panelWithBorder.BorderStyle = BorderStyle.Fixed3D;
            panelWithBorder.Dock = DockStyle.Fill;
            panelWithBorder.Controls.Add(table1);

            table.Controls.Add(panelWithBorder, 4, 2);
            table.SetColumnSpan(panelWithBorder, 3);


            //row-3
            Label totalProductShowLabel = new Label
            {
                Text = "Total Product :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            };
            table.Controls.Add(totalProductShowLabel, 0, 3);

            totalProductLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalProductLabel, 1, 3);

            //Row-4
            TableLayoutPanel table2 = SetBottomButton();
            table.Controls.Add(table2, 1, 4);
            table.SetColumnSpan(table2, 4);

            //row-5
            //blank

            Controls.Add(table);


        }

        private DataGridView SetProductTable()
        {
            DataGridView gridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.LightGray,
                Margin = new Padding(0),
                ScrollBars = ScrollBars.Vertical,
            };
            gridView.RowHeadersDefaultCellStyle.Font = U.StoreLabelFont;

            gridView.ColumnCount = 3;
            gridView.Columns[0].Name = "Id";
            gridView.Columns[1].Name = "Name";
            gridView.Columns[2].Name = "Total Qty";


            gridView.Columns[0].Width = 100;
            gridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridView.Columns[2].Width = 150;


            gridView.Columns[0].DataPropertyName = "Id";
            gridView.Columns[1].DataPropertyName = "Name";
            gridView.Columns[2].DataPropertyName = "TotalQty";
            gridView.Columns[2].DefaultCellStyle.Format = "N";


            gridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; //data display in center
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //header display in center

            gridView.ReadOnly = true;
            gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridView.MultiSelect = false;


            foreach (DataGridViewColumn column in gridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            gridView.AllowUserToAddRows = false;
            gridView.AutoGenerateColumns = false;
            gridView.RowHeadersVisible = false;
            gridView.AllowUserToResizeRows = false;
            gridView.AllowUserToResizeColumns = false;

            return gridView;
        }

        private TableLayoutPanel SetBottomButton()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 290)); //name

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

            table.Controls.Add(updateButton, 1, 0);
            table.Controls.Add(deleteButton, 2, 0);
            table.Controls.Add(viewButton, 3, 0);
            table.Controls.Add(okButton, 4, 0);
            return table;
        }

        private TableLayoutPanel SetProductSellingForm()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 16,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); //Blank
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F)); //Blank
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F)); //Blank
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230)); //name

            Label sellingTitleLabel = new Label
            {
                Text = "Product Selling Details",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Dock = DockStyle.Fill,
                ForeColor = Color.Blue,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            table.Controls.Add(sellingTitleLabel, 0, 0);
            table.SetColumnSpan(sellingTitleLabel, table.ColumnCount);

            separator_1 = new Label
            {
                AutoSize = false,
                Height = 2,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.Fixed3D,
                ForeColor = Color.Blue,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            table.Controls.Add(separator_1, 0, 1);
            table.SetColumnSpan(separator_1, table.ColumnCount);

            //Selling Price A
            //Discount Price A
            table.Controls.Add(new Label
            {
                Text = "Selling Price - A (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 1, 2);

            table.Controls.Add(new Label
            {
                Text = "Discount - A (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 3, 2);

            sellingPriceText_1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(sellingPriceText_1, 1, 3);
            discountText_1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(discountText_1, 3, 3);


            //Row-9
            //Selling Price B
            //Discount Price B
            table.Controls.Add(new Label
            {
                Text = "Selling Price - B (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 1, 4);
            table.Controls.Add(new Label
            {
                Text = "Discount - B (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 3, 4);

            sellingPriceText_2 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(sellingPriceText_2, 1, 5);

            discountText_2 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(discountText_2, 3, 5);

            //Row-10
            //Selling Price C
            //Discount Price C
            table.Controls.Add(new Label
            {
                Text = "Selling Price - C (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 1, 6);
            table.Controls.Add(new Label
            {
                Text = "Discount - C (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 3, 6);

            sellingPriceText_3 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(sellingPriceText_3, 1, 7);

            discountText_3 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(discountText_3, 3, 7);

            //Row-11
            //Selling Price D
            //Discount Price D
            table.Controls.Add(new Label
            {
                Text = "Selling Price - D (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 1, 8);
            table.Controls.Add(new Label
            {
                Text = "Discount - D (Rs.) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 3, 8);

            sellingPriceText_4 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(sellingPriceText_4, 1, 9);

            discountText_4 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(discountText_4, 3, 9);

            //Row-12
            //Horizontal Separator
            /*

            */
            //Row-13
            //CGST
            table.Controls.Add(new Label
            {
                Text = "CGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 1, 11);
            //SGST
            table.Controls.Add(new Label
            {
                Text = "SGST (%) :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.BottomLeft,
            }, 3, 11);

            cgstText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(cgstText, 1, 12);

            sgstText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5),
                Text = _pricePlaceHolder,
                ForeColor = Color.Gray
            };
            table.Controls.Add(sgstText, 3, 12);


            clearSellingButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Fill,
                Anchor = AnchorStyles.Right,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Height = 35,
                Width = 80,
                Margin = new Padding(5)
            };
            table.Controls.Add(clearSellingButton, 1, 14);

            saveButton = new Button
            {
                Text = "Save",
                Dock = DockStyle.Fill,
                Anchor = AnchorStyles.Left,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Height = 35,
                Width  = 80,
                Margin = new Padding(5)
            };

            table.Controls.Add(saveButton, 3, 14);
            return table;
        }

        private void InitProductSellingDisplayFormEvent()
        {
            //Numeric text on text box
            sellingPriceText_1.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            sellingPriceText_2.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            sellingPriceText_3.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            sellingPriceText_4.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            discountText_1.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            discountText_2.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            discountText_3.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            discountText_4.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            cgstText.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;
            sgstText.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;

            // placeholder on text box
            productNameText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(productNameText, _productNamePlaceHolder);
            productNameText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(productNameText, _productNamePlaceHolder);


            sellingPriceText_1.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_1, _pricePlaceHolder);
            sellingPriceText_1.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_1, _pricePlaceHolder);
            sellingPriceText_2.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_2, _pricePlaceHolder);
            sellingPriceText_2.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_2, _pricePlaceHolder);
            sellingPriceText_3.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_3, _pricePlaceHolder);
            sellingPriceText_3.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_3, _pricePlaceHolder);
            sellingPriceText_4.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sellingPriceText_4, _pricePlaceHolder);
            sellingPriceText_4.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sellingPriceText_4, _pricePlaceHolder);

            discountText_1.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_1, _pricePlaceHolder);
            discountText_1.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_1, _pricePlaceHolder);
            discountText_2.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_2, _pricePlaceHolder);
            discountText_2.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_2, _pricePlaceHolder);
            discountText_3.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_3, _pricePlaceHolder);
            discountText_3.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_3, _pricePlaceHolder);
            discountText_4.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(discountText_4, _pricePlaceHolder);
            discountText_4.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(discountText_4, _pricePlaceHolder);

            cgstText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(cgstText, _pricePlaceHolder);
            cgstText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(cgstText, _pricePlaceHolder);
            sgstText.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(sgstText, _pricePlaceHolder);
            sgstText.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(sgstText, _pricePlaceHolder);

            //Capitalise text on text box
            productNameText.TextChanged += SearchCustomerTextBox_TextChanged;
            categoryTypesComboBox.SelectedIndexChanged += CategoryTypesComboBox_SelectedIndexChanged;

            okButton.Click += OkButton_Click;
            viewButton.Click += ViewButton_Click;
            deleteButton.Click += DeleteButton_Click;
            updateButton.Click += UpdateButton_Click;
            clearButton.Click += ClearButton_Click;
            clearSellingButton.Click += ClearSellingButton_Click;
            saveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ClearSellingButton_Click(object sender, EventArgs e)
        {
            TextBoxKeyEvent.BindPlaceholderToTextBox(sellingPriceText_1, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(discountText_1, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(sellingPriceText_2, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(discountText_2, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(sellingPriceText_3, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(discountText_3, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(sellingPriceText_4, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(discountText_4, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(cgstText, _pricePlaceHolder, Color.Gray);
            TextBoxKeyEvent.BindPlaceholderToTextBox(sgstText, _pricePlaceHolder, Color.Gray);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            TextBoxKeyEvent.BindPlaceholderToTextBox(productNameText, _productNamePlaceHolder, Color.Gray);
            categoryTypesComboBox.SelectedIndex = 0;
            InitializeProductData();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            _product = productList[productTable.CurrentCell.RowIndex];
            _productSelling = productSellingDao.Read(_product);

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
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

            productSellingDao = new ProductSellingDaoImpl(DatabaseManager.GetConnection());
            productSellingList = productSellingDao.ReadAll();
        }

        private void UpdateProductCountAtLabel(int count)
        {
            totalProductLabel.Text = count.ToString();
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




    }
}
