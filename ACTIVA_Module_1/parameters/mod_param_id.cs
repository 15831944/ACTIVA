using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using C1.Win.C1FlexGrid;

namespace ACTIVA_Module_1.modules
{
    class mod_param_id
    {
        static XmlNode Selected_Id_Code;
        static XmlNode root;

        public static void Init_Id_Codes_Grid(C1FlexGrid grid)
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

            grid.Cols.Count = 14;

            Dictionary<string, string> field_state = new Dictionary<string, string>();
            field_state.Add(string.Empty, String.Empty);
            field_state.Add("1", "1");
            field_state.Add("2", "2");
            field_state.Add("3", "3");
            field_state.Add("4", "4");

            Dictionary<string, string> field_parent = new Dictionary<string, string>();
            field_parent.Add("Organisme", "Organisme");
            field_parent.Add("Lieu", "Lieu");
            field_parent.Add("Ouvrage", "Ouvrage");
            field_parent.Add("Support", "Support");
            field_parent.Add("Element", "Element");
            field_parent.Add("Condition", "Condition");
            field_parent.Add("Observation", "Observation");

            Dictionary<string, string> field_type = new Dictionary<string, string>();
            field_type.Add("texte", "texte");
            field_type.Add("item", "item");
            field_type.Add("numerique", "numerique");
            field_type.Add("date", "date");
            field_type.Add("horaire", "horaire");
            field_type.Add("video", "video");
            field_type.Add("photo", "photo");
            field_type.Add("audio", "audio");

            grid.Cols[0].Name = "id";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 50;
            grid.Cols[0].Caption = "Id";

            grid.Cols[1].Name = "inspection";
            grid.Cols[1].Style = grid.Styles["CodeStyle"];
            grid.Cols[1].Width = 60;
            grid.Cols[1].DataType = typeof(bool);
            grid.Cols[1].Caption = "Inspection";

            grid.Cols[2].Name = "corresp";
            grid.Cols[2].Width = 120;
            grid.Cols[2].Caption = "Code Correspondant";

            grid.Cols[3].Name = "nbitem";
            grid.Cols[3].Width = 60;
            grid.Cols[3].Caption = "Nbre Items";

            grid.Cols[4].Name = "parent";
            grid.Cols[4].Width = 70;
            grid.Cols[4].Caption = "Parent";
            grid.Cols[4].DataMap = field_parent;

            grid.Cols[5].Name = "intitule";
            grid.Cols[5].Width = 200;
            grid.Cols[5].Caption = "Intitulé";

            grid.Cols[6].Name = "type";
            grid.Cols[6].Width = 70;
            grid.Cols[6].Caption = "Type";
            grid.Cols[6].DataMap = field_type;

            grid.Cols[7].Name = "unite";
            grid.Cols[7].Width = 40;
            grid.Cols[7].Caption = "Unité";

            grid.Cols[8].Name = "reporte";
            grid.Cols[8].Width = 50;
            grid.Cols[8].DataType = typeof(bool);
            grid.Cols[8].Caption = "Reporté";

            grid.Cols[9].Name = "position";
            grid.Cols[9].Width = 50;
            grid.Cols[9].Caption = "Position";
            
            grid.Cols[10].Name = "ajoute";
            grid.Cols[10].Width = 50;
            grid.Cols[10].DataType = typeof(bool);
            grid.Cols[10].Caption = "Ajouté";

            grid.Cols[11].Name = "saisie";
            grid.Cols[11].Width = 50;
            grid.Cols[11].Caption = "Saisie";

            grid.Cols[12].Name = "renseigne";
            grid.Cols[12].Width = 65;
            grid.Cols[12].Caption = "Renseigné";
            grid.Cols[12].DataMap = field_state;

            grid.Cols[13].Name = "info";
            grid.Cols[13].Width = 700;
            grid.Cols[13].Caption = "Info";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.Click += new EventHandler(Code_Id_Click);
            grid.OwnerDrawCell += new OwnerDrawCellEventHandler(Paint_Id_Codes_Cells);
            grid.AfterEdit += new RowColEventHandler(Id_Code_After_Edit);

            Set_Id_Codes_Grid_Update_Fields(grid);
        }

