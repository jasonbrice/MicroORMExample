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
    public partial class PetaPocoForm : Form
    {
        public PetaPocoForm()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e) {
            var db = new PetaPoco.Database("sqlite");
            this.fooQuery1.SetDb(db);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create a PetaPoco database object
            var db = new PetaPoco.Database("sqlite");

            fooQuery1.SetDisplay("SELECT * from foo where Id=1");

            try
            {
                // Show all foo    
                foreach (var a in db.Query<foo>("SELECT * from foo where Id=1"))
                {
                    fooQuery1.AppendDisplay("\r\n" + string.Format("{0} - {1}", a.Id, a.name));
                    Console.WriteLine("{0} - {1}", a.Id, a.name);
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
            this.fooQuery1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a PetaPoco database object
                var db = new PetaPoco.Database("sqlite");

                // find the (presumably) most recently created foo
                int id = db.ExecuteScalar<int>("SELECT max(id) from foo");

                // Get a record
                var foo = db.SingleOrDefault<foo>("SELECT * FROM foo WHERE Id=@0", id);

                // Change it
                foo.name = "PetaPoco changed your name!";

                // Save it
                db.Update("foo", "Id", foo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            this.fooQuery1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a PetaPoco database object
                var db = new PetaPoco.Database("sqlite");

                // find the (presumably) most recently created foo
                int id = db.ExecuteScalar<int>("SELECT max(id) from foo");

                // Delete it
                db.Delete("foo", "Id", null, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            this.fooQuery1.Refresh();
        }

        int count = 10000;

        private void button5_Click(object sender, EventArgs e)
        {
            this.fooQuery1.SetDisplay("Working");

            string results = "";

            DateTime start = System.DateTime.Now;

            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();

            worker.WorkerReportsProgress = true;
            worker.DoWork -= new System.ComponentModel.DoWorkEventHandler(DoBackgroundWork_Insert);
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(DoBackgroundWork_Insert);

            worker.ProgressChanged += new ProgressChangedEventHandler(
             delegate(object _sender, ProgressChangedEventArgs _e)
            {
                Console.WriteLine("Elapsed: " + (System.DateTime.Now - start).TotalMilliseconds);
                this.fooQuery1.AppendDisplay(".");
            }

            );
            
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
             delegate(object _sender, RunWorkerCompletedEventArgs _e)
            {
                Console.WriteLine("Elapsed: " + (System.DateTime.Now - start).TotalMilliseconds);
                this.fooQuery1.SetDisplay("Inserted " + count + " records in " + (System.DateTime.Now - start).TotalMilliseconds + " milliseconds");
            }

            );
            
            worker.RunWorkerAsync();

        }

        private void DoBackgroundWork_Insert(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Create a PetaPoco database object
            var db = new PetaPoco.Database("sqlite");

            for (int i = 0; i < count; i++)
            {
                foo foo = new foo();
                foo.name = "PetaPoco Insert Test " + i;
                db.Insert("foo", "Id", foo);

                if (i % 500 == 0) worker.ReportProgress(i);//this.fooQuery1.AppendDisplay(".");

            }
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
                    dict.Add(a.Id, a.name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            this.fooQuery1.SetDisplay("Read " + dict.Count + " records in " + (System.DateTime.Now - start).TotalMilliseconds + " milliseconds");

        }
    }
}
