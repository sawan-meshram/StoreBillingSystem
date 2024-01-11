using System;
using System.Windows.Forms;
using System.Drawing;

using StoreBillingSystem.StoreForm.CustomerForm;
using StoreBillingSystem.StoreForm.ProductForm;
using StoreBillingSystem.StoreForm.CategoryForm;
using StoreBillingSystem.StoreForm.ProductTypeForm;
using StoreBillingSystem.StoreForm.PurchaseForm;
using StoreBillingSystem.StoreForm.SalesForm;
using StoreBillingSystem.StoreForm.PaymentForm;
namespace StoreBillingSystem
{

    public class MainForm : Form
    {
        private TabControl tabControl;
        //private TabPage customerRegistrationTab;
        private TabPage billingTab;
        private TabPage productTab;

        private StoreForm.BillingForm.BillingForm billingForm;
        private AddCustomer registrationForm;
        private ProductForm productForm;
        public MainForm()
        {
            this.Text = "Main Form";
            //this.Size = new Size(1366, 780);
            //this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            this.MinimumSize = new Size(1366, 768); // Set your minimum size
            //this.MaximumSize = new Size(1366, 768); // Set your maximum size
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.

            InitMenus();
            InitTabs();
            InitForms();

            // Add RegistrationForm to the Registration tab
            //customerRegistrationTab.Controls.Add(registrationForm);
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

            billingForm = new StoreForm.BillingForm.BillingForm();
            billingForm.TopLevel = false;
            billingForm.Dock = DockStyle.Fill;

            productForm = new ProductForm();
            productForm.TopLevel = false;
            productForm.Dock = DockStyle.Fill;
        }

        private void InitTabs()
        {
            // Create tab control
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // Create tabs
            billingTab = new TabPage();
            billingTab.Text = "Billing";
            tabControl.TabPages.Add(billingTab);

            /*
            customerRegistrationTab = new TabPage();
            customerRegistrationTab.Text = "Customer";
            tabControl.TabPages.Add(customerRegistrationTab);
            */

            productTab = new TabPage();
            productTab.Text = "Product";
            tabControl.TabPages.Add(productTab);



            billingTab.Click += new EventHandler(BillingMenuItem_Click);
            // Add tab control to the main form
            this.Controls.Add(tabControl);
        }


        private MenuStrip mainMenu;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem customerMenu;
        private ToolStripMenuItem productMenu;
        private ToolStripMenuItem categoryMenu;
        private ToolStripMenuItem productTypeMenu;
        private ToolStripMenuItem salesMenu;
        private ToolStripMenuItem purchaseMenu;
        private ToolStripMenuItem paymentMenu;

