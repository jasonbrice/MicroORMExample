﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;
using System.Windows.Forms;
using MicroLite.Configuration;
using MicroLite;
using MicroLite.Mapping;

namespace MicroORMTest
{
    public partial class MicroLiteForm : Form
    {
        public MicroLiteForm()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            var db = new PetaPoco.Database("sqlite");
            this.fooQuery1.SetDb(db);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fooQuery1.SetDisplay("SELECT * from foo where Id=1");

            using (var session = GetFactory().OpenSession())
            {
                var query = new SqlQuery("SELECT * from foo where Id=1");
                var foos = session.Fetch<foo>(query); // foos will be an IList<foo>
                foreach (foo a in foos)
                {
                    fooQuery1.AppendDisplay("\r\n" + string.Format("{0} - {1}", a.Id, a.name));
                    Console.WriteLine("{0} - {1}", a.Id, a.name);
                }
            }
        }

        private ISessionFactory GetFactory()
        {
            Configure.Extensions() // If used, load any logging extension first.
         .WithConventionBasedMapping(new ConventionMappingSettings
         {
             IdentifierStrategy = IdentifierStrategy.DbGenerated, // default is DbGenerated if not specified.
             UsePluralClassNameForTableName = false // default is true if not specified.
             
         });

            var sessionFactory = Configure
                .Fluently()
                .ForConnection(connectionName: "sqlite", sqlDialect: "MicroLite.Dialect.SQLiteDialect")
                .CreateSessionFactory();
            return sessionFactory;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var foo = new foo();
            foo.name = "Insert Test";

            using (var session = GetFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Insert(foo);
                    transaction.Commit();
                    // foo.Id will now be set to the value generated by the database when the record was inserted.
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var session = GetFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var foo = session.Single<foo>(new SqlQuery("SELECT * from foo where Id=(SELECT max(Id) from foo)"));
                    //var foo = session.Single<foo>(2);
                    foo.name = "Microlite Updated your name!";
                    session.Update(foo);
                    transaction.Commit();
                }
            }
            this.fooQuery1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool deleted;
            //foo foo = new foo();
            //foo.Id = 2;

            using (var session = GetFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var foo = session.Single<foo>(new SqlQuery("SELECT * from foo where Id=(SELECT max(Id) from foo)"));
                    deleted = session.Delete(foo);
                    transaction.Commit();
                }
            }

            if (!deleted)
            {
                this.fooQuery1.SetDisplay("Could not find/delete foo");
            }
            else
            {

                this.fooQuery1.Refresh();
            }
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


            Console.WriteLine("Elapsed: " + (System.DateTime.Now - start).TotalMilliseconds);

            this.fooQuery1.SetDisplay("Inserted " + count + " records in " + (System.DateTime.Now - start).TotalMilliseconds + " milliseconds");
        }

        private void DoBackgroundWork_Insert(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            using (var session = GetFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    for (int i = 0; i < count; i++)
                    {
                        var foo = new foo();
                        foo.name = "MicroLite Insert Test " + i;
                        session.Insert(foo);
                        if (i % 500 == 0) worker.ReportProgress(i);//this.fooQuery1.AppendDisplay(".");
                    }
                    transaction.Commit();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTime start = System.DateTime.Now;

            Dictionary<int, string> dict = new Dictionary<int, string>();

            using (var session = GetFactory().OpenSession())
            {
                var query = new SqlQuery("SELECT * from foo");
                var foos = session.Fetch<foo>(query); // foos will be an IList<foo>
                foreach (foo a in foos)
                {
                   dict.Add(a.Id, a.name);
                }
            }
            Console.WriteLine("Elapsed: " + (System.DateTime.Now - start).TotalMilliseconds);

            this.fooQuery1.SetDisplay("Read " + dict.Count + " records in " + (System.DateTime.Now - start).TotalMilliseconds + " milliseconds");
        }
    }
}
