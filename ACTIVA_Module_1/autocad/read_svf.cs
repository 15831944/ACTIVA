using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using ACTIVA_Module_1.modules;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Interop;
using System.Globalization;
//using Autodesk.AutoCAD.Runtime;
//using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.ApplicationServices;
//using ActivCAD;
using System.Reflection;
using System.Windows.Forms;

namespace ACTIVA_Module_1.autocad
{
    class read_svf
    {
        public const int UNITE_Y = 30;
        public const int UNITE_X = 50;

        public static int OFFSET_Y = 0;
        public static int OFFSET_X = 0;

        public static dessin[] destab;
        public static Hashtable Section_Infos;

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        public static void Get_All_Observations_From_SVF()
        {
            XmlNodeList nodeList;
            XmlNode root;
            String id;
            String code;

            root = mod_inspection.SVF.DocumentElement;

            nodeList = root.SelectNodes(string.Concat("/inspection/ouvrage[@nom='", mod_inspection.OUVRAGE, "']/observations/code"));

            destab = new dessin[nodeList.Count];

            int i = 0;
            string forme = mod_inspection.FORME_OUVRAGE;

            foreach (XmlNode unNode in nodeList)
            {
                id = unNode.SelectSingleNode("id").InnerText;
                code = Get_C1_Code(unNode);

                dessin des = new dessin();

                //Dans le cas d'un changement de section, on stocke la forme
                if (id == "AEC")
                    forme = code;

                des.id = id;
                des.type = Get_Type(id, code);
                des.forme = forme;

                des.pm1 = mod_observation.Get_Pm(unNode,false);
                des.pm2 = mod_observation.Get_Pm2(unNode,false);
                des.h1 = mod_observation.Get_H1(unNode, false);
                des.h2 = mod_observation.Get_H2(unNode, false);
                des.q1 = mod_observation.Get_Q1(unNode);

                Get_Motif_Params(Get_Motif_Name(id, code), des);

                des.couleur = Get_Couleur(id);

                destab[i] = des;

                i++;
            }
        }

        private static string Get_C1_Code(XmlNode node)
        {
            XmlNode c1Node = node.SelectSingleNode("caracteristiques/caracteristique[@nom='c1']");

            if (c1Node.Attributes.GetNamedItem("code") != null)
                return c1Node.Attributes["code"].InnerText;
            else
                return "c1";
        }

        private static string Get_Couleur(string id)
        {
            XmlNode autocadNode = mod_global.Get_Codes_Obs_DocElement().SelectSingleNode("/codes/code[id='" + id + "']/autocad");

            if (autocadNode.Attributes.GetNamedItem("couleur") != null)
                return autocadNode.Attributes["couleur"].InnerText;
            else
                return "0|0|0";
        }

        private static string Get_Type(string id, string code)
        {
            XmlNodeList representationNodeList = mod_global.Get_Codes_Obs_DocElement().SelectNodes("/codes/code[id='" + id + "']/autocad/representations/*/representation[@nom='" + code + "']");

            if (representationNodeList.Count > 0)
                return representationNodeList[0].ParentNode.Name;
            else
                return string.Empty;
        }

        private static string Get_Motif_Name(string id, string code)
        {
            XmlNodeList representationNodeList = mod_global.Get_Codes_Obs_DocElement().SelectNodes("/codes/code[id='" + id + "']/autocad/representations/*/representation[@nom='" + code + "']");

            if (representationNodeList.Count > 0)
                if (representationNodeList[0].Attributes.GetNamedItem("motif") != null)
                    return representationNodeList[0].Attributes["motif"].InnerText;
                else
                    return string.Empty;
            else
                return string.Empty;
        }

        private static void Get_Motif_Params(string motifname, dessin des)
        {
            if (motifname == string.Empty)
                return;

            XmlNode motifNode = mod_inspection.Motif_Xml.SelectSingleNode("/motifs/*/motif[@nom='" + motifname + "']");

            if (motifNode.Attributes.GetNamedItem("symbole") != null)
                des.symbole = motifNode.Attributes["symbole"].InnerText;
            else
                des.symbole = string.Empty;

            if (motifNode.Attributes.GetNamedItem("ligne") != null)
                des.ligne = motifNode.Attributes["ligne"].InnerText;
            else
                des.ligne = string.Empty;

            if (motifNode.Attributes.GetNamedItem("hachure") != null)
                des.hachure = motifNode.Attributes["hachure"].InnerText;
            else
                des.hachure = string.Empty;

            if (motifNode.Attributes.GetNamedItem("epaisseur") != null)
                des.epaisseur = motifNode.Attributes["epaisseur"].InnerText;
            else
                des.epaisseur = string.Empty;

            if (motifNode.Attributes.GetNamedItem("echelle") != null)
                des.echelle = motifNode.Attributes["echelle"].InnerText;
            else
                des.echelle = string.Empty;

            if (motifNode.Attributes.GetNamedItem("angle") != null)
                des.angle = motifNode.Attributes["angle"].InnerText;
            else
                des.angle = string.Empty;
        }

