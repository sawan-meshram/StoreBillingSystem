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
        private Label totalCategoryLabel;

        private bool _onUpdateCategory = false;
        private Category _category;


        private void InitializeComponent()
        {
            Text = "New / View Category";
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
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 380F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-4
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); //row-4


            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); //name
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 430F)); //name

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

            table.Controls.Add(categoryTable, 0, 3);
            table.SetColumnSpan(categoryTable, table.ColumnCount);

            table.Controls.Add(new Label
            {
                Text = "Total Category :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleRight,
            }, 1, 4);

            totalCategoryLabel = new Label
            {
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Crimson,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            table.Controls.Add(totalCategoryLabel, 2, 4);

            TableLayoutPanel footerPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                //BackColor = Color.Lime,
            };

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


            //table.SetColumnSpan(categoryTable, 2);
            //table.SetColumnSpan(footerPanel, 2);


            Controls.Add(table);

            BindCategoryTypeToDataGridView();


            //add event
            cancelButton.Click += (sender, e) => Close();
            clearButton.Click += ClearButton_Click;
            addButton.Click += AddCategoryButton_Click;
            updateButton.Click += UpdateButton_Click;
            deleteButton.Click += DeleteButton_Click;
            // Attach the CellFormatting event handler
            //categoryTable.CellFormatting += CategoryTable_CellFormatting;

            categoryNameField.TextChanged += (sender, e) => TextBoxKeyEvent.CapitalizeText_TextChanged(categoryNameField);

        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            categoryNameField.Text = "";
        }

        private void UpdateCategoryTotalCount()
        {
            if (totalCategoryLabel == null) Console.WriteLine("label");
            if (categoryList == null) Console.WriteLine("list");
            totalCategoryLabel.Text = categoryList.Count.ToString();
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (categoryTable.Rows.Count == 0) return;

            _category = categoryList[categoryTable.CurrentCell.RowIndex];

            DialogResult result = MessageBox.Show($"Do you want to remove the selected Category with Id :{_category.Id}?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (!categoryDao.Delete(_category.Id))
                {
                    MessageBox.Show("Something occur while deletion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                categoryList.RemoveAt(categoryTable.CurrentCell.RowIndex);
                // Delete the row from the DataGridView
                categoryTable.Rows.RemoveAt(categoryTable.CurrentCell.RowIndex);

                if (categoryTypesComboBox != null)
                    BindCategoryTypeToComboBox();

                UpdateCategoryTotalCount();
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            _category = categoryList[categoryTable.CurrentCell.RowIndex];
            categoryNameField.Text = _category.Name;
            _onUpdateCategory = true;
        }

        private void AddCategoryButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(categoryNameField.Text))
            {
                // Validation failed, display a message box
                MessageBox.Show("Field is empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = categoryNameField.Text.Trim();
            Category category = new Category(name);
            if (_onUpdateCategory)
            {
                if(_category.Name == name)
                {
                    MessageBox.Show("Nothing to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (!categoryDao.IsRecordExists(name))
            {

                if (_onUpdateCategory)
                {
                    _category.Name = category.Name;
                    bool updated = categoryDao.Update(_category);
                    if (!updated)
                    {
                        MessageBox.Show("Something occur while updation.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    categoryTable.Rows[categoryTable.CurrentCell.RowIndex].Cells[1].Value = _category.Name;
                }
                else
                {
                    bool inserted = categoryDao.Insert(category);

                    if (!inserted)
                    {
                        MessageBox.Show("Something occur while insertion.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    categoryTable.Rows.Add(category.Id, category.Name);
                    categoryList.Add(category);
                    UpdateCategoryTotalCount();
                }


                //categoryTable.Rows.Add(category.Id, category.Name);
                //Datagridview
                /*
                BindCategoryTypeToDataGridView();
                */
                //Category ComboBox from Product Form
                if(categoryTypesComboBox != null)
                    BindCategoryTypeToComboBox();
                
                //categorySrNo++;
                categoryNameField.Clear();
                if (_onUpdateCategory)
                {
                    MessageBox.Show("Update successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _onUpdateCategory = false;
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


        private void BindCategoryTypeToDataGridView()
        {
            //categoryTable.DataSource = null;
            //categoryTable.DataSource = categoryList;
            categoryTable.Rows.Clear();
            foreach(Category category in categoryList)
            {
                categoryTable.Rows.Add(category.Id, category.Name);
            }
            UpdateCategoryTotalCount();
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
