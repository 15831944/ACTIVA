using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace ACTIVA_Module_1.modules
{
    class mod_global
    {
        public static MainForm MF;
        
        public static System.Windows.Forms.TextBox Focused_Control;
        public static component.carac_panel Focused_Carac_Panel;

        public static Color Obligatoire_Color = Color.MistyRose;
        public static Color Differe_Color = Color.LightSkyBlue;
        public static Color Facultatif_Color = Color.White;
        public static Color Desactive_Color = Color.Silver;

        public static Color Section_Change_Color = Color.LightSteelBlue;

        public static Hashtable Sections_Canalisation_Equival = new Hashtable();
        public static Hashtable Sections_Regard_Equival = new Hashtable();

        public static Color Get_Field_Color(string field_state)
        {
            Color state_color = Color.White;

            if (field_state == "1")
                state_color = mod_global.Obligatoire_Color;
            else if (field_state == "2")
                state_color = mod_global.Differe_Color;
            else if (field_state == "3")
                state_color = mod_global.Facultatif_Color;
            else if (field_state == "4")
                state_color = mod_global.Desactive_Color;

            return state_color;
        }

        public static string Get_Field_State_By_Color(Color statecolor)
        {
            string state = String.Empty;

            if (statecolor == mod_global.Obligatoire_Color)
                state = "1";
            else if (statecolor == mod_global.Differe_Color)
                state = "2";
            else if (statecolor == mod_global.Facultatif_Color)
                state = "3";
            else if (statecolor == mod_global.Desactive_Color)
                state = "4";

            return state;
        }

        public static bool Check_If_Ouvrage_Name_Exist(string ouvragename)
        {
            bool ouvrage_exist = false;
            XmlNodeList nodelist;

            nodelist = mod_inspection.SVF.SelectNodes("/inspection/ouvrage[@nom='"+ouvragename+"']");

            if (nodelist.Count > 0)
                ouvrage_exist = true;

            return ouvrage_exist;
        }

        public static bool Check_If_Motif_Name_Exist(string motifname)
        {
            bool motif_exist = false;
            XmlNodeList nodelist;

            nodelist = mod_inspection.Motif_Xml.SelectNodes("//motif[@nom='" + motifname + "']");

            if (nodelist.Count > 0)
                motif_exist = true;

            return motif_exist;
        }

        public static bool Check_If_Observation_Code_Name_Exist(string obsid, XmlDocument doc)
        {
            bool obs_exist = false;
            XmlNodeList nodelist;

            nodelist = doc.SelectNodes("codes/code[id='" + obsid + "']");

            if (nodelist.Count > 0)
                obs_exist = true;

            return obs_exist;
        }


        public static bool Check_If_Identification_Code_Name_Exist(string Idid, XmlDocument doc)
        {
            bool id_exist = false;
            XmlNodeList nodelist;

            nodelist = doc.SelectNodes("codes/code[id='" + Idid + "']");

            if (nodelist.Count > 0)
                id_exist = true;

            return id_exist;
        }


        public static bool Check_If_Observation_Item_Name_Exist(string itemname, XmlNode originnod)
        {
            bool item_exist = false;
            XmlNodeList nodelist;

            nodelist = originnod.SelectNodes("item[@nom='" + itemname + "']");

            if (nodelist.Count > 0)
                item_exist = true;

            return item_exist;
        }

        public static bool Check_If_Identification_Item_Name_Exist(string itemname, XmlNode originnod)
        {
            bool item_exist = false;
            XmlNodeList nodelist;

            nodelist = originnod.SelectNodes("item[@nom='" + itemname + "']");

            if (nodelist.Count > 0)
                item_exist = true;

            return item_exist;
        }

        public static bool Check_If_Motif_Is_Still_Used(string motifname)
        {
            bool motif_used = false;
            XmlNodeList nodelist;

            nodelist = mod_observation.Codes_Obs_Cana_Xml.SelectNodes("//representation[@motif='" + motifname + "']");

            if (nodelist.Count > 0)
                motif_used = true;

            nodelist = mod_observation.Codes_Obs_Regard_Xml.SelectNodes("//representation[@motif='" + motifname + "']");

            if (nodelist.Count > 0)
                motif_used = true;

            return motif_used;
        }

        public static string Get_Horaire_Equivalence(string horaire, string forme)
        {
            XmlNode node;
            XmlNode root;

            root = mod_inspection.Section_Ouvrage_Xml.DocumentElement;

            string type = mod_inspection.TYPE_OUVRAGE;

            if (type == "BRANCHEMENT" | type == "TRONCON")
                type = "CANALISATION";

            node = root.SelectSingleNode("section[@ouvrage='" + type + "' and @forme='" + forme + "' and @position='" + mod_inspection.POSITION_SECTION + "']/heure[@id='" + horaire + "']");

            return node.InnerText;
        }

        public static string Get_Id_Parent_Equivalence(string code_parent)
        {
            XmlNode node;
            XmlNode root;

            root = mod_identification.Groupe_Codes_Id_Xml.DocumentElement;

            node = root.SelectSingleNode("identification/titre[@id='" + code_parent + "']");

            return node.InnerText;
        }

        public static string Get_Section_Intitule_By_Code(string code_forme, string type_ouvrage)
        {
            XmlNode node;
            XmlNode root;
            string type;

            root = mod_inspection.Section_Ouvrage_Xml.DocumentElement;

            if (type_ouvrage == String.Empty)
                type = mod_inspection.TYPE_OUVRAGE;
            else
                type = type_ouvrage;

            if (type == "BRANCHEMENT" | type == "TRONCON")
                type = "CANALISATION";

            //On selectionne la section qui correspond a la forme et à la position 1
            node = root.SelectSingleNode("section[@ouvrage='" + type + "' and @forme='" + code_forme + "' and @position='1']");

            return node.Attributes["intitule"].InnerText;
        }

        public static void center_control(Control to_center)
        {
           // to_center.Location = new System.Drawing.Point((to_center.ClientRectangle.Width / 2) - (to_center.Parent.Width / 2), (to_center.ClientRectangle.Height / 2) - (to_center.Parent.Height / 2));
            to_center.Location = new System.Drawing.Point((to_center.Parent.ClientRectangle.Width / 2) - (to_center.Width / 2), (to_center.Parent.ClientRectangle.Height / 2) - (to_center.Height / 2));
        }

        public static XmlNode Get_Codes_Obs_DocElement()
        {
            XmlNode root;

            if (mod_inspection.TYPE_OUVRAGE == "REGARD")
                root = mod_observation.Codes_Obs_Regard_Xml.DocumentElement;
            else
                root = mod_observation.Codes_Obs_Cana_Xml.DocumentElement;

            return root;
        }

        public static XmlNode Get_Codes_Id_DocElement()
        {
            XmlNode root;

            if (mod_inspection.TYPE_OUVRAGE == "REGARD")
                root = mod_identification.Codes_Id_Regard_Xml.DocumentElement;
            else
                root = mod_identification.Codes_Id_Cana_Xml.DocumentElement;

            return root;
        }


        public static void Enable_Ouvrage_Controls()
        {
            MF.AddOuvrageGp.Enabled = true;
            MF.TypeOuvrageGb.Enabled = true;
            MF.OuvrageToolsPanel.Enabled = true;
        }

        public static void Disable_Ouvrage_Controls()
        {
            MF.OuvrageNomTb.Text = String.Empty;
            MF.OuvrageTypeCb.Text = String.Empty;
            MF.OuvrageFormeCb.Text = String.Empty;

            MF.AddOuvrageGp.Enabled = false;
            MF.TypeOuvrageGb.Enabled = false;
            MF.OuvrageToolsPanel.Enabled = false;
        }


        public static void Disable_Main_Tabs()
        {
            MF.IdentificationTab.Enabled = false;
            MF.ObservationTab.Enabled = false;
            MF.RenseignementTab.Enabled = false;
        }

        public static void Enable_Main_Tabs()
        {
            MF.IdentificationTab.Enabled = true;
            MF.ObservationTab.Enabled = true;
            MF.RenseignementTab.Enabled = true;
        }


        public static void Disable_Obs_Tools()
        {
            MF.DataSplit.Panel2Collapsed = true;
            MF.ObsToolsLb.Visible = false;
            MF.ObsCompactGridBt.Visible = false;
            MF.ObsDeplieGridBt.Visible = false;

            MF.ObsDiffereSp.Visible = false;
            MF.ObsDiffereLb.Visible = false;
            MF.ObsDiffereCountLb.Visible = false;
        }

        public static void Enable_Obs_Tools()
        {
            MF.DataSplit.Panel2Collapsed = false;
            MF.ObsToolsLb.Visible = true;
            MF.ObsCompactGridBt.Visible = true;
            MF.ObsDeplieGridBt.Visible = true;
        }

    }
}
