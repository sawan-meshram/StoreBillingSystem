using System;
using System.Drawing;
using System.Windows.Forms;

using StoreBillingSystem.Entity;
using StoreBillingSystem.Events;
using StoreBillingSystem.Util;

namespace StoreBillingSystem.StoreForm.PaymentForm
{
    public class CustomPaymentDialogBox : Form
    {
        private Payment payment;
        public CustomPaymentDialogBox(Payment payment)
        {
            this.payment = payment;

            InitializeComponent();
            InitCustomPaymentDialogEvent();
        }

        private Font labelFont = U.StoreLabelFont;

        private TextBox netAmountText;
        private TextBox paidAmount;
        private TextBox balanceAmount;
        private ComboBox billingStatus;
        private ComboBox paymentModes;


        private string pricePlaceHolder = "0.00";


        private void InitializeComponent()
        {
            Text = "Payment Status";

            HelpButton = true; // Display a help button on the form
            FormBorderStyle = FormBorderStyle.FixedDialog; // Define the border style of the form to a dialog box.
            MaximizeBox = false; // Set the MaximizeBox to false to remove the maximize box.
            MinimizeBox = false; // Set the MinimizeBox to false to remove the minimize box.
            StartPosition = FormStartPosition.CenterScreen; // Set the start position of the form to the center of the screen.
            Size = new Size(350, 400);
            BackColor = U.StoreDialogBackColor;
            AutoScroll = true;


            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 9,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Lime,
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-0
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-1
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-2
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-3
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-4
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); //row-5 //blank
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); //row-6

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f)); 
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));

            //Row-0
            //Blank

            //Row-1
            table.Controls.Add(new Label
            {
                Text = "Net Amount :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 1);

            netAmountText = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = payment.Billing.NetAmount.ToString(),
                ReadOnly = true,
                BackColor = Color.White
                //ForeColor = Color.Gray,
            };
            table.Controls.Add(netAmountText, 1, 1);

            //Row-2
            table.Controls.Add(new Label
            {
                Text = "Status :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 2);


            billingStatus = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(billingStatus, 1, 2);

            //Row-3
            table.Controls.Add(new Label
            {
                Text = "Payment Mode :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 3);

            paymentModes = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(paymentModes, 1, 3);

            //Row-4
            table.Controls.Add(new Label
            {
                Text = "Paid Amount :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 4);

            paidAmount = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = payment.Billing.NetAmount.ToString(),
                //ForeColor = Color.Gray,
            };
            table.Controls.Add(paidAmount, 1, 4);

            //Row-5
            table.Controls.Add(new Label
            {
                Text = "Balance Amount :",
                Font = labelFont,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
            }, 0, 5);

            balanceAmount = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = labelFont,
                Margin = new Padding(5),
                Text = pricePlaceHolder,
                ForeColor = Color.Gray,
                ReadOnly = true,
                BackColor = Color.White,
            };
            table.Controls.Add(balanceAmount, 1, 5);

            //Row-6
            //Blank

            //Row-7
            TableLayoutPanel table1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                //BackColor = Color.Gold
            };
            table1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90f)); //name

            Button okButton = new Button
            {
                Text = "OK",
                Dock = DockStyle.Fill,
                DialogResult = DialogResult.OK,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            Button cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill,
                Font = labelFont,
                ForeColor = Color.White,
                BackColor = Color.Blue,
                Margin = new Padding(5)
            };
            CancelButton = cancelButton;

            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;

            table1.Controls.Add(cancelButton, 1, 0);
            table1.Controls.Add(okButton, 2, 0);

            table.Controls.Add(table1, 0, 7);
            table.SetColumnSpan(table1, 3);



            Controls.Add(table);


        }

        private void InitCustomPaymentDialogEvent()
        {
            BindBillingStatusToComboBox();
            BindPaymentModesToComboBox();

            billingStatus.SelectedIndexChanged += BillingStatus_SelectedIndexChanged;
            paymentModes.SelectedIndexChanged += PaymentModes_SelectedIndexChanged;


            paidAmount.Enter += (sender, e) => TextBoxKeyEvent.PlaceHolderText_GotFocus(paidAmount, pricePlaceHolder);
            paidAmount.Leave += (sender, e) => TextBoxKeyEvent.PlaceHolderText_LostFocus(paidAmount, pricePlaceHolder);

            paidAmount.KeyPress += TextBoxKeyEvent.DecimalNumbericTextBox_KeyPress;

            paidAmount.TextChanged += PaidAmount_TextChanged;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            BillingStatus status = (BillingStatus)billingStatus.SelectedValue;
            if (status != BillingStatus.Unpaid)
            {
                double paidAmt = double.Parse(paidAmount.Text);
                if (paidAmt <= 0)
                {
                    MessageBox.Show("Paid Amount must greater than zero.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                payment.PaidDate = payment.Billing.BillingDateTime;
                payment.PaymentMode = (PaymentMode)paymentModes.SelectedValue;

            }
            else
            {
                payment.PaymentMode = PaymentMode.None;
            }
            payment.PaidAmount = double.Parse(paidAmount.Text);
            payment.BalanceAmount = double.Parse(balanceAmount.Text);
            payment.Status = status;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void PaymentModes_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaymentMode payMode = (PaymentMode)this.paymentModes.SelectedValue;
            if((BillingStatus)billingStatus.SelectedValue != BillingStatus.Unpaid && payMode == PaymentMode.None)
            {
                MessageBox.Show("Option is not available for 'Paid' & 'Partpaid' status.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void PaidAmount_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(paidAmount.Text)) return;
            double netAmt = double.Parse(netAmountText.Text);
            double paidAmt = double.Parse(paidAmount.Text);
            paidAmount.SelectionStart = paidAmount.Text.Length; // Set the cursor at the end

            if (paidAmt <= netAmt)
            {
                TextBoxKeyEvent.BindPlaceholderToTextBox(balanceAmount, (netAmt - paidAmt).ToString(), Color.Black);
            }
            else
            {
                TextBoxKeyEvent.BindPlaceholderToTextBox(paidAmount, netAmountText.Text, Color.Black); //paid = net amt
                TextBoxKeyEvent.BindPlaceholderToTextBox(balanceAmount, pricePlaceHolder, Color.Gray); //zero balance
            }
        }

        private void BindBillingStatusToComboBox()
        {
            billingStatus.DataSource = Enum.GetValues(typeof(BillingStatus));
        }

        private void BindPaymentModesToComboBox()
        {
            paymentModes.DataSource = Enum.GetValues(typeof(PaymentMode));
        }


        private void BillingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BillingStatus status = (BillingStatus)this.billingStatus.SelectedValue;

            if (status == BillingStatus.Unpaid)
            {
                paymentModes.Enabled = false;
                balanceAmount.Text = netAmountText.Text;
                TextBoxKeyEvent.BindPlaceholderToTextBox(paidAmount, pricePlaceHolder, Color.Gray);
                paymentModes.SelectedIndex = paymentModes.Items.Count -1;
            }
            else
            {
                paymentModes.SelectedIndex = 0;
                paymentModes.Enabled = true;
            }

            if (status == BillingStatus.Partpaid)
            {
                balanceAmount.ReadOnly = true;
            }
            else
            {
                balanceAmount.ReadOnly = false;
            }

            if (status == BillingStatus.Paid)
            {
                TextBoxKeyEvent.BindPlaceholderToTextBox(paidAmount, netAmountText.Text, Color.Black);
                TextBoxKeyEvent.BindPlaceholderToTextBox(balanceAmount, pricePlaceHolder, Color.Gray);
            }

        }

    }
}
