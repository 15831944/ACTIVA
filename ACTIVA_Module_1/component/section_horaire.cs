using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1.component
{
    public partial class section_horaire : UserControl
    {
        Graphics g;
        Pen pencil= new Pen(Color.RoyalBlue, 6);

        string current_forme;

        public section_horaire()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void Set_PictureBox_Events()
        {
            pictureBox1.Click += new EventHandler(Autres_Sections_Click);
            pictureBox2.Click += new EventHandler(Autres_Sections_Click);
            pictureBox3.Click += new EventHandler(Autres_Sections_Click);
            pictureBox4.Click += new EventHandler(Autres_Sections_Click);
        }

        public void Autres_Sections_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BorderStyle = BorderStyle.FixedSingle;

            mod_inspection.POSITION_SECTION = (string)pb.Tag;
            SectionPb.ImageLocation = pb.ImageLocation;

            foreach (Control ct in AutresSectionsTlp.Controls)
            {
                PictureBox pb2 = (PictureBox)ct;
                if (pb2.Name != pb.Name)
                    pb2.BorderStyle = BorderStyle.None;
            }

            SectionDockingTab.SelectedIndex = 0;
        }

        public void Init_Section_Img(string forme)
        {
            current_forme = forme;

            string imgfolder = Properties.Settings.Default.SectionImageDirPath;
            SectionPb.SizeMode = PictureBoxSizeMode.Zoom;
            SectionPb.ImageLocation = System.IO.Path.Combine(imgfolder, Get_Section_Image(forme));

            Set_PictureBox_Events();
            Empty_All_Images_n_Text();
            mod_global.center_control(mod_global.MF.section_horaire1);
            mod_global.center_control(mod_global.MF.section_horaire1.AutresSectionsPanel);
            
            string[] imagelist = Get_Section_Other_Images(forme);

            if (imagelist != null)
            {
                if (imagelist[0] != null)
                {
                    int i = 0;
                    foreach (string imagename in imagelist)
                    {
                        AutresFormesTab.Enabled = true;
                        PictureBox pb = (PictureBox)mod_global.MF.section_horaire1.AutresSectionsTlp.Controls[i];
                        Label lb = (Label)mod_global.MF.section_horaire1.AutresSectionsTextTlp.Controls[i];
                        pb.ImageLocation = System.IO.Path.Combine(imgfolder, imagename.Split(Char.Parse("|"))[0]);
                        lb.Text = imagename.Split(Char.Parse("|"))[0];
                        pb.Tag = imagename.Split(Char.Parse("|"))[1];
                        i += 1;
                    }
                }
            }
            else
            {
                AutresFormesTab.Enabled = false;
            }
        }

        public void Empty_All_Images_n_Text()
        {
            foreach (Control ct in AutresSectionsTlp.Controls)
            {
                PictureBox pb = (PictureBox)ct;
                pb.ImageLocation = null;
            }

            foreach (Control ct in AutresSectionsTextTlp.Controls)
            {
                Label lb = (Label)ct;
                lb.Text = String.Empty;
            }
        }

        private void SectionPb_MouseMove(object sender, MouseEventArgs e)
        {
            int x  = e.X;
            int y = e.Y;

            //pos relatives au centre de l'image
            int midx = (SectionPb.Width / 2);
            int midy = (SectionPb.Height / 2);
            
            double cote1 = x - midx;
            double cote2 = y - midy;
            double angle_rad = Math.Atan2(cote1, cote2);
            double angle_deg = angle_rad * 180 / Math.PI;
            double angle_adapte = 180 - angle_deg;
            decimal angle_horaire = (decimal)(angle_adapte /30);
            angle_horaire = Math.Round(angle_horaire, 1);

            if (angle_horaire == 0)
                angle_horaire = 12;

            if (angle_horaire == (decimal)0.5)
                angle_horaire = (decimal)12.5;

            SectionPb.Refresh();

            Point line1 = new Point(x,y);
            Point line2 = new Point(midx, midy);

            g.DrawLine(pencil, line1, line2);

            if ((angle_horaire * 10) % 5 == 0)
            {
                decimal angle_arrondi = Math.Floor(angle_horaire);
                //On verifie que l'angle est un entier ou a une decimal
                //S'il y a une decimale on la transforme en 30
                if (angle_horaire > angle_arrondi)
                {
                    horaireLb.Text = angle_arrondi.ToString() + "H30";
                    equivalLb.Text = mod_global.Get_Horaire_Equivalence(horaireLb.Text, current_forme);
                    mod_global.center_control(equivalLb);
                }
                else
                {
                    horaireLb.Text = angle_arrondi.ToString() + "H";
                    equivalLb.Text = mod_global.Get_Horaire_Equivalence(horaireLb.Text, current_forme);
                    mod_global.center_control(equivalLb);
                }
            }
          }

        private string Get_Section_Image(string forme)
        {
            XmlNode node;
            XmlNodeList nodelist;
            XmlNode root;

            root = mod_inspection.Section_Ouvrage_Xml.DocumentElement;

            string type = mod_inspection.TYPE_OUVRAGE;

            if (type == "BRANCHEMENT" | type == "TRONCON")
                type = "CANALISATION";

            //On verifie qu'il existe bien un noeud avec cette forme et cette position
            nodelist = root.SelectNodes("section[@ouvrage='" + type + "' and @forme='" + forme + "' and @position='" + mod_inspection.POSITION_SECTION + "']");

            //S'il n'existe pas de noeud on reinitialise la position à 1
            if (nodelist.Count == 0)
                mod_inspection.POSITION_SECTION = "1";

            //On selectionne l'image qui correspond a la forme en cours et à la position stockee
            node = root.SelectSingleNode("section[@ouvrage='" + type + "' and @forme='" + forme + "' and @position='" + mod_inspection.POSITION_SECTION + "']");

            return node.Attributes["image"].InnerText;
        }

        private string[] Get_Section_Other_Images(string forme)
        {
            XmlNodeList nodelist;
            XmlNode root;
            string[] imagelist;

            root = mod_inspection.Section_Ouvrage_Xml.DocumentElement;

            //On selectionne les images de la même forme
            nodelist = root.SelectNodes("section[@forme='" + forme + "']");

            //S'il y en a plus d'une on propose d'afficher les autres
            if (nodelist.Count > 1)
            {
                //On enleve 2 au count pour ne pas compter celui avec la position en cours, et pour respecter le debut à l'indice 0 du tableau
                imagelist = new string[nodelist.Count - 1];

                int i = 0;
                foreach (XmlNode nod in nodelist)
                {
                    if (nod.Attributes["position"].InnerText != mod_inspection.POSITION_SECTION)
                    {
                        imagelist[i] = nod.Attributes["image"].InnerText + "|" + nod.Attributes["position"].InnerText;
                        i += 1;
                    }
                }

                return imagelist;
            }
            else
                return null;
            
        }

        private void SectionPb_Paint(object sender, PaintEventArgs e)
        {
           g = SectionPb.CreateGraphics();
        }

        private void SectionPb_MouseLeave(object sender, EventArgs e)
        {
            SectionPb.Invalidate();
        }

        private void SectionPb_Click(object sender, EventArgs e)
        {
            mod_global.Focused_Control.Text = equivalLb.Text + " | " + horaireLb.Text;
        }

        private void splitContainer3_Resize(object sender, EventArgs e)
        {
            //mod_global.center_control(HorairePanel);
        }


    }
}
