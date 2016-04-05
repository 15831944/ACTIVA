using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.Win.C1FlexGrid;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;

namespace ACTIVA_Module_1.modules
{
    class mod_param_motif
    {
        static XmlNode root;


        public static void Init_Motif_Ponctuel_Grid(C1FlexGrid grid)
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

            grid.Cols.Count = 3;

            grid.Cols[0].Name = "nom";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 200;
            grid.Cols[0].Caption = "Nom";

            grid.Cols[1].Name = "symbole";
            grid.Cols[1].Style = grid.Styles["CodeStyle"];
            grid.Cols[1].Width = 150;
            grid.Cols[1].Caption = "Symbole";

            grid.Cols[2].Name = "echelle";
            grid.Cols[2].Width = 250;
            grid.Cols[2].Caption = "Echelle";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.AfterEdit += new RowColEventHandler(Motif_After_Edit);

            Set_Motif_Ponctuel_Grid_Update_Fields(grid);

            mod_global.MF.XmlMotifStripLabel.Text = Properties.Settings.Default.MotifPath;
        }

        public static void Init_Motif_Lineaire_Grid(C1FlexGrid grid)
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

            grid.Cols[0].Name = "nom";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 200;
            grid.Cols[0].Caption = "Nom";

            grid.Cols[1].Name = "ligne";
            grid.Cols[1].Style = grid.Styles["CodeStyle"];
            grid.Cols[1].Width = 150;
            grid.Cols[1].Caption = "Ligne";

            grid.Cols[2].Name = "epaisseur";
            grid.Cols[2].Width = 60;
            grid.Cols[2].Caption = "Epaisseur";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            grid.AfterEdit += new RowColEventHandler(Motif_After_Edit);

