using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;


namespace FTDStatementPrinter
{
    public partial class StatementControl : UserControl
    {
        public MainWindow Master { get; set; }
        public Credentials Credentials { get; set; }
        public Dictionary<string, StatementDetail> Statements { get; set; }
        private BindingSource ds;

        private const string PLACEHOLDER = "------";

        public StatementControl()
        {
            InitializeComponent();

            Font lblFont = new Font("Myanmar Text", 10, FontStyle.Bold);

            lblType.Text = "Type";
            lblType.Font = lblFont;

            lblAccountID.Text = "Account ID";
            lblAccountID.Font = lblFont;

            lblBillDateStart.Text = "Bill Date";
            lblBillDateStart.Font = lblFont;

            lblPreview.Text = "View";
            lblPreview.Font = lblFont;
            lblPreview.Visible = true;

            lblPrinter.Text = "Actions";
            lblPrinter.Font = lblFont;
            lblPrinter.Visible = true;

            lblDelete.Text = "Delete Options";
            lblDelete.Font = lblFont;
            lblDelete.Visible = true;

            Controls.Remove(btnViewWindow);
            Controls.Remove(btnRegenerate);
            Controls.Remove(btnViewOS);
            Controls.Remove(btnDeleteStatement);
            Controls.Remove(btnDeleteFile);
            Controls.Remove(btnEditCreds);
            Controls.Remove(lblBillDateStart);
            Controls.Remove(lblBillCycleStartlbl);
            Controls.Remove(lblBillAmountDetaillbl);

            Controls.Add(lblDelete);

            BorderStyle = BorderStyle.None;
        }

        public StatementControl(MainWindow master, Credentials credentials)
        {
            InitializeComponent();
            Credentials = credentials;
            Master = master;

            lblAccountID.Text = Credentials.AccountID ?? PLACEHOLDER;
            lblBillAmount.Text = PLACEHOLDER;
            lblBillDateEnd.Text = PLACEHOLDER;
            lblBillDateDue.Text = PLACEHOLDER;
            lblBillDateStart.Text = PLACEHOLDER;
            lblBillDateStart.Text = PLACEHOLDER;
            lblType.Text = Credentials.Type;

            btnViewOS.Enabled = false;
            btnViewWindow.Enabled = false;
            btnDeleteFile.Enabled = false;
            Statements = new Dictionary<string, StatementDetail>();

            cbAccIDs.DisplayMember = "Key";
            cbAccIDs.ValueMember = "Key";
        }

        private void btnRegenerate_Click(object sender, EventArgs e)
        {
            if (Statements.Count > 0)
            {
                DialogResult res = MessageBox.Show("You already have generated files. Would you like to regenerate?", "File(s) exist.", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    GenerateStatement();
                }
            }
            else
            {
                GenerateStatement();
            }
        }

        public void GenerateStatement()
        {
            Statements.Clear();
            if (!Credentials.IsEmpty())
            {
                bool doCredentialUpdate = false;

                string[] clArgs = new string[3];

                string scriptLocation = Path.Combine(Master.getProjectDirectory(), "StatementMain.py");
                clArgs[0] = $"py \"{scriptLocation}\" {Credentials.Type} \"-u={Credentials.Username}\" \"-p={Credentials.Password}\"";

                clArgs[1] += $"\"--p={Master.getSaveDirectory()}\"";

                if (!string.IsNullOrEmpty(Credentials.AccountID))
                {
                    clArgs[2] = $"\"-a={Credentials.AccountID}\"";
                }

                EnableAll(false);
                Process p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        UseShellExecute = false,
                        Arguments = $"/C {string.Join(" ", clArgs)}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                    },
                    EnableRaisingEvents = true
                };
                p.Start();
                p.Exited += (s, o) =>
                {
                    EnableAll();
                    string stdOut = p.StandardOutput.ReadToEnd().Trim();
                    string stdErr = p.StandardError.ReadToEnd().Trim();
                    if (p.ExitCode == 0)
                    {
                        Regex fileReg = new Regex("-(f)=\"(.*?)\"");
                        Regex accountReg = new Regex("-(a)=\"(.*?)\"");
                        Regex billEndReg = new Regex("-(be)=\"(.*?)\"");

                        Regex billStartReg = new Regex("-(bs)=\"(.*?)\"");
                        Regex billDueReg = new Regex("-(bd)=\"(.*?)\"");
                        Regex billAmoutReg = new Regex("-(ba)=\"(.*?)\"");

                        foreach (string line in stdOut.Split('\n'))
                        {
                            // required
                            string filename = fileReg.Match(line).Groups[2].Value;
                            string accountID = accountReg.Match(line).Groups[2].Value;
                            string bilEnd = billEndReg.Match(line).Groups[2].Value;


                            // optional
                            decimal? billAmount = null;
                            string billStart = null;
                            string billDue = null;
                            if (billAmoutReg.IsMatch(line))
                            {
                                billAmount = decimal.Parse(billAmoutReg.Match(line).Groups[2].Value);
                            }


                            if (billStartReg.IsMatch(line))
                            {
                                billStart = billStartReg.Match(line).Groups[2].Value;
                            }
                            if (billDueReg.IsMatch(line))
                            {
                                billDue = billDueReg.Match(line).Groups[2].Value;
                            }

                            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(bilEnd) || string.IsNullOrEmpty(accountID))
                            {
                                MessageBox.Show("Unable to parse filename, billdate, account number, or bill amount.");
                            }
                            else
                            {
                                Statements.Add(accountID, new StatementDetail(filename, bilEnd,
                                    amount: billAmount, billDue: billDue, billStart: billStart));
                            }
                        }
                        Master.EnableCombine();
                        EnableViewing();
                    }
                    else
                    {
                        if (p.ExitCode == 9)
                        {
                            DialogResult r = MessageBox.Show("Your credentials might not be correct. Would you like to re-type them?", "Invalid Credentials", MessageBoxButtons.YesNo);

                            if (r == DialogResult.Yes)
                            {
                                doCredentialUpdate = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"{stdOut}\n{stdErr}", $"Error code: {p.ExitCode}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        if (doCredentialUpdate)
                        {
                            CredentialsWindow credWindow = new CredentialsWindow(Credentials);
                            credWindow.ShowDialog();
                            if (credWindow.DialogResult == DialogResult.OK)
                            {
                                Credentials = credWindow.Credentials;
                                GenerateStatement();
                            }
                        }
                        EnableViewing(false);
                    }
                    UpdateBillDateAccountID();
                    p.Dispose();
                };
            }
            else
            {
                new CredentialsWindow();
            }
        }

