using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Data_base
{
    public partial class login : Form
    {
        public int uid = 6;
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            register reg = new register();
            reg.ShowDialog();
            Close();
        }
    }
}
