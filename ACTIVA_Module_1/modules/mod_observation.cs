using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Drawing;
using C1.Win.C1Command;
using C1.Win.C1FlexGrid;
using ACTIVA_Module_1.modules;
using ACTIVA_Module_1.component;
using System.Globalization;
using System.Windows.Forms;

namespace ACTIVA_Module_1.modules
{
    class mod_observation
    {
        public static XmlDocument Famille_Codes_Xml = new XmlDocument();
        public static XmlDocument Codes_Obs_Cana_Xml = new XmlDocument();
        public static XmlDocument Codes_Obs_Regard_Xml = new XmlDocument();

        public static bool OBS_GRID_EXPANDED = true;

        public static int last_selected_obs_num = 0;
        public static C1.Win.C1FlexGrid.SortFlags last_sort_order = SortFlags.Ascending;
        public static int last_sort_column = 2;

        public static ArrayList Collapsed_Rows = new ArrayList();
        

        public static pic_slide photos_slide;

        /// <summary>
        /// Fonction de chargement du fichier XML de configuration Famille_Codes
        /// </summary>
        /// <param name="famille_codes_path"></param>

        public static void Load_Famille_Codes_Obs(string famille_codes_path)
        {
            try
            {
                Famille_Codes_Xml.Load(famille_codes_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Fonction de chargement du fichier XML de configuration Code_Observation_Canalisation
        /// </summary>
        /// <param name="code_obs_cana_path"></param>

        public static void Load_Codes_Obs_Cana(string code_obs_cana_path)
        {
            try
            {
                Codes_Obs_Cana_Xml.Load(code_obs_cana_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Fonction de chargement du fichier XML de configuration Code_Observation_Regard
        /// </summary>
        /// <param name="code_obs_regard_path"></param>

        public static void Load_Codes_Obs_Regard(string code_obs_regard_path)
        {
            try
            {
                Codes_Obs_Regard_Xml.Load(code_obs_regard_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //---------------------------------------------------------------------------------------
        //
        //---------------------------------------------------------------------------------------

        public static void Clear_Observation_Tab(C1TopicBar tpbar)
        {
            tpbar.Pages.Clear();
        }

        public static void Get_Groupe_Observation(C1TopicBar tpbar)
        {
            XmlNodeList myChildNode = Famille_Codes_Xml.GetElementsByTagName("code");

            foreach (XmlNode unNode in myChildNode)
            {
                C1.Win.C1Command.C1TopicPage c1TopicPage1 = new C1.Win.C1Command.C1TopicPage();
                c1TopicPage1.Text = unNode.InnerText + " - " + unNode.Attributes["name"].InnerText;
                c1TopicPage1.Tag = unNode.Attributes["name"].InnerText;
                c1TopicPage1.Collapse();
                tpbar.Pages.Add(c1TopicPage1);
            }
        }

        public static void Get_Code_Observation(C1TopicBar tpbar)
        {
            XmlNode root;
            XPathNavigator IdItem;
            XPathNavigator IntituleItem;

            root = mod_global.Get_Codes_Obs_DocElement();

            //On utilise un navigateur pour pouvoir trier les noeuds
            XPathNavigator nav = root.CreateNavigator();
            XPathExpression exp = nav.Compile("//code");

            exp.AddSort("@position", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);

            foreach (XPathNavigator item in nav.Select(exp))
            {
                string ajout = String.Empty;
                ajout = item.GetAttribute("ajoute","");
                IdItem = item.SelectSingleNode("id");
                IntituleItem = item.SelectSingleNode("intitule");

                C1.Win.C1Command.C1TopicLink link = new C1.Win.C1Command.C1TopicLink();
                
                if (ajout != "true")
                    link.Text = string.Concat(IntituleItem.Value, " - ", IdItem.Value);
                else link.Text = string.Concat("** ",IntituleItem.Value, " - ", IdItem.Value);
                    
                link.Text = string.Concat(IntituleItem.Value, " - ", IdItem.Value);
                link.Tag = IdItem.Value;

                tpbar.FindPageByTag(IdItem.Value.Substring(0, 2)).Links.Add(link);

            }
            /*
            XmlNodeList myChildNode;

            if (mod_inspection.TYPE_OUVRAGE == "REGARD")
                myChildNode = Codes_Obs_Regard_Xml.GetElementsByTagName("code");
            else
                myChildNode = Codes_Obs_Cana_Xml.GetElementsByTagName("code");

            foreach (XmlNode unNode in myChildNode)
            {
                C1.Win.C1Command.C1TopicLink link = new C1.Win.C1Command.C1TopicLink();
                link.Text = string.Concat(unNode.ChildNodes[1].InnerText, " - ", unNode.ChildNodes[0].InnerText);
                link.Tag = unNode.ChildNodes[0].InnerText;
                tpbar.FindPageByTag(unNode.ChildNodes[0].InnerText.Substring(0,2)).Links.Add(link);
            }*/
        }

        public static void Remove_Empty_Pages(C1TopicBar tpbar)
        {
            string tp_index = String.Empty;
            foreach (C1TopicPage tp in tpbar.Pages)
            {
                if (tp.Links.Count == 0)
                {
                    tp_index += tp.Tag + "|";
                }
            }
            
            foreach (string tag in tp_index.Split(char.Parse("|")))
            {
                if (tag != string.Empty)
                    tpbar.Pages.Remove(tpbar.FindPageByTag(tag));
            }
        }

        public static void Fill_Code_Menu(C1TopicBar tpbar)
        {
            Clear_Observation_Tab(tpbar);
            Get_Groupe_Observation(tpbar);
            Get_Code_Observation(tpbar);
            Remove_Empty_Pages(tpbar);
        }

        //---------------------------------------------------------------------------------------
        //                      
        //---------------------------------------------------------------------------------------

        public static void Init_Grid(C1FlexGrid grid)
        {
            CellStyle cs = grid.Styles.Add("PmStyle1");
            cs.BackColor = Color.Gold;
            cs.Font = new Font("Arial", 8, FontStyle.Bold);

            cs = grid.Styles.Add("PmStyle2");
            cs.BackColor = Color.PaleGoldenrod;
            cs.Font = new Font("Arial", 8, FontStyle.Regular);

            cs = grid.Styles.Add("CodeStyle");
            cs.Font = new Font("Arial", 8, FontStyle.Regular);

            cs = grid.Styles.Add("DiffereStyle");
            cs.BackColor = mod_global.Differe_Color;

            cs = grid.Styles.Add("SectionChangeStyle");
            cs.BackColor = mod_global.Section_Change_Color;
            //cs.ForeColor = Color.LightBlue;
            cs.Font = new Font("Arial", 9, FontStyle.Bold);

            grid.DrawMode = DrawModeEnum.OwnerDraw;
            grid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            grid.Dock = System.Windows.Forms.DockStyle.Fill;
            grid.Cols.Count = 0;
            grid.Rows.Count = 1;

            grid.AllowAddNew = false;
            grid.AllowDelete = false;
            grid.AllowEditing = false;

            grid.Cols.Fixed = 1;
            grid.Rows.Fixed = 1;

            grid.Rows.DefaultSize = 100;
            grid.Cols.DefaultSize = 100;
            grid.Rows[0].Height = 24;

            grid.Cols.Count = 8;

            mod_global.MF.DataSplit.Panel2Collapsed = false;
            grid.Cols[0].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.02);
            grid.Cols[0].Name = "line_state";
            grid.Cols[0].Style = grid.Styles["Fixed"];

            grid.Cols[1].Name = "num";
            grid.Cols[1].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.03);
            grid.Cols[1].Caption = "num";
            grid.Cols[1].Visible = false;

            grid.Cols[2].Name = "pm1";
            grid.Cols[2].Style = grid.Styles["PmStyle1"];
            grid.Cols[2].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.05);
            grid.Cols[2].Caption = "Pm1";
            grid.Cols[2].Format = "##0.## m";
            //colonne.Style.BackColor = Color.Gray;

            grid.Cols[3].Name = "pm2";
            grid.Cols[3].Style = grid.Styles["PmStyle2"];
            grid.Cols[3].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.05);
            grid.Cols[3].Caption = "Pm2";
            grid.Cols[3].Format = "##0.## m";

            grid.Cols[4].Name = "code";
            grid.Cols[4].Style = grid.Styles["CodeStyle"];
            grid.Cols[4].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.05);
            grid.Cols[4].Caption = "Code";
            //colonne.Style.BackColor = Color.Gray;

            grid.Cols[5].Name = "forme";
            grid.Cols[5].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.05);
            grid.Cols[5].Caption = "Forme";
            //colonne.Style.BackColor = Color.Gray;

            grid.Cols[6].Name = "observation";
            grid.Cols[6].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.5) + 20;
            grid.Cols[6].Caption = "Observation";

            grid.Cols[7].Name = "visuel";
            grid.Cols[7].Width = Convert.ToInt32((mod_global.MF.DataSplit.Width - mod_global.MF.DataSplit.Panel2.Width) * 0.25);
            grid.Cols[7].Caption = "Référence visuelle";
            grid.Cols[7].AllowResizing = true;
            //colonne.Style.BackColor = Color.Black;

            grid.Cols.Frozen = 5;
            //grid.Styles.Alternate.BackColor = SystemColors.Control;
            grid.OwnerDrawCell += new OwnerDrawCellEventHandler(Paint_RTF_Cells);
        }

        
        public static void Paint_RTF_Cells(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
        {
            string rtfText = mod_global.MF.ObservationGrid.GetDataDisplay(e.Row, e.Col);

            if (rtfText.StartsWith(@"{\rtf"))
            {
                mod_rtf _rtf = new mod_rtf();
                // it does, so draw background
                e.DrawCell(DrawCellFlags.Background);

                // draw the RTF text
                if (e.Bounds.Width > 0 && e.Bounds.Height > 0)
                {
                    _rtf.Rtf = rtfText;
                    _rtf.ForeColor = e.Style.ForeColor;
                    _rtf.BackColor = e.Style.BackColor;
                    _rtf.Render(e.Graphics, e.Bounds);
                }

                // and draw border last
                e.DrawCell(DrawCellFlags.Border);

                // we're done with this cell
                e.Handled = true;
            }
        }

        public static void Collapse_Grid(C1FlexGrid grid)
        {
            OBS_GRID_EXPANDED = false;
            /*
             * 
             * 
             * Erreur*/
            Fill_Observation_Grid(grid); 

            //grid.Rows.DefaultSize = 24;
            foreach (Row row in grid.Rows)
            {
                if (row.SafeIndex > 0)
                {
                    row.HeightDisplay = 50;
                    row.HeightDisplay = 24;

                    Collapsed_Rows.Add(row["num"]);

                    //Hide_Row_Visuel(grid, row.SafeIndex);
                }
            }
            OBS_GRID_EXPANDED = false;
            Fill_Observation_Grid(grid);
            //grid.Cols["visuel"].Visible = false;
            //grid.Cols["observation"].Width = 520;

        }

        public static void Expand_Grid(C1FlexGrid grid)
        {
            OBS_GRID_EXPANDED = true;
            Fill_Observation_Grid(grid);

            //grid.Rows.DefaultSize = 128;
            foreach (Row row in grid.Rows)
            {
                if (row.SafeIndex > 0)
                {
                    row.HeightDisplay = 128;
                    Collapsed_Rows.Clear();
                }
            }

            //grid.Cols["visuel"].Visible = true;
            //grid.Cols["observation"].Width = 400;
            Fill_Visuel_Column(grid);
        }

        public static void Expand_Or_Collapse_Rows(C1FlexGrid grid)
        {
            Fill_Visuel_Column(grid);
            foreach (Row row in grid.Rows)
            {
                if (row.SafeIndex > 0)
                {
                    if (Collapsed_Rows.Contains(row["num"]))
                    {
                        row.HeightDisplay = 50;
                        Hide_Row_Visuel(grid, row.SafeIndex);
                    }
                    else
                    {
                        row.HeightDisplay = 128;
                    }
                }
            }
        }

        public static void Collapse_Line(C1FlexGrid grid)
        {
            grid.Rows[grid.RowSel].HeightDisplay = 24;
            Collapsed_Rows.Add(grid[grid.RowSel,"num"]);
            Hide_Row_Visuel(grid, grid.RowSel);
            //Console.WriteLine(grid.GetData(grid.RowSel, 6).ToString());
            //grid.GetData(grid.RowSel, 6).ToString();
            //grid.Cols["visuel"].Visible = false;
            //grid.Cols["observation"].Width = 520;
            //OBS_GRID_EXPANDED = false;
        }

        public static void Expand_Line(C1FlexGrid grid)
        {
            grid.Rows[grid.RowSel].HeightDisplay = 128;
            Collapsed_Rows.Remove(grid[grid.RowSel, "num"]);
            //grid.Cols["visuel"].Visible = true;
            //grid.Cols["observation"].Width = 400;
            //Fill_Visuel_Column(grid);
            //OBS_GRID_EXPANDED = true;
        }

        public static void Fill_Observation_Grid(C1FlexGrid grid)
        {
            XmlNodeList nodeList;
            XmlNode root;
            int DiffereCount = 0;
            //grid.Clear(ClearFlags.UserData);
            grid.Rows.Count = 1;

            root = mod_inspection.SVF.DocumentElement;

            nodeList = root.SelectNodes(string.Concat("/inspection/ouvrage[@nom='", mod_inspection.OUVRAGE, "']/observations/code"));

            foreach (XmlNode unNode in nodeList)
            {
                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["pm1"] = double.Parse(Get_Pm(unNode, false));
                if (Get_Pm2(unNode, false)!= String.Empty)
                    ligne["pm2"] = double.Parse(Get_Pm2(unNode, false));
                ligne["code"] = unNode.FirstChild.InnerText;
                ligne["forme"] = unNode.Attributes["forme"].InnerText;
                ligne["num"] = unNode.Attributes["num"].InnerText;
                //ligne["observation"] = unNode.ChildNodes[1].InnerText + "\n\n" + Get_Caracteristiques(unNode);
                if (OBS_GRID_EXPANDED == false)
                    ligne["observation"] = Get_Caracteristiques_In_RTF_Paragraph_Collapse(unNode);
                else ligne["observation"] = Get_Caracteristiques_In_RTF_Paragraph(unNode);

                if (Is_Differe_Field_To_Fill(unNode) == true)
                {
                    grid.SetCellStyle(ligne.SafeIndex, 6, "DiffereStyle");
                    DiffereCount += 1;
                }

                //code de changement de section
                if (unNode.FirstChild.InnerText == "AEC" | unNode.FirstChild.InnerText == "CEC")
                    grid.Rows[ligne.SafeIndex].Style = grid.Styles["SectionChangeStyle"];
                    //grid.SetCellStyle(ligne.SafeIndex, 4, "SectionChangeStyle");
            }

            //Si le nbre d'observation à compléter est > 0, on affiche le nbre dans la barre de menu
            if (DiffereCount > 0)
            {
                mod_global.MF.ObsDiffereSp.Visible = true;
                mod_global.MF.ObsDiffereLb.Visible = true;
                mod_global.MF.ObsDiffereCountLb.Visible = true;

                mod_global.MF.ObsDiffereCountLb.Text = DiffereCount.ToString();
            }
            else
            {
                mod_global.MF.ObsDiffereSp.Visible = false;
                mod_global.MF.ObsDiffereLb.Visible = false;
                mod_global.MF.ObsDiffereCountLb.Visible = false;
            }

            //On classe le tableau en fonction de la colonne position (1)
            grid.Sort(last_sort_order, last_sort_column);
        }

        public static void Fill_Visuel_Column(C1FlexGrid grid)
        {
            //int nodecount = 1;
            XmlNodeList nodeList;
            XmlNode root;
            ArrayList photos;
            string codenum;
            int row;

            grid.Controls.Clear();

            root = mod_inspection.SVF.DocumentElement;

            XmlNodeList caracnodeList;
            nodeList = root.SelectNodes(string.Concat("/inspection/ouvrage[@nom='", mod_inspection.OUVRAGE, "']/observations/code"));

            foreach (XmlNode CodeNode in nodeList)
            {
                codenum = CodeNode.Attributes["num"].InnerText;
                row = grid.FindRow(codenum, 1, 1, false, true, true);

                caracnodeList = CodeNode.SelectNodes("caracteristiques/caracteristique");
                foreach (XmlNode unNode in caracnodeList)
                {
                    if (unNode.Attributes["nom"].InnerText == "photo" & unNode.InnerText != String.Empty)
                    {
                        photos = new ArrayList();
                        foreach (string pic in unNode.InnerText.Split(Char.Parse("|")))
                            photos.Add(System.IO.Path.Combine(mod_inspection.SVF_FOLDER, "img\\" + pic));

                        if (photos.Count > 0)
                            grid[row, "visuel"] = photos.Count + " Photo(s)";

                        photos_slide = new pic_slide(photos, row);
                        mod_hosted_control.HostedControl(grid, photos_slide);
                        mod_hosted_control.UpdatePosition(grid, photos_slide, row, 7);
                    }
                }
                //nodecount += 1;
            }
        }

        public static string Get_Pm(XmlNode CodeNode, bool with_unit)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']");

            if (with_unit == true)
                return node.InnerText + " m";
            else
                return node.InnerText;
        }

        public static string Get_Pm2(XmlNode CodeNode, bool with_unit)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']");

            if (node.InnerText != String.Empty)
                if (with_unit == true)
                    return node.InnerText + " m";
                else
                    return node.InnerText;
            else
                return string.Empty;
        }

        public static string Get_H1(XmlNode CodeNode, bool with_correspondance)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='h1']");

