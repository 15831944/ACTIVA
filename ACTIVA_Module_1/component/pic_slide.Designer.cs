namespace ACTIVA_Module_1.component
{
    partial class pic_slide
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.c1PictureBox1 = new C1.Win.C1Input.C1PictureBox();
            this.pic_next_button = new System.Windows.Forms.Button();
            this.NbPhotoLabel = new System.Windows.Forms.Label();
            this.pic_prev_button = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.c1PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.c1PictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pic_next_button);
            this.splitContainer1.Panel2.Controls.Add(this.NbPhotoLabel);
            this.splitContainer1.Panel2.Controls.Add(this.pic_prev_button);
            this.splitContainer1.Size = new System.Drawing.Size(233, 132);
            this.splitContainer1.SplitterDistance = 101;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // c1PictureBox1
            // 
            this.c1PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.c1PictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.c1PictureBox1.Name = "c1PictureBox1";
            this.c1PictureBox1.Size = new System.Drawing.Size(239, 140);
            this.c1PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.c1PictureBox1.TabIndex = 0;
            this.c1PictureBox1.TabStop = false;
            // 
            // pic_next_button
            // 
            this.pic_next_button.BackColor = System.Drawing.Color.DarkGray;
            this.pic_next_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pic_next_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pic_next_button.Location = new System.Drawing.Point(159, 1);
            this.pic_next_button.Margin = new System.Windows.Forms.Padding(4);
            this.pic_next_button.Name = "pic_next_button";
            this.pic_next_button.Size = new System.Drawing.Size(49, 25);
            this.pic_next_button.TabIndex = 3;
            this.pic_next_button.Text = ">>";
            this.pic_next_button.UseVisualStyleBackColor = true;
            this.pic_next_button.Click += new System.EventHandler(this.pic_next_button_Click);
            // 
            // NbPhotoLabel
            // 
            this.NbPhotoLabel.AutoSize = true;
            this.NbPhotoLabel.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NbPhotoLabel.Location = new System.Drawing.Point(88, 5);
            this.NbPhotoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NbPhotoLabel.Name = "NbPhotoLabel";
            this.NbPhotoLabel.Size = new System.Drawing.Size(63, 15);
            this.NbPhotoLabel.TabIndex = 5;
            this.NbPhotoLabel.Text = "10 photos";
            // 
            // pic_prev_button
            // 
            this.pic_prev_button.BackColor = System.Drawing.Color.DarkGray;
            this.pic_prev_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pic_prev_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pic_prev_button.Location = new System.Drawing.Point(31, 1);
            this.pic_prev_button.Margin = new System.Windows.Forms.Padding(4);
            this.pic_prev_button.Name = "pic_prev_button";
            this.pic_prev_button.Size = new System.Drawing.Size(49, 25);
            this.pic_prev_button.TabIndex = 4;
            this.pic_prev_button.Text = "<<";
            this.pic_prev_button.UseVisualStyleBackColor = true;
            this.pic_prev_button.Click += new System.EventHandler(this.pic_prev_button_Click);
            // 
            // pic_slide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "pic_slide";
            this.Size = new System.Drawing.Size(236, 138);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.c1PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private C1.Win.C1Input.C1PictureBox c1PictureBox1;
        private System.Windows.Forms.Button pic_next_button;
        private System.Windows.Forms.Label NbPhotoLabel;
        private System.Windows.Forms.Button pic_prev_button;
    }
}