        public static void Init_Id_Items_Grid(C1FlexGrid grid)
        {
            CellStyle cs = grid.Styles.Add("PosStyle");
            cs.BackColor = Color.Gold;
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

            cs = grid.Styles.Add("CodeStyle");
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

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

            grid.Cols.Count = 5;

            grid.Cols[0].Name = "nom";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 60;
            grid.Cols[0].Caption = "Nom";

            grid.Cols[1].Name = "position";
            grid.Cols[1].Width = 60;
            grid.Cols[1].Caption = "Position";

            grid.Cols[2].Name = "ajoute";
            grid.Cols[2].Width = 50;
            grid.Cols[2].DataType = typeof(bool);
            grid.Cols[2].Caption = "Ajouté";

            grid.Cols[3].Name = "lien";
            grid.Cols[3].Width = 50;
            grid.Cols[3].DataType = typeof(bool);
            grid.Cols[3].Caption = "Lien";

            grid.Cols[4].Name = "valeur";
            grid.Cols[4].Width = 620;
            grid.Cols[4].Caption = "Valeur";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            Set_Id_Item_Grid_Update_Fields(grid);
            grid.AfterEdit += new RowColEventHandler(Id_Item_After_Edit);
        }

        //-------------------------------------------------------------------------------------------------

        public static void Paint_Id_Codes_Cells(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
        {
            if (e.Row > 0)
                if (mod_global.MF.XmlIdCodeGrid.Cols[e.Col].Name == "nbitem")
                    if (e.Text != String.Empty)
                        if (int.Parse(e.Text) > 0)
                            e.Style = mod_global.MF.XmlIdCodeGrid.Styles["ItemExistStyle"];
        }

        public static void Code_Id_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.XmlIdCodeGrid.RowSel < 1)
                return;