            Set_Motif_Lineaire_Grid_Update_Fields(grid);
        }

        public static void Init_Motif_Surfacique_Grid(C1FlexGrid grid)
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

            grid.Cols.Count = 4;

            grid.Cols[0].Name = "nom";
            grid.Cols[0].Style = grid.Styles["PosStyle"];
            grid.Cols[0].Width = 200;
            grid.Cols[0].Caption = "Nom";

            grid.Cols[1].Name = "hachure";
            grid.Cols[1].Style = grid.Styles["CodeStyle"];
            grid.Cols[1].Width = 150;
            grid.Cols[1].Caption = "Hachure";

            grid.Cols[2].Name = "angle";
            grid.Cols[2].Width = 60;
            grid.Cols[2].Caption = "Angle";

            grid.Cols[3].Name = "echelle";
            grid.Cols[3].Width = 60;
            grid.Cols[3].Caption = "Echelle";

            grid.Cols.Frozen = 1;
            grid.ExtendLastCol = true;

            Set_Motif_Surfacique_Grid_Update_Fields(grid);

            grid.AfterEdit += new RowColEventHandler(Motif_After_Edit);
        }


        public static void Fill_Motif_Ponctuel_Grid(C1FlexGrid grid, XmlDocument Doc)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;
            grid.Tag = Doc;

            root = Doc.DocumentElement;

            grid.Tag = root.SelectSingleNode("ponctuels");

            nodeList = root.SelectNodes("ponctuels/motif");

            foreach (XmlNode unNode in nodeList)
            {
                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["nom"] = unNode.Attributes["nom"].InnerText;
                if (unNode.Attributes.GetNamedItem("symbole") != null)
                    ligne["symbole"] = unNode.Attributes["symbole"].InnerText;
                if (unNode.Attributes.GetNamedItem("echelle") != null)
                    ligne["echelle"] = unNode.Attributes["echelle"].InnerText;
            }
        }

        public static void Fill_Motif_Lineaire_Grid(C1FlexGrid grid, XmlDocument Doc)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;
            grid.Tag = Doc;

            root = Doc.DocumentElement;

            grid.Tag = root.SelectSingleNode("lineaires");

            nodeList = root.SelectNodes("lineaires/motif");

            foreach (XmlNode unNode in nodeList)
            {
                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["nom"] = unNode.Attributes["nom"].InnerText;
                if (unNode.Attributes.GetNamedItem("ligne") != null)
                    ligne["ligne"] = unNode.Attributes["ligne"].InnerText;
                if (unNode.Attributes.GetNamedItem("epaisseur") != null)
                    ligne["epaisseur"] = unNode.Attributes["epaisseur"].InnerText;
            }
        }

        public static void Fill_Motif_Surfacique_Grid(C1FlexGrid grid, XmlDocument Doc)
        {
            XmlNodeList nodeList;

            grid.Rows.Count = 1;
            grid.Tag = Doc;

            root = Doc.DocumentElement;

            grid.Tag = root.SelectSingleNode("surfaciques");

            nodeList = root.SelectNodes("surfaciques/motif");

            foreach (XmlNode unNode in nodeList)
            {
                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["nom"] = unNode.Attributes["nom"].InnerText;
                if (unNode.Attributes.GetNamedItem("hachure") != null)
                    ligne["hachure"] = unNode.Attributes["hachure"].InnerText;
                if (unNode.Attributes.GetNamedItem("angle") != null)
                    ligne["angle"] = unNode.Attributes["angle"].InnerText;
                if (unNode.Attributes.GetNamedItem("echelle") != null)
                    ligne["echelle"] = unNode.Attributes["echelle"].InnerText;
            }
        }


        public static void Set_Motif_Ponctuel_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours motif
            grid.Cols["nom"].UserData = "att";
            grid.Cols["symbole"].UserData = "att";
            grid.Cols["echelle"].UserData = "att";
        }

        public static void Set_Motif_Lineaire_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours motif
            grid.Cols["nom"].UserData = "att";
            grid.Cols["ligne"].UserData = "att";
            grid.Cols["epaisseur"].UserData = "att";
        }

        public static void Set_Motif_Surfacique_Grid_Update_Fields(C1FlexGrid grid)
        {
            //On indique l'emplacement de la données par rapport au noeud en cours motif
            grid.Cols["nom"].UserData = "att";
            grid.Cols["hachure"].UserData = "att";
            grid.Cols["angle"].UserData = "att";
            grid.Cols["echelle"].UserData = "att";
        }


        public static void Motif_After_Edit(object sender, RowColEventArgs e)
        {
            C1FlexGrid grid = (C1FlexGrid)sender;

            string nom = grid[e.Row, "nom"].ToString();
            string colname = grid.Cols[e.Col].Name;
            string userdata = grid.Cols[e.Col].UserData.ToString();
            string newvalue = grid[e.Row, e.Col].ToString();
            bool is_attribute = false;

            XmlNode originnod = (XmlNode)grid.Tag;

            if (userdata == string.Empty)
                return;

            XmlNode node;

            node = originnod.SelectSingleNode(string.Concat("motif[@nom='" + nom + "']"));
                
            if (userdata == "val")
                is_attribute = false;
            else if (userdata == "att")
                is_attribute = true;

            mod_save.Save_Param_Field(mod_inspection.Motif_Xml, node, newvalue, is_attribute, colname, mod_global.MF.XmlMotifStripLabel.Text);
        }


        public static void Init_Motif_Buttons_Tags_n_Events()
        {
            mod_global.MF.XmlPonctuelAddValueTb.Tag = mod_global.MF.MotifPonctuelGrid;
            mod_global.MF.XmlLineaireAddValueTb.Tag = mod_global.MF.MotifLineaireGrid;
            mod_global.MF.XmlSurfaciqueAddValueTb.Tag = mod_global.MF.MotifSurfaciqueGrid;

            mod_global.MF.XmlPonctuelAddValueBt.Tag = mod_global.MF.XmlPonctuelAddValueTb;
            mod_global.MF.XmlLineaireAddValueBt.Tag = mod_global.MF.XmlLineaireAddValueTb;
            mod_global.MF.XmlSurfaciqueAddValueBt.Tag = mod_global.MF.XmlSurfaciqueAddValueTb;

            mod_global.MF.XmlPonctuelDelValueBt.Tag = mod_global.MF.MotifPonctuelGrid;
            mod_global.MF.XmlLineaireDelValueBt.Tag = mod_global.MF.MotifLineaireGrid;
            mod_global.MF.XmlSurfaciqueDelValueBt.Tag = mod_global.MF.MotifSurfaciqueGrid;

            mod_global.MF.XmlPonctuelAddValueBt.Click += new EventHandler(Add_Motif);
            mod_global.MF.XmlLineaireAddValueBt.Click += new EventHandler(Add_Motif);
            mod_global.MF.XmlSurfaciqueAddValueBt.Click += new EventHandler(Add_Motif);

            mod_global.MF.XmlPonctuelDelValueBt.Click += new EventHandler(Del_Motif);
            mod_global.MF.XmlLineaireDelValueBt.Click += new EventHandler(Del_Motif);
            mod_global.MF.XmlSurfaciqueDelValueBt.Click += new EventHandler(Del_Motif); 
        }

        public static void Add_Motif(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            TextBox tb = (TextBox)bt.Tag;
            C1FlexGrid grid = (C1FlexGrid)tb.Tag;
            XmlNode originnod = (XmlNode)grid.Tag;

            if (tb.Text != String.Empty)
            {
                if (mod_global.Check_If_Motif_Name_Exist(tb.Text) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Ce nom de motif existe déjà.");
                    return;
                }

                C1.Win.C1FlexGrid.Row ligne = grid.Rows.Add();
                ligne["nom"] = tb.Text;

                XmlElement motifNode = mod_inspection.Motif_Xml.CreateElement("motif");
                motifNode.SetAttribute("nom", tb.Text);

                originnod.AppendChild(motifNode);
                mod_inspection.Motif_Xml.Save(mod_global.MF.XmlMotifStripLabel.Text);

                tb.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Veuillez saisir un nom de motif à ajouter", "Erreur", MessageBoxButtons.OK);
            }
        }

        public static void Del_Motif(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            C1FlexGrid grid = (C1FlexGrid)bt.Tag;
            XmlNode originnod = (XmlNode)grid.Tag;

            if (grid.RowSel > 0)
            {
                if (mod_global.Check_If_Motif_Is_Still_Used(grid[grid.RowSel, "nom"].ToString()) == true)
                {
                    System.Windows.Forms.MessageBox.Show("Ce motif est encore utilisé par une observation.");
                    return;
                }

                XmlNode nodtoremove = originnod.SelectSingleNode("motif[@nom='" + grid[grid.RowSel, "nom"].ToString() + "']");
                originnod.RemoveChild(nodtoremove);

                grid.Rows.Remove(grid.RowSel);
                mod_inspection.Motif_Xml.Save(mod_global.MF.XmlMotifStripLabel.Text);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un motif à supprimer", "Erreur", MessageBoxButtons.OK);
            }
        }
    }
}
