using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MicroORMTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PetaPocoForm form = new PetaPocoForm();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MicroLiteForm form = new MicroLiteForm();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c sqlite3 Db.sqlite");

            procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c sqlite3 Db.sqlite \"VACUUM\"");

            procStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DapperForm form = new DapperForm();
            form.Show();
        }
    }
}
