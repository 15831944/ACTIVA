using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ACTIVA_Module_1.modules
{
    class mod_carac
    {
        public static void New_Caracteristique_Form(string code, string num, bool clone)
        {
            XmlNode root;
            XmlNode nodeIntitule;

            C1.Win.C1Command.C1DockingTabPage CTab = new C1.Win.C1Command.C1DockingTabPage();
            ACTIVA_Module_1.component.carac_panel Cpanel = new ACTIVA_Module_1.component.carac_panel(code, num, clone);
            Cpanel.Dock = System.Windows.Forms.DockStyle.Fill;

            //On récupère l'intitulé du code
            root = mod_global.Get_Codes_Obs_DocElement();
            nodeIntitule = root.SelectSingleNode("/codes/code[id='" + code + "']/intitule");

            CTab.Text =  code + " | " + nodeIntitule.InnerText;

            CTab.Controls.Add(Cpanel);
            mod_global.MF.CaracDockingTab.TabPages.Add(CTab);
        }


        public static void Add_Existing_Code_Tab(string code, string num, bool clone)
        {
            //On supprime les anciennes pages de codes
            if (clone == false)
                mod_global.MF.CaracDockingTab.TabPages.Clear();

            //Création de la page du code sélectionné dans ObservationGrid
            New_Caracteristique_Form(code, num, clone);

            mod_global.MF.CaracDockingTab.SelectedTab = mod_global.MF.CaracDockingTab.TabPages[0];
            mod_global.MF.MainDockingTab.SelectedTab = mod_global.MF.RenseignementTab;
        }


        public static void Add_New_Code_Tabs(string code)
        {
            //On supprime les anciennes pages de codes
            mod_global.MF.CaracDockingTab.TabPages.Clear();

            //Création de la page du code principal (si num=String.Empty, la page créée est vide)
            New_Caracteristique_Form(code, String.Empty, false);

            //Création des pages de codes liés
            XmlNode root;
            XmlNodeList CodeLieNodeList;

            root = mod_global.Get_Codes_Obs_DocElement();
            CodeLieNodeList = root.SelectNodes("/codes/code[id='" + code + "']/lien/codelie");

            foreach (XmlNode CodeLieNode in CodeLieNodeList)
             {
               New_Caracteristique_Form(CodeLieNode.InnerText, String.Empty, false);
             }

            mod_global.MF.CaracDockingTab.SelectedTab = mod_global.MF.CaracDockingTab.TabPages[0];
            mod_global.MF.MainDockingTab.SelectedTab = mod_global.MF.RenseignementTab;
        }
    }



}
