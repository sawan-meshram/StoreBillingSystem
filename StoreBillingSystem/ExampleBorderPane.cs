using System;
using System.Windows.Forms;

namespace StoreBillingSystem
{
    public class ExampleBorderPane : Form
    {


        private Panel topPanel;
        private Panel bottomPanel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel centerPanel;

        public ExampleBorderPane()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Create the main form
            this.Text = "BorderPane Example";
            this.Size = new System.Drawing.Size(800, 600);

            // Create panels for each region
            topPanel = new Panel();
            topPanel.BackColor = System.Drawing.Color.LightBlue;
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 50;

            bottomPanel = new Panel();
            bottomPanel.BackColor = System.Drawing.Color.LightGreen;
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 50;

            leftPanel = new Panel();
            leftPanel.BackColor = System.Drawing.Color.LightYellow;
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Width = 100;

            rightPanel = new Panel();
            rightPanel.BackColor = System.Drawing.Color.LightCoral;
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 100;

            centerPanel = new Panel();
            centerPanel.BackColor = System.Drawing.Color.White;
            centerPanel.Dock = DockStyle.Fill;


            // Add panels to the form
            this.Controls.Add(topPanel);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(leftPanel);

            this.Controls.Add(rightPanel);
            this.Controls.Add(centerPanel);
        }
    }
}
