using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using C1.Win.C1List;
using System.Globalization;

namespace ACTIVA_Module_1.modules
{
    class mod_inspection
    {
        public static XmlDocument SVF = new XmlDocument();
        public static string SVF_FOLDER = String.Empty;
        public static string SVF_FILENAME = String.Empty;
        public static string OUVRAGE = string.Empty;
        public static string TYPE_OUVRAGE = string.Empty;
        public static string FORME_OUVRAGE = string.Empty;

        public static string CHECKED_TYPES = string.Empty;
        public static Boolean SVF_LOADED = false;

        public static double LAST_AEC_CEC_PM = 0;
        public static double LAST_PM = 0;

        public static XmlDocument Section_Ouvrage_Xml = new XmlDocument();
        public static XmlDocument Motif_Xml = new XmlDocument();
        public static string POSITION_SECTION = "1";

        public static void Check_Type_Ouvrage(System.Windows.Forms.CheckBox cb_troncon, System.Windows.Forms.CheckBox cb_branchement, System.Windows.Forms.CheckBox cb_regard)
        {
            CHECKED_TYPES = string.Empty;

            if (cb_troncon.Checked == true)
            {
                CHECKED_TYPES += cb_troncon.Tag + "|";
            }
            if (cb_branchement.Checked == true)
            {
                CHECKED_TYPES += cb_branchement.Tag + "|";
            }
            if (cb_regard.Checked == true)
            {
                CHECKED_TYPES += cb_regard.Tag + "|";
            }
        }

        public static void Load_SVF(string svfpath)
        {
            mod_global.MF.SVFLabel.Text = svfpath;
            SVF.Load(svfpath);
            SVF_FOLDER = System.IO.Path.GetDirectoryName(svfpath);
            SVF_FILENAME = System.IO.Path.GetFileName(svfpath);
            SVF_LOADED = true;
        }

        /// <summary>
        /// Fonction de chargement du fichier XML de configuration Section Ouvrage
        /// </summary>
        /// <param name="groupe_code_id_path"></param>

