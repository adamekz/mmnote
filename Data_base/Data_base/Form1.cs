﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Data_base
{
    public partial class Form1 : Form
    {
        private int userid = 0;
        public Form1()
        {
            InitializeComponent();
            login log = new login();
            Application.Run(log);
            userid = log.uid;
            MessageBox.Show(userid.ToString());
        }
    }
}
