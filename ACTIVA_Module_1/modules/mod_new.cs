using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.Text;

namespace ACTIVA_Module_1.modules
{
    class mod_new
    {
        public static void Create_New_Inspection(string name, string path)
        {
            if (name != String.Empty & path != String.Empty)
            {
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlElement inspectionNode = doc.CreateElement("inspection");
                doc.AppendChild(inspectionNode);

                string new_svf_folder = System.IO.Path.Combine(path, name);
                string new_img_folder = System.IO.Path.Combine(new_svf_folder, "img");
                string new_svf = System.IO.Path.Combine(new_svf_folder, name + ".svf");

                System.IO.Directory.CreateDirectory(new_svf_folder);
                System.IO.Directory.CreateDirectory(new_img_folder);

                doc.Save(new_svf);

                mod_inspection.Load_SVF(new_svf);
                mod_inspection.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
                mod_inspection.Fill_Ouvrage_List(mod_global.MF.OuvrageList);

                mod_global.MF.NewInspectionNameTb.Text = String.Empty;
                mod_global.MF.NewInspectionPathTb.Text = String.Empty;
                mod_global.MF.openSVFTb.Text = String.Empty;

                mod_global.Enable_Ouvrage_Controls();
            }
            else
            {
                //System.Windows.Forms.MessageBox
            }
        }

        public static void Fill_New_Ouvrage_Type_Combo(ComboBox ouvragetypecb)
        {
            ArrayList types = new ArrayList();

            types.Add(new AddValue("TRONCON", "CANALISATION"));
            types.Add(new AddValue("BRANCHEMENT", "CANALISATION"));
            types.Add(new AddValue("REGARD", "REGARD"));

            ouvragetypecb.DataSource = types;
            ouvragetypecb.ValueMember = "Value";
            ouvragetypecb.DisplayMember = "Display";
        }

        public static void Fill_New_Ouvrage_Forme_Combo(string typeouvrage, ComboBox ouvrageformecb)
        {
            XmlNodeList nodelist;
            ArrayList formes = new ArrayList();

            if (typeouvrage == "CANALISATION")
                nodelist = mod_identification.Codes_Id_Cana_Xml.DocumentElement.SelectNodes("code[id='ACA']/valeur/item");
            else if(typeouvrage == "REGARD")
                nodelist = mod_identification.Codes_Id_Regard_Xml.DocumentElement.SelectNodes("code[id='CCA']/valeur/item");
            else
                return;

            //root = mod_inspection.Section_Ouvrage_Xml.DocumentElement;

            //On verifie qu'il existe bien un noeud avec cette forme et cette position
            //nodelist = root.SelectNodes("section[@ouvrage='" + typeouvrage + "' and @position='1']");

            foreach (XmlNode nod in nodelist)
            {
                formes.Add(new AddValue(nod.InnerText, nod.Attributes["nom"].InnerText));
            }

            ouvrageformecb.DataSource = formes;
            ouvrageformecb.ValueMember = "Value";
            ouvrageformecb.DisplayMember = "Display";
        }

        private static XmlElement PreFill_Forme_Id_Code(string typeouvrage, string codeforme, XmlElement idroot)
        {
            XmlNode node;
            string code;

            if (typeouvrage == "TRONCON" | typeouvrage == "BRANCHEMENT" | typeouvrage == "CANALISATION")
            {
                code = "ACA";
                node = mod_identification.Codes_Id_Cana_Xml.DocumentElement.SelectSingleNode("code[id='ACA']");
            }
            else if (typeouvrage == "REGARD")
            {
                code = "CCA";
                node = mod_identification.Codes_Id_Regard_Xml.DocumentElement.SelectSingleNode("code[id='CCA']");
            }
            else
            {
                return idroot;
            }

            string nodeintitule = node.SelectSingleNode("intitule").InnerText;
            string nodeparent = node.Attributes["parent"].InnerText;
            string intituleforme = node.SelectSingleNode("valeur/item[@nom='" + codeforme + "']").InnerText;

             //On crée les noeuds
             XmlElement elem_code = mod_inspection.SVF.CreateElement("code");
             XmlElement newidnode = mod_inspection.SVF.CreateElement("id");
             XmlElement newintitulenode = mod_inspection.SVF.CreateElement("intitule");
             XmlElement newvaleurnode = mod_inspection.SVF.CreateElement("valeur");

             //On remplie la valeur de chaque noeud
             newidnode.InnerText = code;
             newintitulenode.InnerText = nodeintitule;
                
             //On ajoute les attributs aux noeuds
             elem_code.SetAttribute("parent", nodeparent);

             newvaleurnode.InnerText = intituleforme;
             newvaleurnode.SetAttribute("code", codeforme);

             //On ajoute les sous-noeuds au noeud code
             elem_code.AppendChild(newidnode);
             elem_code.AppendChild(newintitulenode);
             elem_code.AppendChild(newvaleurnode);

             //On ajoute le noeud code au noeud identifications du SVF
             idroot.AppendChild(elem_code);

             return idroot;
        }

