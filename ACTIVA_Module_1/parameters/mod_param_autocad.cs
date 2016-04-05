using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using C1.Win.C1FlexGrid;
using System.Drawing;
using System.Windows.Forms;

namespace ACTIVA_Module_1.modules
{
    class mod_param_autocad
    {
        static XmlNode Selected_Code;
        static XmlNode root;
        static ListDictionary TypeList = new ListDictionary();
        static ListDictionary MotifList = new ListDictionary();

        public static void Fill_Type_List()
        {
            TypeList.Add("ponctuels","ponctuels");
            TypeList.Add("lineaires", "lineaires");
            TypeList.Add("surfaciques", "surfaciques");
        }

        public static void Fill_Motif_List(string type)
        {
            MotifList.Clear();
            XmlNodeList nodeList = mod_inspection.Motif_Xml.DocumentElement.SelectNodes(type + "/motif");

            foreach (XmlNode unNode in nodeList)
                {
                    MotifList.Add(unNode.Attributes["nom"].InnerText, unNode.Attributes["nom"].InnerText);
                }
        }

        //--------------------------------------------------------------------------------

        public static void Init_Autocad_Grid(C1FlexGrid grid)
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

            grid.Cols.Count = 5;

            grid.Cols[0].Name = "id";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 130;
            grid.Cols[0].Caption = "Id";
            grid.Cols[0].AllowEditing = false;

            grid.Cols[1].Name = "intitule";
            grid.Cols[1].Width = 500;
            grid.Cols[1].Caption = "Intitulé";
            grid.Cols[1].AllowEditing = false;

            grid.Cols[2].Name = "representer";
            grid.Cols[2].Style = grid.Styles["CodeStyle"];
            grid.Cols[2].Width = 120;
            grid.Cols[2].DataType = typeof(bool);
            grid.Cols[2].Caption = "Représenter";

            grid.Cols[3].Name = "legende";
            grid.Cols[3].Style = grid.Styles["CodeStyle"];
            grid.Cols[3].Width = 120;
            grid.Cols[3].DataType = typeof(bool);
            grid.Cols[3].Caption = "Légende";

            grid.Cols[4].Name = "couleur";
            grid.Cols[4].Width = 100;
            grid.Cols[4].Caption = "Couleur";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.Click += new EventHandler(Autocad_Click);
            grid.AfterEdit += new RowColEventHandler(Autocad_After_Edit);
            grid.StartEdit += new RowColEventHandler(Autocad_Start_Edit);

            Set_Autocad_Grid_Update_Fields(grid);
        }

        public static void Init_Autocad_Item_Grid(C1FlexGrid grid)
        {
            Fill_Type_List();

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

            grid.Cols.Count = 4;

            grid.Cols[0].Name = "nom";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 130;
            grid.Cols[0].Caption = "Nom";
            grid.Cols[0].AllowEditing = false;

            grid.Cols[1].Name = "intitule";
            grid.Cols[1].Width = 500;
            grid.Cols[1].Caption = "Intitulé";
            grid.Cols[1].AllowEditing = false;

            grid.Cols[2].Name = "type";
            grid.Cols[2].Style = grid.Styles["CodeStyle"];
            grid.Cols[2].Width = 120;
            grid.Cols[2].Caption = "Type";
            grid.Cols[2].DataMap = TypeList;

            grid.Cols[3].Name = "motif";
            grid.Cols[3].Width = 50;
            grid.Cols[3].Caption = "Motif";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.AfterEdit += new RowColEventHandler(Autocad_Item_After_Edit);
            grid.RowColChange += new EventHandler(Autocad_Item_Start_Edit);


            Set_Autocad_Item_Grid_Update_Fields(grid);
        }

        //--------------------------------------------------------------------------------

        public static void Init_Autocad_Button_Tag_n_Event()
        {
            mod_global.MF.XmlAutocadCanaButton.Tag = mod_observation.Codes_Obs_Cana_Xml;
            mod_global.MF.XmlAutocadRegButton.Tag = mod_observation.Codes_Obs_Regard_Xml;

            mod_global.MF.XmlAutocadCanaButton.Click += new EventHandler(XmlAutocadCanaButton_Click);
            mod_global.MF.XmlAutocadRegButton.Click += new EventHandler(XmlAutocadRegButton_Click);
        }

        public static void XmlAutocadCanaButton_Click(object sender, EventArgs e)
        {
            ToolStripButton cbut = (ToolStripButton)sender;
            XmlDocument doc = (XmlDocument)cbut.Tag;
            mod_global.MF.XmlAutocadStripLabel.Text = Properties.Settings.Default.CodeObsCanaPath;
            Fill_Autocad_Grid(mod_global.MF.XmlAutocadGrid, doc);
            mod_global.MF.XmlAutocadItemGrid.Rows.Count = 1;
        }

        public static void XmlAutocadRegButton_Click(object sender, EventArgs e)
        {
            ToolStripButton rbut = (ToolStripButton)sender;
            XmlDocument doc = (XmlDocument)rbut.Tag;
            mod_global.MF.XmlAutocadStripLabel.Text = Properties.Settings.Default.CodeObsRegPath;
            Fill_Autocad_Grid(mod_global.MF.XmlAutocadGrid, doc);
            mod_global.MF.XmlAutocadItemGrid.Rows.Count = 1;
        }