        private static double Get_Max_Pm1()
        {
            double max_pm1 = 0;

            foreach (dessin des in destab)
            {
                if (double.Parse(des.pm1) > max_pm1)
                    max_pm1 = double.Parse(des.pm1);
            }

            return max_pm1;
        }

        private static double Get_Max_Pm2()
        {
            double max_pm2 = 0;

            foreach (dessin des in destab)
            {
                if (des.pm2 != string.Empty)
                    if (double.Parse(des.pm2) > max_pm2)
                        max_pm2 = double.Parse(des.pm2);
            }

            return max_pm2;
        }

        private static double Get_Max_Pm()
        {
            //On recupere le pm maximum
            double max_pm1 = Get_Max_Pm1();
            double max_pm2 = Get_Max_Pm2();
            double max_pm = max_pm1;

            if (max_pm2 > max_pm)
                max_pm = max_pm2;

            return max_pm;
        }

        
        //On convertit les horaires en coordonnées

        private static double Get_Y1(string h1)
        {
            double y1 = 0;
            section sect;

            if (h1 != String.Empty)
            {
                sect = (section)Section_Infos[h1];
                y1 = sect.Y * UNITE_Y;
            }

            return y1;
        }

        private static double Get_Y2(string h2, double y1, double x1, double x2)
        {
            double y2 = 0;
            section sect;

            if (h2 != string.Empty)
            {
                sect = (section)Section_Infos[h2];
                y2 = sect.Y * UNITE_Y;

                //Gestion de la taille minimun des desordres
                if (y2 == y1 & x1 == x2)
                    y2 += UNITE_Y;
            }
            else
            {
                y2 = y1;

                //Gestion de la taille minimun des desordres
                if (y2 == y1 & x1 == x2)
                    y2 += UNITE_Y;
            }

            return y2;
        }

        private static void Add_Section_Info(XmlNode nod, int y, bool is_first)
        {
            section sect = new section();
            sect.Y = y;
            sect.intitule = nod.InnerText;
            sect.heure = nod.Attributes["id"].InnerText;
                        
            if (nod.Attributes.GetNamedItem("type") != null)
                    sect.type = nod.Attributes["type"].InnerText;
                else
                    sect.type = string.Empty;

            if (is_first == true)
            {
                sect.type = "YMAX";
                Section_Infos.Add("YMAX", sect);
            }
            else
            {
                Section_Infos.Add(nod.Attributes["id"].InnerText, sect);
            }
        }

        private static void Get_Section_Infos()
        {
            XmlNodeList nodeList;
            XmlNode root;

            //section sect;
            //section first_sect = new section();
            
            root = mod_inspection.Section_Ouvrage_Xml.DocumentElement;

            string type = mod_inspection.TYPE_OUVRAGE;

            if (type == "BRANCHEMENT" | type == "TRONCON")
                type = "CANALISATION";

            nodeList = root.SelectNodes("section[@ouvrage='" + type + "' and @forme='" + mod_inspection.FORME_OUVRAGE + "' and @position='" + mod_inspection.POSITION_SECTION + "']/heure");

            Section_Infos = new Hashtable();

            int Y_MAX = 24;
            int primaire_pos = 0;

            //On cherche la generatrice primaire
            int hpos = 0;
            for (; hpos < nodeList.Count; hpos++)
                if (nodeList[hpos].Attributes.GetNamedItem("type") != null)
                    if (nodeList[hpos].Attributes["type"].InnerText == "generatrice_primaire")
                    {
                        //On stocke sa pos et on se place juste apres la generatrice primaire
                        primaire_pos = hpos;
                        hpos++;
                        break;
                    }

            //On dessine la symetrique de la generatrice primaire, qui sera à Y_MAX
            Add_Section_Info(nodeList[primaire_pos], Y_MAX, true);
            Y_MAX--;

            //On commence à dessiner les lignes en commencant par celle juste après le generatrice primaire
            for (; hpos < nodeList.Count; hpos++)
            {
                Add_Section_Info(nodeList[hpos], Y_MAX, false);
                Y_MAX--;
            }

            //Arrivé à la fin de la liste on reprend depuis le début jusqu'à la génératrice primaire
            for (hpos = 0; hpos <= primaire_pos; hpos++)
            {
                Add_Section_Info(nodeList[hpos], Y_MAX, false);
                Y_MAX--;
            }
        }

