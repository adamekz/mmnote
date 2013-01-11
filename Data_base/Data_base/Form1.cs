using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//ADO.NET
using System.Data.SqlClient;
//LINQ
using System.Linq;
using System.Xml.Linq;

using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Collections;

namespace Data_base
{

    public partial class Form1 : Form
    {
        file_dataclassesDataContext database;
        XDocument pay_doc;
        SqlConnection connect;
        private int userid = 0;
        private bool to_close = false;
        //liczniki plików
        private int mf = 0;
        private int act_f = 0;

        private int money = 0;

        public Form1()
        {
            InitializeComponent();
            database = new file_dataclassesDataContext();
            login log = new login();
            Application.Run(log);
            userid = log.uid;
            log.Dispose();
            if (userid == 0) { to_close = true; return; }
            
            var get_name = from u in database.users
                           where u.u_id == userid
                           select new { u.first_name, u.last_name };
            
            string fn = "";
            string ln = "";
            foreach (var u in get_name)
            {
                fn = u.first_name;
                ln = u.last_name;
            }
            
            this.Text
                = "FilesManager user: " + fn + " " + ln;
           //LINQ to XML
           pay_doc = XDocument.Load("Payments.xml");
           var max_files = from fi in pay_doc.Elements("payments").Elements("userdata").Elements()
                           where (string)fi.Attribute("idf") == userid.ToString()
                           select fi;
           //max_files = max_files;
           
           foreach (var fi in max_files)
           {
               mf = Convert.ToInt32(fi.Value);
           }
            //ADO.NET code
            connect = new SqlConnection("Data Source=.\\SQLSRVR;AttachDbFilename=G:\\GitHub\\mmnote\\Data_base\\Data_base\\files_db.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");

            SqlDataReader get_files = null;
            
            try
            {
                connect.Open();

                string command_str = "select f_id, path, add_time from files where u_id = " + userid.ToString();


                SqlCommand gf = new SqlCommand(command_str, connect);
                

                get_files = gf.ExecuteReader();

                ArrayList elements = new ArrayList();

                while (get_files.Read())
                {
                    ToGridFiles fi = new ToGridFiles();
                    fi.ID = (int)get_files[0];
                    fi.Path = get_files[1].ToString();
                    fi.Time = (DateTime)get_files[2];
                    elements.Add(fi);
                    act_f++;
                }

                FilesGridView1.DataSource = elements;
                FilesGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                
            }
            finally
            {
                if (get_files != null) get_files.Close();
                if (connect != null) connect.Close();
            }
                    
            //-------------------------

            //LINQ
            var get_friends = from fl in database.friends
                              where fl.u2_id == userid
                              select fl.u1_id;


            foreach (var fl in get_friends)
            {
                var to_list = from u in database.users
                              where u.u_id == fl
                              select u.login;
                foreach (var l in to_list)
                {
                    friendsListBox.Items.Add(l);
                }
            }
            var toFilesCounter = mf-act_f;
            FilesCounterLabel.Text = "Files left: " + toFilesCounter.ToString();
            get_xml_stats();
        }

        private void get_xml_stats()
        {
            pay_doc = XDocument.Load("Payments.xml");
            var get_stats_f = from fil in pay_doc.Elements("payments").Elements("userdata").Elements()
                           where (string)fil.Attribute("idf") == userid.ToString()
                           select fil;
            string fi = "";
            foreach(var fil in get_stats_f)
            {
                fi = fil.Value;
            }
            var get_stats_m = from m in pay_doc.Elements("payments").Elements("userdata").Elements()
                              where (string)m.Attribute("idp") == userid.ToString()
                              select m;
            string mo = "";
            foreach (var mon in get_stats_m)
            {
                mo = mon.Value;
            }
            money = Convert.ToInt32(mo);
            PaymentsStatsLabel.Text = "Maximum files: " + mf.ToString() + " money spend: " + mo.ToString() + "$";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (to_close == true) this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label1.Visible == false)
            {
                button1.Text = "Cancel";
                label1.Visible = true;
                fileNameBox.Visible = true;
                addButton.Visible = true;
            }
            else
            {
                button1.Text = "Add new";
                label1.Visible = false;
                fileNameBox.Visible = false;
                addButton.Visible = false;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            file add_new = new file
            {
                u_id = userid,
                path = fileNameBox.Text,
                add_time = DateTime.Now
            };
            database.files.InsertOnSubmit(add_new);
            database.SubmitChanges();

            var get_files = from f in database.files
                            where f.u_id == userid
                            select new { f.f_id, f.path, f.add_time };
            FilesGridView1.DataSource = get_files;
            FilesGridView1.Refresh();
            
            label1.Visible = false;
            fileNameBox.Visible = false;
            addButton.Visible = false;
            button1.Text = "Add new";
            act_f++;
            var toFilesCounter = mf - act_f;
            FilesCounterLabel.Text = "Files left: " + toFilesCounter.ToString();
            if (toFilesCounter == 0)
                button1.Enabled = false;
        }

        private void friendsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var get_id = from u in database.users
                         where u.login == friendsListBox.SelectedItem
                         select u.u_id;
            
            foreach (var u in get_id)
            {
                var get_files = from f in database.files
                                where f.u_id == u
                                select new { f.f_id, f.path, f.add_time };
                dataGridView1.DataSource = get_files;
                dataGridView1.Refresh();
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
                button2.Text = "New friend";
            }
            else
            {
                panel1.Visible = true;
                button2.Text = "Cancel";
            }
        }

        private void addButton2_Click(object sender, EventArgs e)
        {
            var get_login = from u in database.users
                            where u.login == frndtextBox.Text
                            select new {u.u_id};

            int frnd_id = 0;
            foreach (var u in get_login)
            {
                frnd_id = u.u_id;
            }

            if (frnd_id == 0)
            {
                MessageBox.Show("No such user!", "Error!");
                return;
            }

            friend add_friend = new friend
            {
                u1_id = userid,
                u2_id = frnd_id,
                add_time = DateTime.Now
            };
            database.friends.InsertOnSubmit(add_friend);
            database.SubmitChanges();

            panel1.Visible = false;
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            payment pay = new payment(mf,userid);
            pay.ShowDialog();
            mf = pay.max_files;
            var toFC = mf - act_f;
            FilesCounterLabel.Text = "Files left: " + toFC.ToString();
            statusStrip1.Refresh();
            money = pay.money;
            PaymentsStatsLabel.Text = "Maximum files: " + mf.ToString() + " money spend: " + money.ToString() + "$";
        }
    }
    public class ToGridFiles
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private DateTime time;

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