        public static void Autocad_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.XmlAutocadGrid.RowSel == 0)
                return;

            string id = mod_global.MF.XmlAutocadGrid[mod_global.MF.XmlAutocadGrid.RowSel, "id"].ToString();
            Selected_Code = root.SelectSingleNode("code[id='" + id + "']");
            Fill_Autocad_Item_Grid(mod_global.MF.XmlAutocadItemGrid);
            mod_global.MF.XmlAutocadItemGrid.RowSel = 1;
            mod_global.MF.XmlAutocadItemGrid.ColSel = 1;
        }

        public static void Autocad_Start_Edit(object sender, RowColEventArgs e)
        {
            
            if (mod_global.MF.XmlAutocadGrid.Cols[mod_global.MF.XmlAutocadGrid.ColSel].Name == "couleur")
            {
                string stylename = string.Empty;
                mod_global.MF.ChooseColorDialog.ShowDialog();
                stylename = mod_global.MF.XmlAutocadGrid.GetCellStyle(mod_global.MF.XmlAutocadGrid.RowSel, mod_global.MF.XmlAutocadGrid.ColSel).Name;
                mod_global.MF.XmlAutocadGrid.Styles[stylename].BackColor = mod_global.MF.ChooseColorDialog.Color;
            }
        }

        public static void Autocad_Item_Start_Edit(object sender, EventArgs e)
        {
            C1FlexGrid grid = mod_global.MF.XmlAutocadItemGrid;

            if (grid.RowSel < 0 | grid.ColSel < 0)
                return;

            //Console.WriteLine("RowSel : " + grid.RowSel);
            //Console.WriteLine("ColSel : " + grid.ColSel);
            
            if (grid.Cols[grid.ColSel].Name == "motif")
            {
                if (grid[grid.RowSel, "type"] == null)
                {
                    MessageBox.Show("Type ou motif non sélectionné", "Erreur", MessageBoxButtons.OK);
                    grid.Cols["motif"].DataMap = null;
                    return;
                }

                if (grid[grid.RowSel, "type"].ToString() == String.Empty)
                {
                    MessageBox.Show("Type ou motif non sélectionné", "Erreur", MessageBoxButtons.OK);
                    grid.Cols["motif"].DataMap = null;
                    return;
                }
                else
                {
                    Fill_Motif_List(grid[grid.RowSel, "type"].ToString());
                    //grid[e.Row, "motif"] = null;
                    grid.Cols["motif"].DataMap = MotifList;
                }
            }
        }

        //--------------------------------------------------------------------------------

        public static void Fill_Autocad_Grid(C1FlexGrid grid, XmlDocument Doc)
        {
            XmlNodeList nodeList;
            XmlNode idNode;
            XmlNode intituleNode;
            XmlNode autocadNode;
            //XmlNode couleurNode;

            int R = 0;
            int G = 0;
            int B = 0;

            grid.Rows.Count = 1;

            grid.Tag = Doc;

            root = Doc.DocumentElement;

            nodeList = root.SelectNodes(string.Concat("code"));

            int i = 0;
            foreach (XmlNode unNode in nodeList)
            {
                R = 0;
                G = 0;
                B = 0;

                idNode = unNode.SelectSingleNode("id");
                intituleNode = unNode.SelectSingleNode("intitule");
                autocadNode = unNode.SelectSingleNode("autocad");
                //couleurNode = autocadNode.SelectSingleNode("couleur");

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = idNode.InnerText;
                ligne["intitule"] = intituleNode.InnerText;
                ligne["representer"] = autocadNode.Attributes["representer"].InnerText;
                ligne["legende"] = autocadNode.Attributes["legende"].InnerText;

                if (autocadNode.Attributes.GetNamedItem("couleur") != null)
                {
                    R = int.Parse(autocadNode.Attributes["couleur"].InnerText.Split(Char.Parse("|"))[0]);
                    G = int.Parse(autocadNode.Attributes["couleur"].InnerText.Split(Char.Parse("|"))[1]);
                    B = int.Parse(autocadNode.Attributes["couleur"].InnerText.Split(Char.Parse("|"))[2]);
                }

                grid.Styles.Add("color" + i);
                grid.Styles["color" + i].BackColor = Color.FromArgb(R, G, B);
                grid.SetCellStyle(ligne.Index, grid.Cols["couleur"].Index, "color" + i);
                i++;
            }
        }

        public static void Set_Autocad_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["representer"].UserData = "att";
            grid.Cols["legende"].UserData = "att";
            grid.Cols["couleur"].UserData = "att";
        }
        
        //--------------------------------------------------------------------------------

        public static void Fill_Autocad_Item_Grid(C1FlexGrid grid)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;

            //root = Doc.DocumentElement;

            nodeList = Selected_Code.SelectNodes("caracteristiques/caracteristique[@nom='c1']/item");

            if (nodeList.Count > 0)
            {
                foreach (XmlNode unNode in nodeList)
                {
                    C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                    ligne["nom"] = unNode.Attributes["nom"].InnerText;
                    ligne["intitule"] = unNode.InnerText;


                    //XmlNodeList ExistingMotif = Selected_Code.SelectNodes("autocad/" + type + "/" + type + "[@nom='" + unNode.Attributes["nom"].InnerText + "']");
                    XmlNodeList ExistingMotif = Selected_Code.SelectNodes("autocad/representations/*/representation[@nom='" + unNode.Attributes["nom"].InnerText + "']");

                    if (ExistingMotif.Count > 0)
                    {
                        ligne["type"] = ExistingMotif[0].ParentNode.Name;

                        if (ExistingMotif[0].Attributes.GetNamedItem("motif") != null)
                            ligne["motif"] = ExistingMotif[0].Attributes["motif"].InnerText;
                    }
                }
            }
            else
            {

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["nom"] = "c1";
                ligne["intitule"] = "Caractéristique 1";

                XmlNodeList ExistingMotif = Selected_Code.SelectNodes("autocad/representations/*/representation[@nom='c1']");

                if (ExistingMotif.Count > 0)
                {
                    ligne["type"] = ExistingMotif[0].ParentNode.Name;
                    if (ExistingMotif[0].Attributes.GetNamedItem("motif") != null)
                        ligne["motif"] = ExistingMotif[0].Attributes["motif"].InnerText;
                }
            }
        }

        public static void Set_Autocad_Item_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["type"].UserData = String.Empty;
            grid.Cols["motif"].UserData = "att";
        }



        public static void Autocad_After_Edit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grid = (C1FlexGrid)sender;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlAutocadGrid.Tag;

            string id = grid[e.Row, "id"].ToString();
            string colname = grid.Cols[e.Col].Name;
            string userdata = grid.Cols[e.Col].UserData.ToString();
            string newvalue = grid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            node = root.SelectSingleNode("/codes/code[id='" + id + "']/autocad");

            //Cas particulier de la couleur en RGB
            if (colname == "couleur")
                newvalue = grid.GetCellStyle(e.Row, e.Col).BackColor.R + "|" + grid.GetCellStyle(e.Row, e.Col).BackColor.G + "|" + grid.GetCellStyle(e.Row, e.Col).BackColor.B;

            if (userdata == "val")
                is_attribute = false;
            else if (userdata == "att")
                is_attribute = true;

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlAutocadStripLabel.Text);
        }

        public static void Autocad_Item_After_Edit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grid = (C1FlexGrid)sender;
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlAutocadGrid.Tag;

            string nom = grid[e.Row, "nom"].ToString();
            string type = grid[e.Row, "type"].ToString();
            string colname = grid.Cols[e.Col].Name;
            string userdata = grid.Cols[e.Col].UserData.ToString();
            string newvalue = grid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            //Cas particulier du type
            if (colname == "type")
                grid[e.Row, "motif"] = null;

            if (userdata == string.Empty)
                return;


            XmlNode node;
            XmlNodeList nodeList;
            

            //Avant de créer une representation pour un item, on supprime les existantes
            nodeList = Selected_Code.SelectNodes("autocad/representations/ponctuels/representation[@nom='" + nom + "']");
            if (nodeList.Count > 0)
            {
                node = Selected_Code.SelectSingleNode("autocad/representations/ponctuels");
                node.RemoveChild(node.SelectSingleNode("representation[@nom='" + nom + "']"));
                //doc.Save(mod_global.MF.XmlAutocadStripLabel.Text);
            }
            nodeList = Selected_Code.SelectNodes("autocad/representations/lineaires/representation[@nom='" + nom + "']");
            if (nodeList.Count > 0)
            {
                node = Selected_Code.SelectSingleNode("autocad/representations/lineaires");
                node.RemoveChild(node.SelectSingleNode("representation[@nom='" + nom + "']"));
                //doc.Save(mod_global.MF.XmlAutocadStripLabel.Text);
            }
            nodeList = Selected_Code.SelectNodes("autocad/representations/surfaciques/representation[@nom='" + nom + "']");
            if (nodeList.Count > 0)
            {
                node = Selected_Code.SelectSingleNode("autocad/representations/surfaciques");
                node.RemoveChild(node.SelectSingleNode("representation[@nom='" + nom + "']"));
                //doc.Save(mod_global.MF.XmlAutocadStripLabel.Text);
            }
            
            //if (nodeList.Count > 0)
              //  node = nodeList[0];


            //On crée la representation pour l'item
            XmlElement newmotif = doc.CreateElement("representation");
            newmotif.SetAttribute("nom", nom);

            XmlNode typnod = Selected_Code.SelectSingleNode("autocad/representations/" + type);
            typnod.AppendChild(newmotif);

            node = (XmlNode)newmotif;
            //----------------------------------------------------------------------------------------------
            

            if (userdata == "val")
                is_attribute = false;
            else if (userdata == "att")
                is_attribute = true;

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlAutocadStripLabel.Text);

            grid.ColSel = 0;
            grid.RowSel = 0;

            grid.FinishEditing();
        }
    }
}
