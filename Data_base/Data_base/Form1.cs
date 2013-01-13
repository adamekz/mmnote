using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//ADO.NET
using System.Data.SqlClient;
//LINQ
using System.Linq;
using System.Xml.Linq;

using System.Transactions;
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

            List<Data_base.UserRegisterResult> reg_date = database.UserRegister(userid).ToList();

            foreach (var r in reg_date)
            {
                RegDateLabel.Text = "Registration of " + r.login.ToString() + ": " + r.join_date.ToString();
            }
            UpdateHistory();
            UserInfo();

            //dla admina
            if (userid == 1) button3.Visible = true;
        }

        private void UpdateHistory()
        {
            var get_history = from a in database.actions
                              where a.u_id == userid
                              select new { a.action_time, a.act_type, a.file.path };
            HistoryGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            HistoryGrid.DataSource = get_history;
            HistoryGrid.Refresh();

        }

        private void UserInfo()
        {
            var UserData = from u in database.users
                           where u.u_id == userid
                           select new { u.login, u.first_name, u.last_name, u.email };
            foreach (var u in UserData)
            {
                LoginLabel.Text = "Login: " + u.login;
                FNameBox.Text = u.first_name;
                LNameBox.Text = u.last_name;
                EmailBox.Text = u.email;
            }
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
            using (var trans = new TransactionScope())
            {
                DateTime teraz = DateTime.Now;
                file add_new = new file
                {
                    u_id = userid,
                    path = fileNameBox.Text,
                    add_time = teraz
                };
                database.files.InsertOnSubmit(add_new);


                database.SubmitChanges();

                var get_files = from f in database.files
                                where f.u_id == userid
                                select new { f.f_id, f.path, f.add_time };
                FilesGridView1.DataSource = get_files;
                FilesGridView1.Refresh();
                var fid = 0;
                foreach (var f in get_files)
                {
                    fid = f.f_id;
                }
                action fileadd_act = new action
                {
                    u_id = userid,
                    act_type = "AFI",
                    action_time = teraz,
                    fi_id = fid
                };
                database.actions.InsertOnSubmit(fileadd_act);
                database.SubmitChanges();

                trans.Complete();
            }
            UpdateHistory();

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
            dataGridView1.Enabled = false;
            foreach (var u in get_id)
            {
               /* var get_files = from f in database.files
                                where f.u_id == u
                                select new { f.f_id, f.path, f.add_time };*/
                dataGridView1.DataSource = database.GetFiles(u);
                dataGridView1.Refresh();
            }
            UpdateHistory();
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
            using (var trans = new TransactionScope())
            {
                DateTime teraz = DateTime.Now;
                friend add_friend = new friend
                {
                    u1_id = userid,
                    u2_id = frnd_id,
                    add_time = teraz
                };
                database.friends.InsertOnSubmit(add_friend);

                action fradd_act = new action
                {
                    u_id = userid,
                    act_type = "AFR",
                    action_time = teraz
                };
                database.actions.InsertOnSubmit(fradd_act);

                database.SubmitChanges();

                trans.Complete();
            }
            UpdateHistory();
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
            UpdateHistory();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            AddedFilesLabel.Text = "Files: " + database.GetCount(Convert.ToDateTime(e.Start).Date,userid).Value.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are You sure?", "Closing",MessageBoxButtons.YesNo) == DialogResult.No) e.Cancel = true;            
        }

        private void editUserButton_Click_1(object sender, EventArgs e)
        {
            if ((FNameBox.Text == "") || (LNameBox.Text == "") || (EmailBox.Text == "")) { MessageBox.Show("Null values not allowed!", "Error!"); return; }
            user update_usr = database.users.Single(i => i.u_id == userid);
            update_usr.first_name = FNameBox.Text;
            update_usr.last_name = LNameBox.Text;
            update_usr.email = EmailBox.Text;
            database.SubmitChanges();
            
            /*var check = from u in database.users
                        where u.u_id == userid
                        select new { u.first_name, u.last_name, u.email };
            foreach (var u in check)
            {
                MessageBox.Show(u.first_name + " " + u.last_name + " " + u.email);
            }*/
            var new_title = from u in database.users
                            where u.u_id == userid
                            select new { u.first_name, u.last_name };
            foreach (var u in new_title) this.Text = "FilesManager user: " + u.first_name + " " + u.last_name;
        }

        private void FilesGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (FilesGridView1[e.ColumnIndex, e.RowIndex].Value.ToString() == "") { MessageBox.Show("Null values not allowed!", "Error!"); return; }
            }
            catch (Exception)
            {
                MessageBox.Show("Null values not allowed!", "Error!"); return;
            }

            connect = new SqlConnection("Data Source=.\\SQLSRVR;AttachDbFilename=G:\\GitHub\\mmnote\\Data_base\\Data_base\\files_db.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
            string update = "UPDATE files SET path = '" + FilesGridView1[e.ColumnIndex, e.RowIndex].Value.ToString() + "' WHERE f_id = " + FilesGridView1[0, e.RowIndex].Value.ToString();
            using (var trans = new TransactionScope())
            {
                try
                {
                    connect.Open();
                    SqlCommand cmd = new SqlCommand(update);
                    cmd.Connection = connect;
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    if (connect != null) connect.Close();
                }
                trans.Complete();
            }
            /*
            file update = database.files.Single(i => i.f_id == Convert.ToInt32(FilesGridView1[0, e.RowIndex].Value));
            update.path = FilesGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
            database.SubmitChanges();*/
        }

        private void delbutton_Click(object sender, EventArgs e)
        {
            if (delpanel.Visible == true)
            {
                delpanel.Visible = false;
                delbutton.Text = "Delete";
            }
            else
            {
                delpanel.Visible = true;
                delbutton.Text = "Cancel";
            }
        }

        private void confDelButton_Click(object sender, EventArgs e)
        {
            if (deltextBox.Text == "") { MessageBox.Show("Set file ID!", "Error!"); return; }
            file del_f = database.files.Where(f => f.f_id == Convert.ToInt32(deltextBox.Text)).Single();
            action del_a = database.actions.Where(a => a.fi_id == Convert.ToInt32(deltextBox.Text)).Single();
            using (var trans = new TransactionScope())
            {
                database.actions.DeleteOnSubmit(del_a);
                database.files.DeleteOnSubmit(del_f);
                database.SubmitChanges();
                trans.Complete();
            }
            
            delpanel.Visible = false;
            deltextBox.Text = "";

            FilesGridView1.DataSource = database.GetFiles(userid);
            FilesGridView1.Refresh();           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminStats adm = new AdminStats();
            adm.ShowDialog();
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
