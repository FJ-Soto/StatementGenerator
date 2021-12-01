using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace FTDStatementPrinter
{
    public partial class MainWindow : Form
    {
        private List<StatementControl> statementControls;
        private DirectoryInfo saveDirectory;
        private string cumulativeStatementFilename;

        private readonly DirectoryInfo baseProjectDirectory;
        private readonly string baseFileDirectory;
        private readonly int x;

        public MainWindow()
        {
            InitializeComponent();
            MaximumSize = new Size(900, int.MaxValue);
            MinimumSize = new Size(900, 730);
            baseProjectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            baseFileDirectory = Path.Combine(baseProjectDirectory.FullName, "bin");
            statementControls = new List<StatementControl>();
            cumulativeStatementFilename = null;
            btnGenerateCombined.Enabled = false;
            btnCombinedApp.Enabled = false;
            btnCombinedOS.Enabled = false;

            x = ftdHeader.Location.X;

            if (File.Exists(Path.Combine(baseFileDirectory, "config.txt")))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(baseFileDirectory, "config.txt")))
                {
                    string[] lines = reader.ReadToEnd().Split('\n');
                    foreach(string line in lines)
                    {
                        string[] configs = line.Split('=');

                        if (Equals(configs[0], "save_directory"))
                        {
                            saveDirectory = new DirectoryInfo(configs[1]);
                        }
                    }

                }
            }

            if (saveDirectory == null)
            {
                saveDirectory = baseProjectDirectory;
            }

            ReadCreds();
            RedrawStatementControls();
            splitContainer1.FixedPanel = FixedPanel.Panel1;
        }

        public string getProjectDirectory() => baseFileDirectory;
        public string getSaveDirectory() => saveDirectory.FullName;

        public void EnableCombine(bool enable = true)
        {
            EnableControl(btnGenerateCombined, enable);
            EnableControl(btnCombinedApp, enable);
            EnableControl(btnCombinedOS, enable);
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

        // this method simply reads credentials but does not load reports
        private void ReadCreds()
        {
            try
            {
                // read stored creds
                if (File.Exists(Path.Combine(baseFileDirectory, "_cs.bin")))
                {
                    using(StreamReader reader = new StreamReader(Path.Combine(baseFileDirectory, "_cs.bin")))
                    {
                        while (!reader.EndOfStream)
                        {
                            string detail = reader.ReadLine();

                            string type, username, password, accID = null;

                            Regex typeReg = new Regex("-(t)=\"(.*?)\"");
                            Regex userReg = new Regex("-(u)=\"(.*?)\"");
                            Regex passReg = new Regex("-(p)=\"(.*?)\"");
                            Regex accReg = new Regex("-(a)=\"(.*?)\"");
                            try
                            {
                                type = typeReg.Match(detail).Groups[2].Value;
                                username = userReg.Match(detail).Groups[2].Value;
                                password = passReg.Match(detail).Groups[2].Value;
                            }
                            catch (IndexOutOfRangeException)
                            {
                                throw new IndexOutOfRangeException("Unable to parse credentials.");
                            }

                            Match accMatch = accReg.Match(detail);
                            if (accMatch.Success)
                            {
                                accID = string.IsNullOrEmpty(accMatch.Groups[2].Value) ? null : accMatch.Groups[2].Value;
                            }

                            Credentials creds = new Credentials(type, username, password, accID, doSave: true);
                            StatementControl control = new StatementControl(this, creds);
                            splitContainer1.Panel2.Controls.Add(control);
                            statementControls.Add(control);
                        }
                    }
                }
            }
            catch (MalformedFile e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void btnAddStatement_Click(object sender, EventArgs e)
        {
            CredentialsWindow credentialWindow = new CredentialsWindow();
            credentialWindow.ShowDialog();

            if (credentialWindow.DialogResult ==  DialogResult.OK)
            {
                StatementControl control = new StatementControl(this, credentialWindow.Credentials);
                splitContainer1.Panel2.Controls.Add(control);
                statementControls.Add(control);
                RedrawStatementControls();
            }
        }

        public void DeleteStatement(StatementControl sender)
        {
            splitContainer1.Panel2.Controls.Remove(sender);
            statementControls.Remove(sender);
            RedrawStatementControls();
        }

        private void RedrawStatementControls()
        {
            int y = 5;

            statementControls.Sort((x1, x2) => x1.Credentials.Type.CompareTo(x2.Credentials.Type));
            foreach (StatementControl control in statementControls)
            {
                control.Location = new Point(x, y);
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                y += control.Height + 10;
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(baseFileDirectory, "_cs.bin"), FileMode.Create)))
            {
                foreach(StatementControl c in statementControls)
                {
                    if (c.Credentials.DoSave)
                    {
                        string type = string.IsNullOrEmpty(c.Credentials.Type) ? "" : c.Credentials.Type;
                        string accID = string.IsNullOrEmpty(c.Credentials.AccountID) ? "" : c.Credentials.AccountID;
                        string username = string.IsNullOrEmpty(c.Credentials.Username) ? "" : c.Credentials.Username;
                        string password = string.IsNullOrEmpty(c.Credentials.Password) ? "" : c.Credentials.Password;

                        writer.WriteLine($"-t=\"{type}\" -u=\"{username}\" -p=\"{password}\" -a=\"{accID}\"");
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(File.Open(Path.Combine(baseFileDirectory, "config.txt"), FileMode.Create)))
            {
                writer.WriteLine($"save_directory={getSaveDirectory()}");
            }
        }

        private void btnGenAll_Click(object sender, EventArgs e)
        {
            foreach (StatementControl control in statementControls)
            {
                control.GenerateStatement();
            }
        }


        private void MainWindow_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel1MinSize = this.splitContainer1.Panel1.Height;
        }

        private void btnOpenFileExplorer_Click(object sender, EventArgs e)
        {
            // create dir if not exists
            Directory.CreateDirectory(Path.Combine(getSaveDirectory(), "Statements"));

            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    UseShellExecute = false,
                    Arguments = $"/C explorer.exe {Path.Combine(getSaveDirectory(), "Statements")}",
                    CreateNoWindow = true,
                }
            };
            p.Start();
            p.Dispose();
        }

        private void btnChangeFileLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog
            {
                SelectedPath = getSaveDirectory()
            };


            DialogResult res = fileDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                saveDirectory = new DirectoryInfo(fileDialog.SelectedPath);
            }
        }

        private void btnCombine_Click(object sender, EventArgs e)
        {
            if (statementControls.Any(x => x.HasStatements()))
            {
                GenerateCombinedStatement();
            } else
            {
                btnGenerateCombined.Enabled = false;
            }
        }

        private void GenerateCombinedStatement()
        {
            string[] args = new string[2];
            string scriptLocation = Path.Combine(getProjectDirectory(), "CombinePDFs.py");
            args[0] = $"\"py \"{scriptLocation}\" \"--p={getSaveDirectory()}\"";
            List<string> files = new List<string>();
            foreach (StatementControl control in statementControls)
            {
                
                foreach (KeyValuePair<string, StatementDetail> record in control.Statements)
                {
                    files.Add($"\"{record.Value.Filename}\"");
                }
            }
            args[1] = string.Join(" ", files);

            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    UseShellExecute = false,
                    Arguments = $"/C {string.Join(" ", args)}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true
            };

            p.Start();

            p.Exited += (s, o) =>
            {
                if (p.ExitCode == 0)
                {
                    cumulativeStatementFilename = p.StandardOutput.ReadToEnd();
                }
                else
                {
                    MessageBox.Show($"{p.StandardOutput.ReadToEnd()}\n{p.StandardError.ReadToEnd()}", p.ExitCode.ToString());
                }
                p.Dispose();
            };
        }

        private void btnOpenCombined_Click(object sender, EventArgs e)
        {
            if (!statementControls.Any(x => x.HasStatements()))
            {
                btnCombinedApp.Enabled = false;
                MessageBox.Show("There are no generated reports. Generate some and retry to combine.");
            }
            else
            {
                if (string.IsNullOrEmpty(cumulativeStatementFilename) || !File.Exists(cumulativeStatementFilename))
                {
                    DialogResult res = MessageBox.Show("No cumulative statement found. Would you like to generate?", "Missing Statement", MessageBoxButtons.YesNo);

                    if (res == DialogResult.Yes)
                    {
                        GenerateCombinedStatement();
                    }
                }
                new StatementView("Cumulative Statement", cumulativeStatementFilename).Show();
            }
        }

        private void btnCombinedOS_Click(object sender, EventArgs e)
        {
            if (!statementControls.Any(x => x.HasStatements()))
            {
                btnCombinedApp.Enabled = false;
                MessageBox.Show("There are no generated reports. Generate some and retry to combine.");
            } else
            {
                if (string.IsNullOrEmpty(cumulativeStatementFilename) || !File.Exists(cumulativeStatementFilename))
                {
                    DialogResult res = MessageBox.Show("No cumulative statement found. Would you like to generate?", "Missing Statement", MessageBoxButtons.YesNo);

                    if (res == DialogResult.Yes)
                    {
                        GenerateCombinedStatement();
                    }
                }

                Process p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        UseShellExecute = false,
                        Arguments = $"/C start \"\" \"{cumulativeStatementFilename}\"",
                        CreateNoWindow = true,
                    }
                };
                p.Start();
                p.Dispose();
            }
        }
    }
}
