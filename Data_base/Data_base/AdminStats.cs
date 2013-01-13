using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Windows.Forms;

namespace Data_base
{
    public partial class AdminStats : Form
    {
        file_dataclassesDataContext database;
        public AdminStats()
        {
            InitializeComponent();
            database = new file_dataclassesDataContext();
            var get_users = from u in database.users
                            select new { u.u_id, u.login, u.first_name, u.last_name, u.email, u.join_date };
            UsersGridView1.DataSource = get_users;

            var get_actions = from a in database.actions
                              select new { a.a_id, a.u_id, a.act_type, a.action_time, a.fi_id };
            ActionGridView1.DataSource = get_actions;
        }
    }
}
