using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace BGO.Revit.Tools
{
    public partial class FormReIndex : Form
    {
        string _parameterName;
        bool _parameterNameOK;
        
        int _startIndex;
        bool _startIndexOK;
        
        string _displayFormat;
        bool _displayFormatOK;
               
        int _renumOrder;
        
        ReIndex _extCmd;

        public FormReIndex(ReIndex extCmd)
        {
            InitializeComponent();
            _extCmd = extCmd;         
        }

        private void FormReIndex_Load(object sender, EventArgs e)
        {
            _parameterName = _extCmd.LastIndexParameterName;
            _startIndex = _extCmd.LastIndex+1;
            _displayFormat = _extCmd.LastFormat;
            _renumOrder = _extCmd.LastOrder;

            textBoxIndexParameterName.Text = _parameterName;
            textBoxStartIndex.Text = _startIndex.ToString();
            textBoxDisplayFormat.Text = _displayFormat;
            radioButtonHorizontalOrder.Checked = (_renumOrder == 0);
            radioButtonVerticalOrder.Checked = (_renumOrder == 1);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try 
	        {
                _extCmd.Renumber(_parameterName, _startIndex, _displayFormat, _renumOrder);
            }
	        catch (Exception)
	        {
                MessageBox.Show("Invalid start index");
	        }
        }

        private void textBoxIndexParameterName_TextChanged(object sender, EventArgs e)
        {
            _parameterName = (sender as TextBox).Text;
            _parameterNameOK = _parameterName != "";
            CheckOKButton();
        }

        private void textBoxStartIndex_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _startIndex = Convert.ToInt32((sender as TextBox).Text);
                _startIndexOK = true;
                ShowSampleValue();
            }
            catch (Exception)
            {
                _startIndexOK =false;
            }
            CheckOKButton();
        }

        private void textBoxDisplayFormat_TextChanged(object sender, EventArgs e)
        {
            try 
	        {
                _displayFormat = (sender as TextBox).Text;
		        _displayFormatOK = true;
                ShowSampleValue();
	        }
	        catch (Exception)
	        {
		        _displayFormatOK = false;
	        }
            CheckOKButton();
        }

        private void radioButtonHorizontalOrder_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked) _renumOrder = 0;
        }

        private void radioButtonVerticalOrder_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked) _renumOrder = 1;
        }

        private void ShowSampleValue()
        {
            try
            {
                string s = _startIndex.ToString(_displayFormat);
                textBoxSampleFormat.Text = s;
            }
            catch (Exception)
            {
                textBoxSampleFormat.Text = "Bad format";
            }
        }

        private void CheckOKButton()
        {
            buttonOK.Enabled = _parameterNameOK && _startIndexOK && _displayFormatOK;
        }

     }
}
