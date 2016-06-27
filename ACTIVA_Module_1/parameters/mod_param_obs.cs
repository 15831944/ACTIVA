using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using C1.Win.C1FlexGrid;

namespace ACTIVA_Module_1.modules
{
    class mod_param_obs
    {
        static XmlNode Selected_Obs_Code;
        static XmlNode Selected_Obs_Carac;
        static XmlNode Selected_Obs_Codelie;

        static XmlNode root;

        //----------------------------- Fonctions d'initialisation des tableaux -----------------------------

        /*
         * 
         * Initialisation du grid Codes */
        public static void Init_Obs_Codes_Grid(C1FlexGrid grid)
        {
            CellStyle cs = grid.Styles.Add("PosStyle");
            cs.BackColor = Color.Gold;
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

            cs = grid.Styles.Add("CodeStyle");
            cs.Font = new Font("Arial", 7, FontStyle.Regular);

            cs = grid.Styles.Add("CodeExistStyle");
            cs.Font = new Font("Arial", 7, FontStyle.Bold);
            cs.BackColor = Color.Aquamarine;

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

            grid.Cols.Count = 7;

            grid.Cols[0].Name = "id";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 60;
            grid.Cols[0].Caption = "Id";

            grid.Cols[1].Name = "parent";
            grid.Cols[1].Style = grid.Styles["CodeStyle"];
            grid.Cols[1].Width = 120;
            grid.Cols[1].Caption = "Parent";

            grid.Cols[2].Name = "position";
            grid.Cols[2].Width = 50;
            grid.Cols[2].Caption = "Position";

            grid.Cols[3].Name = "ajoute";
            grid.Cols[3].Width = 50;
            grid.Cols[3].DataType = typeof(bool);
            grid.Cols[3].Caption = "Ajouté";

            grid.Cols[4].Name = "intitule";
            grid.Cols[4].Width = 250;
            grid.Cols[4].Caption = "Intitulé";

            grid.Cols[5].Name = "codelie";
            grid.Cols[5].Width = 100;
            grid.Cols[5].Caption = "Nbre Codes Liés";
            grid.Cols[5].Style = grid.Styles["CodeStyle"];
            grid.Cols[5].AllowEditing = false;

            grid.Cols[6].Name = "info";
            grid.Cols[6].Width = 400;
            grid.Cols[6].Caption = "Info";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.Click += new EventHandler(Obs_Code_Click);
            grid.OwnerDrawCell += new OwnerDrawCellEventHandler(Paint_Obs_Codelie_Cells);
            grid.AfterEdit += new RowColEventHandler(Obs_Code_After_Edit);

            Set_Obs_Codes_Grid_Update_Fields(grid);
        }

        /*
         * 
         * Initialisation du grid Code lié */
        public static void Init_Obs_Codelie_Grid(C1FlexGrid grid)
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

            grid.Cols.DefaultSize = 100;
            grid.Rows[0].Height = 20;

            grid.Cols.Count = 2;

            grid.Cols[0].Name = "position";
            grid.Cols[0].Width = 111;
            grid.Cols[0].Caption = "Position";

            grid.Cols[1].Name = "codelie";
            grid.Cols[1].Caption = "Code Lié";
            grid.Cols[1].Width = 1160;

            grid.Click += new EventHandler(Obs_Codelie_Click);
            grid.AfterEdit += new RowColEventHandler(Obs_Codelie_After_Edit);

            Set_Obs_Codelie_Grid_Update_Fields(grid);
        }

        /*
         * 
         * Initialisation du grid Caractéristiques */
        public static void Init_Obs_Carac_Grid(C1FlexGrid grid)
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

            grid.Cols.Count = 9;

            Dictionary<string, string> field_state = new Dictionary<string, string>();
            field_state.Add(string.Empty, String.Empty);
            field_state.Add("1", "1");
            field_state.Add("2", "2");
            field_state.Add("3", "3");
            field_state.Add("4", "4");

            grid.Cols[0].Name = "nom";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 80;
            grid.Cols[0].Caption = "Nom";

