using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using StoreBillingSystem.Database;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Events;
namespace StoreBillingSystem.StoreForm.ProductTypeForm
{
    public class ProductTypeForm : Form
    {

        public ProductTypeForm()
        {
            var _productTypeDao = new ProductTypeDaoImpl(DatabaseManager.GetConnection());

            Init(_productTypeDao, _productTypeDao.ReadAll(false), null);
        }

        public ProductTypeForm(IProductTypeDao productTypeDao, IList<ProductType> productTypeList): this(productTypeDao, productTypeList, null)
        {
        }

        public ProductTypeForm(IProductTypeDao productTypeDao, IList<ProductType> productTypeList, ComboBox productTypesComboBox)
        {
            Init(productTypeDao, productTypeList, productTypesComboBox);
        }

        private void Init(IProductTypeDao _productTypeDao, IList<ProductType> _productTypeList, ComboBox _productTypesComboBox)
        {
            this.productTypeDao = _productTypeDao;
            this.productTypeList = _productTypeList;
            this.productTypesComboBox = _productTypesComboBox;
            InitializeComponent();
        }

        private IList<ProductType> productTypeList;
        private IProductTypeDao productTypeDao;
        private ComboBox productTypesComboBox;


        private Font labelFont = Util.U.StoreLabelFont;
        private Font textfieldFont = Util.U.StoreTextBoxFont;

        private DataGridView productTypeTable;
        private TextBox typeFullNameField;
        private TextBox typeAbbrNameField;
        private void InitializeComponent()
        {
            Text = "New / View Product Type";
            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(600, 600);
            BackColor = Util.U.StoreDialogBackColor;


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
            CancelButton = cancelButton;

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
            productTypeTable.ReadOnly = true;
            productTypeTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            productTypeTable.MultiSelect = false;

            productTypeTable.AllowUserToAddRows = false;
            productTypeTable.AutoGenerateColumns = false;
            productTypeTable.RowHeadersVisible = false;
            productTypeTable.AllowUserToResizeRows = false;
            productTypeTable.AllowUserToResizeColumns = false;


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


            Controls.Add(table);

            // Attach event handlers
            addButton.Click += AddProductTypeButton_Click;

            // Attach the CellFormatting event handler
            //productTypeTable.CellFormatting += ProductTypeTable_CellFormatting;

            typeFullNameField.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(typeFullNameField);
            typeAbbrNameField.TextChanged += (sender, e) => TextBoxKeyEvent.UpperText_TextChanged(typeAbbrNameField);
            typeAbbrNameField.KeyPress += TextBoxKeyEvent.UppercaseTextBox_KeyPress;

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

            if (!productTypeDao.IsRecordExists(name, abbr))
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

                //ProductType Combobox from Product Form
                if(productTypesComboBox != null)
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

        private int productTypeSrNo = 1;
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

    }
}
