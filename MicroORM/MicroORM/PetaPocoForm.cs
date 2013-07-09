using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MicroORM.PetaPoco;

namespace MicroORMTest
{
    public partial class PetaPocoForm : Form
    {
        public PetaPocoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create a PetaPoco database object
            var db = new PetaPoco.Database("sqlite");

            try
            {
                // Show all foo    
                foreach (var a in db.Query<foo>("SELECT * FROM foo"))
                {
                    Console.WriteLine("{0} - {1}", a.id, a.name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a PetaPoco database object
            var db = new PetaPoco.Database("sqlite");
            foo foo = new foo();
            foo.name = "PetaPoco Insert Test";

            try
            {
                db.Insert("foo", "Id", foo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a PetaPoco database object
                var db = new PetaPoco.Database("sqlite");
                // Get a record
                var foo = db.SingleOrDefault<foo>("SELECT * FROM foo WHERE Id=@0", 2);

                // Change it
                foo.name = "PetaPoco was here again";

                // Save it
                db.Update("foo", "Id", foo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a PetaPoco database object
                var db = new PetaPoco.Database("sqlite");

                // Save it
                db.Delete("foo", "Id", null, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            DateTime start = System.DateTime.Now;

            // Create a PetaPoco database object
            var db = new PetaPoco.Database("sqlite");

            for (int i = 0; i < 10000; i++)
            {
                foo foo = new foo();
                foo.name = "PetaPoco Insert Test " + i;
                db.Insert("foo", "Id", foo);
            }
            Console.WriteLine("Elapsed: " + (System.DateTime.Now - start).TotalMilliseconds);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Create a PetaPoco database object
            var db = new PetaPoco.Database("sqlite");

            DateTime start = System.DateTime.Now;

            Dictionary<int, string> dict = new Dictionary<int, string>();

            try
            {
                // Show all foo    
                foreach (var a in db.Query<foo>("SELECT * FROM foo"))
                {
                    dict.Add(a.id, a.name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            Console.WriteLine("Elapsed: " + (System.DateTime.Now - start).TotalMilliseconds);
        }
    }
}
