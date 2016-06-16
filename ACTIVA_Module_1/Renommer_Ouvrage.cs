using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ACTIVA_Module_1
{
    public partial class Renommer_Ouvrage : Form
    {
        public Renommer_Ouvrage()
        {
            InitializeComponent();
        }


        private void ValRenomBt_Click(object sender, EventArgs e)
        {
            if (RenameTb.Text != String.Empty)
            {
                if (modules.mod_global.Check_If_Ouvrage_Name_Exist(RenameTb.Text) == false)
                {
                    XmlNode SVF = modules.mod_accueil.SVF.DocumentElement; ;
                    XmlElement node = (XmlElement)SVF.SelectSingleNode("ouvrage[@nom='" + modules.mod_global.MF.OuvrageList.SelectedText + "']");
                    node.SetAttribute("nom", RenameTb.Text);
                    //modules.mod_global.MF.OuvrageList.SelectedText;
                    modules.mod_accueil.SVF.Save(System.IO.Path.Combine(modules.mod_accueil.SVF_FOLDER, modules.mod_accueil.SVF_FILENAME));
                    modules.mod_accueil.Fill_Ouvrage_List(modules.mod_global.MF.OuvrageList);
                    this.Close();
                }
            }
        }
    }
}
