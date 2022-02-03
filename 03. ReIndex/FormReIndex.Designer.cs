namespace BGO.Revit.Tools
{
    partial class FormReIndex
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReIndex));
            this.textBoxStartIndex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDisplayFormat = new System.Windows.Forms.TextBox();
            this.radioButtonVerticalOrder = new System.Windows.Forms.RadioButton();
            this.radioButtonHorizontalOrder = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxSampleFormat = new System.Windows.Forms.TextBox();
            this.textBoxIndexParameterName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxStartIndex
            // 
            this.textBoxStartIndex.Location = new System.Drawing.Point(134, 52);
            this.textBoxStartIndex.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxStartIndex.Name = "textBoxStartIndex";
            this.textBoxStartIndex.Size = new System.Drawing.Size(76, 20);
            this.textBoxStartIndex.TabIndex = 0;
            this.textBoxStartIndex.Text = "StartIndex";
            this.textBoxStartIndex.TextChanged += new System.EventHandler(this.textBoxStartIndex_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Index:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Display format";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 118);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Numbering Order";
            // 
            // textBoxDisplayFormat
            // 
            this.textBoxDisplayFormat.Location = new System.Drawing.Point(134, 82);
            this.textBoxDisplayFormat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxDisplayFormat.Name = "textBoxDisplayFormat";
            this.textBoxDisplayFormat.Size = new System.Drawing.Size(76, 20);
            this.textBoxDisplayFormat.TabIndex = 4;
            this.textBoxDisplayFormat.Text = "DisplayFormat";
            this.textBoxDisplayFormat.TextChanged += new System.EventHandler(this.textBoxDisplayFormat_TextChanged);
            // 
            // radioButtonVerticalOrder
            // 
            this.radioButtonVerticalOrder.AutoSize = true;
            this.radioButtonVerticalOrder.Location = new System.Drawing.Point(229, 115);
            this.radioButtonVerticalOrder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonVerticalOrder.Name = "radioButtonVerticalOrder";
            this.radioButtonVerticalOrder.Size = new System.Drawing.Size(67, 17);
            this.radioButtonVerticalOrder.TabIndex = 5;
            this.radioButtonVerticalOrder.Text = "Vertically";
            this.radioButtonVerticalOrder.UseVisualStyleBackColor = true;
            this.radioButtonVerticalOrder.CheckedChanged += new System.EventHandler(this.radioButtonVerticalOrder_CheckedChanged);
            // 
            // radioButtonHorizontalOrder
            // 
            this.radioButtonHorizontalOrder.AutoSize = true;
            this.radioButtonHorizontalOrder.Checked = true;
            this.radioButtonHorizontalOrder.Location = new System.Drawing.Point(134, 115);
            this.radioButtonHorizontalOrder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonHorizontalOrder.Name = "radioButtonHorizontalOrder";
            this.radioButtonHorizontalOrder.Size = new System.Drawing.Size(79, 17);
            this.radioButtonHorizontalOrder.TabIndex = 6;
            this.radioButtonHorizontalOrder.TabStop = true;
            this.radioButtonHorizontalOrder.Text = "Horizontally";
            this.radioButtonHorizontalOrder.UseVisualStyleBackColor = true;
            this.radioButtonHorizontalOrder.CheckedChanged += new System.EventHandler(this.radioButtonHorizontalOrder_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(166, 165);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(56, 19);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(237, 165);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 19);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxSampleFormat
            // 
            this.textBoxSampleFormat.Enabled = false;
            this.textBoxSampleFormat.Location = new System.Drawing.Point(218, 84);
            this.textBoxSampleFormat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxSampleFormat.Name = "textBoxSampleFormat";
            this.textBoxSampleFormat.ReadOnly = true;
            this.textBoxSampleFormat.Size = new System.Drawing.Size(76, 20);
            this.textBoxSampleFormat.TabIndex = 9;
            this.textBoxSampleFormat.Text = "1";
            // 
            // textBoxIndexParameterName
            // 
            this.textBoxIndexParameterName.Location = new System.Drawing.Point(134, 20);
            this.textBoxIndexParameterName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxIndexParameterName.Name = "textBoxIndexParameterName";
            this.textBoxIndexParameterName.Size = new System.Drawing.Size(161, 20);
            this.textBoxIndexParameterName.TabIndex = 11;
            this.textBoxIndexParameterName.Text = "Parameter Name";
            this.textBoxIndexParameterName.TextChanged += new System.EventHandler(this.textBoxIndexParameterName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 23);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Parameter Name";
            // 
            // FormReIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 208);
            this.Controls.Add(this.textBoxIndexParameterName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxSampleFormat);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.radioButtonHorizontalOrder);
            this.Controls.Add(this.radioButtonVerticalOrder);
            this.Controls.Add(this.textBoxDisplayFormat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxStartIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormReIndex";
            this.Text = "Re-indexation of selected elements";
            this.Load += new System.EventHandler(this.FormReIndex_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxStartIndex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDisplayFormat;
        private System.Windows.Forms.RadioButton radioButtonVerticalOrder;
        private System.Windows.Forms.RadioButton radioButtonHorizontalOrder;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxSampleFormat;
        private System.Windows.Forms.TextBox textBoxIndexParameterName;
        private System.Windows.Forms.Label label4;
    }
}