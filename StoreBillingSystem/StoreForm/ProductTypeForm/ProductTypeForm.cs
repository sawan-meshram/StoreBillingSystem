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
        private Label totalProductTypeLabel;

        private ProductType _productType;
        private bool _onUpdateProductType;

        private void InitializeComponent()
        {
            Text = "New / View Product Type";
            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(650, 640);
            BackColor = Util.U.StoreDialogBackColor;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 7,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 380F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-4
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-4

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 430F)); //name

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

            table.Controls.Add(productTypeTable, 0, 3);
            table.SetColumnSpan(productTypeTable, table.ColumnCount);

            table.Controls.Add(new Label
            {
                Text = "Total Types :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            }, 1, 4);

            totalProductTypeLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalProductTypeLabel, 2, 4);

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
                Text = "Edit",
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

            table.Controls.Add(footerPanel, 1, 5);


            //table.SetColumnSpan(productTypeTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            Controls.Add(table);

            BindProductTypeToDataGridView();


            // Attach event handlers
            addButton.Click += AddProductTypeButton_Click;
            clearButton.Click += (sender, e) => ClearProducTypeForm();
            cancelButton.Click += (sender, e) => Close();
            deleteButton.Click += DeleteButton_Click;
            updateButton.Click += UpdateButton_Click;

            // Attach the CellFormatting event handler
            //productTypeTable.CellFormatting += ProductTypeTable_CellFormatting;

            typeFullNameField.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(typeFullNameField);
            typeAbbrNameField.TextChanged += (sender, e) => TextBoxKeyEvent.UpperText_TextChanged(typeAbbrNameField);
            typeAbbrNameField.KeyPress += TextBoxKeyEvent.UppercaseTextBox_KeyPress;

        }

        private void UpdateProductTypeTotalCount()
        {
            totalProductTypeLabel.Text = productTypeList.Count.ToString();

        }
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            _productType = productTypeList[productTypeTable.CurrentCell.RowIndex];
            typeFullNameField.Text = _productType.Name;
            typeAbbrNameField.Text = _productType.Abbr;

            _onUpdateProductType = true;
        }


        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (productTypeTable.Rows.Count == 0) return;

            _productType = productTypeList[productTypeTable.CurrentCell.RowIndex];

            DialogResult result = MessageBox.Show($"Do you want to remove the selected Product Type with Id :{_productType.Id}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (!productTypeDao.Delete(_productType.Id))
                {
                    MessageBox.Show("Something occur while deletion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                productTypeList.RemoveAt(productTypeTable.CurrentCell.RowIndex);
                // Delete the row from the DataGridView
                productTypeTable.Rows.RemoveAt(productTypeTable.CurrentCell.RowIndex);

                if (productTypesComboBox != null)
                    BindProductTypeToComboBox();

                UpdateProductTypeTotalCount();
            }
        }

        private void ClearProducTypeForm()
        {
            typeFullNameField.Clear();
            typeAbbrNameField.Clear();
        }

        private void AddProductTypeButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(typeFullNameField.Text))
            {
                MessageBox.Show("Enter type full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(typeAbbrNameField.Text))
            {
                MessageBox.Show("Enter type abbrivation/short name for full name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = typeFullNameField.Text.Trim();
            string abbr = typeAbbrNameField.Text.Trim();

            ProductType productType = new ProductType(name, abbr);

            if (_onUpdateProductType)
            {
                if (_productType.Name == name && _productType.Abbr == abbr)
                {
                    MessageBox.Show("Nothing to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (!productTypeDao.IsRecordExists(name, abbr))
            {
                if (_onUpdateProductType)
                {
                    _productType.Name = productType.Name;
                    _productType.Abbr = productType.Abbr;
                    bool updated = productTypeDao.Update(_productType);
                    if (!updated)
                    {
                        MessageBox.Show("Something occur while updation.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    productTypeTable.Rows[productTypeTable.CurrentCell.RowIndex].Cells[1].Value = _productType.Abbr;
                    productTypeTable.Rows[productTypeTable.CurrentCell.RowIndex].Cells[2].Value = _productType.Name;
                }
                else
                {
                    bool inserted = productTypeDao.Insert(productType);
                    if (!inserted)
                    {
                        MessageBox.Show("Something occur while insertion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    productTypeTable.Rows.Add(productType.Id, productType.Abbr, productType.Name);
                    productTypeList.Add(productType);
                    UpdateProductTypeTotalCount();

                }
                //ProductType Combobox from Product Form
                if (productTypesComboBox != null)
                    BindProductTypeToComboBox();

                //categorySrNo++;
                ClearProducTypeForm();
                if (_onUpdateProductType)
                {
                    MessageBox.Show("Update successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _onUpdateProductType = false;
                }
                else
                {
                    MessageBox.Show("Insert successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Already exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void BindProductTypeToDataGridView()
        {
            //productTypeTable.DataSource = null;
            //productTypeTable.DataSource = productTypeList;
            productTypeTable.Rows.Clear();
            foreach (ProductType type in productTypeList)
            {
                productTypeTable.Rows.Add(type.Id, type.Abbr, type.Name);
            }
            UpdateProductTypeTotalCount();

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
