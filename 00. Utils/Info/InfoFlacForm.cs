using System;
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
    public partial class InfoFlacForm : Form
    {
        public InfoFlacForm()
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
            System.Diagnostics.Process.Start("explorer", "https://www.leclercqassocies.fr/fr");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
