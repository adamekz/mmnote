using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Data_base
{
    public partial class login : Form
    {
        public int uid = 0;
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            file_dataclassesDataContext database = new file_dataclassesDataContext();
           /* var log_data = from u in database.users
                           where u.login == textBox1.Text && u.password == textBox2.Text
                           select new { u.u_id };*/
            SqlConnection conn = new SqlConnection("Data Source=.\\SQLSRVR;AttachDbFilename=G:\\GitHub\\mmnote\\Data_base\\Data_base\\files_db.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");

            SqlDataReader log_data = null;
            var ex = 0;
            string quer = "select u_id from users where login = '"+textBox1.Text+"' and password = '"+textBox2.Text+"'";

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(quer, conn);

                log_data = cmd.ExecuteReader();
            
                while(log_data.Read())
                {
                    ex++;
                    uid = (int)log_data[0];
                }

            }
            finally
            {
                if (conn != null) conn.Close();
            }
            
            
            if (ex == 0)
            {
                label3.Text = "Inncorect login or password!";
                return;
            }
            action log_act = new action
            {
                u_id = uid,
                act_type = "LOG",
                action_time = DateTime.Now
            };
            database.actions.InsertOnSubmit(log_act);
            database.SubmitChanges();
            Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            register reg = new register();
            reg.ShowDialog();
            uid = reg.user_id;
            Close();
        }

        private void login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                file_dataclassesDataContext database = new file_dataclassesDataContext();
                var log_data = from u in database.users
                               where u.login == textBox1.Text && u.password == textBox2.Text
                               select new { u.u_id };
                var ex = 0;
                foreach (var u in log_data)
                {
                    ex++;
                    uid = u.u_id;
                }
                if (ex == 0)
                {
                    label3.Text = "Inncorect login or password!";
                    return;
                }
                action log_act = new action
                {
                    u_id = uid,
                    act_type = "LOG",
                    action_time = DateTime.Now
                };
                database.actions.InsertOnSubmit(log_act);
                database.SubmitChanges();
                Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                file_dataclassesDataContext database = new file_dataclassesDataContext();
                var log_data = from u in database.users
                               where u.login == textBox1.Text && u.password == textBox2.Text
                               select new { u.u_id };
                var ex = 0;
                foreach (var u in log_data)
                {
                    ex++;
                    uid = u.u_id;
                }
                if (ex == 0)
                {
                    label3.Text = "Inncorect login or password!";
                    return;
                }
                Close();
            }
        }
    }
}
