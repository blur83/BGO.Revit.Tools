﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BGO.Revit.Tools
{
    public partial class InfoLAForm : Form
    {
        public InfoLAForm()
        {
            InitializeComponent();
        }

        private void InfoLcaForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "https://agenceflac.sharepoint.com/sites/FlacBIM/SitePages/TrainingHome.aspx");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}