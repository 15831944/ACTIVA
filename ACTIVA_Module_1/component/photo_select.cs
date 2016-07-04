using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1.component
{
    public partial class photo_select : UserControl
    {
        PictureBox Selected_Photo;

        private List<WhatToPaint> ListToPaint = new List<WhatToPaint>();

        public photo_select()
        {
            InitializeComponent();
            Set_PictureBox_Events();
            pictureBox1.AllowDrop = true;
            pictureBox2.AllowDrop = true;
            pictureBox3.AllowDrop = true;
            pictureBox4.AllowDrop = true;
        }

        private class WhatToPaint
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string Text { get; set; }
        }

        public void Empty_All_Photos()
        {
            foreach (Control ct in PhotoTlp.Controls)
            {
                PictureBox pb = (PictureBox)ct;
                pb.ImageLocation = null;
            }
        }

        private void ChoosePhotoBt_Click(object sender, EventArgs e)
        {
            int deb = 0;
            if (openJPGDialog.ShowDialog() == DialogResult.OK)
            {
                string delimiter = String.Empty;
                //mod_global.Focused_Control.Text = String.Empty;
                string savepath = String.Empty;
                string savefolder = System.IO.Path.Combine(mod_accueil.SVF_FOLDER, "img");

                for (int i = 0; i <= 3; i++)
                {
                    PictureBox pb = (PictureBox)PhotoTlp.Controls[i];
                    if (pb == null || pb.Image == null)
                    {
                        deb = i;
                        break;
                    }
                }
                foreach (string photopath in openJPGDialog.FileNames)
                {
                    if (deb < 4)
                    {
                        PictureBox pb = (PictureBox)PhotoTlp.Controls[deb];
                        if (mod_global.Focused_Control.Text == String.Empty)
                        {
                            mod_global.Focused_Control.Text += System.IO.Path.GetFileName(photopath);
                        }
                        else
                        {
                            mod_global.Focused_Control.Text += "|" + System.IO.Path.GetFileName(photopath);
                        }
                        savepath = System.IO.Path.Combine(savefolder, System.IO.Path.GetFileName(photopath));
                        System.IO.File.Copy(photopath, savepath, true);
                        pb.ImageLocation = savepath;
                        deb++;
                    }
                    else
                    {
                        MessageBox.Show("Il faut supprimer au moins une image avant d'ajouter une autre.", "Erreur d'ajout", MessageBoxButtons.OKCancel);
                        return;
                    }
                }
                //Read_Photos_From_TextList(mod_global.Focused_Control.Text);
            }
        }

        private void Set_PictureBox_Events()
        {
            pictureBox1.Click += new EventHandler(Photo_Click);
            pictureBox2.Click += new EventHandler(Photo_Click);
            pictureBox3.Click += new EventHandler(Photo_Click);
            pictureBox4.Click += new EventHandler(Photo_Click);
        }

        public void Photo_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BorderStyle = BorderStyle.FixedSingle;

            Selected_Photo = pb;

            foreach (Control ct in PhotoTlp.Controls)
            {
                PictureBox pb2 = (PictureBox)ct;
                if (pb2.Name != pb.Name)
                    pb2.BorderStyle = BorderStyle.None;

                if (pb == null || pb.Image == null)
                    DelPhotoBt.Enabled = false;
                else
                    DelPhotoBt.Enabled = true;
            }
        }

        private void PhotoTlp_Paint(object sender, PaintEventArgs e)
        {
            modules.mod_global.center_control(photopanel);
        }


        private void DelPhotoBt_Click_1(object sender, EventArgs e)
        {
            if (Selected_Photo != null)
            {
                string ppath = Selected_Photo.ImageLocation;
                Selected_Photo.ImageLocation = null;
                Selected_Photo.Image = null;
                System.IO.File.Delete(ppath);

                mod_global.Focused_Control.Text = String.Empty;
                string delimiter = String.Empty;
                foreach (Control ct in PhotoTlp.Controls)
                {
                    PictureBox pb2 = (PictureBox)ct;
                    if (pb2.ImageLocation != null)
                    {
                        mod_global.Focused_Control.Text += delimiter + System.IO.Path.GetFileName(pb2.ImageLocation);
                        delimiter = "|";
                    }
                }

                Read_Photos_From_TextList(mod_global.Focused_Control.Text);
            }
        }

        public void Read_Photos_From_TextList(string photolist)
        {
            Empty_All_Photos();

            if (photolist == String.Empty)
                return;

            string picpath = String.Empty;
            string picfolder = System.IO.Path.Combine(mod_accueil.SVF_FOLDER, "img");

            int i = 0;
            foreach (string pic in photolist.Split(Char.Parse("|")))
            {
                if (i < 4)
                {
                    PictureBox pb = (PictureBox)PhotoTlp.Controls[i];
                    picpath = System.IO.Path.Combine(picfolder, pic);
                    pb.ImageLocation = picpath;
                    //Console.WriteLine(picpath);
                    i += 1;
                }
            }
        }

        private void AddPhotoBt_Click(object sender, EventArgs e)
        {
            //string s = mod_global.Focused_Control.Text;
            //MessageBox.Show(s.Substring(s.IndexOf("|")+1));
            OpenFileDialog diag = new OpenFileDialog();
            diag.Multiselect = false;
            diag.Filter = " JPG files (*.jpg)|*.jpg";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string delimiter = String.Empty;
                //mod_global.Focused_Control.Text = String.Empty;
                string savepath = String.Empty;
                string savefolder = System.IO.Path.Combine(mod_accueil.SVF_FOLDER, "img");

                PictureBox pb = (PictureBox)PhotoTlp.Controls[3];
                if (pb.Image != null)
                {
                    MessageBox.Show("Il faut supprimer au moins une image avant d'ajouter une autre.", "Erreur d'ajout", MessageBoxButtons.OKCancel);
                }
                for (int i = 0; i <= 3; i++)
                {
                    pb = (PictureBox)PhotoTlp.Controls[i];
                    if (pb == null || pb.Image == null)
                    {
                        pb.ImageLocation = diag.FileName;
                        if (mod_global.Focused_Control.Text == String.Empty)
                        {
                            mod_global.Focused_Control.Text += System.IO.Path.GetFileName(diag.FileName);
                        }
                        else
                        {
                            mod_global.Focused_Control.Text += "|" + System.IO.Path.GetFileName(diag.FileName);
                        }
                        savepath = System.IO.Path.Combine(savefolder, System.IO.Path.GetFileName(diag.FileName));
                        System.IO.File.Copy(diag.FileName, savepath, true);
                        //Read_Photos_From_TextList(mod_global.Focused_Control.Text);
                        return;
                    }
                }
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            int index = PhotoTlp.Controls.GetChildIndex(pb);
            using (Font myFont = new Font("Arial", 8, FontStyle.Bold))
            {
                e.Graphics.DrawString(System.IO.Path.GetFileName(((PictureBox)PhotoTlp.Controls[index - 4]).ImageLocation), myFont, Brushes.Black, new Point(0, 0));
            }
        }

        /* Drag and drop picturebox 
         * 
         * 
         * 

        private void image_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pb = (sender as PictureBox);
            var img = pb.Image;
            if (img == null) return;
            if (DoDragDrop(img, DragDropEffects.Move) == DragDropEffects.Move)
            {
                pb.Image = tmp;
                //Sort_Pic();
            }

        }

        private void pb_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Move;
        }

        private void pb_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox pb = (sender as PictureBox);
            var bmp = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            tmp = pb.Image;
            pb.Image = bmp;
        }

        public void Sort_Pic()
        {
            mod_global.Focused_Carac_Panel.RefPhotoTb.Text = String.Empty;
            PictureBox pb1;
            for (int i = 0; i <= 9; i++)
            {
                pb1 = (PictureBox)PhotoTlp.Controls[i];
                pb1.Refresh();
                if (pb1.Image != null)
                {
                    if (mod_global.Focused_Carac_Panel.RefPhotoTb.Text == String.Empty)
                    {
                        mod_global.Focused_Carac_Panel.RefPhotoTb.Text += System.IO.Path.GetFileName(pb1.ImageLocation);
                    }
                    else
                    {
                        mod_global.Focused_Carac_Panel.RefPhotoTb.Text += "|" + System.IO.Path.GetFileName(pb1.ImageLocation);
                    }
                }
                else return;
            }
        } */
    }
}
