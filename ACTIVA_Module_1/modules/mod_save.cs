using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ACTIVA_Module_1.component;
using System.Windows.Forms;
using System.Globalization;

namespace ACTIVA_Module_1.modules
{
    public class mod_save
    {

        //Sauvegarde de l'onglet IDENTIFICATION
        public static void Save_Identification_Panel(FlowLayoutPanel flp)
        {
            identification_input idinput;

            //On vérifie que le FlowLayoutPanel contient au moins un champs à mettre à jour
            if (flp.Controls.Count > 0)
            {
                foreach (Control ct in flp.Controls)
                {
                    if (ct is identification_input)
                    {
                        idinput = (identification_input)ct;
                        Save_Identification_Field(idinput);
                        mod_global.MF.statusPanel.Text = "Sauvegardé: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
        }

        //Sauvegarde de l'onglet Inspection
        public static void Save_Inspection_Panel(FlowLayoutPanel flp)
        {
            identification_input idinput;

            //On vérifie que le FlowLayoutPanel contient au moins un champs à mettre à jour
            if (flp.Controls.Count > 0)
            {
                foreach (Control ct in flp.Controls)
                {
                    if (ct is identification_input)
                    {
                        idinput = (identification_input)ct;
                        //Si la valeur du champs n'est pas vide on le met à jour
                        //if (idinput.Field_Input.Text.Trim() != String.Empty)
                        Save_Inspection_Field(idinput);
                        mod_global.MF.statusPanel.Text = "Sauvegardé: " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
        }

        private static void Save_Identification_Field(identification_input idinput)
        {
            XmlNodeList svfnodelist;
            XmlNode svfroot = mod_accueil.SVF.DocumentElement;
            XmlNode idroot;
            XmlElement valeur;

            //On verifie que le champ est bien présent dans le SVF on met à jour sa valeur
            svfnodelist = svfroot.SelectNodes("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/identifications/code[id='" + idinput.field_label.Name + "']");
            if (svfnodelist.Count > 0)
            {
                valeur = (XmlElement)svfnodelist[0].SelectSingleNode("valeur");

                if (idinput.Field_Input.Tag.ToString() == "item")
                {
                    if (idinput.Field_Input.Text != String.Empty)
                    {
                        if (idinput.Field_Input.Text.Contains("|"))
                        {
                            valeur.InnerText = idinput.Field_Input.Text.Split(char.Parse("|"))[1].Trim();
                            valeur.SetAttribute("code", idinput.Field_Input.Text.Split(char.Parse("|"))[0].Trim());
                        }
                    }
                    else
                    {
                        valeur.InnerText = String.Empty;
                    }
                }
                else
                {
                    valeur.InnerText = idinput.Field_Input.Text;
                }
            }
            else
            {
                //Si le code n'existe pas dans le SVF on le crée ainsi que ses sous noeuds
                
                //On crée les noeuds
                XmlElement elem_code = mod_accueil.SVF.CreateElement("code");
                XmlElement newidnode = mod_accueil.SVF.CreateElement("id");
                XmlElement newintitulenode = mod_accueil.SVF.CreateElement("intitule");
                XmlElement newvaleurnode = mod_accueil.SVF.CreateElement("valeur");

                //On remplie la valeur de chaque noeud
                newidnode.InnerText = idinput.field_label.Name;
                newintitulenode.InnerText = idinput.field_label.Text.Split(char.Parse("|"))[1].Trim();
                

                //On ajoute les attributs aux noeuds
                elem_code.SetAttribute("parent", idinput.field_label.Tag.ToString());

                if (idinput.Field_Input.Tag.ToString() == "item")
                {
                    if (idinput.Field_Input.Text != String.Empty)
                    {
                        newvaleurnode.InnerText = idinput.Field_Input.Text.Split(char.Parse("|"))[1].Trim();
                        newvaleurnode.SetAttribute("code", idinput.Field_Input.Text.Split(char.Parse("|"))[0].Trim());
                    }
                    else
                    {
                        newvaleurnode.InnerText = String.Empty;
                    }
                }
                else
                {
                    newvaleurnode.InnerText = idinput.Field_Input.Text;
                }

                //------------------------------ Partie désactivée -----------------------------
                //XmlAttribute att_type = mod_inspection.SVF.CreateAttribute("type");
                //att_type.Value = idinput.Field_Input.Tag.ToString();
                //newvaleurnode.Attributes.Append(att_type);

                //if (idinput.is_ajoute() == true)
                //{
                //    XmlAttribute att_ajoute = mod_inspection.SVF.CreateAttribute("ajoute");
                //    att_ajoute.Value = idinput.is_ajoute().ToString();
                //    newintitulenode.Attributes.Append(att_ajoute);
                //}
                //------------------------------------------------------------------------------

                //On ajoute les sous-noeuds au noeud code
                elem_code.AppendChild(newidnode);
                elem_code.AppendChild(newintitulenode);
                elem_code.AppendChild(newvaleurnode);

                //On ajoute le noeud code au noeud identifications du SVF
                idroot = svfroot.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/identifications");
                idroot.AppendChild(elem_code);
            }
            mod_accueil.SVF.Save(System.IO.Path.Combine(mod_accueil.SVF_FOLDER, mod_accueil.SVF_FILENAME));
        }

        private static void Save_Inspection_Field(identification_input idinput)
        {
            XmlNodeList svfnodelist;
            XmlNode svfroot = mod_accueil.SVF.DocumentElement;
            XmlNode idroot;
            XmlElement valeur;

            //On verifie que le champ est bien présent dans le SVF on met à jour sa valeur
            svfnodelist = svfroot.SelectNodes("/inspection/identifications/code[id='" + idinput.field_label.Name + "']");
            if (svfnodelist.Count > 0)
            {
                valeur = (XmlElement)svfnodelist[0].SelectSingleNode("valeur");

                if (idinput.Field_Input.Tag.ToString() == "item")
                {
                    if (idinput.Field_Input.Text != String.Empty)
                    {
                        if (idinput.Field_Input.Text.Contains("|"))
                        {
                            valeur.InnerText = idinput.Field_Input.Text.Split(char.Parse("|"))[1].Trim();
                            valeur.SetAttribute("code", idinput.Field_Input.Text.Split(char.Parse("|"))[0].Trim());
                        }
                    }
                    else
                    {
                        valeur.InnerText = String.Empty;
                    }
                }
                else
                {
                    valeur.InnerText = idinput.Field_Input.Text;
                }
            }
            else
            {
                //Si le code n'existe pas dans le SVF on le crée ainsi que ses sous noeuds

                //On crée les noeuds
                XmlElement elem_code = mod_accueil.SVF.CreateElement("code");
                XmlElement newidnode = mod_accueil.SVF.CreateElement("id");
                XmlElement newintitulenode = mod_accueil.SVF.CreateElement("intitule");
                XmlElement newvaleurnode = mod_accueil.SVF.CreateElement("valeur");
                
                //On remplie la valeur de chaque noeud
                newidnode.InnerText = idinput.field_label.Name;
                newintitulenode.InnerText = idinput.field_label.Text.Split(char.Parse("|"))[1].Trim();


                //On ajoute les attributs aux noeuds
                elem_code.SetAttribute("parent", idinput.field_label.Tag.ToString());
                elem_code.SetAttribute("corresp", mod_global.Get_Codes_Insp_DocElement().SelectSingleNode("code[id='" + idinput.field_label.Name + "']/inspection").Attributes["corresp"].InnerText);

                if (idinput.Field_Input.Tag.ToString() == "item")
                {
                    if (idinput.Field_Input.Text != String.Empty)
                    {
                        newvaleurnode.InnerText = idinput.Field_Input.Text.Split(char.Parse("|"))[1].Trim();
                    }
                    else
                    {
                        newvaleurnode.InnerText = String.Empty;
                    }
                }
                else
                {
                    newvaleurnode.InnerText = idinput.Field_Input.Text;
                    newvaleurnode.SetAttribute("ajoute", "true");
                }

                //On ajoute les sous-noeuds au noeud code
                elem_code.AppendChild(newidnode);
                elem_code.AppendChild(newintitulenode);
                elem_code.AppendChild(newvaleurnode);

                //On ajoute le noeud code au noeud identifications du SVF
                idroot = svfroot.SelectSingleNode("/inspection/identifications");
                idroot.AppendChild(elem_code);
            }
            mod_accueil.SVF.Save(System.IO.Path.Combine(mod_accueil.SVF_FOLDER, mod_accueil.SVF_FILENAME));
        }
        /// <summary>
        /// Fonction de sauvegarde des champs de l'onglet saisie d'observation
        /// </summary>
        /// <param name="code"></param>
        /// <param name="num"></param>
        /// <returns></returns>

        public static string Save_Observation_Panel(string code, string num)
        {

            XmlNode svfroot = mod_accueil.SVF.DocumentElement;
            XmlNode idroot;
            string savenum = string.Empty;

            //Si le num envoyé n'est pas vide, c'est que nous sommes sur une observation a modifier
            if (num != String.Empty)
            {
                XmlNode codenode = svfroot.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/observations/code[@num='" + num + "']");
                XmlNode caracnode = codenode.SelectSingleNode("caracteristiques");
                XmlNode newcaracnode = Make_Obs_Caracteristiques();

                //Si le code sauvé est un changement de forme on met à jour l'attribut forme de son noeud
                if (code == "AEC" | code == "CEC")
                {
                    XmlNode c1node = newcaracnode.SelectSingleNode("caracteristique[@nom='c1']");
                    codenode.Attributes["forme"].Value = c1node.Attributes["code"].InnerText;
                }

                //On met à jour les caractéristiques
                codenode.ReplaceChild(newcaracnode, caracnode);

                savenum = num;
            }
            else
            {
                //Sinon on ajoute le code et ses caractéristiques
                //On crée les noeuds
                XmlElement elem_code = mod_accueil.SVF.CreateElement("code");
                XmlNode newidnode = mod_accueil.SVF.CreateNode("element", "id", "");
                //XmlNode newinspectionnode = mod_inspection.SVF.CreateNode("element", "inspection", "");
                XmlNode newintitulenode = mod_accueil.SVF.CreateNode("element", "intitule", "");

                //On recupere les caractéristiques
                XmlNode newcaracnode = Make_Obs_Caracteristiques();

                //On crée un nouveau numéro d'observation
                savenum = mod_accueil.Get_New_Obs_Num();

                //On récupère l'intitulé du code à ajouter
                XmlNode codeintitulenode = mod_global.Get_Codes_Obs_DocElement().SelectSingleNode("/codes/code[id='"+code+"']/intitule");

                //On remplie la valeur de chaque noeud
                newidnode.InnerText = code;
                newintitulenode.InnerText = codeintitulenode.InnerText;

                XmlAttribute att_forme = mod_accueil.SVF.CreateAttribute("forme");

                //Si le code sauvé est un changement de forme on met à jour l'attribut forme de son noeud, sinon on prend la forme en cours
                if (code == "AEC" | code == "CEC")
                {
                    XmlNode c1node = newcaracnode.SelectSingleNode("caracteristique[@nom='c1']");
                    if (c1node.Attributes.GetNamedItem("code") != null)
                        att_forme.Value = c1node.Attributes["code"].InnerText;
                    else
                        att_forme.Value = mod_accueil.Get_Current_Ouvrage_Forme();
                }
                else
                {
                    att_forme.Value = mod_accueil.Get_Current_Ouvrage_Forme();
                }

                elem_code.Attributes.Append(att_forme);

                XmlAttribute att_num = mod_accueil.SVF.CreateAttribute("num");
                att_num.Value = savenum;
                elem_code.Attributes.Append(att_num);

                //On ajoute les sous-noeuds au noeud code
                elem_code.AppendChild(newidnode);
                elem_code.AppendChild(newintitulenode);
                elem_code.AppendChild(newcaracnode);

                //On ajoute le noeud code au noeud identifications du SVF
                idroot = svfroot.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/observations");
                idroot.AppendChild(elem_code);
            }

            mod_accueil.SVF.Save(System.IO.Path.Combine(mod_accueil.SVF_FOLDER, mod_accueil.SVF_FILENAME));

            mod_global.MF.LineaireStripLabel.Text = mod_accueil.Get_Max_Pm_In_SVF() + " m";

            return savenum;
        }

        /// <summary>
        /// Fonction de vérification du linéaire de changement de forme saisi. Il ne peut être infèrieur au dernier linéaire
        /// </summary>
        /// <returns></returns>

        public static bool Verify_Observation_Panel_Is_Pm_More_Than_Last_Pm()
        {
            double result;
            TextBox CText = (TextBox)mod_global.Focused_Carac_Panel.CaracNameToTb["pm1"];

            if (CText.Text == String.Empty)
                return false;

            if (double.TryParse(CText.Text,out result) == false)
                return false;

            if (double.Parse(CText.Text) < mod_accueil.LAST_PM)
                return false;

            return true;
        }

        /// <summary>
        /// Fonction de vérification du linéaire saisi. Il ne peut être infèrieur au linéaire du dernier changement de forme (pb changement de forme)
        /// </summary>
        /// <returns></returns>

        public static bool Verify_Observation_Panel_Is_Pm_More_Than_Last_AEC_CEC()
        {
            double result;
            TextBox CText = (TextBox)mod_global.Focused_Carac_Panel.CaracNameToTb["pm1"];

            if (CText.Text == String.Empty)
                return false;

            if (double.TryParse(CText.Text, out result) == false)
                return false;

            if (double.Parse(CText.Text) < mod_accueil.LAST_AEC_CEC_PM)
                return false;

            return true;
        }

        /// <summary>
        /// Fonction de vérification de saisie des champs obligatoires de l'onglet de saisie d'observation
        /// </summary>
        /// <returns></returns>

        public static bool Verify_Observation_Panel_Required_Fields()
        {
            TextBox CText;
            //CheckBox CCheck;
            ComboBox CCombo;

            bool all_filled = true;
            //On parcourt tous les input du formulaire d'observation
            foreach (object key in mod_global.Focused_Carac_Panel.CaracNameToTb.Keys)
            {
                //Si on passe sur le cas d'un assemblage ou de posregard, on traite differemment d'une textbox
                if (key.ToString() == "assemblage")
                {
                    //Dans le cas d'assemblage on ne teste pas la valeur saisie, puisque c'est une checkbox
                }
                else if (key.ToString() == "posregard")
                {
                    CCombo = (ComboBox)mod_global.Focused_Carac_Panel.CaracNameToTb[key.ToString()];

                    //Si le champ est obligatoire mais que sa valeur est vide on renvoie false
                    if (CCombo.BackColor == mod_global.Obligatoire_Color & CCombo.Text == String.Empty)
                        all_filled = false;
                }
                else
                {
                    CText = (TextBox)mod_global.Focused_Carac_Panel.CaracNameToTb[key.ToString()];

                    //Si le champ est obligatoire mais que sa valeur est vide on renvoie false
                    if (CText.BackColor == mod_global.Obligatoire_Color & CText.Text == String.Empty)
                        all_filled = false;
                }
            }

            return all_filled;
        }

        /// <summary>
        /// Fonction de parcours des champs de l'onglet de saisie des observation avant sauvegarde
        /// </summary>
        /// <returns></returns>
        /// 
        private static XmlNode Make_Obs_Caracteristiques()
        {
            TextBox CText;
            CheckBox CCheck;
            ComboBox CCombo;

            XmlNode newcaracnode = mod_accueil.SVF.CreateNode("element", "caracteristiques", "");

            //On parcourt tous les input du formulaire d'observation
            foreach (object key in mod_global.Focused_Carac_Panel.CaracNameToTb.Keys)
            {
                //Si on passe sur le cas d'un assemblage ou de posregard, on traite differemment d'une textbox
                if (key.ToString() == "assemblage")
                {
                    CCheck = (CheckBox)mod_global.Focused_Carac_Panel.CaracNameToTb[key.ToString()];

                    XmlNode elem_carac = Create_One_Caracteristique(key.ToString(), mod_global.Get_Field_State_By_Color(CCheck.BackColor));

                    if (mod_global.Get_Field_State_By_Color(CCheck.BackColor) != "4")
                        elem_carac.InnerText = CCheck.Checked.ToString();

                    newcaracnode.AppendChild(elem_carac);
                }
                else if (key.ToString() == "posregard")
                {
                    if (mod_accueil.TYPE_OUVRAGE == "REGARD")
                    {
                        CCombo = (ComboBox)mod_global.Focused_Carac_Panel.CaracNameToTb[key.ToString()];

                        XmlNode elem_carac = Create_One_Caracteristique(key.ToString(), mod_global.Get_Field_State_By_Color(CCombo.BackColor));

                        if (mod_global.Get_Field_State_By_Color(CCombo.BackColor) != "4")
                        {
                            XmlAttribute att_code = mod_accueil.SVF.CreateAttribute("codeR");
                            att_code.Value = CCombo.Text.Split(char.Parse("|"))[0].Trim();
                            elem_carac.Attributes.Append(att_code);
                            elem_carac.InnerText = CCombo.Text.Split(char.Parse("|"))[1].Trim();
                        }

                        newcaracnode.AppendChild(elem_carac);
                    }
                }
                else
                {
                    CText = (TextBox)mod_global.Focused_Carac_Panel.CaracNameToTb[key.ToString()];

                    XmlNode elem_carac = Create_One_Caracteristique(key.ToString(), mod_global.Get_Field_State_By_Color(CText.BackColor));

                    //On verifie que le champs n'est pas désactivé
                    if (mod_global.Get_Field_State_By_Color(CText.BackColor) != "4")
                    {
                        if (CText.Text != String.Empty)
                        {
                            //Dans le cas de c1 ou c2 on crée un attribut code pour stocker le code de la norme du qcm
                            if (key.ToString() == "c1" | key.ToString() == "c2")
                            {
                                if (CText.Text.Contains("|"))
                                {
                                    XmlAttribute att_code = mod_accueil.SVF.CreateAttribute("code");
                                    att_code.Value = CText.Text.Split(char.Parse("|"))[0].Trim();
                                    elem_carac.Attributes.Append(att_code);
                                    elem_carac.InnerText = CText.Text.Split(char.Parse("|"))[1].Trim();
                                }
                                else
                                {
                                    elem_carac.InnerText = CText.Text;
                                }
                            }
                            //Dans le cas de h1 ou h2 on crée un attribut  correspondance pour stocker l'equivalence horaire
                            else if (key.ToString() == "h1" | key.ToString() == "h2")
                            {
                                if (CText.Text.Contains("|"))
                                {
                                    XmlAttribute att_corres = mod_accueil.SVF.CreateAttribute("correspondance");
                                    att_corres.Value = CText.Text.Split(char.Parse("|"))[0].Trim();
                                    elem_carac.Attributes.Append(att_corres);
                                    elem_carac.InnerText = CText.Text.Split(char.Parse("|"))[1].Trim();
                                }
                                else
                                {
                                    elem_carac.InnerText = CText.Text;
                                }
                            }
                            //Sinon on prend directement le texte de la TextBox
                            else
                            {
                                elem_carac.InnerText = CText.Text;
                            }
                        }
                    }
                    //On ajoute la caracteristique au noeud caracteristiques
                    newcaracnode.AppendChild(elem_carac);
                }
            }

            return newcaracnode;
        }

        private static XmlElement Create_One_Caracteristique(string nomval,string renseigneval)
        {
            XmlElement elem_carac = mod_accueil.SVF.CreateElement("caracteristique");
            XmlAttribute att_nom = mod_accueil.SVF.CreateAttribute("nom");
            XmlAttribute att_renseigne = mod_accueil.SVF.CreateAttribute("renseigne");
            att_nom.Value = nomval;
            att_renseigne.Value = renseigneval;
            elem_carac.Attributes.Append(att_nom);
            elem_carac.Attributes.Append(att_renseigne);

            return elem_carac;
        }

        public static void Delete_Observation(string num)
        {
            //Si le num envoyé n'est pas vide, c'est que nous sommes sur une observation a modifier
            if (num != String.Empty)
            {
                XmlNode svfroot = mod_accueil.SVF.DocumentElement;
                XmlNode observationnode = svfroot.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/observations");
                XmlNode codenode = observationnode.SelectSingleNode("code[@num='" + num + "']");

                //On met à jour les caractéristiques
                observationnode.RemoveChild(codenode);

                mod_accueil.SVF.Save(System.IO.Path.Combine(mod_accueil.SVF_FOLDER, mod_accueil.SVF_FILENAME));

                mod_global.MF.LineaireStripLabel.Text = mod_accueil.Get_Max_Pm_In_SVF() + " m";
            }
        }

        //Sauvegarde de l'onglet PARAMETRES

        public static void Save_Param_Field(XmlDocument doc, XmlNode node,string newvalue, bool is_attribute, string attname, string xmlpath)
        {
            if (is_attribute == true)
            {
                if (node.Attributes.GetNamedItem(attname) == null)
                {
                    XmlAttribute newatt = doc.CreateAttribute(attname);
                    newatt.Value = newvalue;
                    node.Attributes.Append(newatt);
                }
                else
                {
                    node.Attributes.GetNamedItem(attname).InnerText = newvalue;
                }
            }
            else
            {
                node.InnerText = newvalue;
            }

            doc.Save(xmlpath);
        }
    }
}
