using System;
using System.Windows.Forms;
using System.Drawing;

namespace StoreBillingSystem
{

    public class MainForm : Form
    {
        private TabControl tabControl;
        private TabPage customerRegistrationTab;
        private TabPage billingTab;
        private TabPage productTab;

        private BillingForm billingForm;
        private AddCustomer registrationForm;
        private ProductForm productForm;
        public MainForm()
        {
            this.Text = "Main Form";
            this.Size = new Size(1366, 780);

            InitComponents();

            InitForms();
            // Add RegistrationForm to the Registration tab
            customerRegistrationTab.Controls.Add(registrationForm);
            billingTab.Controls.Add(billingForm);
            productTab.Controls.Add(productForm);

            for (int index = 0; index < tabControl.TabCount; index++)
            {
                // Check if the TabPage contains any controls
                if (tabControl.TabPages.Count > 0 && tabControl.TabPages[index].Controls.Count > 0)
                {
                    Control control = tabControl.TabPages[index].Controls[0]; // Access the first control in the first tab page

                    if (control is Form)
                    {
                        Form formInTabControl = control as Form;
                        formInTabControl.FormBorderStyle = FormBorderStyle.None;
                    }
                }
            }

            billingForm.Show();
            registrationForm.Show();
            productForm.Show();

        }

        private void InitForms()
        {
            registrationForm = new AddCustomer();
            registrationForm.TopLevel = false;
            registrationForm.Dock = DockStyle.Fill;

            billingForm = new BillingForm();
            billingForm.TopLevel = false;
            billingForm.Dock = DockStyle.Fill;

            productForm = new ProductForm();
            productForm.TopLevel = false;
            productForm.Dock = DockStyle.Fill;
        }

        private void InitComponents()
        {

            //this.Size = new System.Drawing.Size(1366, 768);

            // Create tab control
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // Create tabs
            billingTab = new TabPage();
            billingTab.Text = "Billing";
            tabControl.TabPages.Add(billingTab);

            customerRegistrationTab = new TabPage();
            customerRegistrationTab.Text = "Customer";
            tabControl.TabPages.Add(customerRegistrationTab);

            productTab = new TabPage();
            productTab.Text = "Product";
            tabControl.TabPages.Add(productTab);



            billingTab.Click += new EventHandler(BillingMenuItem_Click);
            // Add tab control to the main form
            this.Controls.Add(tabControl);




            // Create menu items
            MainMenu mainMenu = new MainMenu();
            MenuItem fileMenuItem = new MenuItem("File");
            MenuItem registrationMenuItem = new MenuItem("Open Registration Form");
            MenuItem billingMenuItem = new MenuItem("Open Billing Form");

            // Add menu items to the menu
            fileMenuItem.MenuItems.Add(registrationMenuItem);
            fileMenuItem.MenuItems.Add(billingMenuItem);
            mainMenu.MenuItems.Add(fileMenuItem);
            this.Menu = mainMenu;

            // Event handlers for menu items
            //registrationMenuItem.Click += new EventHandler(RegistrationMenuItem_Click);
            //billingMenuItem.Click += new EventHandler(BillingMenuItem_Click);
        }

        private void RegistrationMenuItem_Click(object sender, EventArgs e)
        {
            // Open Registration Form in the "Registration" tab
            CustomerRegistrationForm registrationForm = new CustomerRegistrationForm();
            registrationForm.TopLevel = false;
            registrationForm.Dock = DockStyle.Fill;
            customerRegistrationTab.Controls.Add(registrationForm);
            registrationForm.Show();
        }

        private void BillingMenuItem_Click(object sender, EventArgs e)
        {
            // Open Billing Form in the "Billing" tab
            BillingForm billingForm = new BillingForm();
            billingForm.TopLevel = false;
            billingForm.Dock = DockStyle.Fill;
            billingTab.Controls.Add(billingForm);
            billingForm.Show();
        }
    }
}