        public static void Create_New_Ouvrage(string nom, object type, string forme)
        {
            if (nom != String.Empty & forme != String.Empty)
            {
               if (mod_global.Check_If_Ouvrage_Name_Exist(nom) == true)
                   {
                       System.Windows.Forms.MessageBox.Show("Ce nom d'ouvrage existe déjà.");
                       return;
                   }

                AddValue atype = (AddValue)type;

                XmlElement ouvrageNode = mod_inspection.SVF.CreateElement("ouvrage");
                ouvrageNode.SetAttribute("nom", nom);
                ouvrageNode.SetAttribute("type", atype.Display);
                ouvrageNode.SetAttribute("forme", forme);
                ouvrageNode.SetAttribute("position", mod_inspection.Get_New_Ouvrage_Position());

                XmlElement IdentificationNode = mod_inspection.SVF.CreateElement("identifications");
                XmlElement ObservationNode = mod_inspection.SVF.CreateElement("observations");

                IdentificationNode = PreFill_Forme_Id_Code(atype.Display, forme, IdentificationNode);

                ouvrageNode.AppendChild(IdentificationNode);
                ouvrageNode.AppendChild(ObservationNode);

                mod_inspection.SVF.DocumentElement.AppendChild(ouvrageNode);

                mod_inspection.SVF.Save(System.IO.Path.Combine(mod_inspection.SVF_FOLDER, mod_inspection.SVF_FILENAME));

                mod_inspection.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
                mod_inspection.Fill_Ouvrage_List(mod_global.MF.OuvrageList);

                mod_global.MF.OuvrageNomTb.Text = String.Empty;
            }
        }

        public static void Clone_Selected_Ouvrage(string ouvragename)
        {
            XmlNode current_ouvrage_node = mod_inspection.SVF.DocumentElement.SelectSingleNode(string.Concat("//ouvrage[@nom='", mod_inspection.OUVRAGE, "']"));

            XmlNode new_ouvrage_node = current_ouvrage_node.CloneNode(false);
            new_ouvrage_node.Attributes["nom"].InnerText = ouvragename;
            new_ouvrage_node.Attributes["position"].InnerText = mod_inspection.Get_New_Ouvrage_Position();

            XmlElement IdentificationNode = mod_inspection.SVF.CreateElement("identifications");
            XmlElement ObservationNode = mod_inspection.SVF.CreateElement("observations");

            new_ouvrage_node.AppendChild(IdentificationNode);
            new_ouvrage_node.AppendChild(ObservationNode);

            mod_inspection.SVF.DocumentElement.AppendChild(new_ouvrage_node);
            mod_inspection.SVF.Save(System.IO.Path.Combine(mod_inspection.SVF_FOLDER, mod_inspection.SVF_FILENAME));

            mod_inspection.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
            mod_inspection.Fill_Ouvrage_List(mod_global.MF.OuvrageList);
        }

        public static void Delete_Selected_Ouvrage()
        {
            XmlNode current_ouvrage_node = mod_inspection.SVF.DocumentElement.SelectSingleNode(string.Concat("//ouvrage[@nom='", mod_inspection.OUVRAGE, "']"));
            mod_inspection.SVF.DocumentElement.RemoveChild(current_ouvrage_node);
            mod_inspection.SVF.Save(System.IO.Path.Combine(mod_inspection.SVF_FOLDER, mod_inspection.SVF_FILENAME));

            mod_inspection.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
            mod_inspection.Fill_Ouvrage_List(mod_global.MF.OuvrageList);
        }
    }

    public class AddValue
    {
        private string m_Display;
        private string m_Value;
        public AddValue(string Display, string Value)
        {
            m_Display = Display;
            m_Value = Value;
        }
        public string Display
        {
            get { return m_Display; }
        }
        public string Value
        {
            get { return m_Value; }
        }
    }
}