        public void UpdateBillDateAccountID()
        {
            if (Statements.Count < 1)
            {
                AlterControlText(lblAccountID, PLACEHOLDER);
                AlterControlText(lblBillDateStart, PLACEHOLDER);
                AlterControlText(lblBillAmount, PLACEHOLDER);
                AlterControlText(lblBillDateStart, PLACEHOLDER);
                AlterControlText(lblBillDateEnd, PLACEHOLDER);
                AlterControlText(lblBillDateDue, PLACEHOLDER);
                VisibleControl(lblAccountID);
                VisibleControl(cbAccIDs, false);
                EnableDeleting(false);
            }
            if (Statements.Count == 1)
            {
                KeyValuePair<string, StatementDetail> record = Statements.First();
                AlterControlText(lblAccountID, record.Key);
                AlterControlText(lblBillDateStart, record.Value.BillStart);

                string billAmountTxt = record.Value.BillAmount == null ? PLACEHOLDER : $"{record.Value.BillAmount:C2}";

                AlterBillDetails(record.Value);
                VisibleControl(lblAccountID);
                VisibleControl(cbAccIDs, false);
            }
            else if (Statements.Count > 1)
            {
                VisibleControl(lblAccountID, false);
                VisibleControl(cbAccIDs);
                ds = new BindingSource(Statements, null);
                UpdateDataSource(ds);
            }
        }

        public bool HasStatements()
        {
            return Statements.Count > 0;
        }

        private void UpdateDataSource(object src)
        {
            if (cbAccIDs.InvokeRequired)
            {
                Action safeEnable = delegate { cbAccIDs.DataSource = src; };
                cbAccIDs.Invoke(safeEnable);
            }
            else
            {
                cbAccIDs.DataSource = src;
            }
        }

        public void EnableAll(bool enable = true)
        {
            EnableControl(btnViewOS, enable);
            EnableControl(btnEditCreds, enable);
            EnableControl(btnViewWindow, enable);
            EnableControl(btnRegenerate, enable);
            EnableControl(btnDeleteFile, enable);
            EnableControl(btnDeleteStatement, enable);
        }

        public void EnableDeleting(bool enable = true)
        {
            EnableControl(btnDeleteFile, enable);
        }

        public void EnableAccountDeleting(bool enable = true)
        {
            EnableControl(btnDeleteStatement, enable);
        }

        public void EnableViewing(bool enable = true)
        {
            EnableControl(btnViewOS, enable);
            EnableControl(btnViewWindow, enable);
        }

        private void AlterControlText(Control c, string txt)
        {
            if (c.InvokeRequired)
            {
                Action safeEnable = delegate { c.Text = txt; };
                c.Invoke(safeEnable);
            }
            else
            {
                c.Text = txt;
            }
        }

        private void EnableControl(Control c, bool enable = true)
        {
            if (c.InvokeRequired)
            {
                Action safeEnable = delegate { c.Enabled = enable; };
                c.Invoke(safeEnable);
            }
            else
            {
                c.Enabled = enable;
            }
        }

