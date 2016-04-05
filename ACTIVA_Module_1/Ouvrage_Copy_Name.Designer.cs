namespace ACTIVA_Module_1
{
    partial class Ouvrage_Copy_Name
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
            this.nameTb = new System.Windows.Forms.TextBox();
            this.ValBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom (sans caractères spéciaux) :";
            // 
            // nameTb
            // 
            this.nameTb.Location = new System.Drawing.Point(13, 23);
            this.nameTb.Name = "nameTb";
            this.nameTb.Size = new System.Drawing.Size(198, 20);
            this.nameTb.TabIndex = 1;
            // 
            // ValBt
            // 
            this.ValBt.Image = global::ACTIVA_Module_1.Properties.Resources.accept1;
            this.ValBt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ValBt.Location = new System.Drawing.Point(139, 51);
            this.ValBt.Name = "ValBt";
            this.ValBt.Size = new System.Drawing.Size(72, 24);
            this.ValBt.TabIndex = 2;
            this.ValBt.Text = "Valider";
            this.ValBt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ValBt.UseVisualStyleBackColor = true;
            this.ValBt.Click += new System.EventHandler(this.ValBt_Click);
            // 
            // Ouvrage_Copy_Name
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 82);
            this.Controls.Add(this.ValBt);
            this.Controls.Add(this.nameTb);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Ouvrage_Copy_Name";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nom du nouvel ouvrage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTb;
        private System.Windows.Forms.Button ValBt;
    }
}