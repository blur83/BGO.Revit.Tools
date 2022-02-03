namespace BGO.Revit.Tools
{
    partial class formSelectWorkset
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formSelectWorkset));
            this.radioButtonActiveWorksetOnly = new System.Windows.Forms.RadioButton();
            this.radioButtonAllWorksets = new System.Windows.Forms.RadioButton();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButtonActiveWorksetOnly
            // 
            this.radioButtonActiveWorksetOnly.AutoSize = true;
            this.radioButtonActiveWorksetOnly.Checked = true;
            this.radioButtonActiveWorksetOnly.Location = new System.Drawing.Point(37, 41);
            this.radioButtonActiveWorksetOnly.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonActiveWorksetOnly.Name = "radioButtonActiveWorksetOnly";
            this.radioButtonActiveWorksetOnly.Size = new System.Drawing.Size(117, 17);
            this.radioButtonActiveWorksetOnly.TabIndex = 0;
            this.radioButtonActiveWorksetOnly.TabStop = true;
            this.radioButtonActiveWorksetOnly.Text = "Active workset only";
            this.radioButtonActiveWorksetOnly.UseVisualStyleBackColor = true;
            // 
            // radioButtonAllWorksets
            // 
            this.radioButtonAllWorksets.AutoSize = true;
            this.radioButtonAllWorksets.Location = new System.Drawing.Point(37, 80);
            this.radioButtonAllWorksets.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioButtonAllWorksets.Name = "radioButtonAllWorksets";
            this.radioButtonAllWorksets.Size = new System.Drawing.Size(81, 17);
            this.radioButtonAllWorksets.TabIndex = 1;
            this.radioButtonAllWorksets.Text = "All worksets";
            this.radioButtonAllWorksets.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(236, 158);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 19);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(165, 158);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(56, 19);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // formSelectWorkset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 207);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.radioButtonAllWorksets);
            this.Controls.Add(this.radioButtonActiveWorksetOnly);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "formSelectWorkset";
            this.Text = "Update Diagnostic Shared Parameters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonActiveWorksetOnly;
        private System.Windows.Forms.RadioButton radioButtonAllWorksets;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}