        private void VisibleControl(Control c, bool enable = true)
        {
            if (c.InvokeRequired)
            {
                Action safeEnable = delegate { c.Visible = enable; };
                c.Invoke(safeEnable);
            }
            else
            {
                c.Visible = enable;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

            if (Statements.Count > 0)
            {
                KeyValuePair<string, StatementDetail> record;
                if (Statements.Count == 1)
                {
                    record = Statements.First();
                }
                else
                {
                    record = Statements.Where(x => Equals(x.Key, cbAccIDs.SelectedValue)).First();
                }
                string accID = record.Key;
                string filename = record.Value.Filename;

                if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
                {
                    new StatementView($"Statement for {record.Key}", filename).Show();
                }
                else
                {
                    DialogResult res = MessageBox.Show("File for this statement not found. Regenerate?", "Statement not found", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.Yes)
                    {
                        GenerateStatement();
                    }
                }
            }
        }

        private void btnDeleteStatement_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show($"This will delete the credentials for this statement. Continue?",
                "Delete Statement", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res == DialogResult.Yes)
            {
                Master.DeleteStatement(this);
                if (Statements.Count > 0)
                {
                    DialogResult deleteFile = MessageBox.Show("Would you like to also delete the generated file(s)?", "Delete File?", MessageBoxButtons.YesNo);

                    if (deleteFile == DialogResult.Yes)
                    {
                        DeleteFiles();
                    }
                }
            }
        }

        private KeyValuePair<string, StatementDetail> getSelectedStatement()
        {
            if (Statements.Count > 0)
            {
                if (Statements.Count == 1)
                {
                    return Statements.First();
                }
                else
                {
                    return Statements.Where(x => Equals(x.Key, cbAccIDs.SelectedValue)).First();
                }
            }
            return new KeyValuePair<string, StatementDetail>(null, null);
        }

        private void DeleteFiles(bool all = true)
        {
            if (all)
            {
                foreach (KeyValuePair<string, StatementDetail> record in Statements)
                {

                    DeleteFile(record);
                }
            }
            else
            {
                KeyValuePair<string, StatementDetail> record = getSelectedStatement();
                DeleteFile(record);
            }

            if (Statements.Count < 1)
            {
                EnableViewing(false);
                EnableDeleting(false);
            }

            UpdateBillDateAccountID();
        }

        private void DeleteFile(KeyValuePair<string, StatementDetail> record)
        {
            try
            {
                bool recordHasFile = !string.IsNullOrEmpty(record.Value.Filename);
                if (recordHasFile && File.Exists(record.Value.Filename))
                {
                    File.Delete(record.Value.Filename);
                    RemoveStatement(record);
                }
                else if (recordHasFile && !File.Exists(record.Value.Filename))
                {
                    MessageBox.Show("File is no longer available.", "File not found.", MessageBoxButtons.OK);
                }
            }
            catch (IOException)
            {
                MessageBox.Show($"Unable to delete saved statement file. Please make sure that the statement for {record.Key} is closed.",
                    "Failed to delete old file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            DeleteFiles(all: false);
        }

        private void btnEditCreds_Click(object sender, EventArgs e)
        {
            CredentialsWindow credWindow = new CredentialsWindow(Credentials);
            credWindow.ShowDialog();
            if (credWindow.DialogResult == DialogResult.OK)
            {
                Credentials = credWindow.Credentials;
            }
        }

        private void btnViewOS_Click(object sender, EventArgs e)
        {
            if (Statements.Count > 0)
            {
                KeyValuePair<string, StatementDetail> record = getSelectedStatement();

                Process p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        UseShellExecute = false,
                        Arguments = $"/C start \"\" \"{record.Value.Filename}\"",
                        CreateNoWindow = true,
                    }
                };
                p.Start();
                p.Dispose();
            }
        }

        private void cbAccIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Statements.Count > 0)
            {
                KeyValuePair<string, StatementDetail> record = Statements.Where(x => Equals(x.Key, cbAccIDs.SelectedValue)).First();
                AlterBillDetails(record.Value);
            }
        }

        private void AlterBillDetails(StatementDetail recordDetails)
        {
            string amount = recordDetails.BillAmount == null ? PLACEHOLDER : $"{recordDetails.BillAmount:C2}";
            AlterControlText(lblBillDateEnd, recordDetails.BillEnd ?? PLACEHOLDER);
            AlterControlText(lblBillDateStart, recordDetails.BillStart ?? PLACEHOLDER);
            AlterControlText(lblBillDateDue, recordDetails.BillDue ?? PLACEHOLDER);
            AlterControlText(lblBillAmount, amount);
        }

        private void RemoveStatement(KeyValuePair<string, StatementDetail> record)
        {
            Statements.Remove(record.Key);

            if (ds != null && ds.Count > 0)
            {
                ds.Remove(record);
            }
            UpdateBillDateAccountID();
        }
    }
}
