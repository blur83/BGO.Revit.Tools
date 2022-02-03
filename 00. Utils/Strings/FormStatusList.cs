/*
 * Revit Macro created by SharpDevelop
 * User: FANGCHAO
 * Date: 19/12/2013
 * Time: 19:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace BGO.Revit.Tools
{
	/// <summary>
	/// Description of FormStatusList.
	/// </summary>
	public partial class FormStatusList : Form
	{
		public FormStatusList(List<string> StatusList)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			this.listBox1.DataSource = StatusList;
		}
		
		void ButtonSaveClick(object sender, EventArgs e)
		{
			var sfd = new SaveFileDialog();
			sfd.Title = "Save To File";
			sfd.FileName = "Status List.txt";
			sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; 
			sfd.FilterIndex=1;
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				var FileName = sfd.FileName;
				if (!string.IsNullOrEmpty(FileName))
				{
					var sw = File.CreateText(FileName);
					foreach (var s in this.listBox1.Items)
					{
						sw.WriteLine(s);
					}
					sw.Close();	
				}
			}
		}

        private void FormStatusList_Load(object sender, EventArgs e)
        {

        }
    }
}
