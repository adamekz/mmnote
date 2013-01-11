using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;

namespace Data_base
{
    public partial class payment : Form
    {
        public int max_files;
        public int user_id;
        public int money;
        public XDocument pay_doc;
        public payment(int max, int us_id)
        {
            InitializeComponent();
            max_files = max;
            user_id = us_id;
            pay_doc = XDocument.Load("Payments.xml");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {             

                var get_money = from m in pay_doc.Element("payments").Elements("userdata").Elements("u_paid")
                                where (string)m.Attribute("idp").Value == user_id.ToString()
                                select m;
                
                foreach (var m in get_money)
                {
                    money = Convert.ToInt32(m.Value);
                }
                
                money++;

                var inc = max_files + 5;
                max_files += 5;
                XElement ref_to_files =  pay_doc.Element("payments").Elements("userdata").Elements("u_files").Where(id => (string)id.Attribute("idf").Value == user_id.ToString()).FirstOrDefault();

                ref_to_files.SetValue(inc.ToString());

                XElement ref_to_paid = pay_doc.Element("payments").Elements("userdata").Elements("u_paid").Where(id => (string)id.Attribute("idp").Value == user_id.ToString()).FirstOrDefault();

                ref_to_paid.SetValue(money.ToString());

                pay_doc.Save("Payments.xml");

                Close();
            }
            else
            {
                MessageBox.Show("Invalid code!", "Error");
                return;
            }
        }
    }
}
