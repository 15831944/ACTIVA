using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACTIVA_Module_1
{
    public partial class Ouvrage_Copy_Name : Form
    {
        public Ouvrage_Copy_Name()
        {
            InitializeComponent();
        }

        private void ValBt_Click(object sender, EventArgs e)
        {
            if (nameTb.Text != String.Empty)
            {
                if (modules.mod_global.Check_If_Ouvrage_Name_Exist(nameTb.Text) == false)
                {
                    modules.mod_new.Clone_Selected_Ouvrage(nameTb.Text);
                    this.Close();
                }
            }
        }
    }
}
