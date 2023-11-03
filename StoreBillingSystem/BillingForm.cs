﻿using System;
using System.Windows.Forms;
using System.Drawing;
namespace StoreBillingSystem
{
    public class BillingForm : Form
    {
        private DataGridView dataGridView;
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

        }


        private TextBox billingNumText;
        private TextBox customerNameText;
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
                ColumnCount = 6,
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F)); 
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

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

            dateTimePicker = new DateTimePicker
            {
                //CustomFormat = "yyyy-MM-dd HH:mm:ss",
                Format = DateTimePickerFormat.Short,
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(dateTimePicker, 4, 0);

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
                Margin = new Padding(5)
            };
            panel.Controls.Add(customerNameText, 1, 1);
            panel.SetColumnSpan(customerNameText, 2);
            return panel;

        }


        private TextBox productNameText;
        private TextBox searchCodeText;
        private TextBox askingPriceText;
        private TextBox qtyText;
        private Button clearProductButton;
        private Button addProductButton;
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
                Margin = new Padding(5)
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

            searchCodeText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = textfieldFont,
                Margin = new Padding(5)
            };
            panel.Controls.Add(searchCodeText, 5, 0);

            //Clear Button
            clearProductButton = new Button
            {
                Text = "Clear",
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
                Margin = new Padding(5)
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
                Margin = new Padding(5)
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
                Margin = new Padding(5)
            };
            panel.Controls.Add(askingPriceText,5, 1);

            addProductButton = new Button
            {
                Text = "Add",
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            panel.Controls.Add(addProductButton, 6, 1);
            box.Controls.Add(panel);
            return box;
        }

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
            dataGridView = new DataGridView
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
            dataGridView.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            //dataGridView.ColumnHeadersDefaultCellStyle.Font = textfieldFont;

            //Console.WriteLine(dataGridView.Location.X + " " + dataGridView.Location.Y);
            // Create columns for the DataGridView
            dataGridView.ColumnCount = 8;
            dataGridView.Columns[0].Name = "Sr.No.";
            dataGridView.Columns[1].Name = "Product Name";
            dataGridView.Columns[2].Name = "Type";
            dataGridView.Columns[3].Name = "Qty.";
            dataGridView.Columns[4].Name = "Amount";
            dataGridView.Columns[5].Name = "GST(%)";
            dataGridView.Columns[6].Name = "Discount(%)";
            dataGridView.Columns[7].Name = "Total";


            dataGridView.Columns[0].Width = 80;
            dataGridView.Columns[1].Width = 400;
            dataGridView.Columns[2].Width = 80;
            dataGridView.Columns[3].Width = 100;
            dataGridView.Columns[4].Width = 150;
            dataGridView.Columns[5].Width = 100;
            dataGridView.Columns[6].Width = 100;
            dataGridView.Columns[7].Width = 150;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView.Rows.Add("1", "Daal", "KG", "1", "100.00", "5", "0", "100.00");
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
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.RowHeadersVisible = false;

            panel.Controls.Add(dataGridView);
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
            amtTable.ColumnCount = 8;
            amtTable.Columns[0].Width = 80;
            amtTable.Columns[1].Width = 400;
            amtTable.Columns[2].Width = 80;
            amtTable.Columns[3].Width = 100;
            amtTable.Columns[4].Width = 150;
            amtTable.Columns[5].Width = 100;
            amtTable.Columns[6].Width = 100;
            amtTable.Columns[7].Width = 150;
            amtTable.Rows.Add("", "Total (Rs.)", "", "", "100.00", "0", "0", "100.00");
            amtTable.ColumnHeadersVisible = false;
            amtTable.AllowUserToAddRows = false;
            amtTable.AutoGenerateColumns = false;
            amtTable.RowHeadersVisible = false;

            //amtTable.RowHeadersDefaultCellStyle.Font = new Font("Arial", 10);
            //amtTable.ColumnHeadersDefaultCellStyle.Font = textfieldFont;
            amtTable.Rows[0].DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold); // Replace "Arial" with your desired font



            panel.Controls.Add(amtTable);
            return panel;
        }


    }
}