        public static void Load_Section_Ouvrage(string section_ouvrage_path)
        {
            try
            {
                Section_Ouvrage_Xml.Load(section_ouvrage_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Fonction de chargement du fichier XML des motifs de la representation AutoCAD
        /// </summary>
        /// <param name="groupe_code_id_path"></param>

        public static void Load_Motif(string motif_path)
        {
            try
            {
                Motif_Xml.Load(motif_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Fill_Ouvrage_List(C1.Win.C1List.C1List olist)
        {
            if (SVF_LOADED == true)
            {
                olist.ClearItems();
                olist.SelectedStyle.BackColor = System.Drawing.Color.Gold;

                XmlNodeList nodelist;
                XmlNode unNode;
                //XmlNode root = SVF.Se;

                olist.AddItemTitles("Nom; Type; Code forme; Intitulé forme; Position");

                olist.Columns[0].Caption = "Nom";
                olist.Columns[1].Caption = "Type";
                olist.Columns[2].Caption = "Code forme";
                olist.Columns[3].Caption = "Intitulé forme";
                olist.Columns[4].Caption = "Position";

                nodelist = SVF.SelectNodes("/inspection/ouvrage");

                SortedList sortednodelist = SortNodeList_By_Position(nodelist);

                foreach (object obj in sortednodelist.Values)
                {
                    unNode = (XmlNode)obj;
                    if (CHECKED_TYPES.Contains(unNode.Attributes["type"].InnerText))
                        olist.AddItem(unNode.Attributes["nom"].InnerText + ";" + unNode.Attributes["type"].InnerText + ";" + unNode.Attributes["forme"].InnerText + ";" + mod_global.Get_Section_Intitule_By_Code(unNode.Attributes["forme"].InnerText, unNode.Attributes["type"].InnerText) + ";" + unNode.Attributes["position"].InnerText);
                }

                mod_global.Enable_Ouvrage_Controls();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Veuillez ouvrir un fichier SVF", "Erreur", System.Windows.Forms.MessageBoxButtons.OK); 
            }
        }

        public static void Reset_Inspection_Tab()
        {
            SVF_FOLDER = String.Empty;
            SVF_FILENAME = String.Empty;
            SVF_LOADED = false;

            OUVRAGE = String.Empty;
            TYPE_OUVRAGE = String.Empty;
            FORME_OUVRAGE = String.Empty;

            SVF = new XmlDocument();

            mod_global.MF.SVFLabel.Text = String.Empty;

            mod_global.MF.openSVFTb.Text = String.Empty;
            mod_global.MF.NewInspectionNameTb.Text = String.Empty;
            mod_global.MF.NewInspectionPathTb.Text = String.Empty;


            mod_global.MF.OuvrageList.ClearItems();

            mod_global.Disable_Ouvrage_Controls();
            mod_global.Disable_Main_Tabs();
        }

        public static SortedList SortNodeList_By_Position(XmlNodeList nodelist)
        {
            SortedList sortlist = new SortedList();
            
            foreach (XmlNode nod in nodelist)
            {
                sortlist.Add(nod.Attributes["position"].InnerText, nod);
            }

            return sortlist;
        }

        public static SortedList SortNodeList_By_Alphabet(XmlNodeList nodelist)
        {
            SortedList sortlist = new SortedList();

            foreach (XmlNode nod in nodelist)
            {
                sortlist.Add(nod.FirstChild.InnerText, nod);
            }

            return sortlist;
        }

        public static void Get_Selected_Ouvrage_Info(string nodename, string nodetype, string nodeforme, System.Windows.Forms.Label obs_name, System.Windows.Forms.Label obs_nb)
        {
            if (mod_global.MF.OuvrageList.SelectionMode != SelectionModeEnum.One)
                return;

            XmlNodeList nodeList;
            XmlNode root = SVF.DocumentElement;

            obs_name.Text = nodename;
            mod_global.MF.ouvrageLb.Text = ACTIVA_Module_1.modules.mod_inspection.OUVRAGE;
            OUVRAGE = nodename;
            TYPE_OUVRAGE = nodetype;
            FORME_OUVRAGE = nodeforme;

            mod_global.MF.CurrentOuvrageNameLb.Text = OUVRAGE;
            mod_global.MF.CurrentOuvrageTypeLb.Text = TYPE_OUVRAGE;
            mod_global.MF.CurrentOuvrageFormeLb.Text = mod_global.Get_Section_Intitule_By_Code(FORME_OUVRAGE, string.Empty);

            nodeList = root.SelectNodes(string.Concat("//ouvrage[@nom='", nodename, "']/observations/*"));
            obs_nb.Text = nodeList.Count.ToString();

            mod_global.MF.LineaireStripLabel.Text = Get_Max_Pm_In_SVF() + " m";

            mod_global.Enable_Main_Tabs();
        }

        public static string Get_Current_Ouvrage_Forme()
        {
            return FORME_OUVRAGE;
        }

        /// <summary>
        /// Fonction d'attribution d'un nouveau numéro d'observation
        /// </summary>
        /// <returns></returns>
        public static string Get_New_Obs_Num()
        {

            XmlNodeList svfnodelist = SVF.DocumentElement.SelectNodes("/inspection/ouvrage[@nom='" + mod_inspection.OUVRAGE + "']/observations/code");

            int i = 0;
            int[] codenums = new int[svfnodelist.Count];
            foreach (XmlNode code in svfnodelist)
            {
                codenums[i] = int.Parse(code.Attributes["num"].InnerText);
                i += 1;
            }

            int new_max = 1;

            if (codenums.Length>0)
                new_max = codenums.Max() + 1;

            return new_max.ToString();
        }


        /// <summary>
        /// Fonction d'attribution d'un nouveau numéro d'observation
        /// </summary>
        /// <returns></returns>
        public static string Get_Max_Pm_In_SVF()
        {

            XmlNodeList svfnodelist = SVF.DocumentElement.SelectNodes("/inspection/ouvrage[@nom='" + mod_inspection.OUVRAGE + "']/observations/code");

            int i = 0;
            double[] pms = new double[svfnodelist.Count];
            string id;
         
            foreach (XmlNode code in svfnodelist)
            {
                id = code.SelectSingleNode("id").InnerText;

                //on recupere le pm au format invariant culture (point comme separateur décimal)
                pms[i] = double.Parse(code.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText);
               
                if (id == "AEC" | id == "CEC")
                {
                    FORME_OUVRAGE = code.Attributes["forme"].InnerText;
                    LAST_AEC_CEC_PM = pms[i];
                    mod_global.MF.CurrentOuvrageFormeLb.Text = mod_global.Get_Section_Intitule_By_Code(FORME_OUVRAGE, String.Empty);
                }

                i += 1;
            }

            double pm_max = 1;

            if (pms.Length > 0)
                pm_max = pms.Max();

            LAST_PM = pm_max;

            return pm_max.ToString();
        }

        /// <summary>
        /// Fonction d'attribution d'une nouvelle position d'ouvrage
        /// </summary>
        /// <returns></returns>
        public static string Get_New_Ouvrage_Position()
        {

            XmlNodeList svfnodelist = SVF.DocumentElement.SelectNodes("/inspection/ouvrage");

            int i = 0;
            int[] ouvragepos = new int[svfnodelist.Count];

            foreach (XmlNode ouvrage in svfnodelist)
            {
                ouvragepos[i] = int.Parse(ouvrage.Attributes["position"].InnerText);
                i += 1;
            }

            int new_pos = 1;

            if (ouvragepos.Length>0)
                new_pos = ouvragepos.Max() + 1;

            return new_pos.ToString();
        }

        /// <summary>
        /// Fonction d'ordonnancement des ouvrages (monter)
        /// </summary>
        /// <param name="olist"></param>
        public static void Move_Ouvrage_Up(C1.Win.C1List.C1List olist)
        {
            if (olist.SelectedIndex >= 1)
            {
                int saveIndex = olist.SelectedIndex;
                olist.InsertItem(olist.Columns["Nom"].CellText(saveIndex) + ";" + olist.Columns["Type"].CellText(saveIndex) + ";" + olist.Columns["Code forme"].CellText(saveIndex) + ";" + olist.Columns["Intitulé forme"].CellText(saveIndex) + ";" + olist.Columns["Position"].CellText(saveIndex), saveIndex - 1);
                olist.RemoveItem(saveIndex + 1);
                olist.SelectedIndex = saveIndex - 1;
            }
        }

        /// <summary>
        /// Fonction d'ordonnancement des ouvrages (descendre)
        /// </summary>
        /// <param name="olist"></param>
        public static void Move_Ouvrage_Down(C1.Win.C1List.C1List olist)
        {
             if (olist.SelectedIndex < olist.ListCount - 1)
             {
                int saveIndex = olist.SelectedIndex;
                olist.InsertItem(olist.Columns["Nom"].CellText(saveIndex + 1) + ";" + olist.Columns["Type"].CellText(saveIndex + 1) + ";" + olist.Columns["Code forme"].CellText(saveIndex + 1) + ";" + olist.Columns["Intitulé forme"].CellText(saveIndex + 1) + ";" + olist.Columns["Position"].CellText(saveIndex+1), saveIndex);
                olist.RemoveItem(saveIndex + 2);
                olist.SelectedIndex = saveIndex + 1;
             }
        }

        /// <summary>
        /// Fonction de sauvegarde de l'odre des ouvrages
        /// </summary>
        /// <param name="olist"></param>
        public static void Save_Ouvrage_Order(C1.Win.C1List.C1List olist)
        {
            XmlNode onod;

            for (int i = 0; i<olist.ListCount; i++)
            {
                onod = SVF.DocumentElement.SelectSingleNode("/inspection/ouvrage[@nom='" + olist.Columns["nom"].CellText(i) + "']");
                onod.Attributes["position"].InnerText = (i+1).ToString();
            }

            mod_inspection.SVF.Save(System.IO.Path.Combine(mod_inspection.SVF_FOLDER, mod_inspection.SVF_FILENAME));
            mod_inspection.Fill_Ouvrage_List(mod_global.MF.OuvrageList);
        }
    }
}
