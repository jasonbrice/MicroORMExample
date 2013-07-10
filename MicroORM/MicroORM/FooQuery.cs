using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MicroORMTest
{
    public partial class FooQuery : UserControl
    {
        private PetaPoco.Database db;

        public FooQuery()
        {
            InitializeComponent();

        }

        public void SetDb(PetaPoco.Database db) {
            this.db = db;
            SelectAll();
        }

        public void AppendDisplay(string text) {
            this.richTextBox1.Text += text;
        }

        public void SetDisplay(string display) {
            this.richTextBox1.Text = display;
        }

        public void Refresh() { SelectAll(); }

        private void SelectAll() {
            // Create a PetaPoco database object
            var db = new PetaPoco.Database("sqlite");

            string query = "SELECT * FROM foo";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(query);
            sb.AppendLine("--------------");

            try
            {
                // Show all foo    
                foreach (var a in db.Query<foo>(query))
                {
                    sb.AppendLine(string.Format("{0} - {1}", a.Id, a.name));
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.Message);
                sb.Append(ex.StackTrace);
            }

            this.richTextBox1.Text = sb.ToString();
        }
    }
}
