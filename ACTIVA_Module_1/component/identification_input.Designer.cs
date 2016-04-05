namespace ACTIVA_Module_1.component
{
    partial class identification_input
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.field_label = new System.Windows.Forms.Label();
            this.Field_Input = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // field_label
            // 
            this.field_label.AutoSize = true;
            this.field_label.Location = new System.Drawing.Point(3, 6);
            this.field_label.Name = "field_label";
            this.field_label.Size = new System.Drawing.Size(35, 13);
            this.field_label.TabIndex = 0;
            this.field_label.Text = "label1";
            // 
            // Field_Input
            // 
            this.Field_Input.Location = new System.Drawing.Point(6, 22);
            this.Field_Input.Name = "Field_Input";
            this.Field_Input.Size = new System.Drawing.Size(199, 20);
            this.Field_Input.TabIndex = 1;
            this.Field_Input.TextChanged += new System.EventHandler(this.Field_Input_TextChanged);
            this.Field_Input.Enter += new System.EventHandler(this.Field_Input_Enter);
            this.Field_Input.Validating += new System.ComponentModel.CancelEventHandler(this.Field_Input_Validating);
            // 
            // identification_input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Field_Input);
            this.Controls.Add(this.field_label);
            this.Name = "identification_input";
            this.Size = new System.Drawing.Size(210, 46);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label field_label;
        public System.Windows.Forms.TextBox Field_Input;

    }
}
