using System;
using System.Drawing;
using System.Windows.Forms;
using StoreBillingSystem.Database;
using StoreBillingSystem.StoreForm.CategoryForm;
using StoreBillingSystem.StoreForm.ProductTypeForm;
using StoreBillingSystem.StoreForm.CustomerForm;
using StoreBillingSystem.StoreForm.ProductForm;
using StoreBillingSystem.StoreForm.PurchaseForm;
namespace StoreBillingSystem
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {

            //Open sqlite connection
            DatabaseManager.GetConnection();
            Console.WriteLine("Database connection established .....");

            StoreDbTableManager manager = StoreDbTableManager.Instance;
            //Application.EnableVisualStyles();
            //Application.Run(new MainClass());
            // Console.WriteLine("Hello World!");

            //Application.Run(new MainForm());

            //Application.Run(new BillingForm());
            //Application.Run(new BillingForm1());
            //Application.Run(new AddCustomer());
            //Application.Run(new ProductForm());
            //Application.Run(new CategoryForm());
            //Application.Run(new ProductTypeForm());

            //Application.Run(new CustomerDisplayForm());
            //Application.Run(new ProductDisplayForm());
            //Application.Run(new CustomerInsertForm());
            //Application.Run(new ProductSellingDisplayForm());
            Application.Run(new PurchaseHistoryForm());


            //Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            //Console.WriteLine("Project Path: " + Application.StartupPath);



        }

        // Make sure to close the connection when the form is closing or when it's no longer needed.
        public static void ProductForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //closed sqlite connection
            DatabaseManager.CloseConnection();
            Console.WriteLine("Database connection closed.");

            // Your logic to execute when the form is closed.
            //MessageBox.Show("Form is closed!");
        }
    }

    class MainClass : Form
    {
        private FlowLayoutPanel flowPanel;

        public MainClass()
        {
            InitComponents();
        }
        private void InitComponents()
        {
            Text = "Billing Application";
            ClientSize = new Size(800, 450);

            flowPanel = new FlowLayoutPanel();
            /*
            var ftip = new ToolTip();
            ftip.SetToolTip(flowPanel, "This is a FlowLayoutPanel");

            flowPanel.Dock = DockStyle.Fill;
            flowPanel.BorderStyle = BorderStyle.FixedSingle;

            var button = new Button();
            button.Text = "Button";
            button.AutoSize = true;

            var btip = new ToolTip();
            btip.SetToolTip(button, "This is a Button Control");

            var button2 = new Button();
            button2.Text = "Button 2";
            button2.AutoSize = true;

            flowPanel.Controls.Add(button);
            flowPanel.Controls.Add(button2);
            */

            flowPanel.Dock = DockStyle.Fill;
            flowPanel.BorderStyle = BorderStyle.FixedSingle;


            var button = new Button();
            button.Margin = new Padding(10, 10, 0, 0);

            button.Text = "Quit";
            button.AutoSize = true;
            button.Click += ExitApplication; //(sender, e) => Close();

            flowPanel.Controls.Add(button);



            Controls.Add(flowPanel);

            CenterToScreen();
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Close();
        }


    }


}