            string id = mod_global.MF.XmlIdCodeGrid[mod_global.MF.XmlIdCodeGrid.RowSel, "id"].ToString();
            Selected_Id_Code = root.SelectSingleNode(string.Concat("code[id='"+ id +"']"));
            Fill_Id_Item_Grid(mod_global.MF.XmlIdItemGrid);
        }

        public static void Id_Code_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlIdCodeGrid.Tag;

            string id = mod_global.MF.XmlIdCodeGrid[e.Row, "id"].ToString();
            string colname = mod_global.MF.XmlIdCodeGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlIdCodeGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlIdCodeGrid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            if (userdata.Contains("|"))
            {
                //Si le userdata de la colonne contient un |, c'est que la valeur ou l'attribut à changer se situe sur un noeud
                //On récupère le sous noeud à l'aide du second argument après le |
                node = root.SelectSingleNode(string.Concat("/codes/code[id='" + id + "']/" + userdata.Split(Char.Parse("|"))[1]));
                //On regarde ensuite si la valeur à changer est une valeur ou un attribut
                if (userdata.Split(Char.Parse("|"))[0] == "val")
                    is_attribute = false;
                else if (userdata.Split(Char.Parse("|"))[0] == "att")
                    is_attribute = true;
            }
            else
            {
                node = root.SelectSingleNode(string.Concat("/codes/code[id='" + id + "']"));
                if (userdata == "val")
                    is_attribute = false;
                else if (userdata == "att")
                    is_attribute = true;
            }

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlIdStripLabel.Text);
        }

        public static void Id_Item_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlIdCodeGrid.Tag;

            string nom = mod_global.MF.XmlIdItemGrid[e.Row, "nom"].ToString();
            string colname = mod_global.MF.XmlIdItemGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlIdItemGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlIdItemGrid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            if (userdata.Contains("|"))
            {
                //Si le userdata de la colonne contient un |, c'est que la valeur ou l'attribut à changer se situe sur un noeud
                //On récupère le sous noeud à l'aide du second argument après le |
                node = Selected_Id_Code.SelectSingleNode(string.Concat("valeur/item[@nom='" + nom + "']/" + userdata.Split(Char.Parse("|"))[1]));
                //On regarde ensuite si la valeur à changer est une valeur ou un attribut
                if (userdata.Split(Char.Parse("|"))[0] == "val")
                    is_attribute = false;
                else if (userdata.Split(Char.Parse("|"))[0] == "att")
                    is_attribute = true;
            }
            else
            {
                node = Selected_Id_Code.SelectSingleNode(string.Concat("valeur/item[@nom='" + nom + "']"));
                if (userdata == "val")
                    is_attribute = false;
                else if (userdata == "att")
                    is_attribute = true;
            }

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlIdStripLabel.Text);
        }

        //--------------------------------------------------------------------------------------------------

        public static void Add_Id_Code(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlIdCodeGrid.Tag;
            XmlNode originnod = doc.DocumentElement;

            if (tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Identification_Code_Name_Exist(tb.Text.ToUpper(), doc) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Ce code d'identification existe déjà.");
                    return;
                }

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = tb.Text;

                //ici
                XmlElement codeNode = doc.CreateElement("code");
                XmlElement idNode = doc.CreateElement("id");
                idNode.InnerText = tb.Text.ToUpper();
                XmlElement intituleNode = doc.CreateElement("intitule");
                XmlElement valeurNode = doc.CreateElement("valeur");
                XmlElement saisieNode = doc.CreateElement("saisie");
                XmlElement renseigneNode = doc.CreateElement("renseigne");
                XmlElement inspectionNode = doc.CreateElement("inspection");

                codeNode.AppendChild(idNode);
                codeNode.AppendChild(intituleNode);
                codeNode.AppendChild(valeurNode);
                codeNode.AppendChild(saisieNode);
                codeNode.AppendChild(renseigneNode);
                codeNode.AppendChild(inspectionNode);

                originnod.AppendChild(codeNode);

                doc.Save(mod_global.MF.XmlIdStripLabel.Text);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un code à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        public static void Del_Id_Code(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlIdCodeGrid.Tag;
            XmlNode originnod = doc.DocumentElement;

            if (grid.RowSel > 0)
            {
                XmlNode nodtoremove = originnod.SelectSingleNode("code[id='" + grid[grid.RowSel, "id"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);
                doc.Save(mod_global.MF.XmlIdStripLabel.Text);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un code à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }

        public static void Add_Id_Item(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
            XmlNode originnod = Selected_Id_Code.SelectSingleNode("valeur");

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlIdCodeGrid.Tag;

            if (tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Identification_Item_Name_Exist(tb.Text, originnod) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Cet item existe déjà.");
                    return;
                }

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["nom"] = tb.Text;

                XmlElement itemNode = doc.CreateElement("item");
                itemNode.SetAttribute("nom", tb.Text);

                originnod.AppendChild(itemNode);
                doc.Save(mod_global.MF.XmlIdStripLabel.Text);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un nom d'item à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        public static void Del_Id_Item(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;
            XmlNode originnod = Selected_Id_Code.SelectSingleNode("valeur");

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlIdCodeGrid.Tag;

            if (grid.RowSel > 0)
            {
                XmlNode nodtoremove = originnod.SelectSingleNode("item[@nom='" + grid[grid.RowSel, "nom"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);
                doc.Save(mod_global.MF.XmlIdStripLabel.Text);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un item à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }

        //--------------------------------------------------------------------------------------------------

        public static void Init_Id_Button_Tag_n_Event()
        {
            mod_global.MF.XmlIdCanaButton.Tag = mod_identification.Codes_Id_Cana_Xml;
            mod_global.MF.XmlIdRegButton.Tag = mod_identification.Codes_Id_Regard_Xml;

            mod_global.MF.XmlIdCanaButton.Click += new EventHandler(XmlIdCanaButton_Click);
            mod_global.MF.XmlIdRegButton.Click += new EventHandler(XmlIdRegButton_Click);

            mod_global.MF.XmlIdCodeAddValueTb.Tag = mod_global.MF.XmlIdCodeGrid;
            mod_global.MF.XmlIdCodeAddValueBt.Tag = mod_global.MF.XmlIdCodeAddValueTb;
            mod_global.MF.XmlIdCodeDelValueBt.Tag = mod_global.MF.XmlIdCodeGrid;

            mod_global.MF.XmlIdItemAddValueTb.Tag = mod_global.MF.XmlIdItemGrid;
            mod_global.MF.XmlIdItemAddValueBt.Tag = mod_global.MF.XmlIdItemAddValueTb;
            mod_global.MF.XmlIdItemDelValueBt.Tag = mod_global.MF.XmlIdItemGrid;

            mod_global.MF.XmlIdCodeAddValueBt.Click += new EventHandler(Add_Id_Code);
            mod_global.MF.XmlIdCodeDelValueBt.Click += new EventHandler(Del_Id_Code);

            mod_global.MF.XmlIdItemAddValueBt.Click += new EventHandler(Add_Id_Item);
            mod_global.MF.XmlIdItemDelValueBt.Click += new EventHandler(Del_Id_Item);
        }

        public static void XmlIdCanaButton_Click(object sender, EventArgs e)
        {
            ToolStripButton cbut = (ToolStripButton)sender;
            XmlDocument doc = (XmlDocument)cbut.Tag;
            mod_global.MF.XmlIdStripLabel.Text = Properties.Settings.Default.CodeIdCanaPath;
            Fill_Id_Codes_Grid(mod_global.MF.XmlIdCodeGrid, doc);
            mod_global.MF.XmlIdItemGrid.Rows.Count = 1;
        }

        public static void XmlIdRegButton_Click(object sender, EventArgs e)
        {
            ToolStripButton rbut = (ToolStripButton)sender;
            XmlDocument doc = (XmlDocument)rbut.Tag;
            mod_global.MF.XmlIdStripLabel.Text = Properties.Settings.Default.CodeIdRegPath;
            Fill_Id_Codes_Grid(mod_global.MF.XmlIdCodeGrid, doc);
            mod_global.MF.XmlIdItemGrid.Rows.Count = 1;
        }

        public static void Fill_Id_Codes_Grid(C1FlexGrid grid,XmlDocument Doc)
        {
            XmlNodeList nodeList;
            XmlNode idNode;
            XmlNode intituleNode;
            XmlNode renseigneNode;
            XmlNode saisieNode;
            XmlNode valeurNode;

            grid.Rows.Count = 1;
            mod_global.MF.XmlIdItemGrid.Rows.Count = 1;
            grid.Tag = Doc;

            root = Doc.DocumentElement;

            nodeList = root.SelectNodes(string.Concat("code"));

            foreach (XmlNode unNode in nodeList)
            {
                idNode = unNode.SelectSingleNode("id");
                intituleNode = unNode.SelectSingleNode("intitule");
                renseigneNode = unNode.SelectSingleNode("renseigne");
                saisieNode = unNode.SelectSingleNode("saisie");
                valeurNode = unNode.SelectSingleNode("valeur");

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = idNode.InnerText;
                ligne["nbitem"] = valeurNode.ChildNodes.Count;
                ligne["parent"] = unNode.Attributes["parent"].InnerText;
                ligne["position"] = unNode.Attributes["position"].InnerText;
                ligne["intitule"] = intituleNode.InnerText;
                ligne["renseigne"] = renseigneNode.InnerText;
                ligne["saisie"] = saisieNode.InnerText;

                if (valeurNode.Attributes.GetNamedItem("type") != null)
                    ligne["type"] = valeurNode.Attributes["type"].InnerText;
                if (valeurNode.Attributes.GetNamedItem("unite") != null)
                    ligne["unite"] = valeurNode.Attributes["unite"].InnerText;

                if (unNode.Attributes.GetNamedItem("reporte") != null)
                    ligne["reporte"] = unNode.Attributes["reporte"].InnerText;
                if (unNode.Attributes.GetNamedItem("ajoute") != null)
                    ligne["ajoute"] = unNode.Attributes["ajoute"].InnerText;
                if (unNode.SelectSingleNode("inspection") != null)
                    ligne["inspection"] = unNode.SelectSingleNode("inspection").InnerText;
                if (unNode.SelectSingleNode("inspection").Attributes["corresp"] != null)
                    ligne["corresp"] = unNode.SelectSingleNode("inspection").Attributes["corresp"].InnerText;
                if (unNode.SelectSingleNode("intitule").Attributes["info"] != null)
                    ligne["info"] = unNode.SelectSingleNode("intitule").Attributes["info"].InnerText;
            }
        }

        public static void Set_Id_Codes_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["parent"].UserData = "att";
            grid.Cols["position"].UserData = "att";
            grid.Cols["reporte"].UserData = "att";
            grid.Cols["ajoute"].UserData = "att";
            grid.Cols["id"].UserData = "val|id";
            grid.Cols["inspection"].UserData = "val|inspection";
            grid.Cols["corresp"].UserData = "att|inspection";
            grid.Cols["saisie"].UserData = "val|saisie";
            grid.Cols["renseigne"].UserData = "val|renseigne";
            grid.Cols["intitule"].UserData = "val|intitule";
            grid.Cols["info"].UserData = "att|intitule";
            grid.Cols["type"].UserData = "att|valeur";
            grid.Cols["unite"].UserData = "att|valeur";




        }

        public static void Fill_Id_Item_Grid(C1FlexGrid grid)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;

            nodeList = Selected_Id_Code.SelectNodes(string.Concat("valeur/item"));

            if (nodeList.Count > 0)
            {
                foreach (XmlNode unNode in nodeList)
                {
                    C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                    ligne["nom"] = unNode.Attributes["nom"].InnerText;
                    ligne["valeur"] = unNode.InnerText;

                    if (unNode.Attributes.GetNamedItem("position") != null)
                        ligne["position"] = unNode.Attributes["position"].InnerText;
                    //if (unNode.Attributes.GetNamedItem("type") != null)
                        //ligne["type"] = unNode.Attributes["type"].InnerText;
                    if (unNode.Attributes.GetNamedItem("lien") != null)
                        ligne["lien"] = unNode.Attributes["lien"].InnerText;
                    if (unNode.Attributes.GetNamedItem("ajoute") != null)
                        ligne["ajoute"] = unNode.Attributes["ajoute"].InnerText;
                }
            }
        
        }

        public static void Set_Id_Item_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["nom"].UserData = "att";
            grid.Cols["position"].UserData = "att";
            grid.Cols["valeur"].UserData = "val";
            //grid.Cols["type"].UserData = "att";
            grid.Cols["lien"].UserData = "att";
            grid.Cols["ajoute"].UserData = "att";
        }

        //---------------------------------------------------------------------------------------------------


    }
}
