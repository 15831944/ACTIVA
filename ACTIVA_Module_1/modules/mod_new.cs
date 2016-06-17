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
        public static void Exporter_XML(string name, string path)
        {
            if (name != String.Empty & path != String.Empty)
            {
                XmlNodeList nodelist, ouvragelist, inspectlist;
                XmlDocument doc = new XmlDocument();
                XmlDocument svf = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "ISO-8859-1", "yes");
                doc.AppendChild(docNode);

                XmlElement inspectionNode = doc.CreateElement("ZZA");
                doc.AppendChild(inspectionNode);

                XmlElement elem = doc.CreateElement("ZA");             
                inspectionNode.AppendChild(elem);

                //A2
                XmlElement var = doc.CreateElement("A2");
                var.InnerText = "fr";
                elem.AppendChild(var);             
                
                //A6
                var = doc.CreateElement("A6");
                var.InnerText = "2010";
                elem.AppendChild(var);
               
                //Ouvrir le fichier SVF et ranger les infos dans XML
                svf.Load(path);
                ouvragelist = svf.SelectNodes("/inspection/ouvrage");

                foreach (XmlNode ouvrage in ouvragelist)
                {
                    int cpt = 1;
                    // Liste des ZB
                    elem = doc.CreateElement("ZB");
                    inspectionNode.AppendChild(elem);
                    
                    //Ajouter les codes Inspections
                    inspectlist = svf.SelectNodes("/inspection/identifications/code");
                    foreach (XmlNode inspnode in inspectlist)
                    {
                        XmlElement inspElem = doc.CreateElement(inspnode.SelectSingleNode("id").InnerText);
                        inspElem.InnerText = inspnode.SelectSingleNode("valeur").InnerText;
                        elem.AppendChild(inspElem);
                    }

                    nodelist = svf.SelectNodes(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/identifications/code"));
                    SortedList sortednodelist = mod_accueil.SortNodeList_By_Alphabet(nodelist);
                    foreach (object obj in sortednodelist.Values)
                    {
                        if (((XmlNode)obj)["valeur"].InnerText != "")
                        {
                            XmlElement code = doc.CreateElement(((XmlNode)obj).FirstChild.InnerText);
                            if (((XmlNode)obj)["valeur"].Attributes["code"] == null)
                                code.InnerText = ((XmlNode)obj)["valeur"].InnerText;
                            else code.InnerText = ((XmlNode)obj)["valeur"].Attributes["code"].Value;
                            elem.AppendChild(code);
                        }
                    }

                    XmlNodeList obsList = svf.SelectNodes(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code"));
                    foreach (XmlNode obs in obsList)
                    {

                        // Liste des ZC
                        var = doc.CreateElement("ZC");
                        elem.AppendChild(var);
                        XmlElement code = doc.CreateElement("A");
                        code.InnerText = obs["id"].InnerText;
                        var.AppendChild(code);

                        //C1
                        if (svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c1\"]")).Attributes["code"] != null)
                        {
                            code = doc.CreateElement("B");
                            code.InnerText = svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c1\"]")).Attributes["code"].InnerText;
                            var.AppendChild(code);
                        }

                        //C2
                        if (svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c2\"]")).Attributes["code"] != null)
                        {
                            code = doc.CreateElement("C");
                            code.InnerText = svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c2\"]")).Attributes["code"].InnerText;
                            var.AppendChild(code);
                        }

                        //Q1
                        String str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"q1\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            code = doc.CreateElement("D");
                            code.InnerText = svf.SelectSingleNode(str).InnerText;
                            var.AppendChild(code);
                        }

                        //Q2
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"q2\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            code = doc.CreateElement("E");
                            code.InnerText = svf.SelectSingleNode(str).InnerText;
                            var.AppendChild(code);
                        }

                        //Remarques
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"remarques\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            code = doc.CreateElement("F");
                            code.InnerText = svf.SelectSingleNode(str).InnerText;
                            var.AppendChild(code);
                        }

                        //Emplacement circonférentiel 1
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"h1\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            code = doc.CreateElement("G");
                            String[] split = svf.SelectSingleNode(str).InnerText.Split('H');
                            code.InnerText = split[0];
                            var.AppendChild(code);
                        }

                        //Emplacement circonférentiel 2
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"h2\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            code = doc.CreateElement("H");
                            String[] split = svf.SelectSingleNode(str).InnerText.Split('H');
                            code.InnerText = split[0];
                            var.AppendChild(code);
                        }

                        //Emplacement longitudinal ou vertical
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"pm1\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            code = doc.CreateElement("I");
                            code.InnerText = svf.SelectSingleNode(str).InnerText;
                            var.AppendChild(code);
                        }

                        //Code de défaut continu
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"pm2\"]");
                        String pm1 = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"pm1\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            if (svf.SelectSingleNode(str).InnerText != svf.SelectSingleNode(pm1).InnerText)
                            {
                                code = doc.CreateElement("J");
                                code.InnerText = "A" + cpt.ToString();
                                var.AppendChild(code);
                            }
                        }

                        //Assemblage
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"assemblage\"]");
                        if (svf.SelectSingleNode(str).InnerText == "True")
                        {
                            code = doc.CreateElement("K");
                            code.InnerText = "A";
                            var.AppendChild(code);
                        }
                        else if (svf.SelectSingleNode(str).InnerText == "False")
                        {
                            code = doc.CreateElement("K");
                            var.AppendChild(code);
                        }

                        //Emplacement Regard
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"posregard\"]");
                        if (svf.SelectSingleNode(str) != null)
                        {
                            if (svf.SelectSingleNode(str).Attributes["codeR"].InnerText != "")
                            {
                                code = doc.CreateElement("L");
                                code.InnerText = svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"posregard\"]")).Attributes["codeR"].InnerText;
                                var.AppendChild(code);
                            }
                        }   

                        //Photo
                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"photo\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            code = doc.CreateElement("M");
                            code.InnerText = svf.SelectSingleNode(str).InnerText;
                            var.AppendChild(code);
                        }

                        str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"pm2\"]");
                        pm1 = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"pm1\"]");
                        if (svf.SelectSingleNode(str).InnerText != "")
                        {
                            if (svf.SelectSingleNode(str).InnerText != svf.SelectSingleNode(pm1).InnerText)
                            {
                                Exporter_PM2(doc, svf, var, elem, obs, ouvrage, cpt);
                                cpt++;
                            }
                        }
                    }
                }

                string new_svf = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), name.Substring(0, name.Length - 4) + ".xml");

                doc.Save(new_svf);
 
                String s = "Le fichier XML a été créer dans " + System.IO.Path.GetDirectoryName(path) + "\\";


                MessageBox.Show(s, "Exportation réussie", MessageBoxButtons.OK);
            }
        }

        /*
         * Exporter quand PM2 existe et égal à PM1
         */
        public static void Exporter_PM2(XmlDocument doc, XmlDocument svf, XmlElement var, XmlElement elem, XmlNode obs, XmlNode ouvrage, int cpt)
        {
            // Liste des ZC
            var = doc.CreateElement("ZC");
            elem.AppendChild(var);
            XmlElement code = doc.CreateElement("A");
            code.InnerText = obs["id"].InnerText;
            var.AppendChild(code);

            //C1
            if (svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c1\"]")).Attributes["code"] != null)
            {
                code = doc.CreateElement("B");
                code.InnerText = svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c1\"]")).Attributes["code"].InnerText;
                var.AppendChild(code);
            }

            //C2
            if (svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c2\"]")).Attributes["code"] != null)
            {
                code = doc.CreateElement("C");
                code.InnerText = svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"c2\"]")).Attributes["code"].InnerText;
                var.AppendChild(code);
            }

            //Q1
            String str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"q1\"]");
            if (svf.SelectSingleNode(str).InnerText != "")
            {
                code = doc.CreateElement("D");
                code.InnerText = svf.SelectSingleNode(str).InnerText;
                var.AppendChild(code);
            }

            //Q2
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"q2\"]");
            if (svf.SelectSingleNode(str).InnerText != "")
            {
                code = doc.CreateElement("E");
                code.InnerText = svf.SelectSingleNode(str).InnerText;
                var.AppendChild(code);
            }

            //Remarques
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"remarques\"]");
            if (svf.SelectSingleNode(str).InnerText != "")
            {
                code = doc.CreateElement("F");
                code.InnerText = svf.SelectSingleNode(str).InnerText;
                var.AppendChild(code);
            }

            //Emplacement circonférentiel 1
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"h1\"]");
            if (svf.SelectSingleNode(str).InnerText != "")
            {
                code = doc.CreateElement("G");
                String[] split = svf.SelectSingleNode(str).InnerText.Split('H');
                code.InnerText = split[0];
                var.AppendChild(code);
            }

            //Emplacement circonférentiel 2
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"h2\"]");
            if (svf.SelectSingleNode(str).InnerText != "")
            {
                code = doc.CreateElement("H");
                String[] split = svf.SelectSingleNode(str).InnerText.Split('H');
                code.InnerText = split[0];
                var.AppendChild(code);
            }

            //Emplacement longitudinal ou vertical
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"pm2\"]");
            if (svf.SelectSingleNode(str).InnerText != "")
            {
                code = doc.CreateElement("I");
                code.InnerText = svf.SelectSingleNode(str).InnerText;
                var.AppendChild(code);
            }

            //Code de défaut continu
            code = doc.CreateElement("J");
            code.InnerText = "B" + cpt.ToString();
            var.AppendChild(code);

            //Assemblage
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"assemblage\"]");
            if (svf.SelectSingleNode(str).InnerText == "True")
            {
                code = doc.CreateElement("K");
                code.InnerText = "A";
                var.AppendChild(code);
            }
            else if (svf.SelectSingleNode(str).InnerText == "False")
            {
                code = doc.CreateElement("K");
                var.AppendChild(code);
            }
            else if (svf.SelectSingleNode(str).InnerText == "False")
            {
                code = doc.CreateElement("K");
                var.AppendChild(code);
            }

            //Emplacement Regard
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"posregard\"]");
            if (svf.SelectSingleNode(str) != null)
            {
                if (svf.SelectSingleNode(str).Attributes["codeR"].InnerText != "")
                {
                    code = doc.CreateElement("L");
                    code.InnerText = svf.SelectSingleNode(string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"posregard\"]")).Attributes["codeR"].InnerText;
                    var.AppendChild(code);
                }
            }

            //Photo
            str = string.Concat("/inspection/ouvrage[@nom ='", ouvrage.Attributes["nom"].InnerText, "']/observations/code[@num = \"", obs.Attributes["num"].InnerText, "\"]", "/caracteristiques/caracteristique[@nom=\"photo\"]");
            if (svf.SelectSingleNode(str).InnerText != "")
            {
                code = doc.CreateElement("M");
                code.InnerText = svf.SelectSingleNode(str).InnerText;
                var.AppendChild(code);
            }
        }

        public static void Create_New_Accueil(string name, string path)
        {
            if (name != String.Empty & path != String.Empty)
            {
                XmlDocument doc = new XmlDocument();
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlElement inspectionNode = doc.CreateElement("inspection");
                doc.AppendChild(inspectionNode);

                /*
                 * Nouveaux codes ajoutés
                 */ 
                XmlElement identificationsNode = doc.CreateElement("identifications");
                inspectionNode.AppendChild(identificationsNode);

                string new_svf_folder = System.IO.Path.Combine(path, name);
                string new_img_folder = System.IO.Path.Combine(new_svf_folder, "img");
                string new_svf = System.IO.Path.Combine(new_svf_folder, name + ".svf");

                System.IO.Directory.CreateDirectory(new_svf_folder);
                System.IO.Directory.CreateDirectory(new_img_folder);

                doc.Save(new_svf);

                String s = "Le dossier SVF a été créé dans " + path;
                MessageBox.Show(s, "Création réussie", MessageBoxButtons.OK);
                mod_accueil.Load_SVF(new_svf);
                mod_accueil.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
                mod_accueil.Fill_Ouvrage_List(mod_global.MF.OuvrageList);

                mod_global.MF.NewAccueilNameTb.Text = String.Empty;
                mod_global.MF.NewAccueilPathTb.Text = String.Empty;
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
             XmlElement elem_code = mod_accueil.SVF.CreateElement("code");
             XmlElement newidnode = mod_accueil.SVF.CreateElement("id");
             XmlElement newintitulenode = mod_accueil.SVF.CreateElement("intitule");
             XmlElement newvaleurnode = mod_accueil.SVF.CreateElement("valeur");

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

                XmlElement ouvrageNode = mod_accueil.SVF.CreateElement("ouvrage");
                ouvrageNode.SetAttribute("nom", nom);
                ouvrageNode.SetAttribute("type", atype.Display);
                ouvrageNode.SetAttribute("forme", forme);
                ouvrageNode.SetAttribute("position", mod_accueil.Get_New_Ouvrage_Position());

                XmlElement IdentificationNode = mod_accueil.SVF.CreateElement("identifications");
                XmlElement ObservationNode = mod_accueil.SVF.CreateElement("observations");

                IdentificationNode = PreFill_Forme_Id_Code(atype.Display, forme, IdentificationNode);

                ouvrageNode.AppendChild(IdentificationNode);
                ouvrageNode.AppendChild(ObservationNode);

                mod_accueil.SVF.DocumentElement.AppendChild(ouvrageNode);

                mod_accueil.SVF.Save(System.IO.Path.Combine(mod_accueil.SVF_FOLDER, mod_accueil.SVF_FILENAME));

                mod_accueil.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
                mod_accueil.Fill_Ouvrage_List(mod_global.MF.OuvrageList);

                mod_global.MF.OuvrageNomTb.Text = String.Empty;
            }
        }

        public static void Clone_Selected_Ouvrage(string ouvragename)
        {
            XmlNode current_ouvrage_node = mod_accueil.SVF.DocumentElement.SelectSingleNode(string.Concat("//ouvrage[@nom='", mod_accueil.OUVRAGE, "']"));

            XmlNode new_ouvrage_node = current_ouvrage_node.CloneNode(false);
            new_ouvrage_node.Attributes["nom"].InnerText = ouvragename;
            new_ouvrage_node.Attributes["position"].InnerText = mod_accueil.Get_New_Ouvrage_Position();

            XmlElement IdentificationNode = mod_accueil.SVF.CreateElement("identifications");
            

            XmlNodeList nodelist = mod_accueil.SVF.DocumentElement.SelectNodes(string.Concat("//ouvrage[@nom='", mod_accueil.OUVRAGE, "']/identifications/code"));
            foreach (XmlNode node in nodelist) 
            {
                String id = node.SelectSingleNode("id").InnerText;
                XmlNode root;
                // Vérifier si c'est le type Regard ou Canalisation
                if (current_ouvrage_node.Attributes["type"].InnerText == "REGARD")
                    root = mod_identification.Codes_Id_Regard_Xml.DocumentElement.SelectSingleNode("code[id='" + id + "']");
                else
                    root = mod_identification.Codes_Id_Cana_Xml.DocumentElement.SelectSingleNode("code[id='" + id + "']");
                
                // Vérifier si c'est un champs reporté et pas un inspection
                if (root.Attributes["reporte"].InnerText == "True" && root.SelectSingleNode("inspection").InnerText != "")
                {
                    XmlNode clone_node = node.CloneNode(true);
                    IdentificationNode.AppendChild(clone_node); 
                }
            }

            new_ouvrage_node.AppendChild(IdentificationNode);
            /*XmlElement IdentificationNode = mod_accueil.SVF.CreateElement("identifications");
            XmlElement ObservationNode = mod_accueil.SVF.CreateElement("observations");

            new_ouvrage_node.AppendChild(IdentificationNode);
            new_ouvrage_node.AppendChild(ObservationNode);*/

            mod_accueil.SVF.DocumentElement.AppendChild(new_ouvrage_node);
            mod_accueil.SVF.Save(System.IO.Path.Combine(mod_accueil.SVF_FOLDER, mod_accueil.SVF_FILENAME));

            mod_accueil.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
            mod_accueil.Fill_Ouvrage_List(mod_global.MF.OuvrageList);

        }

        public static void Delete_Selected_Ouvrage()
        {
            XmlNode current_ouvrage_node = mod_accueil.SVF.DocumentElement.SelectSingleNode(string.Concat("//ouvrage[@nom='", mod_accueil.OUVRAGE, "']"));
            mod_accueil.SVF.DocumentElement.RemoveChild(current_ouvrage_node);
            mod_accueil.SVF.Save(System.IO.Path.Combine(mod_accueil.SVF_FOLDER, mod_accueil.SVF_FILENAME));

            mod_accueil.Check_Type_Ouvrage(mod_global.MF.cb_troncon, mod_global.MF.cb_branchement, mod_global.MF.cb_regard);
            mod_accueil.Fill_Ouvrage_List(mod_global.MF.OuvrageList);
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
