using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Transactions;  
using System.Data.SQLite;
using Dapper;

namespace MicroORMTest
{
    public partial class DapperForm : Form
    {
        public DapperForm()
        {
            InitializeComponent();
        }

        private SQLiteConnection GetSqlConnection(){
            string connectionString = ConfigurationManager.ConnectionStrings["sqlite"].ConnectionString;
            SQLiteConnection sqlConnection = new SQLiteConnection(connectionString);

            return sqlConnection;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            var db = new PetaPoco.Database("sqlite");
            this.fooQuery1.SetDb(db);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foo foo = new foo();
            foo.name = "Created by Dapper";

            using (SQLiteConnection conn = GetSqlConnection())
            {
                conn.Open();

                string sqlQuery = "INSERT INTO foo(name) VALUES (@name)";
                conn.Execute(sqlQuery,
                    new
                    {
                        name="Created by Dapper"//foo.name
                    });

                conn.Close();
            }

            this.fooQuery1.Refresh();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            fooQuery1.SetDisplay("SELECT * from foo where Id=1");
                        using (SQLiteConnection conn = GetSqlConnection())
            {
                conn.Open();
            var foo = conn.Query<foo>("select * from foo where Id = @Id", new { Id = 1 }).ToList<foo>();
            foreach (foo a in foo) fooQuery1.AppendDisplay("\r\n" + string.Format("{0} - {1}", a.Id, a.name));
            conn.Close();
                        }
        }

        //cnn.Execute("update Table val = @val where Id = @id", new {val, id = 1});
        private void button3_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = GetSqlConnection())
            {
                conn.Open();
                var foo = (foo)conn.Query<foo>("select * from foo where Id = (select max(Id) from foo)").ToList<foo>()[0];
                string sqlQuery = "update foo set name=@name where Id=@Id";
                conn.Execute(sqlQuery, new { name="Updated by Dapper", Id=foo.Id });
                conn.Close();
            }

            this.fooQuery1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = GetSqlConnection())
            {
                conn.Open();
                var foo = (foo)conn.Query<foo>("select * from foo where Id = (select max(Id) from foo)").ToList<foo>()[0];
                string sqlQuery = "delete from foo where Id=@Id";
                conn.Execute(sqlQuery, new { Id = foo.Id });
                conn.Close();
            }

            this.fooQuery1.Refresh();
        }

        int count = 10000;

        private void button5_Click(object sender, EventArgs e)
        {
            
            this.fooQuery1.SetDisplay("Working");
            
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

            using (var transactionScope = new TransactionScope())
            {
                using (SQLiteConnection conn = GetSqlConnection())
                {
                    conn.Open();

                    string sqlQuery = "INSERT INTO foo(name) VALUES (@name)";

                    for (int i = 0; i < count; i++)
                    {

                        string s = "Created by Dapper " + i;
                        if (i % 500 == 0) worker.ReportProgress(i);
                        conn.Execute(sqlQuery,
                            new
                            {
                                name = s
                            });
                    }
                    conn.Close();

                    transactionScope.Complete();
                }
                
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTime start = System.DateTime.Now;

            Dictionary<int, string> dict = new Dictionary<int, string>();

            fooQuery1.SetDisplay("SELECT * from foo");
            using (SQLiteConnection conn = GetSqlConnection())
            {
                conn.Open();
                var foo = conn.Query<foo>("select * from foo").ToList<foo>();
                foreach (foo a in foo) dict.Add(a.Id, a.name);
                conn.Close();
            }

            this.fooQuery1.SetDisplay("Read " + dict.Count + " records in " + (System.DateTime.Now - start).TotalMilliseconds + " milliseconds");
        }
    }
}
