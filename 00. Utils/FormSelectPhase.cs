using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace BGO.Revit.Tools
{
    public partial class FormSelectPhase : System.Windows.Forms.Form
    {
        public FormSelectPhase(Document doc)
        {
            InitializeComponent();
            foreach (Phase ph in doc.Phases)
            {
                comboBox1.Items.Add(ph.Name);
            }
            if (doc.Phases.Size > 1) comboBox1.SelectedIndex = 1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Tag = comboBox1.SelectedIndex;
        }
    }
}
