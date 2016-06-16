using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Drawing;
using System.Text;
using C1.Win.C1FlexGrid;
using System.Windows.Forms;

namespace ACTIVA_Module_1.modules
{
    class mod_param_section
    {
        static XmlNode Selected_Section;
        static XmlNode root;

        public static void Init_Section_Grid(C1FlexGrid grid)
        {
            CellStyle cs = grid.Styles.Add("PosStyle");
            cs.BackColor = Color.Gold;
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

            cs = grid.Styles.Add("CodeStyle");
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

            cs = grid.Styles.Add("ItemExistStyle");
            cs.Font = new Font("Arial", 7, FontStyle.Bold);
            cs.BackColor = Color.Orange;

            grid.DrawMode = DrawModeEnum.OwnerDraw;
            grid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            grid.Dock = System.Windows.Forms.DockStyle.Fill;
            grid.Cols.Count = 0;
            grid.Rows.Count = 1;

            grid.AllowAddNew = false;
            grid.AllowDelete = false;
            grid.AllowEditing = true;

            grid.Cols.Fixed = 0;
            grid.Rows.Fixed = 1;

            grid.Rows.DefaultSize = 20;
            grid.Cols.DefaultSize = 100;
            grid.Rows[0].Height = 20;

            grid.Cols.Count = 6;

            Dictionary<string, string> field_ouvrage = new Dictionary<string, string>();
            field_ouvrage.Add("Canalisation", "CANALISATION");
            field_ouvrage.Add("Regard", "REGARD");


            grid.Cols[0].Name = "id";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 130;
            grid.Cols[0].Caption = "Id";

            grid.Cols[1].Name = "ouvrage";
            grid.Cols[1].Width = 100;
            grid.Cols[1].Caption = "Ouvrage";
            grid.Cols[1].DataMap = field_ouvrage;

            grid.Cols[2].Name = "forme";
            grid.Cols[2].Style = grid.Styles["CodeStyle"];
            grid.Cols[2].Width = 120;
            grid.Cols[2].Caption = "Forme";

            grid.Cols[3].Name = "position";
            grid.Cols[3].Width = 50;
            grid.Cols[3].Caption = "Position";

            grid.Cols[4].Name = "intitule";
            grid.Cols[4].Width = 250;
            grid.Cols[4].Caption = "Intitulé";

            grid.Cols[5].Name = "image";
            grid.Cols[5].Width = 50;
            grid.Cols[5].Caption = "Image";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.Click += new EventHandler(Section_Click);
            //grid.OwnerDrawCell += new OwnerDrawCellEventHandler(Paint_Section_Cells);
            grid.AfterEdit += new RowColEventHandler(Section_After_Edit);

            mod_global.MF.XmlSectionStripLabel.Text = Properties.Settings.Default.SectionOuvragePath;

            Set_Section_Grid_Update_Fields(grid);
        }

        public static void Init_Heure_Grid(C1FlexGrid grid)
        {
            CellStyle cs = grid.Styles.Add("PosStyle");
            cs.BackColor = Color.Gold;
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

            cs = grid.Styles.Add("CodeStyle");
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

            cs = grid.Styles.Add("ItemExistStyle");
            cs.Font = new Font("Arial", 7, FontStyle.Bold);
            cs.BackColor = Color.Orange;

            grid.DrawMode = DrawModeEnum.OwnerDraw;
            grid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            grid.Dock = System.Windows.Forms.DockStyle.Fill;
            grid.Cols.Count = 0;
            grid.Rows.Count = 1;

            grid.AllowAddNew = false;
            grid.AllowDelete = false;
            grid.AllowEditing = true;

            grid.Cols.Fixed = 0;
            grid.Rows.Fixed = 1;

            grid.Rows.DefaultSize = 20;
            grid.Cols.DefaultSize = 100;
            grid.Rows[0].Height = 20;

            grid.Cols.Count = 3;

            grid.Cols[0].Name = "id";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 60;
            grid.Cols[0].Caption = "Id";

            grid.Cols[1].Name = "type";
            grid.Cols[1].Width = 130;
            grid.Cols[1].Caption = "Type";

            grid.Cols[2].Name = "correspondance";
            grid.Cols[2].Style = grid.Styles["CodeStyle"];
            grid.Cols[2].Width = 120;
            grid.Cols[2].Caption = "Correspondance";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            //grid.Click += new EventHandler(Heure_Click);
            //grid.OwnerDrawCell += new OwnerDrawCellEventHandler(Paint_Heure_Cells);
            grid.AfterEdit += new RowColEventHandler(Heure_After_Edit);

            Set_Heure_Grid_Update_Fields(grid);
        }

        //-------------------------------------------------------------------------------

