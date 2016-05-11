using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACTIVA_Module_1.component
{
    public partial class pic_slide : UserControl
    {
        ArrayList Photos;
        int pic_pos = 0;
        public int PhotoCount = 0;
        public int ParentRow;

        public pic_slide(ArrayList pics, int parent_row)
        {
            Photos = pics;
            PhotoCount = pics.Count;
            ParentRow = parent_row;
            InitializeComponent();
            Fill_Slide();
        }

        private void Fill_Slide()
        {
            c1PictureBox1.ImageLocation = Photos[pic_pos].ToString();
            //Console.WriteLine(Photos[pic_pos].ToString());
            NbPhotoLabel.Text = Photos.Count + " photos";
        }


        private void pic_next_button_Click(object sender, EventArgs e)
        {
            if (pic_pos + 1 <= Photos.Count - 1)
            {
                c1PictureBox1.ImageLocation = Photos[pic_pos + 1].ToString();
                pic_pos += 1;
            }
        }

        private void pic_prev_button_Click(object sender, EventArgs e)
        {
            if (pic_pos - 1 >= 0)
            {
                c1PictureBox1.ImageLocation = Photos[pic_pos - 1].ToString();
                pic_pos -= 1;                
            }
        }
    }
}
