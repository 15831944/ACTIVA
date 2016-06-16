namespace ACTIVA_Module_1
{
    partial class Renommer_Ouvrage
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
            this.label1 = new System.Windows.Forms.Label();
            this.RenameTb = new System.Windows.Forms.TextBox();
            this.ValRenomBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom (sans caractères spéciaux) :";
            // 
            // RenameTb
            // 
            this.RenameTb.Location = new System.Drawing.Point(13, 23);
            this.RenameTb.Name = "RenameTb";
            this.RenameTb.Size = new System.Drawing.Size(198, 23);
            this.RenameTb.TabIndex = 1;
            // 
            // ValRenomBt
            // 
            this.ValRenomBt.Image = global::ACTIVA_Module_1.Properties.Resources.accept1;
            this.ValRenomBt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ValRenomBt.Location = new System.Drawing.Point(139, 51);
            this.ValRenomBt.Name = "ValRenomBt";
            this.ValRenomBt.Size = new System.Drawing.Size(72, 24);
            this.ValRenomBt.TabIndex = 2;
            this.ValRenomBt.Text = "Valider";
            this.ValRenomBt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ValRenomBt.UseVisualStyleBackColor = true;
            this.ValRenomBt.Click += new System.EventHandler(this.ValRenomBt_Click);
            // 
            // Renommer_Ouvrage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 82);
            this.Controls.Add(this.ValRenomBt);
            this.Controls.Add(this.RenameTb);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Renommer_Ouvrage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nouveau nom de l\'ouvrage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RenameTb;
        private System.Windows.Forms.Button ValRenomBt;
    }
}