using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Drawing;
using System.Text;
using C1.Win.C1FlexGrid;

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

            grid.Cols[0].Name = "id";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 130;
            grid.Cols[0].Caption = "Id";

            grid.Cols[1].Name = "ouvrage";
            grid.Cols[1].Width = 100;
            grid.Cols[1].Caption = "Ouvrage";

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
            if (mod_global.MF.XmlSectionGrid.RowSel == 0)
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
                node = root.SelectSingleNode("/sections/section[id='" + id + "']/");
                //On regarde ensuite si la valeur à changer est une valeur ou un attribut
                if (userdata.Split(Char.Parse("|"))[0] == "val")
                    is_attribute = false;
                else if (userdata.Split(Char.Parse("|"))[0] == "att")
                    is_attribute = true;
            }
            else
            {
                node = root.SelectSingleNode("/sections/section[id='" + id + "']");
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

            nodeList = Selected_Section.SelectNodes("heure");

            if (nodeList.Count > 0)
            {
                foreach (XmlNode unNode in nodeList)
                {
                    C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                    ligne["id"] = unNode.Attributes["id"].InnerText;
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
    }
}
