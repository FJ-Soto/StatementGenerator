using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace FTDStatementPrinter
{
    public partial class CredentialsWindow : Form
    {
        public Credentials Credentials { get; set; }

        public CredentialsWindow(string btnText = "Add")
        {
            InitializeComponent();
            btnAdd.Text = btnText;

            List<StatementOption> opts = StatementOption.StatementTypes();
            cbStatementTypes.DataSource = opts;
            cbStatementTypes.DisplayMember = "Name";
            cbStatementTypes.ValueMember = "Value";
        }

        public CredentialsWindow(Credentials creds, string btnText = "Edit")
        {
            InitializeComponent();
            btnAdd.Text = btnText;
            Credentials = creds;

            List<StatementOption> opts = StatementOption.StatementTypes();
            cbStatementTypes.DataSource = opts;
            cbStatementTypes.DisplayMember = "Name";
            cbStatementTypes.ValueMember = "Value";

            foreach (StatementOption o in opts)
            {
                if (o.Value == Credentials.Type)
                {
                    cbStatementTypes.SelectedItem = o;
                    break;
                }
            }

            txtAccID.Text = Credentials.AccountID ?? "";
            txtUsername.Text = Credentials.Username ?? "";
            txtPassword.Text = Credentials.Password ?? "";
            ckSave.Checked = Credentials.DoSave;
        } 

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                string accID = txtAccID.Text.Trim();
                string uname = txtUsername.Text.Trim();
                string pwd = txtPassword.Text.Trim();
                string type = (string)cbStatementTypes.SelectedValue;
                btnAdd.Enabled = false;
                lblErrorMsg.Text = "";
                Credentials = new Credentials(type, uname, pwd, accID, ckSave.Checked);

                DialogResult = DialogResult.OK;
                Close();
            } else
            {
                DialogResult = DialogResult.None;
            }
        }

        private void txtUserID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtUsername, "Enter user ID");
            }
            else
            {
                errorProvider.SetError(txtUsername, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtPassword, "Enter password");
            }
            else
            {
                errorProvider.SetError(txtPassword, "");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
