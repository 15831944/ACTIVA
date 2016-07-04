using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using C1.Win.C1Command;
using ACTIVA_Module_1.component;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1.modules
{
    public static class mod_identification
    {
        public static XmlDocument Groupe_Codes_Id_Xml = new XmlDocument();
        public static XmlDocument Codes_Id_Cana_Xml = new XmlDocument();
        public static XmlDocument Codes_Id_Regard_Xml = new XmlDocument();

        public static bool SaveIDFlag;

        /// <summary>
        /// Fonction de chargement du fichier XML de configuration Groupe_Code_Identification
        /// </summary>
        /// <param name="groupe_code_id_path"></param>

        public static void Load_Groupe_Codes_Id(string groupe_code_id_path)
        {
           try
            {
               Groupe_Codes_Id_Xml.Load(groupe_code_id_path);
            }
           catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Fonction de chargement du fichier XML de configuration Code_Identification_Canalisation
        /// </summary>
        /// <param name="code_id_cana_path"></param>

        public static void Load_Codes_Id_Cana(string code_id_cana_path)
        {
            try
            {
                Codes_Id_Cana_Xml.Load(code_id_cana_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Fonction de chargement du fichier XML de configuration Code_Identification_Regard
        /// </summary>
        /// <param name="code_id_regard_path"></param>

        public static void Load_Codes_Id_Regard(string code_id_regard_path)
        {
            try
            {
                Codes_Id_Regard_Xml.Load(code_id_regard_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //-------------------------------------------------------------------------

        public static void Fill_Id_Menu(C1TopicBar tpbar, FlowLayoutPanel flp)
        {
            Clear_Identification_Tab(tpbar,flp);
            Get_Groupe_Identification(tpbar);
            Get_Code_Identification(tpbar);

            // Supprimer les parents vides
            for (int i = 0; i < tpbar.Pages.Count; i++)
            {
                if (tpbar.Pages[i].Links.Count == 0)
                    tpbar.Pages.Remove(tpbar.Pages[i]);
            }

            Init_Fields_Status(tpbar);


            //on initialise le flag de sauvegarde 
            SaveIDFlag = false;
        }

        public static void Clear_Identification_Tab(C1TopicBar tpbar, FlowLayoutPanel flp)
        {
            mod_global.MF.IdFormLabel.Text = String.Empty;
            tpbar.Pages.Clear();
            flp.Controls.Clear();
        }

        public static void Get_Groupe_Identification(C1TopicBar tpbar)
        {
            XmlNodeList myChildNode = Groupe_Codes_Id_Xml.GetElementsByTagName("titre");

            foreach (XmlNode unNode in myChildNode)
            {
                C1.Win.C1Command.C1TopicPage c1TopicPage1 = new C1.Win.C1Command.C1TopicPage();
                c1TopicPage1.Text = unNode.InnerText;
                c1TopicPage1.Tag = unNode.Attributes["id"].InnerText;
                c1TopicPage1.Collapse();
                tpbar.Pages.Add(c1TopicPage1);
            }
        }

        public static void Get_Code_Identification(C1TopicBar tpbar)
        {
            XmlNode root;
            XPathNavigator IdItem;
            XPathNavigator IntituleItem;
            XPathNavigator InspectItem;
          
            root = mod_global.Get_Codes_Id_DocElement();
  
            //On utilise un navigateur pour pouvoir trier les noeuds
            XPathNavigator nav = root.CreateNavigator();
            XPathExpression exp = nav.Compile("//code");

            exp.AddSort("@position", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);

            foreach (XPathNavigator item in nav.Select(exp))
            {
                IdItem = item.SelectSingleNode("id");

                IntituleItem = item.SelectSingleNode("intitule");
                InspectItem = item.SelectSingleNode("inspection");

                if (InspectItem.GetAttribute("corresp", "") == "")
                {
                    C1.Win.C1Command.C1TopicLink link = new C1.Win.C1Command.C1TopicLink();
                    if (item.GetAttribute("ajoute", "") == "true")
                        link.Text = string.Concat("** " + IntituleItem.Value, " - ", IdItem.Value);
                    else
                        link.Text = string.Concat(IntituleItem.Value, " - ", IdItem.Value);
                    link.Tag = IdItem.Value;
                    tpbar.FindPageByTag(item.GetAttribute("parent", "")).Links.Add(link);
                }
            }
            /* ANCIEN CODE DE NS (remplacé par GB le 16/12/2009)
            XmlNodeList nodeList;
            XmlNode IdNode;
            XmlNode IntituleNode;

            //nodeList = root.SelectNodes(string.Concat("//code"));

            foreach (XmlNode unNode in nodeList)
            {
                IdNode = unNode.SelectSingleNode("id");
                IntituleNode = unNode.SelectSingleNode("intitule");

                C1.Win.C1Command.C1TopicLink link = new C1.Win.C1Command.C1TopicLink();
                link.Text = string.Concat(IntituleNode.InnerText, " - ", IdNode.InnerText);
                link.Tag = IdNode.InnerText;
                tpbar.FindPageByTag(unNode.Attributes["parent"].InnerText).Links.Add(link);
            }*/
        }

        public static string Get_Value_Identification(string code_to_get)
        {
            XmlNode node;
            XmlNode root = mod_accueil.SVF.DocumentElement;
            string value = string.Empty;

            try
            {
                node = root.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/identifications/code[id='" + code_to_get + "']/valeur");
                if (node.Attributes["code"] == null)
                    value = node.InnerText;
                else value = node.Attributes["code"].InnerText + " | " + node.InnerText; 
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return value;
            } 
        }

        public static string Get_Unite_For_Id_Code(string code)
        {
            XmlNode node;

            node = mod_global.Get_Codes_Id_DocElement().SelectSingleNode("code[id='" + code + "']/valeur");

            if (node.Attributes.GetNamedItem("unite") != null)
                return node.Attributes["unite"].InnerText;
            else
                return string.Empty;
        }

        //Donne le focus au control ayant codeId comme nom
        public static void FocusInputBoxParID(FlowLayoutPanel flp, string codeId)
        {
            identification_input Box;

            int temp = flp.Controls.Count;
            foreach (Control Ctrl in flp.Controls) {
                Console.WriteLine(Ctrl.GetType().ToString());

                Box = (identification_input)Ctrl;
                if (Box.field_label.Name == codeId)
                {
                
                    Box.Focus();
                    return;
                }
            }
         
        }

      
        public static void Fill_Identification_Form(string groupe, FlowLayoutPanel flp)
        {
            XmlNode root;
            string unite = string.Empty;
            bool ajoute = false;
            XPathNavigator IdItem;
            XPathNavigator ValItem;
            XPathNavigator IntituleItem;
            XPathNavigator RenseigneItem;
            XPathNavigator InspectItem;

            root = mod_global.Get_Codes_Id_DocElement();

            flp.Controls.Clear();

            //On utilise un navigateur pour pouvoir trier les noeuds
            XPathNavigator nav = root.CreateNavigator();
            XPathExpression exp = nav.Compile(string.Concat("//code[@parent='", groupe, "']"));

            exp.AddSort("@position", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);

            foreach (XPathNavigator item in nav.Select(exp))
            {
                IdItem = item.SelectSingleNode("id");
                ValItem = item.SelectSingleNode("valeur");
                IntituleItem = item.SelectSingleNode("intitule");
                RenseigneItem = item.SelectSingleNode("renseigne");
                InspectItem = item.SelectSingleNode("inspection");

                if (InspectItem.GetAttribute("corresp", "") == "")
                {
                    if (item.GetAttribute("ajoute", "") != "")
                        ajoute = bool.Parse(item.GetAttribute("ajoute", ""));

                    string nom_complet = IdItem.Value + " | " + IntituleItem.Value;

                    unite = Get_Unite_For_Id_Code(IdItem.Value);
                    if (unite != string.Empty)
                        nom_complet += " (" + unite + ")";

                    string value = Get_Value_Identification(IdItem.Value);

                    identification_input id_box = new identification_input(nom_complet, IdItem.Value, value, RenseigneItem.Value, ValItem.GetAttribute("type", ""), ajoute, groupe);

                    ajoute = false;
                    flp.Controls.Add(id_box);
                }
            }

            /* ANCIEN CODE DE NS (remplacé par GB le 16/12/2009)
            XmlNodeList nodeList;
            XmlNode IdNode;
            XmlNode ValNode;
            XmlNode IntituleNode;
            XmlNode RenseigneNode;
            
            nodeList = root.SelectNodes(string.Concat("//code[@parent='", groupe, "']"));

            foreach (XmlNode unNode in nodeList)
            {
                IdNode = unNode.SelectSingleNode("id");
                ValNode = unNode.SelectSingleNode("valeur");
                IntituleNode = unNode.SelectSingleNode("intitule");
                RenseigneNode = unNode.SelectSingleNode("renseigne");

                if (unNode.Attributes.GetNamedItem("ajoute") != null)
                    ajoute = bool.Parse(unNode.Attributes.GetNamedItem("ajoute").InnerText);

                string nom_complet = IdNode.InnerText + " | " + IntituleNode.InnerText;

                unite = Get_Unite_For_Id_Code(IdNode.InnerText);
                if (unite != string.Empty)
                    nom_complet += " (" + unite + ")";

                string value = Get_Value_Identification(IdNode.InnerText);

                identification_input id_box = new identification_input(nom_complet, IdNode.InnerText, value, RenseigneNode.InnerText, ValNode.Attributes["type"].InnerText, ajoute, groupe);

                ajoute = false;
                flp.Controls.Add(id_box);
            }
            */
        }

        public static void Check_Fields_Status(C1TopicPage Tp)
        {
            XmlNode svfroot = mod_accueil.SVF.DocumentElement;
            XmlNode idroot = mod_global.Get_Codes_Id_DocElement();
            string value = string.Empty;
            XmlNodeList svfnodelist;
            XmlNode idnode;

            Tp.SpecialStyle = false;

                foreach (C1TopicLink Tl in Tp.Links)
                {
                    //Si le champ est obligatoire
                    idnode = idroot.SelectSingleNode("/codes/code[id='" + Tl.Tag.ToString() + "']/renseigne");
                    if (idnode.InnerText == "1")
                    {
                        //On verifie que le champ est bien présent dans le SVF
                        svfnodelist = svfroot.SelectNodes("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/identifications/code[id='" + Tl.Tag.ToString() + "']/valeur");
                        if (svfnodelist.Count > 0)
                        {
                            //Et que sa valeur n'est pas nulle
                            if (svfnodelist[0].InnerText == String.Empty)
                                Tp.SpecialStyle = true;
                        }
                        else
                        {
                            //Si le champ manque dans le SVF
                            Tp.SpecialStyle = true;
                        }
                    }

                }
            }

        public static void Init_Fields_Status(C1TopicBar Tb)
        {
            foreach (C1TopicPage Tp in Tb.Pages)
                Check_Fields_Status(Tp);
        }

        //-------------------------------------------------------------------------

  }
        
}
