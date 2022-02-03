namespace BGO.Revit.Tools
{
    partial class FormCreate3DView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button buttonCancel;
            this.buttonCreate = new System.Windows.Forms.Button();
            this.comboBoxScopeBoxes = new System.Windows.Forms.ComboBox();
            this.groupBox_LevelLimits = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxLevelEnd = new System.Windows.Forms.ComboBox();
            this.comboBoxLevelStart = new System.Windows.Forms.ComboBox();
            this.groupBoxXYLimits = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxElementId = new System.Windows.Forms.TextBox();
            this.radioButtonByElement = new System.Windows.Forms.RadioButton();
            this.radioButtonByScopeBox = new System.Windows.Forms.RadioButton();
            this.radioButtonByGrid = new System.Windows.Forms.RadioButton();
            this.groupBoxGridLimits = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxYGridEnd = new System.Windows.Forms.ComboBox();
            this.comboBoxYGridStart = new System.Windows.Forms.ComboBox();
            this.comboBoxXGridEnd = new System.Windows.Forms.ComboBox();
            this.comboBoxXGridStart = new System.Windows.Forms.ComboBox();
            buttonCancel = new System.Windows.Forms.Button();
            this.groupBox_LevelLimits.SuspendLayout();
            this.groupBoxXYLimits.SuspendLayout();
            this.groupBoxGridLimits.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Location = new System.Drawing.Point(512, 390);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonCreate
            // 
            this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreate.Enabled = false;
            this.buttonCreate.Location = new System.Drawing.Point(512, 348);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(75, 23);
            this.buttonCreate.TabIndex = 0;
            this.buttonCreate.Text = "Create";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // comboBoxScopeBoxes
            // 
            this.comboBoxScopeBoxes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxScopeBoxes.Enabled = false;
            this.comboBoxScopeBoxes.FormattingEnabled = true;
            this.comboBoxScopeBoxes.Location = new System.Drawing.Point(129, 201);
            this.comboBoxScopeBoxes.Name = "comboBoxScopeBoxes";
            this.comboBoxScopeBoxes.Size = new System.Drawing.Size(442, 21);
            this.comboBoxScopeBoxes.Sorted = true;
            this.comboBoxScopeBoxes.TabIndex = 13;
            this.comboBoxScopeBoxes.SelectedIndexChanged += new System.EventHandler(this.comboBoxScopeBoxes_SelectedIndexChanged);
            // 
            // groupBox_LevelLimits
            // 
            this.groupBox_LevelLimits.Controls.Add(this.label7);
            this.groupBox_LevelLimits.Controls.Add(this.label6);
            this.groupBox_LevelLimits.Controls.Add(this.comboBoxLevelEnd);
            this.groupBox_LevelLimits.Controls.Add(this.comboBoxLevelStart);
            this.groupBox_LevelLimits.Location = new System.Drawing.Point(16, 327);
            this.groupBox_LevelLimits.Name = "groupBox_LevelLimits";
            this.groupBox_LevelLimits.Size = new System.Drawing.Size(444, 102);
            this.groupBox_LevelLimits.TabIndex = 14;
            this.groupBox_LevelLimits.TabStop = false;
            this.groupBox_LevelLimits.Text = "Level Limits";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "To:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "From:";
            // 
            // comboBoxLevelEnd
            // 
            this.comboBoxLevelEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLevelEnd.FormattingEnabled = true;
            this.comboBoxLevelEnd.Location = new System.Drawing.Point(56, 58);
            this.comboBoxLevelEnd.Name = "comboBoxLevelEnd";
            this.comboBoxLevelEnd.Size = new System.Drawing.Size(361, 21);
            this.comboBoxLevelEnd.Sorted = true;
            this.comboBoxLevelEnd.TabIndex = 9;
            this.comboBoxLevelEnd.SelectedIndexChanged += new System.EventHandler(this.comboBoxLevelEnd_SelectedIndexChanged);
            // 
            // comboBoxLevelStart
            // 
            this.comboBoxLevelStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLevelStart.FormattingEnabled = true;
            this.comboBoxLevelStart.Location = new System.Drawing.Point(56, 31);
            this.comboBoxLevelStart.Name = "comboBoxLevelStart";
            this.comboBoxLevelStart.Size = new System.Drawing.Size(361, 21);
            this.comboBoxLevelStart.Sorted = true;
            this.comboBoxLevelStart.TabIndex = 8;
            this.comboBoxLevelStart.SelectedIndexChanged += new System.EventHandler(this.comboBoxLevelStart_SelectedIndexChanged);
            // 
            // groupBoxXYLimits
            // 
            this.groupBoxXYLimits.Controls.Add(this.label5);
            this.groupBoxXYLimits.Controls.Add(this.textBoxElementId);
            this.groupBoxXYLimits.Controls.Add(this.radioButtonByElement);
            this.groupBoxXYLimits.Controls.Add(this.radioButtonByScopeBox);
            this.groupBoxXYLimits.Controls.Add(this.radioButtonByGrid);
            this.groupBoxXYLimits.Controls.Add(this.groupBoxGridLimits);
            this.groupBoxXYLimits.Controls.Add(this.comboBoxScopeBoxes);
            this.groupBoxXYLimits.Location = new System.Drawing.Point(16, 12);
            this.groupBoxXYLimits.Name = "groupBoxXYLimits";
            this.groupBoxXYLimits.Size = new System.Drawing.Size(610, 295);
            this.groupBoxXYLimits.TabIndex = 16;
            this.groupBoxXYLimits.TabStop = false;
            this.groupBoxXYLimits.Text = "XY Limits";
            this.groupBoxXYLimits.Enter += new System.EventHandler(this.groupBoxXYLimits_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(158, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Element Id :";
            // 
            // textBoxElementId
            // 
            this.textBoxElementId.Enabled = false;
            this.textBoxElementId.Location = new System.Drawing.Point(235, 255);
            this.textBoxElementId.Name = "textBoxElementId";
            this.textBoxElementId.Size = new System.Drawing.Size(121, 20);
            this.textBoxElementId.TabIndex = 20;
            this.textBoxElementId.TextChanged += new System.EventHandler(this.textBoxElementId_TextChanged);
            // 
            // radioButtonByElement
            // 
            this.radioButtonByElement.AutoSize = true;
            this.radioButtonByElement.Location = new System.Drawing.Point(31, 257);
            this.radioButtonByElement.Name = "radioButtonByElement";
            this.radioButtonByElement.Size = new System.Drawing.Size(95, 17);
            this.radioButtonByElement.TabIndex = 19;
            this.radioButtonByElement.Text = "By 3D Element";
            this.radioButtonByElement.UseVisualStyleBackColor = true;
            this.radioButtonByElement.CheckedChanged += new System.EventHandler(this.radioButtonByElement_CheckedChanged);
            // 
            // radioButtonByScopeBox
            // 
            this.radioButtonByScopeBox.AutoSize = true;
            this.radioButtonByScopeBox.Location = new System.Drawing.Point(31, 201);
            this.radioButtonByScopeBox.Name = "radioButtonByScopeBox";
            this.radioButtonByScopeBox.Size = new System.Drawing.Size(92, 17);
            this.radioButtonByScopeBox.TabIndex = 18;
            this.radioButtonByScopeBox.Text = "By Scope Box";
            this.radioButtonByScopeBox.UseVisualStyleBackColor = true;
            this.radioButtonByScopeBox.CheckedChanged += new System.EventHandler(this.radioButtonByScopeBox_CheckedChanged);
            // 
            // radioButtonByGrid
            // 
            this.radioButtonByGrid.AutoSize = true;
            this.radioButtonByGrid.Checked = true;
            this.radioButtonByGrid.Location = new System.Drawing.Point(31, 19);
            this.radioButtonByGrid.Name = "radioButtonByGrid";
            this.radioButtonByGrid.Size = new System.Drawing.Size(59, 17);
            this.radioButtonByGrid.TabIndex = 17;
            this.radioButtonByGrid.TabStop = true;
            this.radioButtonByGrid.Text = "By Grid";
            this.radioButtonByGrid.UseVisualStyleBackColor = true;
            this.radioButtonByGrid.CheckedChanged += new System.EventHandler(this.radioButtonByGrid_CheckedChanged);
            // 
            // groupBoxGridLimits
            // 
            this.groupBoxGridLimits.Controls.Add(this.label4);
            this.groupBoxGridLimits.Controls.Add(this.label3);
            this.groupBoxGridLimits.Controls.Add(this.label2);
            this.groupBoxGridLimits.Controls.Add(this.label1);
            this.groupBoxGridLimits.Controls.Add(this.comboBoxYGridEnd);
            this.groupBoxGridLimits.Controls.Add(this.comboBoxYGridStart);
            this.groupBoxGridLimits.Controls.Add(this.comboBoxXGridEnd);
            this.groupBoxGridLimits.Controls.Add(this.comboBoxXGridStart);
            this.groupBoxGridLimits.Location = new System.Drawing.Point(129, 19);
            this.groupBoxGridLimits.Name = "groupBoxGridLimits";
            this.groupBoxGridLimits.Size = new System.Drawing.Size(442, 162);
            this.groupBoxGridLimits.TabIndex = 16;
            this.groupBoxGridLimits.TabStop = false;
            this.groupBoxGridLimits.Text = "Grid Limits";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Y Grid";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "X Grid";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "End Limit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(103, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Start Limit";
            // 
            // comboBoxYGridEnd
            // 
            this.comboBoxYGridEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYGridEnd.FormattingEnabled = true;
            this.comboBoxYGridEnd.Location = new System.Drawing.Point(280, 107);
            this.comboBoxYGridEnd.Name = "comboBoxYGridEnd";
            this.comboBoxYGridEnd.Size = new System.Drawing.Size(121, 21);
            this.comboBoxYGridEnd.Sorted = true;
            this.comboBoxYGridEnd.TabIndex = 15;
            this.comboBoxYGridEnd.SelectedIndexChanged += new System.EventHandler(this.comboBoxYGridEnd_SelectedIndexChanged);
            // 
            // comboBoxYGridStart
            // 
            this.comboBoxYGridStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYGridStart.FormattingEnabled = true;
            this.comboBoxYGridStart.Location = new System.Drawing.Point(106, 107);
            this.comboBoxYGridStart.Name = "comboBoxYGridStart";
            this.comboBoxYGridStart.Size = new System.Drawing.Size(121, 21);
            this.comboBoxYGridStart.Sorted = true;
            this.comboBoxYGridStart.TabIndex = 14;
            this.comboBoxYGridStart.SelectedIndexChanged += new System.EventHandler(this.comboBoxYGridStart_SelectedIndexChanged);
            // 
            // comboBoxXGridEnd
            // 
            this.comboBoxXGridEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXGridEnd.FormattingEnabled = true;
            this.comboBoxXGridEnd.Location = new System.Drawing.Point(280, 61);
            this.comboBoxXGridEnd.Name = "comboBoxXGridEnd";
            this.comboBoxXGridEnd.Size = new System.Drawing.Size(121, 21);
            this.comboBoxXGridEnd.Sorted = true;
            this.comboBoxXGridEnd.TabIndex = 13;
            this.comboBoxXGridEnd.SelectedIndexChanged += new System.EventHandler(this.comboBoxXGridEnd_SelectedIndexChanged);
            // 
            // comboBoxXGridStart
            // 
            this.comboBoxXGridStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXGridStart.FormattingEnabled = true;
            this.comboBoxXGridStart.Location = new System.Drawing.Point(106, 61);
            this.comboBoxXGridStart.Name = "comboBoxXGridStart";
            this.comboBoxXGridStart.Size = new System.Drawing.Size(121, 21);
            this.comboBoxXGridStart.Sorted = true;
            this.comboBoxXGridStart.TabIndex = 12;
            this.comboBoxXGridStart.SelectedIndexChanged += new System.EventHandler(this.comboBoxXGridStart_SelectedIndexChanged);
            // 
            // FormCreate3DView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 445);
            this.Controls.Add(this.groupBoxXYLimits);
            this.Controls.Add(this.groupBox_LevelLimits);
            this.Controls.Add(buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.Name = "FormCreate3DView";
            this.Text = "Create a 3D View From XY And Level Limits";
            this.groupBox_LevelLimits.ResumeLayout(false);
            this.groupBox_LevelLimits.PerformLayout();
            this.groupBoxXYLimits.ResumeLayout(false);
            this.groupBoxXYLimits.PerformLayout();
            this.groupBoxGridLimits.ResumeLayout(false);
            this.groupBoxGridLimits.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.ComboBox comboBoxScopeBoxes;
        private System.Windows.Forms.GroupBox groupBox_LevelLimits;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxLevelEnd;
        private System.Windows.Forms.ComboBox comboBoxLevelStart;
        private System.Windows.Forms.GroupBox groupBoxXYLimits;
        private System.Windows.Forms.RadioButton radioButtonByElement;
        private System.Windows.Forms.RadioButton radioButtonByScopeBox;
        private System.Windows.Forms.RadioButton radioButtonByGrid;
        private System.Windows.Forms.GroupBox groupBoxGridLimits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxYGridEnd;
        private System.Windows.Forms.ComboBox comboBoxYGridStart;
        private System.Windows.Forms.ComboBox comboBoxXGridEnd;
        private System.Windows.Forms.ComboBox comboBoxXGridStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxElementId;
    }
}