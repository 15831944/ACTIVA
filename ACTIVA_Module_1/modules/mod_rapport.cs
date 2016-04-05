using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.XPath;
using System.Globalization;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1.modules
{
    class mod_rapport
    {
        private static C1.C1Pdf.C1PdfDocument thepdf;
        private static string SELECTED_OUVRAGE = String.Empty;
        private static string LIST_OUVRAGES = String.Empty;
        private static string SELECTED_TYPE_OUVRAGE = String.Empty;
        private static XmlNodeList CODE_NODE_LIST;

        public static void Create_Pdf_Document(C1.C1Pdf.C1PdfDocument pdf, string filename, bool with_id, bool with_obs, bool with_synthese)
        {
            // step 1: create the C1PdfDocument object
            pdf.Clear();
            pdf.DocumentInfo.Title = "Rapport d'ouvrage";

            thepdf = pdf;

            // calculate page rect (discounting margins)
            RectangleF rcPage = GetPageRect();
            RectangleF rc = rcPage;

            XmlNode root = ACTIVA_Module_1.modules.mod_inspection.SVF.DocumentElement;

            SELECTED_OUVRAGE = string.Empty;
            LIST_OUVRAGES = string.Empty;
            SELECTED_TYPE_OUVRAGE = string.Empty;

            rc.Offset(0,30);

            int ouvragecount = 0;
            foreach (int row in mod_global.MF.OuvrageList.SelectedIndices)
            {
                SELECTED_OUVRAGE = mod_global.MF.OuvrageList.GetItemText(row, "Nom");
                SELECTED_TYPE_OUVRAGE = mod_global.MF.OuvrageList.GetItemText(row, "Type");
                LIST_OUVRAGES += "@nom='" + mod_global.MF.OuvrageList.GetItemText(row, "Nom") + "' or ";

                mod_inspection.OUVRAGE = SELECTED_OUVRAGE;
                mod_inspection.TYPE_OUVRAGE = SELECTED_TYPE_OUVRAGE;

                if (ouvragecount > 0)
                {
                    pdf.NewPage();
                    rc = rcPage;
                    rc.Offset(0, 30);
                }

                // render some tables
                if (with_id == true)
                {
                    rc = RenderTitle(rc, rcPage);
                    RenderIdentificationTable(rc, rcPage, "Identification de l'ouvrage " + SELECTED_OUVRAGE);
                }

                if (with_obs == true)
                {
                    pdf.NewPage();
                    rc = rcPage;
                    rc.Offset(0, 30);
                    RenderObservationTable(rc, rcPage, "Observations de l'ouvrage " + SELECTED_OUVRAGE, new string[] { "position", "code", "forme", "observation", "photo" });
                }

                if (with_synthese == true)
                {
                    pdf.NewPage();
                    rc = rcPage;
                    rc.Offset(0, 30);
                    CODE_NODE_LIST = root.SelectNodes(string.Concat("/inspection/ouvrage[@nom='", SELECTED_OUVRAGE, "']/observations/code"));
                    RenderComptageTable(rc, rcPage, "Comptage des désordres de l'ouvrage " + SELECTED_OUVRAGE);
                    pdf.NewPage();
                    rc = rcPage;
                    rc.Offset(0, 30);
                    RenderSyntheseTable(rc, rcPage, "Synthèse de l'ouvrage " + SELECTED_OUVRAGE);
                }

                ouvragecount++;
                //pdf.NewPage();
                //rc = rcPage;
                //rc.Offset(0, 30);
            }

            LIST_OUVRAGES = LIST_OUVRAGES.Substring(0, LIST_OUVRAGES.Length - 4);

            if (with_synthese == true & ouvragecount > 1)
            {
                pdf.NewPage();
                rc = rcPage;
                rc.Offset(0, 30);
                CODE_NODE_LIST = root.SelectNodes("/inspection/ouvrage[" + LIST_OUVRAGES + "]/observations/code");
                RenderComptageTable(rc, rcPage, "Comptage global des désordres");
                pdf.NewPage();
                rc = rcPage;
                rc.Offset(0, 30);
                RenderSyntheseTable(rc, rcPage, "Synthèse globale");
            }

            // second pass to number pages
            AddHeaders();
            AddFooters();

            // step 3: save the document to a file
            pdf.Save(filename);
            System.Diagnostics.Process.Start(filename);
        }

        private static RectangleF RenderTitle(RectangleF rc, RectangleF rcPage)
        {
            // center vertically and horizontally
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            RectangleF rcId = rc;
            Font hdrFont = new Font("Arial", 14, FontStyle.Bold);
            Pen pen = new Pen(Brushes.Black, 0.1f);

            rcId.Width = 480;
            rcId.Height = 30;

            string idtitle = SELECTED_TYPE_OUVRAGE + " - " + SELECTED_OUVRAGE;
            idtitle = idtitle.ToUpper();

            System.Drawing.SizeF round_corner = new System.Drawing.SizeF(5,5);

            thepdf.FillRectangle(Brushes.LightGray, rcId, round_corner);
            thepdf.DrawRectangle(pen, rcId, round_corner);
            thepdf.DrawString(idtitle, hdrFont, Brushes.Black, rcId, sf);

            rcId.Offset(0, 45);

            return rcId;
        }

        private static RectangleF RenderIdentificationTable(RectangleF rc, RectangleF rcPage, string headertext)
        {
            // select fonts
            Font hdrFont = new Font("Arial", 14, FontStyle.Bold);
            Font lilhdrFont = new Font("Arial", 10, FontStyle.Bold);
            Font supalilhdrFont = new Font("Arial", 7, FontStyle.Bold);
            Font txtFont = new Font("Arial", 7);
            Pen pen = new Pen(Brushes.Black, 0.1f);

            RectangleF rcId = rc;
            
            //En tete de la table d'identification
            rcId.Width = 480;
            rcId.Height = 30;
            thepdf.DrawRectangle(pen, rcId);
            rcId.Offset(5, 5);
            thepdf.DrawString(headertext, lilhdrFont, Brushes.Black, rcId);
            rcId.Offset(-5,-2);

            //Cadre de la zone d'identification
            rcId.Offset(0, 30);
            rcId.Width = 480;
            rcId.Height = 550;
            thepdf.DrawRectangle(pen, rcId);
            rcId.Offset(5, 5);

            // get data
            XmlNodeList nodeList;
            XmlNode root;
            string intituletext = string.Empty;
            string text = String.Empty;
            string idtext = string.Empty;
            root = ACTIVA_Module_1.modules.mod_inspection.SVF.DocumentElement;
            
            ArrayList parents = Get_Identification_Parent_List();

            foreach (object parent in parents)
            {
                if ((string)parent != string.Empty)
                {
                    nodeList = root.SelectNodes("/inspection/ouvrage[@nom='" + SELECTED_OUVRAGE + "']/identifications/code[@parent='" + (string)parent + "']");

                    rcId.Height = 10;

                    thepdf.DrawString(mod_global.Get_Id_Parent_Equivalence((string)parent), supalilhdrFont, Brushes.Black, rcId);

                    rcId.Offset(0, 10);

                    text = string.Empty;

                    foreach (XmlNode nod in nodeList)
                    {
                        //On n'ecrit la valeur que si elle n'est pas nulle
                        if (nod.ChildNodes[2].InnerText != String.Empty)
                        {
                            intituletext = nod.ChildNodes[1].InnerText + " (" + nod.ChildNodes[0].InnerText + ")";
                            //intituletext = intituletext.PadRight(45, Char.Parse(" "));
                            idtext = " : " + nod.ChildNodes[2].InnerText + " " + mod_identification.Get_Unite_For_Id_Code(nod.ChildNodes[0].InnerText) + "\n";

                            text += intituletext + idtext;
                        }
                    }

                    rcId.Height = thepdf.MeasureString(text, txtFont, rcId.Width).Height;
                    thepdf.DrawString(text, txtFont, Brushes.Black, rcId);
                    rcId.Offset(0, rcId.Height + 5);
                }
            }

            // done
            return rcId;

            // build table
            //thepdf.AddBookmark(headertext, 0, rc.Y);
            //rc = RenderParagraph("", txtFont, rcPage, rc, false);
        }

        private static RectangleF RenderObservationTable(RectangleF rc, RectangleF rcPage, string headertext, string[] fields)
        {
            // get data
          //  XmlNodeList nodeList;
            XmlNode root;
            XmlNode unNode;

            root = ACTIVA_Module_1.modules.mod_inspection.SVF.DocumentElement;
            //nodeList = root.SelectNodes(string.Concat("/inspection/ouvrage[@nom='", SELECTED_OUVRAGE, "']/observations/code"));

            XPathNavigator nav = root.CreateNavigator();
            XPathExpression exp = nav.Compile(string.Concat("/inspection/ouvrage[@nom='", SELECTED_OUVRAGE, "']/observations/code"));

            // select fonts
            Font hdrFont = new Font("Arial", 8, FontStyle.Bold);
            Font lilhdrFont = new Font("Arial", 10, FontStyle.Bold);
            Font txtFont = new Font("Arial", 7);

            Pen pen = new Pen(Brushes.Black, 0.1f);

            // build table
            thepdf.AddBookmark(headertext, 0, rc.Y);
            rc = RenderParagraph("", txtFont, rcPage, rc, false);

            //En tete du tableau d'observations
            rc.Width = 480;
            rc.Height = 30;
            thepdf.DrawRectangle(pen, rc);
            rc.Offset(5, 5);
            thepdf.DrawString(headertext, lilhdrFont, Brushes.Black, rc);
            rc.Offset(-5, -2);
            rc.Offset(0, 35);

            string[] photo_path;
            // build table
            //rc = RenderTableHeader(hdrFont, rc, fields);

            //on trie par pm1 croissant (necessite que les numériques soient avec un point comme séparateur décimal)
            exp.AddSort("./caracteristiques/caracteristique[@nom='pm1']", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);
            foreach (XPathNavigator item in nav.Select(exp))
            {
           
                //foreach (XmlNode unNode in nodeList)       {
                if (item is IHasXmlNode)
                {
                    unNode = ((IHasXmlNode)item).GetNode();
                    photo_path = Retrieve_First_Four_Photo_Path(unNode.Attributes["num"].InnerText);
                    rc = RenderTableRow(txtFont, hdrFont, rcPage, rc, fields, new string[] { mod_observation.Get_Pm(unNode, true), mod_observation.Get_Pm2(unNode,true), mod_observation.Get_H1(unNode,true), mod_observation.Get_H2(unNode,true), unNode.FirstChild.InnerText, unNode.ChildNodes[1].InnerText, unNode.Attributes["forme"].InnerText, Get_Caracteristiques(unNode) }, photo_path);
                }
            }
            // done
            return rc;
        }

        private static RectangleF RenderTableRow(Font font, Font hdrFont, RectangleF rcPage, RectangleF rc, string[] fields, string[] rowvalues, string[] photopath)
        {
            RectangleF rcCell = rc;
            StringFormat sf = new StringFormat();
            Pen pen = new Pen(Brushes.Gray, 0.1f);

            // break page if we have to (60 représente l'espace de l'en-tete)
            if (rcCell.Bottom > (rcPage.Bottom - 60))
            {
                thepdf.NewPage();
                rcPage = GetPageRect();
                rcPage.Offset(0, 30);
                rcCell.Y = rcPage.Y;
                rc.Y = rcCell.Y;
            }

            // center vertically and horizontally
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;

            // render info cells
            rcCell.Width = 120;
            rcCell.Height = 10;

            //On dessine et remplit les en-tètes des cellules d'information
            thepdf.FillRectangle(Brushes.LightGray, rcCell);
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString("Début", font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.FillRectangle(Brushes.LightGray, rcCell);
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString("Fin", font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.FillRectangle(Brushes.LightGray, rcCell);
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString("Code", font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.FillRectangle(Brushes.LightGray, rcCell);
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString("Intitulé", font, Brushes.Black, rcCell, sf);
            rcCell.Offset(-rcCell.Width*3, rcCell.Height);
            //--------------------------------------------------------------

            //On dessine et remplit les cellules d'information
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString(rowvalues[0], font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString(rowvalues[1], font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString(rowvalues[4], font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString(rowvalues[5], font, Brushes.Black, rcCell, sf);
            rcCell.Offset(-rcCell.Width * 3, rcCell.Height);
            //--------------------------------------------------------------

            rcCell.Width = 240;
            rcCell.Height = 10;

            //On dessine et remplit les en-tètes des cellules d'information
            thepdf.FillRectangle(Brushes.LightGray, rcCell);
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString("Horaire début", font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.FillRectangle(Brushes.LightGray, rcCell);
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString("Horaire fin", font, Brushes.Black, rcCell, sf);
            rcCell.Offset(-rcCell.Width, rcCell.Height);
            //--------------------------------------------------------------

            //On dessine et remplit les cellules d'information
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString(rowvalues[2], font, Brushes.Black, rcCell, sf);
            rcCell.Offset(rcCell.Width, 0);
            //--------------------------------------------------------------
            thepdf.DrawRectangle(pen, rcCell);
            thepdf.DrawString(rowvalues[3], font, Brushes.Black, rcCell, sf);
            rcCell.Offset(-rcCell.Width, rcCell.Height);
            //--------------------------------------------------------------

            sf.LineAlignment = StringAlignment.Near;
            sf.Alignment = StringAlignment.Near;

            // render observation cell
            rcCell.Width = 480;
            rcCell.Height = 60;
            thepdf.DrawRectangle(pen, rcCell);
            rcCell.Inflate(-4, 0);
            thepdf.DrawString(rowvalues[7], font, Brushes.Black, rcCell, sf);
            rcCell.Inflate(4, 0);

            rcCell.Offset(0, rcCell.Height);

            //// render photo cell
            rcCell.Width = 120;
            rcCell.Height = 90;

            Image img;
            for (int i = 0; i < photopath.Length; i++)
            {
                if (photopath[i] != null)
                {
                    img = Image.FromFile(photopath[i]);
                    thepdf.DrawImage(img, rcCell, ContentAlignment.MiddleCenter, C1.C1Pdf.ImageSizeModeEnum.Stretch);  
                }
                thepdf.DrawRectangle(pen, rcCell);
                rcCell.Offset(rcCell.Width, 0);
                
            }

            //rcCell.Inflate(-4, 0);

            //// scale the image to fit the rectangle while preserving the aspect ratio

            //thepdf.DrawRectangle(pen, rcCell);

            //rcCell.Inflate(4, 0);
            //rcCell.Offset(rcCell.Width, 0);

            pen.Dispose();

            // update rectangle and return it
            rcCell.Offset(-rcCell.Width*4, rcCell.Height+20);

            return rcCell;
        }

        private static RectangleF RenderComptageTable(RectangleF rc, RectangleF rcPage, string headertext)
        {
            // select fonts
            Font hdrFont = new Font("Arial", 14, FontStyle.Bold);
            Font lilhdrFont = new Font("Arial", 10, FontStyle.Bold);
            Font supalilhdrFont = new Font("Arial", 7, FontStyle.Bold);
            Font txtFont = new Font("Arial", 7);
            Pen pen = new Pen(Brushes.Black, 0.1f);

            RectangleF rcId = rc;

            //En tete de la table d'identification
            rcId.Width = 480;
            rcId.Height = 30;
            thepdf.DrawRectangle(pen, rcId);
            rcId.Offset(5, 5);
            thepdf.DrawString(headertext, lilhdrFont, Brushes.Black, rcId);
            rcId.Offset(-5, -2);

            //Cadre de la zone d'identification
            rcId.Offset(0, 30);
            rcId.Width = 480;
            rcId.Height = 550;
            thepdf.DrawRectangle(pen, rcId);
            rcId.Offset(5, 5);

            string intituletext = string.Empty;
            string id = string.Empty;
            string text = String.Empty;
            string counttext = string.Empty;
            Dictionary<string, int> codecount = new Dictionary<string, int>();

                    text = string.Empty;

                    foreach (XmlNode nod in CODE_NODE_LIST)
                    {
                        id = nod.SelectSingleNode("id").InnerText;
                        if (codecount.ContainsKey(id))
                            codecount[id] = codecount[id] + 1;
                        else
                            codecount.Add(id, 1);
                    }

                    foreach (string code in codecount.Keys)
                    {
                        intituletext = mod_observation.Get_Intitule_From_Code(code);
                        if (intituletext != string.Empty)
                            text += code + " ( " + intituletext + " ) = " + codecount[code] + "\n";
                        else
                            text += code + " = " + codecount[code] + "\n";
                    }

                    rcId.Height = thepdf.MeasureString(text, txtFont, rcId.Width).Height;
                    thepdf.DrawString(text, txtFont, Brushes.Black, rcId);
                    rcId.Offset(0, rcId.Height + 5);


            // done
            return rcId;

            // build table
            //thepdf.AddBookmark(headertext, 0, rc.Y);
            //rc = RenderParagraph("", txtFont, rcPage, rc, false);
        }

        private static RectangleF RenderSyntheseTable(RectangleF rc, RectangleF rcPage, string headertext)
        {
            // select fonts
            Font hdrFont = new Font("Arial", 14, FontStyle.Bold);
            Font lilhdrFont = new Font("Arial", 10, FontStyle.Bold);
            Pen pen = new Pen(Brushes.Black, 0.1f);

            RectangleF rcSynt = rc;

            //En tete de la table de synthèse
            rcSynt.Width = 480;
            rcSynt.Height = 30;
            thepdf.DrawRectangle(pen, rcSynt);
            rcSynt.Offset(5, 5);
            thepdf.DrawString(headertext, lilhdrFont, Brushes.Black, rcSynt);
            rcSynt.Offset(-5, -2);

            //Cadre de la zone de synthèse
            rcSynt.Offset(0, 30);
            rcSynt.Width = 480;
            rcSynt.Height = 550;
            thepdf.DrawRectangle(pen, rcSynt);
            rcSynt.Offset(5, 5);

            // get data
            string intituletext = string.Empty;
            string text = String.Empty;
            string idtext = string.Empty;
            bool is_header = false;

            float rcYinit = rcSynt.Y;

            //----------------------------
            //Synthese des fissures
            //----------------------------
            rcSynt.Height = 10;
            thepdf.DrawString("Fissures (BAB/DAB)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] fissures = Get_Synthese_Fissure();
            for (int i = 0; i < fissures.GetLength(0); i++)
            {
                is_header = false;
                if (fissures[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(fissures[i, 0], is_header, rcSynt);
            }

            //----------------------------
            //Synthese des dégradations de surface
            //----------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Dégradations de surface (BAF/DAF)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] degrad_surface = Get_Synthese_Degradation_Surface();
            for (int i = 0; i < degrad_surface.GetLength(0); i++)
            {
                is_header = false;
                if (degrad_surface[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(degrad_surface[i, 0], is_header, rcSynt);
            }

            //----------------------------
            //Synthese des défauts de revetement
            //----------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Défauts de revêtement (BAK/DAK)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] revetements = Get_Synthese_Revetement();
            for (int i = 0; i < revetements.GetLength(0); i++)
            {
                is_header = false;
                if (revetements[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(revetements[i, 0], is_header, rcSynt);
            }

            //----------------------------
            //Synthese des réparations défectueuses
            //----------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Réparations défectueuses (BAL/DAL)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] reparations = Get_Synthese_Reparation_Defectueuse();
            for (int i = 0; i < reparations.GetLength(0); i++)
            {
                is_header = false;
                if (reparations[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(reparations[i, 0], is_header, rcSynt);
            }

            //----------------------------
            //Synthese des infiltrations
            //----------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Infiltrations (BBF/DBF)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] infiltrations = Get_Synthese_Infiltration();
            for (int i = 0; i < infiltrations.GetLength(0); i++)
            {
                is_header = false;
                if (infiltrations[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(infiltrations[i, 0], is_header, rcSynt);
            }


            //----------------------------
            //Synthese des drains
            //----------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Drains (BAQ/DAS)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] drains = Get_Synthese_Drain();
            for (int i = 0; i < drains.GetLength(0); i++)
            {
                is_header = false;
                if (drains[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(drains[i, 0], is_header, rcSynt);
            }


            //-------------------------------
            //Synthese des dépôts adhérents
            //-------------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Dépôts adhérents (BBB/DBB)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] depots_adherent = Get_Synthese_Depot_Adherent();
            for (int i = 0; i < depots_adherent.GetLength(0); i++)
            {
                is_header = false;
                if (depots_adherent[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(depots_adherent[i, 0], is_header, rcSynt);
            }


            //On cree la deuxieme colonne
            rcSynt.Y = rcYinit;
            rcSynt.Offset(250, 0);

            //-----------------------------------
            //Synthese des intrusions de racines
            //-----------------------------------
            rcSynt.Height = 10;
            thepdf.DrawString("Intrusions de racines (BBA/DBA)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] racines = Get_Synthese_Intrusion_Racine();
            for (int i = 0; i < racines.GetLength(0); i++)
            {
                is_header = false;
                if (racines[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(racines[i, 0], is_header, rcSynt);
            }

            //-------------------------------
            //Synthese des depots
            //-------------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Dépôts (BBC/DBC)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] depots = Get_Synthese_Depot();
            for (int i = 0; i < depots.GetLength(0); i++)
            {
                is_header = false;
                if (depots[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(depots[i, 0], is_header, rcSynt);
            }

            //-------------------------------
            //Synthese des niveaux d'eau
            //-------------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Niveau d'eau (BBD)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] niveau = Get_Synthese_Niveau_Eau();
            for (int i = 0; i < niveau.GetLength(0); i++)
            {
                is_header = false;
                if (niveau[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(niveau[i, 0], is_header, rcSynt);
            }

            //----------------------------------
            //Synthese des supports désaffectés
            //----------------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("Supports désaffectés (BCI/DCR)", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] supports = Get_Synthese_Support_Desaffecte();
            for (int i = 0; i < supports.GetLength(0); i++)
            {
                is_header = false;
                if (supports[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(supports[i, 0], is_header, rcSynt);
            }

            
            //----------------------------
            //Synthese des valeurs comptées (BCA,BCB,BAG,BAH,DAQ)
            //----------------------------
            rcSynt.Offset(0, 10);
            rcSynt.Height = 10;
            thepdf.DrawString("---------------------------------------------", lilhdrFont, Brushes.SlateGray, rcSynt);
            rcSynt.Offset(0, 15);
            string[,] valeur_comptees = Get_Counted_Values();
            for (int i = 0; i < reparations.GetLength(0); i++)
            {
                is_header = false;
                if (valeur_comptees[i, 1] == "header")
                    is_header = true;

                rcSynt = Write_Text_Line(valeur_comptees[i, 0], is_header, rcSynt);
            }

            
            // done
            return rcSynt;

            // build table
            //thepdf.AddBookmark(headertext, 0, rc.Y);
            //rc = RenderParagraph("", txtFont, rcPage, rc, false);
        }

        private static RectangleF Write_Text_Line(string text, bool is_header, RectangleF rc)
        {
            Font supalilhdrFont = new Font("Arial", 7, FontStyle.Bold);
            Font txtFont = new Font("Arial", 7);

            RectangleF rcT = rc;

            if (is_header == true)
            {
                rcT.Height = 10;
                thepdf.DrawString(text, supalilhdrFont, Brushes.Black, rcT);
                rcT.Offset(0, 10);
            }
            else
            {
                rcT.Height = thepdf.MeasureString(text, txtFont, rcT.Width).Height;
                thepdf.DrawString(text, txtFont, Brushes.Black, rcT);
                rcT.Offset(0, rcT.Height + 2);
            }

            return rcT;
        }
        /// <summary>
        /// Fonction d'ajout d'en-tête de pages des rapports
        /// </summary>
        private static void AddHeaders()
        {
            Font fontHorz = new Font("Arial", 10, FontStyle.Regular);
            Font titlefont = new Font("Arial", 14, FontStyle.Bold);

            Pen pen = new Pen(Brushes.White, 0.1f);

            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Near;
            sfRight.LineAlignment = StringAlignment.Far;

            StringFormat sfLeft = new StringFormat();
            sfLeft.Alignment = StringAlignment.Near;
            sfLeft.LineAlignment = StringAlignment.Near;

            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;

            for (int page = 0; page < thepdf.Pages.Count; page++)
            {
                // select page we want (could change PageSize)
                thepdf.CurrentPage = page;

                // build rectangles for rendering text
                RectangleF rcPage = GetPageRect();
                RectangleF rcHeader = rcPage;

                rcHeader.Offset(0, 20);

                // add left-aligned header
                Image img;
                string adress = string.Empty;
                if (Properties.Settings.Default.ReportLogoPath != String.Empty & System.IO.File.Exists(Properties.Settings.Default.ReportLogoPath))
                    img = Image.FromFile(Properties.Settings.Default.ReportLogoPath);
                else
                    img = Properties.Resources.Activa_Default_Logo;

                if (Properties.Settings.Default.ReportHeaderText != String.Empty)
                    adress = Properties.Settings.Default.ReportHeaderText;

                float adress_width = thepdf.MeasureString(adress, fontHorz).Width;

                rcHeader.Y = rcHeader.Top - (60 + 10);

                //Affichage de l'image
                rcHeader.Width = 60;
                rcHeader.Height = 60;
                thepdf.DrawRectangle(pen, rcHeader);
                thepdf.DrawImage(img, rcHeader, ContentAlignment.MiddleLeft, C1.C1Pdf.ImageSizeModeEnum.Stretch);

                rcHeader.Offset(60, 0);
                rcHeader.Width = 20;
                thepdf.DrawRectangle(pen, rcHeader);

                rcHeader.Offset(20, 0);
                rcHeader.Width = adress_width;
                thepdf.DrawRectangle(pen, rcHeader);
                thepdf.DrawString(adress, fontHorz, Brushes.Black, rcHeader, sfLeft);

                rcHeader.Offset(adress_width+70, 0);
                rcHeader.Width = 480 - (adress_width + 5 + 60);
                thepdf.DrawRectangle(pen, rcHeader);
                thepdf.DrawString("Inspection\n des réseaux\n d'assainissement", titlefont, Brushes.Black, rcHeader, sfCenter);
            }
        }

        /// <summary>
        /// Fonction d'ajout de pieds de pages des rapports
        /// </summary>
        private static void AddFooters()
        {
            Font fontHorz = new Font("Arial", 7, FontStyle.Regular);

            Pen pen = new Pen(Brushes.Gray, 0.1f);

            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;
            sfRight.LineAlignment = StringAlignment.Far;

            StringFormat sfLeft = new StringFormat();
            sfLeft.Alignment = StringAlignment.Near;
            sfLeft.LineAlignment = StringAlignment.Near;

            for (int page = 0; page < thepdf.Pages.Count; page++)
            {
                // select page we want (could change PageSize)
                thepdf.CurrentPage = page;

                // build rectangles for rendering text
                RectangleF rcPage = GetPageRect();
                RectangleF rcFooter = rcPage;

                rcFooter.Y = rcFooter.Bottom;

                rcFooter.Width = 240;
                rcFooter.Height = 50;

                thepdf.DrawRectangle(pen, rcFooter);
                // add left-aligned footer
                string text = thepdf.DocumentInfo.Title;
                thepdf.DrawString(text, fontHorz, Brushes.Black, rcFooter, sfLeft);

                rcFooter.Offset(rcFooter.Width, 0);

                thepdf.DrawRectangle(pen, rcFooter);
                // add right-aligned footer
                text = string.Format("Page {0} of {1}", page + 1, thepdf.Pages.Count);
                thepdf.DrawString(text, fontHorz, Brushes.Black, rcFooter, sfRight);
            }
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

                if (unNode.InnerText != String.Empty & caracname != "photo" & caracname != "pm1" & caracname != "pm2" & caracname != "h1" & caracname != "h2")
                {
                    if (caracname == "assemblage")
                    {
                        string asm_value = unNode.InnerText;

                        if (asm_value.ToLower() == "true")
                            carac += "Assemblage : Oui\n";
                    }
                    else
                    {
                        if (unNode.Attributes.GetNamedItem("correspondance") != null)
                            correspondance = " (" + unNode.Attributes["correspondance"].InnerText + ")";

                        carac += mod_observation.Get_Info_For_Carac(code, caracname) + " : " + unNode.InnerText + correspondance + " " + mod_observation.Get_Unite_For_Carac(code, caracname) + "\n";

                        correspondance = String.Empty;
                    }
                }
            }

            return carac;
        }

        // get the current page rectangle (depends on paper size)
        // and apply a 1" margin all around it.
        internal static RectangleF GetPageRect()
        {
            RectangleF rcPage = thepdf.PageRectangle;
            rcPage.Inflate(-72, -72);
            return rcPage;
        }

        // measure a paragraph, skip a page if it won't fit, render it into a rectangle,
        // and update the rectangle for the next paragraph.
        // 
        // optionally mark the paragraph as an outline entry and as a link target.
        //
        // this routine will not break a paragraph across pages. for that, see the Text Flow sample.
        //

        internal static RectangleF RenderParagraph(string text, Font font, RectangleF rcPage, RectangleF rc, bool outline, bool linkTarget)
        {
            // if it won't fit this page, do a page break
            rc.Height = thepdf.MeasureString(text, font, rc.Width).Height;
            if (rc.Bottom > rcPage.Bottom)
            {
                thepdf.NewPage();
                rc.Y = rcPage.Top;
            }

            // draw the string
            thepdf.DrawString(text, font, Brushes.Black, rc);

            // show bounds (mainly to check word wrapping)
            //thepdf.DrawRectangle(Pens.Sienna, rc);

            // add headings to outline
            if (outline)
            {
                thepdf.DrawLine(Pens.Black, rc.X, rc.Y, rc.Right, rc.Y);
                thepdf.AddBookmark(text, 0, rc.Y);
            }

            // add link target
            if (linkTarget)
            {
                thepdf.AddTarget(text, rc);
            }

            // update rectangle for next time
            rc.Offset(0, rc.Height);
            return rc;
        }

        internal static RectangleF RenderParagraph(string text, Font font, RectangleF rcPage, RectangleF rc, bool outline)
        {
            return RenderParagraph(text, font, rcPage, rc, outline, false);
        }

        internal static RectangleF RenderParagraph(string text, Font font, RectangleF rcPage, RectangleF rc)
        {
            return RenderParagraph(text, font, rcPage, rc, false, false);
        }

        private static string[] Retrieve_First_Four_Photo_Path(string num)
        {
            XmlNode root;
            XmlNode PhotoNode;
            string[] picpath = new string[4];

            root = mod_inspection.SVF.DocumentElement;

            PhotoNode = root.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_inspection.OUVRAGE + "']/observations/code[@num='" + num + "']/caracteristiques/caracteristique[@nom='photo']");

            if (PhotoNode.InnerText != String.Empty)
            {
                string picfolder = System.IO.Path.Combine(mod_inspection.SVF_FOLDER, "img");;
                string pic;

                for (int i = 0; i < picpath.GetLength(0) & i < PhotoNode.InnerText.Split(Char.Parse("|")).GetLength(0); i++)
                {
                    pic = PhotoNode.InnerText.Split(Char.Parse("|"))[i];
                    picpath[i] = System.IO.Path.Combine(picfolder, pic);
                }
            }

            return picpath;
        }

        private static ArrayList Get_Identification_Parent_List()
        {
            XmlNodeList nodeList;
            XmlNode root;
            ArrayList parent_list = new ArrayList();

            root = ACTIVA_Module_1.modules.mod_inspection.SVF.DocumentElement;
            nodeList = root.SelectNodes(string.Concat("/inspection/ouvrage[@nom='", SELECTED_OUVRAGE, "']/identifications/code"));

            string last_parent = String.Empty;
            
            foreach (XmlNode nod in nodeList)
            {
                if (nod.Attributes["parent"].InnerText != last_parent)
                {
                    last_parent = nod.Attributes["parent"].InnerText;
                    parent_list.Add(last_parent);
                }
            }

            return parent_list;
        }

        //---------------------------------------------------------------------------------------
        //-------------------------------- Fonctions de synthèse --------------------------------
        //---------------------------------------------------------------------------------------

        private static string[,] Get_Synthese_Fissure()
        {

            string[,] synthese_fissure = new string[20,2];
            
            string id = string.Empty;
            string code = string.Empty;

            string pm1 = string.Empty;
            string pm2 = string.Empty;
            int q1 = 0;

            double A_largeur_mini = 0;
            double A_largeur_maxi = 0;
            double A_lineaire = 0;

            double B_largeur_mini = 0;
            double B_largeur_maxi = 0;
            double B_nombre = 0;

            double C_largeur_mini = 0;
            double C_largeur_maxi = 0;
            double C_lineaire = 0;

            double D_largeur_mini = 0;
            double D_largeur_maxi = 0;
            double D_lineaire = 0;

            double XA_largeur_mini = 0;
            double XA_largeur_maxi = 0;
            double XA_lineaire = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BAB" | id == "DAB")
                {
                    code = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='c2']").Attributes["code"].InnerText;
                    pm1 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText;
                    pm2 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']").InnerText;
                    int.TryParse(mod_observation.Get_Q1(nod), out q1);
                    
                    //On les largeurs mini et maxi

                    if (code == "A")
                    {
                        A_lineaire += Get_Lineaire(pm1, pm2);
                        if (q1 < A_largeur_mini)
                            A_largeur_mini = q1;
                        if (q1 > A_largeur_maxi)
                            A_largeur_maxi = q1;
                    }
                    else if (code == "B")
                    {
                        B_nombre++;
                        if (q1 < B_largeur_mini)
                            B_largeur_mini = q1;
                        if (q1 > B_largeur_maxi)
                            B_largeur_maxi = q1;
                    }   
                    else if (code == "C")
                    {
                        C_lineaire += Get_Lineaire(pm1, pm2);
                        if (q1 < C_largeur_mini)
                            C_largeur_mini = q1;
                        if (q1 > C_largeur_maxi)
                            C_largeur_maxi = q1;
                    }
                    else if (code == "D")
                    {
                        D_lineaire += Get_Lineaire(pm1, pm2);
                        if (q1 < D_largeur_mini)
                            D_largeur_mini = q1;
                        if (q1 > D_largeur_maxi)
                            D_largeur_maxi = q1;
                    }
                    else if (code == "XA")
                    {
                        XA_lineaire += Get_Lineaire(pm1, pm2);
                        if (q1 < XA_largeur_mini)
                            XA_largeur_mini = q1;
                        if (q1 > XA_largeur_maxi)
                            XA_largeur_maxi = q1;
                    }  
                }
            }

            synthese_fissure[0, 0] = "Fissures Longitudinales";
            synthese_fissure[0, 1] = "header";
            synthese_fissure[1, 0] = "Linéaire total : " + A_lineaire + " m";
            synthese_fissure[2, 0] = "Largeur minimale : " + A_largeur_mini + " mm";
            synthese_fissure[3, 0] = "Largeur maximale : " + A_largeur_maxi + " mm";

            synthese_fissure[4, 0] = "Fissures Circonférentielles";
            synthese_fissure[4, 1] = "header";
            synthese_fissure[5, 0] = "Nombre total : " + B_nombre;
            synthese_fissure[6, 0] = "Largeur minimale : " + B_largeur_mini + " mm";
            synthese_fissure[7, 0] = "Largeur maximale : " + B_largeur_maxi + " mm";

            synthese_fissure[8, 0] = "Fissures Complexes";
            synthese_fissure[8, 1] = "header";
            synthese_fissure[9, 0] = "Linéaire total : " + C_lineaire + " m";
            synthese_fissure[10, 0] = "Largeur minimale : " + C_largeur_mini + " mm";
            synthese_fissure[11, 0] = "Largeur maximale : " + C_largeur_maxi + " mm";

            synthese_fissure[12, 0] = "Fissures Helicoïdales";
            synthese_fissure[12, 1] = "header";
            synthese_fissure[13, 0] = "Linéaire total : " + D_lineaire + " m";
            synthese_fissure[14, 0] = "Largeur minimale : " + D_largeur_mini + " mm";
            synthese_fissure[15, 0] = "Largeur maximale : " + D_largeur_maxi + " mm";

            synthese_fissure[16, 0] = "Fissures Obliques";
            synthese_fissure[16, 1] = "header";
            synthese_fissure[17, 0] = "Linéaire total : " + XA_lineaire + " m";
            synthese_fissure[18, 0] = "Largeur minimale : " + XA_largeur_mini + " mm";
            synthese_fissure[19, 0] = "Largeur maximale : " + XA_largeur_maxi + " mm";

            return synthese_fissure;
        }

        private static string[,] Get_Synthese_Degradation_Surface()
        {
            string[,] synthese_degradation_surface = new string[3, 2];

            string id = string.Empty;

            string pm1 = string.Empty;
            string pm2 = string.Empty;
            double q1 = 0;
            double q2 = 0;

            int nombre = 0;
            double superficie = 0;
            double volume = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BAF" | id == "DAF")
                {
                    pm1 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText;
                    pm2 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']").InnerText;

                    double.TryParse(mod_observation.Get_Q1(nod), out q1);
                    double.TryParse(mod_observation.Get_Q2(nod), out q2);

                    //On les largeurs mini et maxi
                    nombre++;
                    superficie += q2 * Get_Lineaire(pm1, pm2);
                    volume += q1 * superficie;
                }
            }

            synthese_degradation_surface[0, 0] = "Nombre de dégradations : " + nombre;
            synthese_degradation_surface[1, 0] = "Superficie affectée : " + superficie + " m2";
            synthese_degradation_surface[2, 0] = "Volume affecté : " + volume + " m3";

            return synthese_degradation_surface;
        }

        private static string[,] Get_Synthese_Revetement()
        {
            string[,] synthese_revet = new string[2, 2];

            string id = string.Empty;

            string pm1 = string.Empty;
            string pm2 = string.Empty;
            double q1 = 0;

            int nombre_revet = 0;
            double superficie_revet = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BAK" | id == "DAK")
                {
                    pm1 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText;
                    pm2 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']").InnerText;

                    double.TryParse(mod_observation.Get_Q1(nod), out q1);

                    //On les largeurs mini et maxi
                    nombre_revet++;
                    superficie_revet += q1 * Get_Lineaire(pm1, pm2);
                }
            }

            synthese_revet[0, 0] = "Nombre de défauts de revêtement : " + nombre_revet;
            synthese_revet[1, 0] = "Superficie affectée : " + superficie_revet + " m2";

            return synthese_revet;
        }

        private static string[,] Get_Synthese_Reparation_Defectueuse()
        {
            string[,] synthese_reparation_defectueuse = new string[3, 2];

            string id = string.Empty;

            string pm1 = string.Empty;
            string pm2 = string.Empty;
            double q1 = 0;
            double q2 = 0;

            int nombre = 0;
            double superficie = 0;
            double volume = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BAL" | id == "DAL")
                {
                    pm1 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText;
                    pm2 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']").InnerText;

                    double.TryParse(mod_observation.Get_Q1(nod), out q1);
                    double.TryParse(mod_observation.Get_Q2(nod), out q2);

                    //On les largeurs mini et maxi
                    nombre++;
                    superficie += q2 * Get_Lineaire(pm1, pm2);
                    volume += q1 * superficie;
                }
            }

            synthese_reparation_defectueuse[0, 0] = "Nombre de réparations défectueuses : " + nombre;
            synthese_reparation_defectueuse[1, 0] = "Superficie affectée : " + superficie + " m2";
            synthese_reparation_defectueuse[2, 0] = "Volume affecté : " + volume + " m3";

            return synthese_reparation_defectueuse;
        }

        private static string[,] Get_Synthese_Infiltration()
        {
            string[,] synthese_infiltration = new string[2, 2];

            string id = string.Empty;

            int q1 = 0;

            int nombre = 0;
            int debit = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BBF" | id == "DBF")
                {
                    int.TryParse(mod_observation.Get_Q1(nod), out q1);

                    //On les largeurs mini et maxi
                    nombre++;
                    debit += q1;
                }
            }

            synthese_infiltration[0, 0] = "Nombre d'infiltrations : " + nombre;
            synthese_infiltration[1, 0] = "Débit d'infiltration total estimé : " + debit + " l/min";

            return synthese_infiltration;
        }

        private static string[,] Get_Synthese_Drain()
        {
            string[,] synthese_drain = new string[2, 2];

            string id = string.Empty;

            int q1 = 0;

            int nombre = 0;
            int debit = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BAQ" | id == "DAS")
                {
                    int.TryParse(mod_observation.Get_Q1(nod), out q1);

                    //On les largeurs mini et maxi
                    nombre++;
                    debit += q1;
                }
            }

            synthese_drain[0, 0] = "Nombre total de drains : " + nombre;
            synthese_drain[1, 0] = "Débit d'infiltration total estimé : " + debit + " l/min";

            return synthese_drain;
        }

        private static string[,] Get_Synthese_Depot_Adherent()
        {
            string[,] synthese_depot = new string[1, 2];

            string pm1 = String.Empty;
            string pm2 = String.Empty;

            string id = string.Empty;

            double q2 = 0;

            double surface = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BBB" | id == "DBB")
                {
                    pm1 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText;
                    pm2 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']").InnerText;
                    double.TryParse(mod_observation.Get_Q2(nod), out q2);

                    //On les largeurs mini et maxi
                    surface += q2 * Get_Lineaire(pm1, pm2);
                }
            }

            synthese_depot[0, 0] = "Superficie affectée : " + surface + " m2";

            return synthese_depot;
        }

        private static string[,] Get_Synthese_Intrusion_Racine()
        {
            string[,] synthese_racine = new string[2, 2];

            string pm1 = String.Empty;
            string pm2 = String.Empty;

            string id = string.Empty;

            double q2 = 0;

            int nombre = 0;
            double surface = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BBA" | id == "DBA")
                {
                    pm1 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText;
                    pm2 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']").InnerText;
                    double.TryParse(mod_observation.Get_Q2(nod), out q2);

                    //On les largeurs mini et maxi
                    nombre++;
                    surface += q2 * Get_Lineaire(pm1, pm2);
                }
            }

            synthese_racine[0, 0] = "Nombre d'intrusions de racine : " + nombre;
            synthese_racine[1, 0] = "Superficie affectée : " + surface + " m2";

            return synthese_racine;
        }

        private static string[,] Get_Synthese_Depot()
        {
            string[,] synthese_depot = new string[1, 2];

            string pm1 = String.Empty;
            string pm2 = String.Empty;

            string id = string.Empty;

            double q1 = 0;
            double q2 = 0;

            double volume = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BBC" | id == "DBC")
                {
                    pm1 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm1']").InnerText;
                    pm2 = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='pm2']").InnerText;
                    double.TryParse(mod_observation.Get_Q1(nod), out q1);
                    double.TryParse(mod_observation.Get_Q2(nod), out q2);

                    //On les largeurs mini et maxi
                    volume += q1 * q2 * Get_Lineaire(pm1, pm2);
                }
            }

            synthese_depot[0, 0] = "Volume affecté : " + volume + " m3";

            return synthese_depot;
        }

        private static string[,] Get_Synthese_Niveau_Eau()
        {
            string[,] synthese_niveau = new string[3, 2];

            int vitesse_max = 0;
            int hauteur_max = 0;
            int largeur_max = 0;
            int debit_max = 0;

            string id = string.Empty;

            int c2 = 0;
            int q1 = 0;
            int q2 = 0;

            int debit = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BDD")
                {
                    int.TryParse(mod_observation.Get_Q1(nod), out q1);
                    int.TryParse(mod_observation.Get_Q2(nod), out q2);
                    int.TryParse(mod_observation.Get_C2(nod), out c2);

                    if (hauteur_max < c2)
                        hauteur_max = c2;
                    if (largeur_max < q1)
                        largeur_max = q1;
                    if (vitesse_max < q2)
                        vitesse_max = q2;

                    debit = hauteur_max * largeur_max * vitesse_max;

                    if (debit_max < debit)
                        debit_max = debit;
                }
            }

            synthese_niveau[0, 0] = "Hauteur maximum : " + hauteur_max + " m";
            synthese_niveau[1, 0] = "Vitesse maximum : " + vitesse_max + " m/s";
            synthese_niveau[2, 0] = "Débit maximum : " + debit_max + " m3/s";

            return synthese_niveau;
        }

        private static string[,] Get_Synthese_Support_Desaffecte()
        {
            string[,] synthese_support = new string[4, 2];

            string code = String.Empty;
            string id = string.Empty;

            int q1 = 0;
            int q2 = 0;

            int conduites = 0;
            int cables = 0;
            int supports = 0;
            int chemins = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BCI" | id == "DCR")
                {
                    code = nod.SelectSingleNode("caracteristiques/caracteristique[@nom='c2']").Attributes["code"].InnerText;
                    int.TryParse(mod_observation.Get_Q1(nod), out q1);
                    int.TryParse(mod_observation.Get_Q2(nod), out q2);

                    if (code == "A" | code == "B" | code == "C")
                        conduites += q2;
                    else if (code == "D" | code == "E" | code == "J")
                        cables += q1;
                    else if (code == "F" | code == "G" | code == "H")
                        supports += q2;
                    else if (code == "I")
                        chemins += q1;
                }
            }

            synthese_support[0, 0] = "Nombre de supports désaffectés : " + supports + " m";
            synthese_support[1, 0] = "Linéaire de chemins de câbles désaffectés : " + chemins + " m";
            synthese_support[2, 0] = "Linéaire de gaines et câbles désaffectés : " + cables + " m";
            synthese_support[3, 0] = "Linéaire de conduites désaffectées : " + conduites + " m";

            return synthese_support;
        }

        private static string[,] Get_Counted_Values()
        {
            string[,] synthese_valeurs_comptees = new string[5, 2];

            string id = string.Empty;

            int bcb = 0;
            int bca = 0;
            int bag = 0;
            int bah = 0;
            int daq = 0;

            foreach (XmlNode nod in CODE_NODE_LIST)
            {
                id = nod.SelectSingleNode("id").InnerText;
                if (id == "BCB" | id == "DCB")
                    bcb++;
                else if (id == "BCA" | id == "DCA")
                    bca++;
                else if (id == "BAG" | id == "DAG")
                    bag++;
                else if (id == "BAH" | id == "DAH")
                    bah++;
                else if (id == "DAQ")
                    daq++;
            }

            synthese_valeurs_comptees[0,0] = "Nombre de réparations ponctuelles (BCB/DCB) : " + bcb;
            synthese_valeurs_comptees[0, 1] = "header";
            synthese_valeurs_comptees[1, 0] = "Nombre de raccordements (BCA/DCA) : " + bca;
            synthese_valeurs_comptees[1, 1] = "header";
            synthese_valeurs_comptees[2, 0] = "Nombre de branchements pénétrants (BAG/DAG) : " + bag;
            synthese_valeurs_comptees[2, 1] = "header";
            synthese_valeurs_comptees[3, 0] = "Nombre de raccordements défectueux (BAH/DAH) : " + bah;
            synthese_valeurs_comptees[3, 1] = "header";
            synthese_valeurs_comptees[4, 0] = "Nombre d'éléments défectueux (DAQ) : " + daq;
            synthese_valeurs_comptees[4, 1] = "header";

            return synthese_valeurs_comptees;
        }

        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------

        private static double Get_Lineaire(string pm1, string pm2)
        {
            //Si pm2 n'est pas saisi on quitte
            if (pm2 == String.Empty)
                return 0D;

            double intpm1 = double.Parse(pm1);
            double intpm2 = double.Parse(pm2);

            //Si pm2 est plus petit que pm1 on quitte
            if (intpm1 > intpm2)
                return 0D;

            return (intpm2 - intpm1); 
        }

    }
}