            grid.Cols[1].Name = "nbitem";
            grid.Cols[1].Style = grid.Styles["CodeStyle"];
            grid.Cols[1].Width = 65;
            grid.Cols[1].Caption = "Nbre Items";
            grid.Cols[1].AllowEditing = false;

            grid.Cols[2].Name = "intitule";
            grid.Cols[2].Width = 350;
            grid.Cols[2].Caption = "Intitulé";

            grid.Cols[3].Name = "type";
            grid.Cols[3].Width = 60;
            grid.Cols[3].Caption = "Type";

            grid.Cols[4].Name = "unite";
            grid.Cols[4].Width = 50;
            grid.Cols[4].Caption = "Unité";

            grid.Cols[5].Name = "renseigne";
            grid.Cols[5].Width = 70;
            grid.Cols[5].Caption = "Renseigné";
            grid.Cols[5].DataMap = field_state;

            grid.Cols[6].Name = "ajoute";
            grid.Cols[6].Width = 50;
            grid.Cols[6].DataType = typeof(bool);
            grid.Cols[6].Caption = "Ajouté";

            grid.Cols[7].Name = "abreviation";
            grid.Cols[7].Width = 70;
            grid.Cols[7].Caption = "Abréviation";

            grid.Cols[8].Name = "info";
            grid.Cols[8].Width = 350;
            grid.Cols[8].Caption = "Info";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.Click += new EventHandler(Obs_Carac_Click);
            grid.OwnerDrawCell += new OwnerDrawCellEventHandler(Paint_Obs_Carac_Cells);
            grid.AfterEdit += new RowColEventHandler(Obs_Carac_After_Edit);

