using System;
using System.Drawing;
using System.Windows.Forms;
namespace StoreBillingSystem
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.Run(new MainClass());
            // Console.WriteLine("Hello World!");

            Application.Run(new MainForm());

            //Application.Run(new BillingForm());
            //Application.Run(new AddCustomer());
            //Application.Run(new ProductForm());

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