        public static void Launch_Autocad()
        {
            //object res;

            // "AutoCAD.Application.17" uses 2007 or 2008,
            //  whichever was most recently run

            // "AutoCAD.Application.17.1" uses 2008, specifically
            // "AutoCAD.Application.16.2" uses 2006
            // "AutoCAD.Application.18" uses 2010

            const string progID = "AutoCAD.Application.18";

            AcadApplication acApp = null;
            try
            {
                acApp = (AcadApplication)Marshal.GetActiveObject(progID);
            }
            catch
            {
                try
                {
                    Type acType = Type.GetTypeFromProgID(progID);
                    acApp = (AcadApplication)Activator.CreateInstance(acType, true);
                }
                catch
                {
                    MessageBox.Show("Cannot create object of type \"" +progID + "\"");
                }
            }
            if (acApp != null)
            {
                try
                {
                    // By the time this is reached AutoCAD is fully
                    // functional and can be interacted with through code
                    acApp.Visible = true;

                    ActivCAD.ActivCAD.IActivCAD app = (ActivCAD.ActivCAD.IActivCAD)acApp.GetInterfaceObject("ActivCAD.Commands");

                    if (app != null)
                    {

                        //---------------------------------------- On dessine le repère --------------------------------------------------
                        //----------------------------------------------------------------------------------------------------------------
                        Draw_Repere(app);
                        //----------------------------------------------------------------------------------------------------------------
                        //----------------------------------------------------------------------------------------------------------------
                        //New_Calc();
                        //---------------------------------------- On dessine les observations -------------------------------------------
                        //----------------------------------------------------------------------------------------------------------------
                        Draw_Observations(app);
                        //----------------------------------------------------------------------------------------------------------------
                        //----------------------------------------------------------------------------------------------------------------


                        // Let's generate the arguments to pass in:
                        // an integer and a double

                        //x1 / y1 / x2 / y2 / epaisseur / R / G / B / linetype / linefile
                        //Observations
                        //object[] args_poly1 = { 12, 34, 22, 43, 2, 255, 255, 0, "legende", "Continuous", Properties.Settings.Default.LignePath };
                        //object[] args_poly2 = { 37, 19, 42, 50, 1, 55, 255, 0, "legende", "Continuous", Properties.Settings.Default.LignePath };
                        //object[] args_poly3 = { 136, 70, 166, 52, 1, 55, 205, 0, "legende", "Continuous", Properties.Settings.Default.LignePath };

                        //Hachures
                        //object[] args_surf1 = { 69, 78, 47, 65, 120, 0.6, 252, 125, 252, "legende", "NET3", Properties.Settings.Default.HachurePath };

                        // Now let's call our method dynamically
                        //object res = app.GetType().InvokeMember("Init_Autocad_Environment", BindingFlags.InvokeMethod, null, app, null);
                        //res = app.GetType().InvokeMember("AjoutePolyline", BindingFlags.InvokeMethod, null, app, args_poly1);
                        //res = app.GetType().InvokeMember("AjoutePolyline", BindingFlags.InvokeMethod, null, app, args_poly2);
                        //res = app.GetType().InvokeMember("AjoutePolyline", BindingFlags.InvokeMethod, null, app, args_poly3);

                        //res = app.GetType().InvokeMember("AjouteSurface", BindingFlags.InvokeMethod, null, app, args_surf1);

                        //app.GetType().InvokeMember("MyPoly", BindingFlags.InvokeMethod, null, app, null);

                        acApp.ZoomExtents();
                        //MessageBox.Show("AddNumbers returned: " + res.ToString());
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Problem executing component: " + ex.Message);
                }
            }
        }

        public static void Draw_All_By_Forme()
        {
            string forme = destab[0].forme;
            dessin[] destmp = new dessin[destab.GetLength(0)];

            //On parcourt les observations
            foreach (dessin des in destab)
            {
                if (des.forme != forme)
                {
                    forme = des.forme;
                    destmp = new dessin[destab.GetLength(0)];
                }

                
            }
        }

        public static void Draw_Repere(ActivCAD.ActivCAD.IActivCAD app)
        {
            string res;
            double max_pm = Get_Max_Pm();
            section sect;

            //On calcule le nombre de ligne verticales
            double vertical_nbr = Math.Ceiling(max_pm);

            double max_pm_adapte = max_pm * UNITE_X;

            Get_Section_Infos();

            //Console.WriteLine("Get sections OK " + Section_Infos.Count);

            //On dessine les lignes horizontales
            foreach (object obj in Section_Infos.Values)
            {
                sect = (section)obj;

                //Console.WriteLine(sect.Y);
                //Console.WriteLine(sect.intitule);
                //Console.WriteLine(sect.heure);
                //Console.WriteLine(sect.type);

                if (sect.type != string.Empty)
                {
                    //object[] horizontal_params = { 0, sect.Y * UNITE_Y, 0, max_pm_adapte, sect.Y * UNITE_Y, 0, 252, 252, 252, "legende", "Continuous", Properties.Settings.Default.LignePath };
                    //res = app.GetType().InvokeMember("AjouteLigne", BindingFlags.InvokeMethod, null, app, horizontal_params);
                    //Console.WriteLine(sect.type);
                    if (sect.type == "generatrice_primaire")
                        res = app.AjouteLigne(0, sect.Y * UNITE_Y, 0, max_pm_adapte, sect.Y * UNITE_Y, 0, 51, 51, 51, "legende", "Continuous", Properties.Settings.Default.LignePath);
                    else if (sect.type == "generatrice_secondaire")
                        res = app.AjouteLigne(0, sect.Y * UNITE_Y, 0, max_pm_adapte, sect.Y * UNITE_Y, 0, 51, 51, 51, "legende", "ACAD_ISO07W100", Properties.Settings.Default.LignePath);
                    else if (sect.type == "ligne_milieu")
                        res = app.AjouteLigne(0, sect.Y * UNITE_Y, 0, max_pm_adapte, sect.Y * UNITE_Y, 0, 51, 51, 51, "legende", "CENTER2", Properties.Settings.Default.LignePath);
                    else
                        res = app.AjouteLigne(0, sect.Y * UNITE_Y, 0, max_pm_adapte, sect.Y * UNITE_Y, 0, 51, 51, 51, "legende", "Continuous", Properties.Settings.Default.LignePath);

                }

                //Legende de l axe vertical
                string legende = string.Empty;
                if (sect.intitule != string.Empty)
                    legende = sect.intitule + " - " + sect.heure;
                else
                    legende = sect.heure;

                //object[] legende_params = { -120, sect.Y * UNITE_Y, 0, legende, 7 };
                //res = app.GetType().InvokeMember("AjouteLegende", BindingFlags.InvokeMethod, null, app, legende_params);
                res = app.AjouteLegende(-120, sect.Y * UNITE_Y, 0, legende, 7);
            }
  

            sect = (section)Section_Infos["YMAX"];
            double vertical_line_YMAX = (sect.Y * UNITE_Y) + 10;

            //On dessine les lignes verticales
            double x = 0;
            for (int i = 0; i <= vertical_nbr; i++)
            {
                x = i * UNITE_X;
                if (i %5 == 0)
                {
                    //object[] vertical_params = { x, 0, 0, x, vertical_line_YMAX, 0, 252, 252, 252, "legende", "Continuous", Properties.Settings.Default.LignePath };
                    //res = app.GetType().InvokeMember("AjouteLigne", BindingFlags.InvokeMethod, null, app, vertical_params);
                    res = app.AjouteLigne(x, 0, 0, x, vertical_line_YMAX, 0, 51, 51, 51, "legende", "Continuous", Properties.Settings.Default.LignePath);
                    //object[] legende_params = { x, vertical_line_YMAX + 20, 0, i + " m", 7 };
                    //res = app.GetType().InvokeMember("AjouteLegende", BindingFlags.InvokeMethod, null, app, legende_params);
                    res = app.AjouteLegende(x, vertical_line_YMAX + 20, 0, i + " m", 7);
                }
                else
                {
                    //object[] vertical_params = { x, vertical_line_YMAX - 10, 0, x, vertical_line_YMAX, 0, 252, 252, 252, "legende", "Continuous", Properties.Settings.Default.LignePath };
                    //res = app.GetType().InvokeMember("AjouteLigne", BindingFlags.InvokeMethod, null, app, vertical_params);
                    res = app.AjouteLigne(x, vertical_line_YMAX - 10, 0, x, vertical_line_YMAX, 0, 51, 51, 51, "legende", "Continuous", Properties.Settings.Default.LignePath);
                }
            }
        }

        public static void Draw_Observations(ActivCAD.ActivCAD.IActivCAD app)
        {
            double x1 = 0;
            double x2 = 0;

            double y1 = 50;
            double y2 = 50;

            double epaisseur = 1;
            double echelle = 1;
            double angle = 0;

            //section sect;

            string res;

            //On parcourt les observations
            foreach (dessin des in destab)
            {
                x1 = double.Parse(des.pm1) * UNITE_X;
                y1 = Get_Y1(des.h1);

                byte R = byte.Parse(des.couleur.Split(Char.Parse("|"))[0]);
                byte G = byte.Parse(des.couleur.Split(Char.Parse("|"))[1]);
                byte B = byte.Parse(des.couleur.Split(Char.Parse("|"))[2]);

                if (des.type == "lineaires")
                {
                    if (des.pm2 == string.Empty & des.h2 == string.Empty)
                        return;

                    if (des.pm2 != string.Empty)
                    {
                        x2 = double.Parse(des.pm2) * UNITE_X;
                        y2 = Get_Y2(des.h2, y1, x1, x2);

                        if (des.epaisseur != null | des.epaisseur != string.Empty)
                            epaisseur = double.Parse(des.epaisseur);

                        //app.AddNewLineType(des.ligne, Properties.Settings.Default.LignePath);

                        //On récupère le point d'intersection de l'object et de l'axe Y=0
                        double new_x2 = Get_Cut_X(des.h1, des.h2, x1, y1, x2, y2);
                        if (new_x2 >= 0)
                        {
                            res = app.AjoutePolyline(x1, y1, new_x2, 0, epaisseur, R, G, B, "legende", des.ligne, Properties.Settings.Default.LignePath);
                            res = app.AjoutePolyline(new_x2, 24 * UNITE_Y, x2, y2, epaisseur, R, G, B, "legende", des.ligne, Properties.Settings.Default.LignePath);
                        }
                        else
                        {
                            res = app.AjoutePolyline(x1, y1, x2, y2, epaisseur, R, G, B, "legende", des.ligne, Properties.Settings.Default.LignePath);
                        }
                    }
                    else
                    {
                        x2 = x1;
                        y2 = Get_Y2(des.h2, y1, x1, x2);
                        res = app.AjoutePolyline(x1, y1, x2, y2, epaisseur, R, G, B, "legende", des.ligne, Properties.Settings.Default.LignePath);
                    }
                }
                else if (des.type == "surfaciques")
                {
                    //Surfacique
                    if (des.pm2 != string.Empty)
                    {
                        x2 = double.Parse(des.pm2) * UNITE_X;
                        y2 = Get_Y2(des.h2, y1 , x1, x2);

                        if (des.angle != null | des.angle != string.Empty)
                            angle = double.Parse(des.angle);

                        if (des.echelle != null | des.echelle != string.Empty)
                            echelle = double.Parse(des.echelle);

                        //Console.WriteLine(des.hachure);
                        //Console.WriteLine("surf angle : " + des.angle);
                        //Console.WriteLine("surf echelle : " + des.echelle);

                        //On verifie si l'objet est coupé ou pas
                        if (Is_Cut(des.h1, des.h2) == true)
                        {
                            //object[] observation_params_1 = { x1, y1, x2, 0, 35, 0.6, R, G, B, "legende", "NET3", Properties.Settings.Default.HachurePath };
                            //res = app.GetType().InvokeMember("AjouteSurface", BindingFlags.InvokeMethod, null, app, observation_params_1);

                            res = app.AjouteSurface(x1, y1, x2, 0, angle, echelle, R, G, B, "legende", des.hachure, Properties.Settings.Default.HachurePath);

                            //object[] observation_params_2 = { x1, 24 * UNITE_Y, x2, y2, 35, 0.6, R, G, B, "legende", "NET3", Properties.Settings.Default.HachurePath };
                            //res = app.GetType().InvokeMember("AjouteSurface", BindingFlags.InvokeMethod, null, app, observation_params_2);

                            res = app.AjouteSurface(x1, 24 * UNITE_Y, x2, y2, angle, echelle, R, G, B, "legende", des.hachure, Properties.Settings.Default.HachurePath);
                        }
                        else
                        {
                            //object[] observation_params = { x2, y2, x1, y1, 35, 0.6, R, G, B, "legende", "NET3", Properties.Settings.Default.HachurePath };
                            //res = app.GetType().InvokeMember("AjouteSurface", BindingFlags.InvokeMethod, null, app, observation_params);

                            res = app.AjouteSurface(x2, y2, x1, y1, angle, echelle, R, G, B, "legende", des.hachure, Properties.Settings.Default.HachurePath);
                        }

                    }
                }
                else if (des.type == "ponctuels")
                {
                    if (des.echelle != null | des.echelle != string.Empty)
                        echelle = double.Parse(des.echelle);

                    //Console.WriteLine("symbole : " + des.symbole);
                    //Console.WriteLine("ponc echelle : " + des.echelle);
                    //object[] observation_params = {x1, y1, 0, R, G, B, 0.8, "legende", "bloc2", Properties.Settings.Default.SymbolePath + "bloc.dwg" };
                    //app.GetType().InvokeMember("AjoutePonctuel", BindingFlags.InvokeMethod, null, app, observation_params);

                    app.AjoutePonctuel(x1, y1, 0, R, G, B, echelle, "legende", des.symbole, Properties.Settings.Default.SymbolePath + des.symbole + ".dwg");
                }
            }
        }

        public static void New_Calc(object app)
        {

        }

        private static double Get_Cut_X(string h1, string h2, double x1, double y1, double x2, double y2)
        {
            section sect1 = (section)Section_Infos[h1];
            section sect2 = (section)Section_Infos[h2];

            section sect = (section)Section_Infos["YMAX"];
            double YMAX = (sect.Y * UNITE_Y);
            double new_y2 = y2 - YMAX;

            double XMAX = Get_Max_Pm() * UNITE_X;

            if (sect1.Y - sect2.Y < 0)
            {
                double intersect_x = Get_Intersect_X(x1, y1, x2, new_y2, 0, 0, XMAX, 0);
                return intersect_x;
            }
            else
            {
                return -1;
            }
        }

        private static bool Is_Cut(string h1, string h2)
        {
            section sect1 = (section)Section_Infos[h1];
            section sect2 = (section)Section_Infos[h2];

            if (sect1.Y - sect2.Y < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static double Get_Intersect_X(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            double x;
            
            double denominateur = ((y4 - y3)*(x2 - x1)) - ((x4 - x3)*(y2 - y1));
            double nominateur_1 = ((x4 - x3)*(y1 - y3)) - ((y4 - y3)*(x1 - x3));
            double nominateur_2 = ((x2 - x1)*(y1 - y3)) - ((y2 - y1)*(x1 - x3));

            if (denominateur == 0 | nominateur_1 == 0 | nominateur_2 == 0)
                return -1;

            double ua = nominateur_1 / denominateur;
            double ub = nominateur_2 / denominateur;

            if(ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f)
            {
                // Get the intersection point.
                x = x1 + ua*(x2 - x1);
                //y = y1 + ua*(y2 - y1);
                return x;
            }

            return -1;
        }	
        
        
        public static void Show_result()
        {
            foreach (dessin des in destab)
            {
                Console.WriteLine("id : " + des.id);
                Console.WriteLine("type : " + des.type);
                Console.WriteLine("forme : " + des.forme);
                Console.WriteLine("pm1 : " + des.pm1);
                Console.WriteLine("max_pm1 : " + Get_Max_Pm1());
                Console.WriteLine("pm2 : " + des.pm2);
                Console.WriteLine("max_pm2 : " + Get_Max_Pm2());
                Console.WriteLine("h1 : " + des.h1);
                Console.WriteLine("h2 : " + des.h2);
                Console.WriteLine("couleur : " + des.couleur);
                Console.WriteLine("echelle : " + des.echelle);
                Console.WriteLine("epaisseur : " + des.epaisseur);
                Console.WriteLine("symbole : " + des.symbole);
                Console.WriteLine("ligne : " + des.ligne);
                Console.WriteLine("hachure : " + des.hachure);
                Console.WriteLine("legende : " + des.legende);
                Console.WriteLine("-------------------------------");
            }
        }
    }
}