            if (node.InnerText != String.Empty)
                if (with_correspondance == true & node.Attributes["correspondance"].InnerText != string.Empty)
                    return node.InnerText + "(" + node.Attributes["correspondance"].InnerText + ")";
                else
                    return node.InnerText;
            else
                return string.Empty;
        }

        public static string Get_H2(XmlNode CodeNode, bool with_correspondance)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='h2']");

            if (node.InnerText != String.Empty)
                if (with_correspondance == true & node.Attributes["correspondance"].InnerText != string.Empty)
                    return node.InnerText + "(" + node.Attributes["correspondance"].InnerText + ")";
                else
                    return node.InnerText;
            else
                return string.Empty;
        }

        public static string Get_Q1(XmlNode CodeNode)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='q1']");

            if (node.InnerText != String.Empty)
                return node.InnerText;
            else
                return string.Empty;
        }

        public static string Get_Q2(XmlNode CodeNode)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='q2']");

            if (node.InnerText != String.Empty)
                return node.InnerText;
            else
                return string.Empty;
        }

        public static string Get_C1(XmlNode CodeNode)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='c1']");

            if (node.InnerText != String.Empty)
                return node.InnerText;
            else
                return string.Empty;
        }

        public static string Get_C2(XmlNode CodeNode)
        {
            XmlNode node;

            node = CodeNode.SelectSingleNode("caracteristiques/caracteristique[@nom='c2']");

            if (node.InnerText != String.Empty)
                return node.InnerText;
            else
                return string.Empty;
        }

        public static string Get_Info_For_Carac(string code, string carac)
        {
            XmlNode node;

            node = mod_global.Get_Codes_Obs_DocElement().SelectSingleNode("code[id='" + code + "']/caracteristiques/caracteristique[@nom='" + carac + "']");
            try
            {
                if (node.Attributes["intitule"] != null)
                    return node.Attributes["intitule"].InnerText;
                else
                    return carac;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Veuillez choisir la bonne version du fichier XML.", e);
            }
        }

        public static string Get_Intitule_From_Code(string code)
        {
            XmlNode node;

            node = mod_global.Get_Codes_Obs_DocElement().SelectSingleNode("code[id='" + code + "']");

            if (node != null)
                return node.SelectSingleNode("intitule").InnerText;
            else
                return string.Empty;
        }

        public static string Get_Unite_For_Carac(string code, string carac)
        {
            XmlNode node;

            node = mod_global.Get_Codes_Obs_DocElement().SelectSingleNode("code[id='" + code + "']/caracteristiques/caracteristique[@nom='" + carac + "']");

            if (node.Attributes.GetNamedItem("unite") != null)
                return node.Attributes["unite"].InnerText;
            else
                return string.Empty;
        }

        private static string Get_Caracteristiques(XmlNode CodeNode)
        {
            XmlNodeList nodeList;
            
            nodeList = CodeNode.SelectNodes("caracteristiques/caracteristique");

            string code = CodeNode.FirstChild.InnerText;
            string caracname = String.Empty;

            string carac = String.Empty;
            string correspondance = String.Empty;

            foreach (XmlNode unNode in nodeList)
            {
                caracname = unNode.Attributes["nom"].InnerText;

                if (unNode.InnerText != String.Empty & caracname != "photo" & caracname != "pm1" & caracname != "assemblage")
                {
                    if (unNode.Attributes.GetNamedItem("correspondance") != null)
                        correspondance = " (" + unNode.Attributes["correspondance"].InnerText + ")";

                    carac += Get_Info_For_Carac(code, caracname) + " : " + unNode.InnerText + correspondance + " " + Get_Unite_For_Carac(code, caracname) + "\n";

                    correspondance = String.Empty;
                }

                if (caracname == "assemblage")
                    if (unNode.InnerText.ToLower() == "true")
                        carac += "Assemblage : Oui\n";
            }

            return carac;
        }

        private static string Get_Caracteristiques_In_RTF_Table(XmlNode CodeNode)
        {
            XmlNodeList nodeList;

            nodeList = CodeNode.SelectNodes("caracteristiques/caracteristique");

            string code = CodeNode.FirstChild.InnerText;
            string caracname = String.Empty;

            string intitule = CodeNode.ChildNodes[1].InnerText;
            string h1 = string.Empty;
            string h2 = string.Empty;

            string rtf_code = string.Empty;
            string carac = String.Empty;
            string correspondance = String.Empty;

            foreach (XmlNode unNode in nodeList)
            {
                caracname = unNode.Attributes["nom"].InnerText;

                if (caracname == "h1")
                    h1 = unNode.InnerText;
                if (caracname == "h2")
                    h2 = unNode.InnerText;

                if (unNode.InnerText != String.Empty & caracname != "photo" & caracname != "pm1" & caracname != "pm2" & caracname != "h1" & caracname != "h2" & caracname != "assemblage")
                {
                    if (unNode.Attributes.GetNamedItem("correspondance") != null)
                        correspondance = " (" + unNode.Attributes["correspondance"].InnerText + ")";

                    carac += @" \widctlpar\intbl " + Get_Info_For_Carac(code, caracname) + " : " + unNode.InnerText + correspondance + " " + Get_Unite_For_Carac(code, caracname) + " ";

                    correspondance = String.Empty;
                }

                if (caracname == "assemblage")
                    if (unNode.InnerText.ToLower() == "true")
                        carac += @" \widctlpar\intbl " + "Assemblage : Oui ";
            }

            rtf_code += @"{\rtf1\ansi\ \deff0 \deflang1033\deflangfe1036 ";
            rtf_code += @"{\colortbl ";
            rtf_code += @";\red0\green0\blue255;\red255\green0\blue0; ";
            rtf_code += @"}";
            rtf_code += @" \par ";
            rtf_code += @"\trowd \trgaph70 ";
            rtf_code += @" \clbrdrt\brdrth \clbrdrl\brdrth \clbrdrb\brdrth \clbrdrr\brdrth ";
            rtf_code += @" \cellx6000 \b ";
            rtf_code += intitule;
            rtf_code += @" \b0 \cell ";
            rtf_code += @"{\row }";
            rtf_code += @"\trowd \trgaph70 ";
            rtf_code += @"\clbrdrt\brdrth \clbrdrl\brdrth \clbrdrb\brdrth \clbrdrr\brdrth ";
            rtf_code += @"\cellx3000 ";
            rtf_code += @"\pard\intbl\qc ";
            rtf_code += h1;
            rtf_code += @" \cell ";
            rtf_code += @" \clbrdrt\brdrth \clbrdrl\brdrth \clbrdrb\brdrth \clbrdrr\brdrth ";
            rtf_code += @" \cellx6000 ";
            rtf_code += h2;
            rtf_code += @" \cell ";
            rtf_code += @"{\row }";
            rtf_code += @" \trowd \trgaph70 ";
            rtf_code += @" \clbrdrt\brdrth \clbrdrl\brdrth \clbrdrb\brdrth \clbrdrr\brdrth ";
            rtf_code += @" \cellx6000 ";
            rtf_code += @" \pard\intbl ";
            rtf_code += carac;
            rtf_code += @" \cell ";
            rtf_code += @"{\row}";
            rtf_code += @"}";

            return rtf_code;
        }

        private static string Get_Caracteristiques_In_RTF_Paragraph(XmlNode CodeNode)
        {
            XmlNodeList nodeList;

            nodeList = CodeNode.SelectNodes("caracteristiques/caracteristique");

            string code = CodeNode.FirstChild.InnerText;
            string caracname = String.Empty;

            string intitule = CodeNode.ChildNodes[1].InnerText;
            /*if (Get_C2(CodeNode) != String.Empty)
                intitule = CodeNode.ChildNodes[1].InnerText + Environment.NewLine + Get_C1(CodeNode) + " " + Get_C2(CodeNode);
            else
                intitule = CodeNode.ChildNodes[1].InnerText + Environment.NewLine + Get_C1(CodeNode);*/
            string h1 = string.Empty;
            string h2 = string.Empty;

            string rtf_code = string.Empty;
            string carac = String.Empty;
            string correspondance = String.Empty;

            foreach (XmlNode unNode in nodeList)
            {
                caracname = unNode.Attributes["nom"].InnerText;

                if (caracname == "h1")
                {
                    if (unNode.InnerText.Length > 0)
                    {
                        if (unNode.Attributes.GetNamedItem("correspondance") != null)
                            correspondance = " (" + unNode.Attributes["correspondance"].InnerText + ")";

                        h1 = @" \cf3 \b Horaire début : \b0 \cf2 " + unNode.InnerText + correspondance;
                    }
                }
                if (caracname == "h2")
                {
                    if (unNode.InnerText.Length > 0)
                    {
                        if (unNode.Attributes.GetNamedItem("correspondance") != null)
                            correspondance = " (" + unNode.Attributes["correspondance"].InnerText + ")";

                        h2 = @" \cf3 \b Horaire fin : \b0 \cf2 " + unNode.InnerText + correspondance;
                    }
                }

                if (unNode.InnerText != String.Empty & caracname != "photo" & caracname != "pm1" & caracname != "pm2" & caracname != "h1" & caracname != "h2" & caracname != "assemblage")
                {
                    correspondance = String.Empty;
                    if (unNode.Attributes.GetNamedItem("correspondance") != null)
                        correspondance = " (" + unNode.Attributes["correspondance"].InnerText + ")";

                    carac += @" \b " + Get_Info_For_Carac(code, caracname) + @" \b0 : " + unNode.InnerText + correspondance + " " + Get_Unite_For_Carac(code, caracname) + @" \par ";

                    correspondance = String.Empty;
                }

                if (caracname == "assemblage")
                    if (unNode.InnerText.ToLower() == "true")
                        carac += @" \b Assemblage \b0 : Oui \par ";
            }

            rtf_code += @"{\rtf1\ansi\ \deff0 \deflang1033\deflangfe1036 ";
            rtf_code += @"{\colortbl ";
            rtf_code += @";\red0\green0\blue255;\red102\green102\blue102;\red0\green153\blue0; ";
            rtf_code += @"}";
            //rtf_code += @" \par ";
            rtf_code += @"\b \cf1\f0 ";
            rtf_code += intitule;
            rtf_code += @" \b0 ";
            rtf_code += @" \par ";

            rtf_code += @"\par ";

            if (h1 != string.Empty)
                rtf_code += h1 + @" \par ";
            if (h1 != string.Empty)
                rtf_code += h2 + @" \par ";

            rtf_code += @" \par \cf2 ";
            rtf_code += carac;
            rtf_code += @" } ";

            return rtf_code;
        }

        private static string Get_Caracteristiques_In_RTF_Paragraph_Collapse(XmlNode CodeNode)
        {
            XmlNodeList nodeList;

            nodeList = CodeNode.SelectNodes("caracteristiques/caracteristique");

            string code = CodeNode.FirstChild.InnerText;
            string caracname = String.Empty;

            string intitule = CodeNode.ChildNodes[1].InnerText;
            /*if (Get_C2(CodeNode) != String.Empty)
                intitule = CodeNode.ChildNodes[1].InnerText + Environment.NewLine + Get_C1(CodeNode) + " " + Get_C2(CodeNode);
            else
                intitule = CodeNode.ChildNodes[1].InnerText + Environment.NewLine + Get_C1(CodeNode);*/

            string rtf_code = string.Empty;
            string carac = String.Empty;
            string correspondance = String.Empty;

            foreach (XmlNode unNode in nodeList)
            {
                caracname = unNode.Attributes["nom"].InnerText;

                if (unNode.InnerText != String.Empty & caracname != "photo" & caracname != "pm1" & caracname != "pm2" & caracname != "h1" & caracname != "h2" & caracname != "q1" & caracname != "q2" & caracname != "assemblage")
                {
                    correspondance = String.Empty;
                    if (unNode.Attributes.GetNamedItem("correspondance") != null)
                        correspondance = " (" + unNode.Attributes["correspondance"].InnerText + ")";

                    carac += @" - \cf2 \b0 " + unNode.InnerText + correspondance + " " + Get_Unite_For_Carac(code, caracname);

                    correspondance = String.Empty;
                }

            }

            rtf_code += @"{\rtf1\ansi\ \deff0 \deflang1033\deflangfe1036 ";
            rtf_code += @"{\colortbl ";
            rtf_code += @";\red0\green0\blue255;\red102\green102\blue102;\red0\green153\blue0; ";
            rtf_code += @"}";
            rtf_code += @" \par ";
            rtf_code += @"\b \cf1\f0 ";
            rtf_code += intitule;
            rtf_code += @" \b0 ";
            rtf_code += carac;
            rtf_code += @"";

            return rtf_code;
        }

        private static bool Is_Differe_Field_To_Fill(XmlNode CodeNode)
        {
            XmlNodeList nodeList;
            bool is_differe_empty = false;

            nodeList = CodeNode.SelectNodes("caracteristiques/caracteristique");

            foreach (XmlNode unNode in nodeList)
            {
                if (unNode.Attributes["renseigne"].InnerText == "2" & unNode.InnerText == String.Empty)
                {
                    is_differe_empty = true;
                }
            }

            return is_differe_empty;
        }

        public static void Resize_Observation_Column(C1FlexGrid grid)
        {
            //grid.Cols["observation"].Width = grid.Width - grid.Cols["position"].Width - grid.Cols["visuel"].Width;

            bool test;
            pic_slide ps;

            foreach (System.Windows.Forms.Control ct in grid.Controls)
            {
                if (ct is pic_slide)
                {
                    ps = (pic_slide)ct;
                    test = mod_hosted_control.UpdatePosition(grid, ps, ps.ParentRow, 7);
                }
            }
        }

        public static void Hide_Row_Visuel(C1FlexGrid grid, int rowsel)
        {
            //grid.Cols["observation"].Width = grid.Width - grid.Cols["position"].Width - grid.Cols["visuel"].Width;

            bool test;
            pic_slide ps;

            foreach (System.Windows.Forms.Control ct in grid.Controls)
            {
                if (ct is pic_slide)
                {
                    ps = (pic_slide)ct;
                    if (ps.ParentRow == rowsel)
                    {
                        test = mod_hosted_control.HidePosition(ps);
                        //if (ps.PhotoCount>0)
                        //    grid[grid.RowSel, "visuel"] = ps.PhotoCount + " Photo(s)";
                        return;
                    }
                }
            }
        }

        public static void Show_Last_Selected_Obs(C1FlexGrid grid)
        {
            int row=-1;

            if (last_selected_obs_num > 0)
                row = grid.FindRow(last_selected_obs_num.ToString(), 1, 1, false, true, true);

            if (row > 0)
            {
                grid.ShowCell(row, 3);
            }
        }
    }
}
