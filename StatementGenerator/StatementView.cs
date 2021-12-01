using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTDStatementPrinter
{
    public partial class StatementView : Form
    {
        private string Filename { get; set; }
        public StatementView(string title, string filename)
        {
            InitializeComponent();
            Text = title;
            Filename = filename;
            WebBrowser.Url = new Uri(filename);
        }
    }
}
