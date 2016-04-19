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

        public photo_select()
        {
            InitializeComponent();
            Set_PictureBox_Events();
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
            if (openJPGDialog.ShowDialog() == DialogResult.OK)
            {
                int i = 0;
                string delimiter = String.Empty;
                mod_global.Focused_Control.Text = String.Empty;
                string savepath = String.Empty;
                string savefolder = System.IO.Path.Combine(mod_inspection.SVF_FOLDER, "img");

                foreach (string photopath in openJPGDialog.FileNames)
                {
                    if (i < 10)
                    {
                        PictureBox pb = (PictureBox)PhotoTlp.Controls[i];
                        mod_global.Focused_Control.Text += delimiter + System.IO.Path.GetFileName(photopath);
                        savepath = System.IO.Path.Combine(savefolder, System.IO.Path.GetFileName(photopath));
                        System.IO.File.Copy(photopath, savepath,true);
                        delimiter = "|";
                        pb.ImageLocation = savepath;
                        i += 1;
                    }
                }
                Read_Photos_From_TextList(mod_global.Focused_Control.Text);
            }
        }

        private void Set_PictureBox_Events()
        {
            pictureBox1.Click += new EventHandler(Photo_Click);
            pictureBox2.Click += new EventHandler(Photo_Click);
            pictureBox3.Click += new EventHandler(Photo_Click);
            pictureBox4.Click += new EventHandler(Photo_Click);
            pictureBox7.Click += new EventHandler(Photo_Click);
            pictureBox8.Click += new EventHandler(Photo_Click);
            pictureBox9.Click += new EventHandler(Photo_Click);
            pictureBox10.Click += new EventHandler(Photo_Click);
            pictureBox6.Click += new EventHandler(Photo_Click);
            pictureBox5.Click += new EventHandler(Photo_Click);
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
                    pb2.BorderStyle=BorderStyle.None;

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
            string picfolder = System.IO.Path.Combine(mod_inspection.SVF_FOLDER, "img");

            int i = 0;
            foreach (string pic in photolist.Split(Char.Parse("|")))
            {
                if (i < 10)
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
            OpenFileDialog diag = new OpenFileDialog();
            diag.Multiselect = false;
            diag.Filter = " JPG files (*.jpg)|*.jpg";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string delimiter = String.Empty;
                //mod_global.Focused_Control.Text = String.Empty;
                string savepath = String.Empty;
                string savefolder = System.IO.Path.Combine(mod_inspection.SVF_FOLDER, "img");

                PictureBox pb = (PictureBox)PhotoTlp.Controls[9];
                if (pb.Image != null)
                {
                    MessageBox.Show("Il faut supprimer au moins une image avant d'ajouter une autre.", "Erreur d'ajout", MessageBoxButtons.OKCancel);
                }
                for (int i = 1; i <=10; i++)
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
                        Read_Photos_From_TextList(mod_global.Focused_Control.Text);
                        return;
                    }
                    i += 1;
                }
            }
        }
    }
}