            Set_Obs_Carac_Grid_Update_Fields(grid);
        }

        /*
         * 
         * Initialisation du grid Items */
        public static void Init_Obs_Items_Grid(C1FlexGrid grid)
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

            grid.Cols.Count = 11;

            //Hashtable field_state = new Hashtable();
            Dictionary<string, string> field_state = new Dictionary<string, string>();
            field_state.Add(string.Empty,String.Empty);
            field_state.Add("1", "1");
            field_state.Add("2", "2");
            field_state.Add("3", "3");
            field_state.Add("4", "4");
            field_state.Add("5", "5");
            field_state.Add("6", "6");


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

            grid.Cols[3].Name = "q1";
            grid.Cols[3].Style = grid.Styles["CodeStyle"];
            grid.Cols[3].Width = 60;
            grid.Cols[3].Caption = "q1";
            grid.Cols[3].DataMap = field_state;

            grid.Cols[4].Name = "q2";
            grid.Cols[4].Width = 60;
            grid.Cols[4].Caption = "q2";
            grid.Cols[4].DataMap = field_state;

            grid.Cols[5].Name = "h1";
            grid.Cols[5].Width = 60;
            grid.Cols[5].Caption = "h1";
            grid.Cols[5].DataMap = field_state;

            grid.Cols[6].Name = "h2";
            grid.Cols[6].Width = 60;
            grid.Cols[6].Caption = "h2";
            grid.Cols[6].DataMap = field_state;

            grid.Cols[7].Name = "pm1";
            grid.Cols[7].Width = 60;
            grid.Cols[7].Caption = "pm1";
            grid.Cols[7].DataMap = field_state;

            grid.Cols[8].Name = "pm2";
            grid.Cols[8].Width = 60;
            grid.Cols[8].Caption = "pm2";
            grid.Cols[8].DataMap = field_state;

            grid.Cols[9].Name = "lien";
            grid.Cols[9].Width = 50;
            grid.Cols[9].DataType = typeof(bool);
            grid.Cols[9].Caption = "Lien";

            grid.Cols[10].Name = "info";
            grid.Cols[10].Width = 400;
            grid.Cols[10].Caption = "Info";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            Set_Obs_Item_Grid_Update_Fields(grid);

            grid.AfterEdit += new RowColEventHandler(Obs_Item_After_Edit);
        }

        //----------------------------- Fonctions de remplissage des tableaux -------------------------------

        /*
         * 
         * Remplir le grid Codes */
        public static void Fill_Obs_Codes_Grid(C1FlexGrid grid, XmlDocument Doc)
        {
            XmlNodeList nodeList;
            XmlNode idNode;
            XmlNode intituleNode;
            XmlNode caracNode;

            grid.Rows.Count = 1;
            mod_global.MF.XmlObsCaracGrid.Rows.Count = 1;
            mod_global.MF.XmlObsItemGrid.Rows.Count = 1;

            grid.Tag = Doc;
            //mod_global.MF.XmlObsCaracGrid.Tag = DocElem;
            //mod_global.MF.XmlObsItemGrid.Tag = DocElem;

            root = Doc.DocumentElement;

            nodeList = root.SelectNodes(string.Concat("code"));

            foreach (XmlNode unNode in nodeList)
            {
                idNode = unNode.SelectSingleNode("id");
                intituleNode = unNode.SelectSingleNode("intitule");
                caracNode = unNode.SelectSingleNode("caracteristiques");

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = idNode.InnerText;

                if (unNode.SelectSingleNode("lien") != null)
                    ligne["codelie"] = unNode.SelectSingleNode("lien").ChildNodes.Count;

                if (unNode.Attributes.GetNamedItem("parent") != null)
                    ligne["parent"] = unNode.Attributes["parent"].InnerText;
                if (unNode.Attributes.GetNamedItem("position") != null)
                    ligne["position"] = unNode.Attributes["position"].InnerText;

                ligne["intitule"] = intituleNode.InnerText;

                if (unNode.Attributes.GetNamedItem("ajoute") != null)
                    ligne["ajoute"] = unNode.Attributes["ajoute"].InnerText;
                if (intituleNode.Attributes.GetNamedItem("intitule") != null)
                    ligne["intitule"] = intituleNode.Attributes["intitule"].InnerText;
                if (intituleNode.Attributes.GetNamedItem("info") != null)
                    ligne["info"] = intituleNode.Attributes["info"].InnerText;
            }
        }
        
        /*
         * 
         * Remplir le grid Code lié */
        public static void Fill_Obs_Codelie_Grid(C1FlexGrid grid)
        {
            XmlNodeList nodeList;
            XmlNode positionNode;

            grid.Rows.Count = 1;

            nodeList = Selected_Obs_Code.SelectNodes(string.Concat("lien/codelie"));

            if (nodeList.Count > 0)
            {
                foreach (XmlNode unNode in nodeList)
                {
                    positionNode = unNode.Attributes["position"];

                    C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                    ligne["position"] = positionNode.InnerText;
                    ligne["codelie"] = unNode.InnerText;
                }
            }
            else return;
        }

        /*
         * 
         * Remplir le grid Caractéristiques */
        public static void Fill_Obs_Carac_Grid(C1FlexGrid grid)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;
            mod_global.MF.XmlObsItemGrid.Rows.Count = 1;

            nodeList = Selected_Obs_Code.SelectNodes(string.Concat("caracteristiques/caracteristique"));

            foreach (XmlNode unNode in nodeList)
            {
                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();

                ligne["nom"] = unNode.Attributes["nom"].InnerText;
                ligne["nbitem"] = unNode.ChildNodes.Count;

                if (unNode.Attributes.GetNamedItem("type") != null)
                    ligne["type"] = unNode.Attributes["type"].InnerText;
                if (unNode.Attributes.GetNamedItem("intitule") != null)
                    ligne["intitule"] = unNode.Attributes["intitule"].InnerText;
                if (unNode.Attributes.GetNamedItem("unite") != null)
                    ligne["unite"] = unNode.Attributes["unite"].InnerText;
                if (unNode.Attributes.GetNamedItem("renseigne") != null)
                    ligne["renseigne"] = unNode.Attributes["renseigne"].InnerText;
                if (unNode.Attributes.GetNamedItem("ajoute") != null)
                    ligne["ajoute"] = unNode.Attributes["ajoute"].InnerText;
                if (unNode.Attributes.GetNamedItem("abreviation") != null)
                    ligne["abreviation"] = unNode.Attributes["abreviation"].InnerText;
            }
        }

        /*
         * 
         * Remplir le grid Items */
        public static void Fill_Obs_Item_Grid(C1FlexGrid grid)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;

            nodeList = Selected_Obs_Carac.SelectNodes(string.Concat("item"));

            if (nodeList.Count > 0)
            {
                foreach (XmlNode unNode in nodeList)
                {
                    C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                    if (unNode.Attributes.GetNamedItem("nom") != null)
                        ligne["nom"] = unNode.Attributes["nom"].InnerText;
                    if (unNode.Attributes.GetNamedItem("position") != null)
                        ligne["position"] = unNode.Attributes["position"].InnerText;
                    if (unNode.InnerText != null)
                        ligne["info"] = unNode.InnerText;
                    if (unNode.Attributes.GetNamedItem("q1") != null)
                        ligne["q1"] = unNode.Attributes["q1"].InnerText;
                    if (unNode.Attributes.GetNamedItem("q2") != null)
                        ligne["q2"] = unNode.Attributes["q2"].InnerText;
                    if (unNode.Attributes.GetNamedItem("h1") != null)
                        ligne["h1"] = unNode.Attributes["h1"].InnerText;
                    if (unNode.Attributes.GetNamedItem("h2") != null)
                        ligne["h2"] = unNode.Attributes["h2"].InnerText;
                    if (unNode.Attributes.GetNamedItem("pm1") != null)
                        ligne["pm1"] = unNode.Attributes["pm1"].InnerText;
                    if (unNode.Attributes.GetNamedItem("pm2") != null)
                        ligne["pm2"] = unNode.Attributes["pm2"].InnerText;
                    if (unNode.Attributes.GetNamedItem("ajoute") != null)
                        ligne["ajoute"] = unNode.Attributes["ajoute"].InnerText;
                    if (unNode.Attributes.GetNamedItem("lien") != null)
                        ligne["lien"] = unNode.Attributes["lien"].InnerText;
                }
            }
        }

        //-------------------------------------------------------------------------------------------------
        /*
         * 
         * Colorer la colonne Nb Items si > 0 */
        public static void Paint_Obs_Carac_Cells(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
        {
            if (e.Row>0)
                if (mod_global.MF.XmlObsCaracGrid.Cols[e.Col].Name == "nbitem")
                {
                    if (e.Text != String.Empty)
                    {
                        if (int.Parse(e.Text) > 0)
                        {
                            e.Style = mod_global.MF.XmlObsCaracGrid.Styles["ItemExistStyle"];
                        }
                    }
                }                    
        }

        /*
         * 
         * Colorer la colonne Nb Codes Liés si > 0 */
        public static void Paint_Obs_Codelie_Cells(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
        {
            if (e.Row > 0)
                if (mod_global.MF.XmlObsCodesGrid.Cols[e.Col].Name == "codelie")
                {
                    if (e.Text != String.Empty)
                    {
                        if (int.Parse(e.Text) > 0)
                        {
                            e.Style = mod_global.MF.XmlObsCodesGrid.Styles["CodeExistStyle"];
                        }
                    }
                }
        }

        /*
         * 
         * Event quand on clique sur le grid Code*/
        public static void Obs_Code_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.XmlObsCodesGrid.RowSel < 1)
                return;

            string id = mod_global.MF.XmlObsCodesGrid[mod_global.MF.XmlObsCodesGrid.RowSel, "id"].ToString();
            Selected_Obs_Code = root.SelectSingleNode(string.Concat("code[id='" + id + "']"));
            Fill_Obs_Carac_Grid(mod_global.MF.XmlObsCaracGrid);
            Fill_Obs_Codelie_Grid(mod_global.MF.XmlObsCodeLieGrid);
        }

        /*
         * 
         * Event quand on clique sur le grid Codelie */
        public static void Obs_Codelie_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.XmlObsCodeLieGrid.RowSel < 1)
                return;

            string pos = mod_global.MF.XmlObsCodeLieGrid[mod_global.MF.XmlObsCodeLieGrid.RowSel, "position"].ToString();
            Selected_Obs_Codelie = Selected_Obs_Code.SelectSingleNode(string.Concat("lien/codelie[@position='" + pos + "']"));
        }

        /*
         * 
         * Event quand on clique sur le grid Caractéristiques */
        public static void Obs_Carac_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.XmlObsCaracGrid.RowSel < 1)
                return;

            string carac = mod_global.MF.XmlObsCaracGrid[mod_global.MF.XmlObsCaracGrid.RowSel, "nom"].ToString();
            Selected_Obs_Carac = Selected_Obs_Code.SelectSingleNode(string.Concat("caracteristiques/caracteristique[@nom='" + carac + "']"));
            Fill_Obs_Item_Grid(mod_global.MF.XmlObsItemGrid);
        }

        //---------------------------- Fonctions d'edition des tableaux -----------------------------------

        /*
         * 
         * Editer le fichier XML apres avoir changer la valeur du grid Code */
        public static void Obs_Code_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;

            string id = mod_global.MF.XmlObsCodesGrid[e.Row, "id"].ToString();
            string colname = mod_global.MF.XmlObsCodesGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlObsCodesGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlObsCodesGrid[e.Row, e.Col].ToString();
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

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlObsStripLabel.Text);
        }

        /*
        * 
        * Editer le fichier XML apres avoir changer la valeur du grid Code Lié */
        public static void Obs_Codelie_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;

            string position = mod_global.MF.XmlObsCodeLieGrid[e.Row, "position"].ToString();
            string colname = mod_global.MF.XmlObsCodeLieGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlObsCodeLieGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlObsCodeLieGrid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;
            node = Selected_Obs_Code.SelectSingleNode(string.Concat("lien/codelie[@position='" + position + "']"));
            if (userdata == "val")
                is_attribute = false;
            else if (userdata == "att")
                is_attribute = true;
            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlObsStripLabel.Text);
        }

        /*
        * 
        * Editer le fichier XML apres avoir changer la valeur du grid Caractéristiques */
        public static void Obs_Carac_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;

            string nom = mod_global.MF.XmlObsCaracGrid[e.Row, "nom"].ToString();
            string colname = mod_global.MF.XmlObsCaracGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlObsCaracGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlObsCaracGrid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            if (userdata.Contains("|"))
            {
                //Si le userdata de la colonne contient un |, c'est que la valeur ou l'attribut à changer se situe sur un noeud
                //On récupère le sous noeud à l'aide du second argument après le |
                node = Selected_Obs_Code.SelectSingleNode(string.Concat("caracteristiques/caracteristique[@nom='" + nom + "']/" + userdata.Split(Char.Parse("|"))[1]));
                //On regarde ensuite si la valeur à changer est une valeur ou un attribut
                if (userdata.Split(Char.Parse("|"))[0] == "val")
                    is_attribute = false;
                else if (userdata.Split(Char.Parse("|"))[0] == "att")
                    is_attribute = true;
            }
            else
            {
                node = Selected_Obs_Code.SelectSingleNode(string.Concat("caracteristiques/caracteristique[@nom='" + nom + "']"));
                if (userdata == "val")
                    is_attribute = false;
                else if (userdata == "att")
                    is_attribute = true;
            }

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlObsStripLabel.Text);
        }

        /*
        * 
        * Editer le fichier XML apres avoir changer la valeur du grid Item */
        public static void Obs_Item_After_Edit(object sender, RowColEventArgs e)
        {
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;

            string nom = mod_global.MF.XmlObsItemGrid[e.Row, "nom"].ToString();
            string colname = mod_global.MF.XmlObsItemGrid.Cols[e.Col].Name;
            string userdata = mod_global.MF.XmlObsItemGrid.Cols[e.Col].UserData.ToString();
            string newvalue = mod_global.MF.XmlObsItemGrid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            if (userdata.Contains("|"))
            {
                //Si le userdata de la colonne contient un |, c'est que la valeur ou l'attribut à changer se situe sur un noeud
                //On récupère le sous noeud à l'aide du second argument après le |
                node = Selected_Obs_Carac.SelectSingleNode(string.Concat("item[@nom='" + nom + "']/" + userdata.Split(Char.Parse("|"))[1]));
                //On regarde ensuite si la valeur à changer est une valeur ou un attribut
                if (userdata.Split(Char.Parse("|"))[0] == "val")
                    is_attribute = false;
                else if (userdata.Split(Char.Parse("|"))[0] == "att")
                    is_attribute = true;
            }
            else
            {
                node = Selected_Obs_Carac.SelectSingleNode(string.Concat("item[@nom='" + nom + "']"));
                if (userdata == "val")
                    is_attribute = false;
                else if (userdata == "att")
                    is_attribute = true;
            }

            mod_save.Save_Param_Field(doc, node, newvalue, is_attribute, colname, mod_global.MF.XmlObsStripLabel.Text);
        }

        //--------------------------- Fonctions d'ajout et de suppression ---------------------------------

        /*
        * 
        * Ajouter un Code dans le fichier XML */
        public static void Add_Obs_Code(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;
            XmlNode originnod = doc.DocumentElement;

            if (tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Observation_Code_Name_Exist(tb.Text.ToUpper(), doc) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Ce code d'observation existe déjà.");
                    return;
                }

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["id"] = tb.Text;

                //ici
                XmlElement codeNode = doc.CreateElement("code");
                codeNode.SetAttribute("ajoute", "true");
                XmlElement idNode = doc.CreateElement("id");
                idNode.InnerText = tb.Text.ToUpper();
                XmlElement intituleNode = doc.CreateElement("intitule");
                XmlElement caracteristiquesNode = doc.CreateElement("caracteristiques");

                XmlElement carac = Create_One_Caracteristique("c1",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("c2",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("q1",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("q2",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("h1",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("h2",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("pm1",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("pm2",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("assemblage",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("video",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("photo",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("audio",doc);
                caracteristiquesNode.AppendChild(carac);
                carac = Create_One_Caracteristique("remarques",doc);
                caracteristiquesNode.AppendChild(carac);

                codeNode.AppendChild(idNode);
                codeNode.AppendChild(intituleNode);
                codeNode.AppendChild(caracteristiquesNode);

                originnod.AppendChild(codeNode);

                doc.Save(mod_global.MF.XmlObsStripLabel.Text);

                // Recharger les tableaux
                Fill_Obs_Codes_Grid(mod_global.MF.XmlObsCodesGrid, doc);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un code à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Supprimer un Code dans le fichier XML */
        public static void Del_Obs_Code(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;
            XmlNode originnod = doc.DocumentElement;

            if (grid.RowSel > 0)
            {
                XmlNode nodtoremove = originnod.SelectSingleNode("code[id='" + grid[grid.RowSel, "id"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);

                doc.Save(mod_global.MF.XmlObsStripLabel.Text);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un code à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
         * 
         * Ajouter un Code Lié dans le fichier XML */

        public static void Add_Obs_Code_Lie(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
          
            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;
            XmlNode originnod = Selected_Obs_Code; 

            if(tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Observation_Code_Lie_Exist(tb.Text, doc))
                {
                    System.Windows.Forms.MessageBox.Show("Ce code lié existe déjà.");
                    return;
                }

                XmlNodeList nodelist = Selected_Obs_Code.SelectNodes("lien/codelie");
                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["codelie"] = tb.Text;
                ligne["position"] = nodelist.Count;


                XmlElement itemNode = doc.CreateElement("codelie");
                itemNode.InnerText = tb.Text;
                itemNode.SetAttribute("position", nodelist.Count.ToString());

                originnod = originnod.SelectSingleNode("lien");
                originnod.AppendChild(itemNode);
                
                // Recharger les tableaux
                Fill_Obs_Codes_Grid(mod_global.MF.XmlObsCodesGrid, doc);
                Fill_Obs_Codelie_Grid(mod_global.MF.XmlObsCodeLieGrid);
                Fill_Obs_Carac_Grid(mod_global.MF.XmlObsCaracGrid);

                doc.Save(mod_global.MF.XmlObsStripLabel.Text);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un nom d'item à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
         * 
         * Supprimer un Code Lié dans le fichier XML */
        public static void Del_Obs_Code_Lie(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;
            XmlNode originnod = Selected_Obs_Code.SelectSingleNode("lien"); 

            if (grid.RowSel > 0)
            {
                XmlNode nodtoremove = originnod.SelectSingleNode("codelie[@position='" + grid[grid.RowSel, "position"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);

                // Recharger les tableaux
                Fill_Obs_Codes_Grid(mod_global.MF.XmlObsCodesGrid, doc);
                Fill_Obs_Codelie_Grid(mod_global.MF.XmlObsCodeLieGrid);
                Fill_Obs_Carac_Grid(mod_global.MF.XmlObsCaracGrid);

                doc.Save(mod_global.MF.XmlObsStripLabel.Text);

            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un item à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Ajouter un Item dans le fichier XML */
        public static void Add_Obs_Item(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
            XmlNode originnod = Selected_Obs_Carac;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;

            if (tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Observation_Item_Name_Exist(tb.Text, originnod) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Cet item existe déjà.");
                    return;
                }

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["nom"] = tb.Text;

                XmlElement itemNode = doc.CreateElement("item");
                itemNode.SetAttribute("nom", tb.Text);
                itemNode.SetAttribute("ajoute", "true");
                originnod.AppendChild(itemNode);

                // Recharger les tableaux
                Fill_Obs_Carac_Grid(mod_global.MF.XmlObsCaracGrid);
                Fill_Obs_Item_Grid(mod_global.MF.XmlObsItemGrid);

                doc.Save(mod_global.MF.XmlObsStripLabel.Text);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un nom d'item à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Supprimer un Item dans le fichier XML */
        public static void Del_Obs_Item(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;
            XmlNode originnod = Selected_Obs_Carac;

            XmlDocument doc = (XmlDocument)mod_global.MF.XmlObsCodesGrid.Tag;

            if (grid.RowSel > 0)
            {
                XmlNode nodtoremove = originnod.SelectSingleNode("item[@nom='" + grid[grid.RowSel, "nom"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);

                // Recharger les tableaux
                Fill_Obs_Carac_Grid(mod_global.MF.XmlObsCaracGrid);
                Fill_Obs_Item_Grid(mod_global.MF.XmlObsItemGrid);

                doc.Save(mod_global.MF.XmlObsStripLabel.Text);

            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un item à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }

        /*
        * 
        * Ajouter un Caracteristique dans le fichier XML */
        private static XmlElement Create_One_Caracteristique(string nomval, XmlDocument doc)
        {
            XmlElement elem_carac = doc.CreateElement("caracteristique");
            XmlAttribute att_nom = doc.CreateAttribute("nom");
            XmlAttribute att_renseigne = doc.CreateAttribute("renseigne");
            att_renseigne.InnerText = "3";
            XmlAttribute att_ajout = doc.CreateAttribute("ajoute");
            att_ajout.InnerText = "true";
            att_nom.Value = nomval;
            elem_carac.Attributes.Append(att_nom);
            elem_carac.Attributes.Append(att_renseigne);
            elem_carac.Attributes.Append(att_ajout);
            return elem_carac;
        }

        //------------------ Fonctions de parametrage des colonnes pour la mise a jour --------------------

        public static void Set_Obs_Codes_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["id"].UserData = "val|id";
            grid.Cols["parent"].UserData = "att";
            grid.Cols["position"].UserData = "att";
            grid.Cols["intitule"].UserData = "val|intitule";
            grid.Cols["info"].UserData = "att|intitule";
            grid.Cols["ajoute"].UserData = "att";
            grid.Cols["codelie"].UserData = "att";
        }

        private static void Set_Obs_Codelie_Grid_Update_Fields(C1FlexGrid grid)
        {
            grid.Cols["position"].UserData = "att";
            grid.Cols["codelie"].UserData = "val";
        }

        public static void Set_Obs_Carac_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["nom"].UserData = "att";
            grid.Cols["nbitem"].UserData = "att";
            grid.Cols["type"].UserData = "att";
            grid.Cols["info"].UserData = "att";
            grid.Cols["intitule"].UserData = "att";
            grid.Cols["unite"].UserData = "att";
            grid.Cols["renseigne"].UserData = "att";
            grid.Cols["ajoute"].UserData = "att";
            grid.Cols["abreviation"].UserData = "att";
        }

        public static void Set_Obs_Item_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours code
            grid.Cols["nom"].UserData = "att";
            grid.Cols["position"].UserData = "att";
            grid.Cols["info"].UserData = "val";
            grid.Cols["q1"].UserData = "att";
            grid.Cols["q2"].UserData = "att";
            grid.Cols["h1"].UserData = "att";
            grid.Cols["h2"].UserData = "att";
            grid.Cols["pm1"].UserData = "att";
            grid.Cols["pm2"].UserData = "att";
            grid.Cols["ajoute"].UserData = "att";
            grid.Cols["lien"].UserData = "att";
        }

        //------------ Fonctions de configuration des boutons de choix de fichiers XML a modifier ---------

        public static void Init_Obs_Button_Tag_n_Event()
        {
            mod_global.MF.XmlObsCodeAddValueTb.Tag = mod_global.MF.XmlObsCodesGrid;
            mod_global.MF.XmlObsCodeAddValueBt.Tag = mod_global.MF.XmlObsCodeAddValueTb;
            mod_global.MF.XmlObsCodeDelValueBt.Tag = mod_global.MF.XmlObsCodesGrid;

            mod_global.MF.XmlObsItemAddValueTb.Tag = mod_global.MF.XmlObsItemGrid;
            mod_global.MF.XmlObsItemAddValueBt.Tag = mod_global.MF.XmlObsItemAddValueTb;
            mod_global.MF.XmlObsItemDelValueBt.Tag = mod_global.MF.XmlObsItemGrid;

            mod_global.MF.XmlObsCodeAddValueBt.Click += new EventHandler(Add_Obs_Code);
            mod_global.MF.XmlObsCodeDelValueBt.Click += new EventHandler(Del_Obs_Code);

            mod_global.MF.XmlObsCodeLieAddValueBt.Click += new EventHandler(Add_Obs_Code_Lie);
            mod_global.MF.XmlObsCodeLieDelValueBt.Click += new EventHandler(Del_Obs_Code_Lie);

            mod_global.MF.XmlObsItemAddValueBt.Click += new EventHandler(Add_Obs_Item);
            mod_global.MF.XmlObsItemDelValueBt.Click += new EventHandler(Del_Obs_Item);

            mod_global.MF.XmlObsCanaButton.Tag = mod_observation.Codes_Obs_Cana_Xml;
            mod_global.MF.XmlObsRegButton.Tag = mod_observation.Codes_Obs_Regard_Xml;

            mod_global.MF.XmlObsCanaButton.Click += new EventHandler(XmlObsCanaButton_Click);
            mod_global.MF.XmlObsRegButton.Click += new EventHandler(XmlObsRegButton_Click);
        }

        public static void XmlObsCanaButton_Click(object sender, EventArgs e)
        {
            ToolStripButton cbut = (ToolStripButton)sender;
            XmlDocument doc = (XmlDocument)cbut.Tag;
            mod_global.MF.XmlObsStripLabel.Text = Properties.Settings.Default.CodeObsCanaPath;
            Fill_Obs_Codes_Grid(mod_global.MF.XmlObsCodesGrid, doc);
            mod_global.MF.XmlObsCaracGrid.Rows.Count = 1;
            mod_global.MF.XmlObsCodeLieGrid.Rows.Count = 1;
            mod_global.MF.XmlObsItemGrid.Rows.Count = 1;
        }

        public static void XmlObsRegButton_Click(object sender, EventArgs e)
        {
            ToolStripButton rbut = (ToolStripButton)sender;
            XmlDocument doc = (XmlDocument)rbut.Tag;
            mod_global.MF.XmlObsStripLabel.Text = Properties.Settings.Default.CodeObsRegPath;
            Fill_Obs_Codes_Grid(mod_global.MF.XmlObsCodesGrid, doc);
            mod_global.MF.XmlObsCaracGrid.Rows.Count = 1;
            mod_global.MF.XmlObsCodeLieGrid.Rows.Count = 1;
            mod_global.MF.XmlObsItemGrid.Rows.Count = 1;
        }

        //---------------------------------------------------------------------------------------------------
    }
}
