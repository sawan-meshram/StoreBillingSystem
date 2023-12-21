using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using StoreBillingSystem.Database;
using StoreBillingSystem.DAO;
using StoreBillingSystem.DAOImpl;
using StoreBillingSystem.Entity;
using StoreBillingSystem.Events;
namespace StoreBillingSystem.StoreForm.CategoryForm
{
    public class CategoryForm : Form
    {

        public CategoryForm()
        {
            var _categoryDao = new CategoryDaoImpl(DatabaseManager.GetConnection());

            Init(_categoryDao, _categoryDao.ReadAll(false), null);
        }

        public CategoryForm(ICategoryDao categoryDao, IList<Category> categoryList): this(categoryDao, categoryList, null)
        {
        }

        public CategoryForm(ICategoryDao categoryDao, IList<Category> categoryList, ComboBox categoryTypesComboBox)
        {
            Init(categoryDao, categoryList, categoryTypesComboBox);
        }

        private void Init(ICategoryDao _categoryDao, IList<Category> _categoryList, ComboBox _categoryTypesComboBox)
        {
            this.categoryDao = _categoryDao;
            this.categoryList = _categoryList;
            this.categoryTypesComboBox = _categoryTypesComboBox;
            InitializeComponent();
        }


        private Font labelFont = Util.U.StoreLabelFont;
        private Font textfieldFont = Util.U.StoreTextBoxFont;
        private IList<Category> categoryList;
        private ICategoryDao categoryDao;
        private ComboBox categoryTypesComboBox;


        private DataGridView categoryTable;
        private TextBox categoryNameField;
        private void InitializeComponent()
        {
            Text = "New / View Category";
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
            CancelButton = cancelButton;

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

            categoryTable.ReadOnly = true;
            categoryTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            categoryTable.MultiSelect = false;

            categoryTable.AllowUserToAddRows = false;
            categoryTable.AutoGenerateColumns = false;
            categoryTable.RowHeadersVisible = false;
            categoryTable.AllowUserToResizeRows = false;
            categoryTable.AllowUserToResizeColumns = false;

            //Binf list with datagridview
            categoryTable.Columns[0].DataPropertyName = "Id";
            categoryTable.Columns[1].DataPropertyName = "Name";
            //categoryTable.DataSource = categoryList;
            BindCategoryTypeToDataGridView();

            table.Controls.Add(categoryTable, 0, 3);

            table.Controls.Add(footerPanel, 1, 4);


            table.SetColumnSpan(categoryTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            Controls.Add(table);

            //add event
            addButton.Click += AddCategoryButton_Click;

            // Attach the CellFormatting event handler
            //categoryTable.CellFormatting += CategoryTable_CellFormatting;

            categoryNameField.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(categoryNameField);

        }

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

                //Category ComboBox from Product Form
                if(categoryTypesComboBox != null)
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

        private void CategoryTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell is in the first column (index 0) and not a header cell
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // Set the value to the current row index plus the initial serial number
                e.Value = e.RowIndex + 1;
                e.FormattingApplied = true;
            }
        }


    }
}