        private void InitMenus()
        {
            mainMenu = new MenuStrip();
            /*
             * File Menu           
             */
            fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem exitFileMenu = new ToolStripMenuItem("Exit");
            fileMenu.DropDownItems.Add(exitFileMenu);

            exitFileMenu.Click += ExitFileMenu_Click;

            /*
             * Customer Menu
             */
            customerMenu = new ToolStripMenuItem("Customer");
            ToolStripMenuItem customerNewMenu = new ToolStripMenuItem("New");
            ToolStripMenuItem customerViewMenu = new ToolStripMenuItem("View");
            customerMenu.DropDownItems.Add(customerNewMenu);
            customerMenu.DropDownItems.Add(customerViewMenu);

            customerNewMenu.Click += (sender, e) => CustomerNewMenu();
            customerViewMenu.Click += (sender, e) => CustomerViewMenu();

            /*
             * Product Menu
             */
            productMenu = new ToolStripMenuItem("Product");
            ToolStripMenuItem productViewMenu = new ToolStripMenuItem("View / Edit Product");
            ToolStripMenuItem productSellingViewMenu = new ToolStripMenuItem("View / Edit Selling Price");
            productMenu.DropDownItems.Add(productViewMenu);
            productMenu.DropDownItems.Add(productSellingViewMenu);

            productViewMenu.Click += (sender, e) => ProductViewMenu();
            productSellingViewMenu.Click += (sender, e) => ProductSellingViewEditMenu();

            /*
             * Category Menu
             */
            categoryMenu = new ToolStripMenuItem("Category");
            ToolStripMenuItem categoryNewAndViewMenu = new ToolStripMenuItem("New / View");
            categoryMenu.DropDownItems.Add(categoryNewAndViewMenu);

            categoryNewAndViewMenu.Click += (sender, e) => CategoryNewAndViewMenu();


            /*
             * ProductType Menu
             */
            productTypeMenu = new ToolStripMenuItem("Product Type");
            ToolStripMenuItem productTypeNewAndViewMenu = new ToolStripMenuItem("New / View");
            productTypeMenu.DropDownItems.Add(productTypeNewAndViewMenu);

            productTypeNewAndViewMenu.Click += (sender, e) => ProductTypeNewAndViewMenu();

            /*
             * Sales Menu
             */
            salesMenu = new ToolStripMenuItem("Sales");
            ToolStripMenuItem salesHistoryMenu = new ToolStripMenuItem("View Sales History");
            salesMenu.DropDownItems.Add(salesHistoryMenu);

            salesHistoryMenu.Click += (sender, e) => SalesHistoryViewMenu();

            /*
             * Purchase Menu
             */
            purchaseMenu = new ToolStripMenuItem("Purchase");
            ToolStripMenuItem purchaseHistoryMenu = new ToolStripMenuItem("View Purchase History");
            ToolStripMenuItem productPurchaseHistoryMenu = new ToolStripMenuItem("View Product Purchase History");

            purchaseMenu.DropDownItems.Add(purchaseHistoryMenu);
            purchaseMenu.DropDownItems.Add(productPurchaseHistoryMenu);

            purchaseHistoryMenu.Click += (sender, e) => PurchaseHistoryViewMenu();
            productPurchaseHistoryMenu.Click += (sender, e) => ProductPurchaseHistoryViewMenu();

            /*
             * Payment Menu
             */
            paymentMenu = new ToolStripMenuItem("Payment");
            ToolStripMenuItem paymentHistoryMenu = new ToolStripMenuItem("View Payment History");
            ToolStripMenuItem balanceHistoryMenu = new ToolStripMenuItem("View Balance History");
            paymentMenu.DropDownItems.Add(paymentHistoryMenu);
            paymentMenu.DropDownItems.Add(balanceHistoryMenu);

            paymentHistoryMenu.Click += (sender, e) => PaymentHistoryViewMenu();
            balanceHistoryMenu.Click += (sender, e) => BalanceHistoryViewMenu();
            /*
             * Add all menus to menu bar
             */
            mainMenu.Items.Add(fileMenu);
            mainMenu.Items.Add(customerMenu);
            mainMenu.Items.Add(productMenu);
            mainMenu.Items.Add(categoryMenu);
            mainMenu.Items.Add(productTypeMenu);
            mainMenu.Items.Add(salesMenu);
            mainMenu.Items.Add(purchaseMenu);
            mainMenu.Items.Add(paymentMenu);

            this.MainMenuStrip = mainMenu;
            Controls.Add(mainMenu);
        }

        private void ExitFileMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CustomerNewMenu()
        {
            new CustomerInsertForm().ShowDialog();
        }

        private void CustomerViewMenu()
        {
            new CustomerDisplayForm().ShowDialog();
        }

        private void ProductViewMenu()
        {
            new ProductDisplayForm().ShowDialog();
        }

        private void ProductSellingViewEditMenu()
        {
            new ProductSellingDisplayForm().ShowDialog();
        }

        private void CategoryNewAndViewMenu()
        {
            new CategoryForm().ShowDialog();
        }
        private void ProductTypeNewAndViewMenu()
        {
            new ProductTypeForm().ShowDialog();
        }
        private void PurchaseHistoryViewMenu()
        {
            new PurchaseHistoryForm().ShowDialog();
        }

        private void ProductPurchaseHistoryViewMenu()
        {
            new ProductPurchaseHistoryForm().ShowDialog();
        }

        private void SalesHistoryViewMenu()
        {
            new SalesHistoryForm().ShowDialog();
        }

        private void PaymentHistoryViewMenu()
        {
            new PaymentHistoryForm().ShowDialog();
        }

        private void BalanceHistoryViewMenu()
        {
            new PaymentBalanceHistoryForm().ShowDialog();
        }
        /*
        private void RegistrationMenuItem_Click(object sender, EventArgs e)
        {
            // Open Registration Form in the "Registration" tab
            CustomerRegistrationForm registrationForm = new CustomerRegistrationForm();
            registrationForm.TopLevel = false;
            registrationForm.Dock = DockStyle.Fill;
            customerRegistrationTab.Controls.Add(registrationForm);
            registrationForm.Show();
        }
        */

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
