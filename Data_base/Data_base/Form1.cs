using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Data_base
{
    public partial class Form1 : Form
    {
        file_dataclassesDataContext database;
        private int userid = 0;
        private bool to_close = false;
        public Form1()
        {
            InitializeComponent();
            database = new file_dataclassesDataContext();
            login log = new login();
            Application.Run(log);
            userid = log.uid;
            if (userid == 0) { to_close = true; return; }
            //MessageBox.Show(userid.ToString());
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
            var get_files = from f in database.files
                            where f.u_id == userid
                            select new { f.f_id, f.path, f.add_time };
            FilesGridView1.DataSource = get_files;

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
    }
}
