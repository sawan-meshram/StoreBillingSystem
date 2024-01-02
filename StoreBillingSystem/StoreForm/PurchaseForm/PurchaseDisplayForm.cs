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
namespace StoreBillingSystem.StoreForm.PurchaseForm
{
    public class PurchaseDisplayForm : Form
    {
        public PurchaseDisplayForm()
        {
            InitializeComponent();

        }


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
        private DataGridView productTable;
        private DateTimePicker fromDate;
        private DateTimePicker ToDate;

        private void InitializeComponent()
        {
            Text = "Product History";

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
                //Text = _productNamePlaceHolder,
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
    }
}
