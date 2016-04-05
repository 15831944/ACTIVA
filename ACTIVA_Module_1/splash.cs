using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1
{
    public partial class splash : Form
    {
        public splash()
        {
            InitializeComponent();
            this.TopMost = true;
            this.CenterToScreen();
            this.SetTopLevel(true);
        }

        private void splash_Load(object sender, EventArgs e)
        {
            bgworker.RunWorkerAsync();
        }

        private void bgworker_DoWork(object sender, DoWorkEventArgs e)
        {
            mod_init.Data_Loading();
        }

        private void bgworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar1.Update();
            //mod_global.MF.Opacity = 75;
            this.Close();
            this.Dispose();
        }


    }
}