        public static void Section_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.XmlSectionGrid.RowSel < 1)
                return;

            string id = mod_global.MF.XmlSectionGrid[mod_global.MF.XmlSectionGrid.RowSel, "id"].ToString();
            Selected_Section = root.SelectSingleNode("section[@id='" + id + "']");
            Fill_Heure_Grid(mod_global.MF.XmlHeureGrid);
        }

        public static void Section_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlSectionGrid.Tag;

            string id = mod_global.MF.XmlSectionGrid[e.Row, "id"].ToString();
            string colname = mod_global.MF.XmlSectionGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlSectionGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlSectionGrid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            if (userdata.Contains("|"))
            {
                //Si le userdata de la colonne contient un |, c'est que la valeur ou l'attribut à changer se situe sur un noeud
                //On récupère le sous noeud à l'aide du second argument après le |
                node = root.SelectSingleNode("section[@id='" + id + "']");
                //On regarde ensuite si la valeur à changer est une valeur ou un attribut
                if (userdata.Split(Char.Parse("|"))[0] == "val")
                    is_attribute = false;
                else if (userdata.Split(Char.Parse("|"))[0] == "att")
                    is_attribute = true;
            }
            else
            {
                node = root.SelectSingleNode("section[@id='" + id + "']");
                if (userdata == "val")
                    is_attribute = false;
                else if (userdata == "att")
                    is_attribute = true;
            }

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlSectionStripLabel.Text);
        }

        public static void Heure_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlSectionGrid.Tag;

            string id = mod_global.MF.XmlHeureGrid[e.Row, "id"].ToString();
            string colname = mod_global.MF.XmlHeureGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlHeureGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlHeureGrid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            if (userdata.Contains("|"))
            {
                //Si le userdata de la colonne contient un |, c'est que la valeur ou l'attribut à changer se situe sur un noeud
                //On récupère le sous noeud à l'aide du second argument après le |
                node = Selected_Section.SelectSingleNode("heure[@id='" + id + "']");
                //On regarde ensuite si la valeur à changer est une valeur ou un attribut
                if (userdata.Split(Char.Parse("|"))[0] == "val")
                    is_attribute = false;
                else if (userdata.Split(Char.Parse("|"))[0] == "att")
                    is_attribute = true;
            }
            else
            {
                node = Selected_Section.SelectSingleNode(string.Concat("heure[@id='" + id + "']"));
                if (userdata == "val")
                    is_attribute = false;
                else if (userdata == "att")
                    is_attribute = true;
            }

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlSectionStripLabel.Text);
        }

        //-------------------------------------------------------------------------------

        public static void Init_Section_Button_Tag_n_Event()
        {
            //mod_global.MF.XmlIdCanaButton.Tag = mod_identification.Codes_Id_Cana_Xml;
            //mod_global.MF.XmlIdRegButton.Tag = mod_identification.Codes_Id_Regard_Xml;

            //mod_global.MF.XmlIdCanaButton.Click += new EventHandler(XmlIdCanaButton_Click);
            //mod_global.MF.XmlIdRegButton.Click += new EventHandler(XmlIdRegButton_Click);
            mod_global.MF.XmlSectionAddTb.Tag = mod_global.MF.XmlSectionGrid;
            mod_global.MF.XmlSectionAddBt.Tag = mod_global.MF.XmlSectionAddTb;
            mod_global.MF.XmlSectionDelBt.Tag = mod_global.MF.XmlSectionGrid;

            mod_global.MF.XmlHeureAddTb.Tag = mod_global.MF.XmlHeureGrid;
            mod_global.MF.XmlHeureAddBt.Tag = mod_global.MF.XmlHeureAddTb;
            mod_global.MF.XmlHeureDelBt.Tag = mod_global.MF.XmlHeureGrid;

            mod_global.MF.XmlSectionAddBt.Click += new EventHandler(Add_Section_Type);
            mod_global.MF.XmlSectionDelBt.Click += new EventHandler(Del_Section_Type);

            mod_global.MF.XmlHeureAddBt.Click += new EventHandler(Add_Section_Horaire);
            mod_global.MF.XmlHeureDelBt.Click += new EventHandler(Del_Section_Horaire);
        }

        public static void Fill_Section_Grid(C1FlexGrid grid, XmlDocument Doc)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;
            mod_global.MF.XmlHeureGrid.Rows.Count = 1;
            grid.Tag = Doc;

            root = Doc.DocumentElement;

            nodeList = root.SelectNodes(string.Concat("section"));

            foreach (XmlNode unNode in nodeList)
            {
                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = unNode.Attributes["id"].InnerText;
                ligne["ouvrage"] = unNode.Attributes["ouvrage"].InnerText;
                ligne["forme"] = unNode.Attributes["forme"].InnerText;
                ligne["position"] = unNode.Attributes["position"].InnerText;
                ligne["intitule"] = unNode.Attributes["intitule"].InnerText;
                ligne["image"] = unNode.Attributes["image"].InnerText;
            }
        }

        public static void Set_Section_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["id"].UserData = "att";
            grid.Cols["ouvrage"].UserData = "att";
            grid.Cols["forme"].UserData = "att";
            grid.Cols["intitule"].UserData = "att";
            grid.Cols["position"].UserData = "att";
            grid.Cols["image"].UserData = "att";
        }

        public static void Fill_Heure_Grid(C1FlexGrid grid)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;

            nodeList = Selected_Section.SelectNodes(string.Concat("heure"));


            if (nodeList.Count > 0)
            {
                foreach (XmlNode unNode in nodeList)
                {
                    C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();

                    if (unNode.Attributes.GetNamedItem("id") != null)
                        ligne["id"] = unNode.Attributes["id"].InnerText;
                    if (unNode.InnerText != null)
                        ligne["correspondance"] = unNode.InnerText;
                    if (unNode.Attributes.GetNamedItem("type") != null)
                        ligne["type"] = unNode.Attributes["type"].InnerText;
                }
            }

        }

        public static void Set_Heure_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["id"].UserData = "att";
            grid.Cols["type"].UserData = "att";
            grid.Cols["correspondance"].UserData = "val";
        }

        //--------------------------- Fonctions d'ajout et de suppression ---------------------------------

        /*
        * 
        * Ajouter un type de section dans le fichier XML */
        public static void Add_Section_Type(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlSectionGrid.Tag;
            XmlNode originnod = doc.DocumentElement;

            if (tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Section_Type_Exist(tb.Text.ToUpper(), doc) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Ce type de section existe déjà.");
                    return;
                }

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = tb.Text;

                //ici
                XmlElement sectionNode = doc.CreateElement("section");
                sectionNode.SetAttribute("id", tb.Text);
                sectionNode.SetAttribute("ouvrage", "");
                sectionNode.SetAttribute("forme", "");
                sectionNode.SetAttribute("intitule", "");
                sectionNode.SetAttribute("position", "");
                sectionNode.SetAttribute("image", "");
                XmlElement horaire = Create_One_Horaire("12H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("12H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("1H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("1H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("2H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("2H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("3H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("3H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("4H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("4H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("5H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("5H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("6H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("6H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("7H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("7H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("8H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("8H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("9H00", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("9H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("10H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("10H30", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("11H", doc);
                sectionNode.AppendChild(horaire);
                horaire = Create_One_Horaire("11H30", doc);
                sectionNode.AppendChild(horaire);

                originnod.AppendChild(sectionNode);

                doc.Save(mod_global.MF.XmlSectionStripLabel.Text);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un type de section à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Supprimer un type de section dans le fichier XML */
        public static void Del_Section_Type(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlSectionGrid.Tag;
            XmlNode originnod = doc.DocumentElement;

            if (grid.RowSel > 0)
            {
                XmlNode nodtoremove = originnod.SelectSingleNode("section[@id='" + grid[grid.RowSel, "id"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);
                doc.Save(mod_global.MF.XmlSectionStripLabel.Text);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un type de section à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Ajouter un emplacement horaire dans le fichier XML */
        public static void Add_Section_Horaire(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
            XmlNode originnod = Selected_Section;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlSectionGrid.Tag;

            if (tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Section_Horaire_Exist(tb.Text, originnod) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Cet horaire existe déjà.");
                    return;
                }

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = tb.Text;

                XmlElement heureNode = doc.CreateElement("heure");
                heureNode.SetAttribute("id", tb.Text);

                originnod.AppendChild(heureNode);
                doc.Save(mod_global.MF.XmlSectionStripLabel.Text);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un emplacement horaire à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Supprimer un emplacement horaire dans le fichier XML */
        public static void Del_Section_Horaire(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;
            XmlNode originnod = Selected_Section;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlSectionGrid.Tag;

            if (grid.RowSel > 0)
            {
                XmlNode nodtoremove = originnod.SelectSingleNode("heure[@id='" + grid[grid.RowSel, "id"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);
                doc.Save(mod_global.MF.XmlSectionStripLabel.Text);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un emplacement horaire à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Ajouter un horaire dans le fichier XML */
        private static XmlElement Create_One_Horaire(string id, XmlDocument doc)
        {
            XmlElement elem_horaire = doc.CreateElement("heure");
            XmlAttribute att_id = doc.CreateAttribute("id");
            att_id.Value = id;
            elem_horaire.Attributes.Append(att_id);

            return elem_horaire;
        }
    }
}
