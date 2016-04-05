using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

namespace ACTIVA_Module_1.modules
{
    class mod_param_path
    {
        public static void Init_Path_Button_Tag_n_Event()
        {
            //On associe les boutons aux textbox
            mod_global.MF.XmlIdCanaBt.Tag = mod_global.MF.XmlIdCanaTb;
            mod_global.MF.XmlIdRegBt.Tag = mod_global.MF.XmlIdRegTb;

            mod_global.MF.XmlObsCanaBt.Tag = mod_global.MF.XmlObsCanaTb;
            mod_global.MF.XmlObsRegBt.Tag = mod_global.MF.XmlObsRegTb;

            mod_global.MF.XmlGroupeCodeIdBt.Tag = mod_global.MF.XmlGroupCodeIdTb;
            mod_global.MF.XmlFamilleCodeIdBt.Tag = mod_global.MF.XmlFamilleCodeIdTb;

            mod_global.MF.XmlSectionOuvrageBt.Tag = mod_global.MF.XmlSectionOuvrageTb;
            mod_global.MF.XmlSectionImgDirBt.Tag = mod_global.MF.XmlSectionImgDirTb;

            mod_global.MF.XmlMotifBt.Tag = mod_global.MF.XmlMotifTb;

            mod_global.MF.FileLigneBt.Tag = mod_global.MF.FileLigneTb;
            mod_global.MF.FileHachureBt.Tag = mod_global.MF.FileHachureTb;
            mod_global.MF.FileSymboleBt.Tag = mod_global.MF.FileSymboleTb;

            //------------------------------------------------------------------------------

            //On associe la fonction Choose_Xml a l'evenement Click des boutons
            mod_global.MF.XmlIdCanaBt.Click += new EventHandler(Choose_Xml_Click);
            mod_global.MF.XmlIdRegBt.Click += new EventHandler(Choose_Xml_Click);

            mod_global.MF.XmlObsCanaBt.Click += new EventHandler(Choose_Xml_Click);
            mod_global.MF.XmlObsRegBt.Click += new EventHandler(Choose_Xml_Click);

            mod_global.MF.XmlGroupeCodeIdBt.Click += new EventHandler(Choose_Xml_Click);
            mod_global.MF.XmlFamilleCodeIdBt.Click += new EventHandler(Choose_Xml_Click);

            mod_global.MF.XmlSectionOuvrageBt.Click += new EventHandler(Choose_Xml_Click);
            mod_global.MF.XmlSectionImgDirBt.Click += new EventHandler(Choose_Folder_Click);

            mod_global.MF.XmlMotifBt.Click += new EventHandler(Choose_Xml_Click);

            mod_global.MF.FileLigneBt.Click += new EventHandler(Choose_File_Click);
            mod_global.MF.FileHachureBt.Click += new EventHandler(Choose_File_Click);
            mod_global.MF.FileSymboleBt.Click += new EventHandler(Choose_Folder_Click);

            //-------------------------------------------------------------------------------

            //On associe les textbox a leur setting correspondant
            mod_global.MF.XmlIdCanaTb.Tag = Properties.Settings.Default.PropertyValues["CodeIdCanaPath"];
            mod_global.MF.XmlIdRegTb.Tag = Properties.Settings.Default.PropertyValues["CodeIdRegPath"];

            mod_global.MF.XmlObsCanaTb.Tag = Properties.Settings.Default.PropertyValues["CodeObsCanaPath"];
            mod_global.MF.XmlObsRegTb.Tag = Properties.Settings.Default.PropertyValues["CodeObsRegPath"];

            mod_global.MF.XmlGroupCodeIdTb.Tag = Properties.Settings.Default.PropertyValues["GroupCodeIdPath"];
            mod_global.MF.XmlFamilleCodeIdTb.Tag = Properties.Settings.Default.PropertyValues["FamilleCodePath"];

            mod_global.MF.XmlSectionOuvrageTb.Tag = Properties.Settings.Default.PropertyValues["SectionOuvragePath"];
            mod_global.MF.XmlSectionImgDirTb.Tag = Properties.Settings.Default.PropertyValues["SectionImageDirPath"];

            mod_global.MF.XmlMotifTb.Tag = Properties.Settings.Default.PropertyValues["MotifPath"];

            mod_global.MF.FileLigneTb.Tag = Properties.Settings.Default.PropertyValues["LignePath"];
            mod_global.MF.FileHachureTb.Tag = Properties.Settings.Default.PropertyValues["HachurePath"];
            mod_global.MF.FileSymboleTb.Tag = Properties.Settings.Default.PropertyValues["SymbolePath"];
        }

        public static void Choose_Xml_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;

            tb.BackColor = Color.Gold;

            if (mod_global.MF.openXMLDialog.ShowDialog() == DialogResult.OK)
            {
                tb.Text = mod_global.MF.openXMLDialog.FileName;

                System.Configuration.SettingsPropertyValue PropValue = (System.Configuration.SettingsPropertyValue)tb.Tag;
                PropValue.PropertyValue = tb.Text;

                Properties.Settings.Default.Save();
                
                tb.BackColor = Color.WhiteSmoke;
            }
            else if (mod_global.MF.openXMLDialog.ShowDialog() == DialogResult.Cancel)
            {
                tb.BackColor = Color.WhiteSmoke;
            }
        }

        public static void Choose_File_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;

            tb.BackColor = Color.Gold;

            if (mod_global.MF.openAllTypeDialog.ShowDialog() == DialogResult.OK)
            {
                tb.Text = mod_global.MF.openAllTypeDialog.FileName;

                System.Configuration.SettingsPropertyValue PropValue = (System.Configuration.SettingsPropertyValue)tb.Tag;
                PropValue.PropertyValue = tb.Text;

                Properties.Settings.Default.Save();

                tb.BackColor = Color.WhiteSmoke;
            }
            else if (mod_global.MF.openAllTypeDialog.ShowDialog() == DialogResult.Cancel)
            {
                tb.BackColor = Color.WhiteSmoke;
            }
        }

        public static void Choose_Folder_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;

            tb.BackColor = Color.Gold;

            DialogResult test = mod_global.MF.FolderDialog.ShowDialog();
            if (test == DialogResult.OK)
            {
                tb.Text = mod_global.MF.FolderDialog.SelectedPath;

                System.Configuration.SettingsPropertyValue PropValue = (System.Configuration.SettingsPropertyValue)tb.Tag;
                PropValue.PropertyValue = tb.Text;

                Properties.Settings.Default.Save();

                tb.BackColor = Color.WhiteSmoke;
            }
            else if (test == DialogResult.Cancel)
            {
                tb.BackColor = Color.WhiteSmoke;
            }
        }

    }
}
