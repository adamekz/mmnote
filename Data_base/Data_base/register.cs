using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Transactions;

namespace Data_base
{
    public partial class register : Form
    {
        public int user_id = 0;
        public register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            file_dataclassesDataContext database = new file_dataclassesDataContext();

            var if_exist = from u in database.users
                           where u.login == textBox1.Text
                           select new { u.u_id };
            var ex = 0;
            foreach (var u in if_exist)
            {
                ex++;
                
            }
            if (ex != 0)
            {
                statuslabel.Text = "This login is used!";
                return;
            }
            if (textBox2.Text != textBox3.Text)
            {
                statuslabel.Text = "Password boxes are not equal!";
                return;
            }
            using (var trans = new TransactionScope())
            {
                DateTime teraz = DateTime.Now;
                user register_new = new user
                {
                    login = textBox1.Text,
                    password = textBox2.Text,
                    first_name = textBox4.Text,
                    last_name = textBox5.Text,
                    email = textBox6.Text,
                    join_date = teraz
                };
                database.users.InsertOnSubmit(register_new);

                try
                {
                    database.SubmitChanges();
                }
                catch (Exception)
                {

                }
                var get_uid = from usr in database.users
                              where usr.login == textBox1.Text
                              select new { usr.u_id };

                foreach (var usr in get_uid)
                {
                    user_id = usr.u_id;
                }
                action reg_act = new action
                {
                    u_id = user_id,
                    act_type = "REG",
                    action_time = teraz
                };
                database.actions.InsertOnSubmit(reg_act);
                database.SubmitChanges();
                trans.Complete();
            }
            if (user_id != 0)
            {
                XDocument pay_doc = XDocument.Load("Payments.xml");

                pay_doc.Element("payments").Add(new XElement("userdata",
                                                                new XElement("u_files", "5", new XAttribute("idf", user_id.ToString()))));

                pay_doc.Element("payments").Elements("userdata").Elements("u_files")
                    .Where(u_f => u_f.Attribute("idf").Value == user_id.ToString()).FirstOrDefault()
                    .AddAfterSelf(new XElement("u_paid", "0", new XAttribute("idp", user_id.ToString())));
                pay_doc.Save("Payments.xml");
            }
            Close();
        }
    }
}
