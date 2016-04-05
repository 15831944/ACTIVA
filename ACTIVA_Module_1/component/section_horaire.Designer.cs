namespace ACTIVA_Module_1.component
{
    partial class section_horaire
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
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.SectionPb = new System.Windows.Forms.PictureBox();
            this.SectionDockingTab = new C1.Win.C1Command.C1DockingTab();
            this.HoraireTab = new C1.Win.C1Command.C1DockingTabPage();
            this.HorairePanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.horaireLb = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.equivalLb = new System.Windows.Forms.Label();
            this.AutresFormesTab = new C1.Win.C1Command.C1DockingTabPage();
            this.AutresSectionsPanel = new System.Windows.Forms.Panel();
            this.AutresSectionsTlp = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.AutresSectionsTextTlp = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SectionPb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SectionDockingTab)).BeginInit();
            this.SectionDockingTab.SuspendLayout();
            this.HoraireTab.SuspendLayout();
            this.HorairePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.AutresFormesTab.SuspendLayout();
            this.AutresSectionsPanel.SuspendLayout();
            this.AutresSectionsTlp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.AutresSectionsTextTlp.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.Color.DimGray;
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.SectionPb);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.SectionDockingTab);
            this.splitContainer3.Size = new System.Drawing.Size(1014, 260);
            this.splitContainer3.SplitterDistance = 271;
            this.splitContainer3.TabIndex = 1;
            this.splitContainer3.Resize += new System.EventHandler(this.splitContainer3_Resize);
            // 
            // SectionPb
            // 
            this.SectionPb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionPb.Location = new System.Drawing.Point(0, 0);
            this.SectionPb.Name = "SectionPb";
            this.SectionPb.Size = new System.Drawing.Size(269, 258);
            this.SectionPb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SectionPb.TabIndex = 0;
            this.SectionPb.TabStop = false;
            this.SectionPb.MouseLeave += new System.EventHandler(this.SectionPb_MouseLeave);
            this.SectionPb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SectionPb_MouseMove);
            this.SectionPb.Click += new System.EventHandler(this.SectionPb_Click);
            this.SectionPb.Paint += new System.Windows.Forms.PaintEventHandler(this.SectionPb_Paint);
            // 
            // SectionDockingTab
            // 
            this.SectionDockingTab.Controls.Add(this.HoraireTab);
            this.SectionDockingTab.Controls.Add(this.AutresFormesTab);
            this.SectionDockingTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionDockingTab.Location = new System.Drawing.Point(0, 0);
            this.SectionDockingTab.Name = "SectionDockingTab";
            this.SectionDockingTab.Size = new System.Drawing.Size(737, 258);
            this.SectionDockingTab.TabIndex = 6;
            this.SectionDockingTab.TabsSpacing = 5;
            this.SectionDockingTab.TabStyle = C1.Win.C1Command.TabStyleEnum.Office2007;
            this.SectionDockingTab.VisualStyle = C1.Win.C1Command.VisualStyle.Office2007Black;
            this.SectionDockingTab.VisualStyleBase = C1.Win.C1Command.VisualStyle.Office2007Black;
            // 
            // HoraireTab
            // 
            this.HoraireTab.BackColor = System.Drawing.Color.DimGray;
            this.HoraireTab.Controls.Add(this.HorairePanel);
            this.HoraireTab.Controls.Add(this.panel1);
            this.HoraireTab.Location = new System.Drawing.Point(1, 24);
            this.HoraireTab.Name = "HoraireTab";
            this.HoraireTab.Size = new System.Drawing.Size(735, 233);
            this.HoraireTab.TabIndex = 0;
            this.HoraireTab.Text = "Horaire";
            // 
            // HorairePanel
            // 
            this.HorairePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HorairePanel.Controls.Add(this.label2);
            this.HorairePanel.Controls.Add(this.horaireLb);
            this.HorairePanel.Location = new System.Drawing.Point(110, 27);
            this.HorairePanel.Name = "HorairePanel";
            this.HorairePanel.Size = new System.Drawing.Size(527, 100);
            this.HorairePanel.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(16, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(294, 72);
            this.label2.TabIndex = 1;
            this.label2.Text = "Horaire : ";
            // 
            // horaireLb
            // 
            this.horaireLb.AutoSize = true;
            this.horaireLb.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.horaireLb.ForeColor = System.Drawing.Color.YellowGreen;
            this.horaireLb.Location = new System.Drawing.Point(299, 10);
            this.horaireLb.Name = "horaireLb";
            this.horaireLb.Size = new System.Drawing.Size(0, 72);
            this.horaireLb.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.equivalLb);
            this.panel1.Location = new System.Drawing.Point(110, 132);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 67);
            this.panel1.TabIndex = 5;
            // 
            // equivalLb
            // 
            this.equivalLb.AutoSize = true;
            this.equivalLb.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equivalLb.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.equivalLb.Location = new System.Drawing.Point(34, 10);
            this.equivalLb.Name = "equivalLb";
            this.equivalLb.Size = new System.Drawing.Size(0, 42);
            this.equivalLb.TabIndex = 0;
            // 
            // AutresFormesTab
            // 
            this.AutresFormesTab.BackColor = System.Drawing.Color.DimGray;
            this.AutresFormesTab.Controls.Add(this.AutresSectionsPanel);
            this.AutresFormesTab.Location = new System.Drawing.Point(1, 24);
            this.AutresFormesTab.Name = "AutresFormesTab";
            this.AutresFormesTab.Size = new System.Drawing.Size(735, 233);
            this.AutresFormesTab.TabIndex = 1;
            this.AutresFormesTab.Text = "Autres formes";
            // 
            // AutresSectionsPanel
            // 
            this.AutresSectionsPanel.Controls.Add(this.AutresSectionsTlp);
            this.AutresSectionsPanel.Controls.Add(this.AutresSectionsTextTlp);
            this.AutresSectionsPanel.Location = new System.Drawing.Point(3, 3);
            this.AutresSectionsPanel.Name = "AutresSectionsPanel";
            this.AutresSectionsPanel.Size = new System.Drawing.Size(729, 238);
            this.AutresSectionsPanel.TabIndex = 2;
            // 
            // AutresSectionsTlp
            // 
            this.AutresSectionsTlp.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.AutresSectionsTlp.ColumnCount = 4;
            this.AutresSectionsTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTlp.Controls.Add(this.pictureBox1, 0, 0);
            this.AutresSectionsTlp.Controls.Add(this.pictureBox2, 1, 0);
            this.AutresSectionsTlp.Controls.Add(this.pictureBox3, 2, 0);
            this.AutresSectionsTlp.Controls.Add(this.pictureBox4, 3, 0);
            this.AutresSectionsTlp.Location = new System.Drawing.Point(9, 7);
            this.AutresSectionsTlp.Name = "AutresSectionsTlp";
            this.AutresSectionsTlp.RowCount = 1;
            this.AutresSectionsTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AutresSectionsTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 182F));
            this.AutresSectionsTlp.Size = new System.Drawing.Size(710, 185);
            this.AutresSectionsTlp.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(167, 173);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(182, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(167, 173);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Location = new System.Drawing.Point(358, 6);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(167, 173);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Location = new System.Drawing.Point(534, 6);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(170, 173);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // AutresSectionsTextTlp
            // 
            this.AutresSectionsTextTlp.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.AutresSectionsTextTlp.ColumnCount = 4;
            this.AutresSectionsTextTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTextTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTextTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTextTlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.AutresSectionsTextTlp.Controls.Add(this.label1, 0, 0);
            this.AutresSectionsTextTlp.Controls.Add(this.label3, 1, 0);
            this.AutresSectionsTextTlp.Controls.Add(this.label4, 2, 0);
            this.AutresSectionsTextTlp.Controls.Add(this.label5, 3, 0);
            this.AutresSectionsTextTlp.Location = new System.Drawing.Point(9, 197);
            this.AutresSectionsTextTlp.Name = "AutresSectionsTextTlp";
            this.AutresSectionsTextTlp.RowCount = 1;
            this.AutresSectionsTextTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.AutresSectionsTextTlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.AutresSectionsTextTlp.Size = new System.Drawing.Size(710, 25);
            this.AutresSectionsTextTlp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gold;
            this.label1.Location = new System.Drawing.Point(5, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gold;
            this.label3.Location = new System.Drawing.Point(182, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Gold;
            this.label4.Location = new System.Drawing.Point(359, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Gold;
            this.label5.Location = new System.Drawing.Point(536, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "label5";
            // 
            // section_horaire
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer3);
            this.Name = "section_horaire";
            this.Size = new System.Drawing.Size(1014, 260);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SectionPb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SectionDockingTab)).EndInit();
            this.SectionDockingTab.ResumeLayout(false);
            this.HoraireTab.ResumeLayout(false);
            this.HorairePanel.ResumeLayout(false);
            this.HorairePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.AutresFormesTab.ResumeLayout(false);
            this.AutresSectionsPanel.ResumeLayout(false);
            this.AutresSectionsTlp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.AutresSectionsTextTlp.ResumeLayout(false);
            this.AutresSectionsTextTlp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.PictureBox SectionPb;
        private System.Windows.Forms.Label horaireLb;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Panel HorairePanel;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label equivalLb;
        private C1.Win.C1Command.C1DockingTab SectionDockingTab;
        private C1.Win.C1Command.C1DockingTabPage HoraireTab;
        private C1.Win.C1Command.C1DockingTabPage AutresFormesTab;
        public System.Windows.Forms.TableLayoutPanel AutresSectionsTlp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.TableLayoutPanel AutresSectionsTextTlp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel AutresSectionsPanel;
    }
}
