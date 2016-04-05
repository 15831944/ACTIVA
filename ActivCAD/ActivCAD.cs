using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using System.Runtime.InteropServices;
using System.EnterpriseServices;

namespace ActivCAD
{
    public class ActivCAD
    {
        //[Guid("AD4B5F57-16B8-4311-8401-BA98690E94B3")]
        public interface IActivCAD
        {
            [DispId(1)]
            string AjouteLigne(double startx, double starty, double startz, double endx, double endy, double endz, byte R, byte G, byte B, string legende, string linetype, string linfilepath);
            [DispId(2)]
            string AjoutePolyline(double startx, double starty, double endx, double endy, double epaisseur, byte R, byte G, byte B, string legende, string linetype, string linfilepath);
            [DispId(3)]
            string AjouteSurface(double upperx, double uppery, double lowerx, double lowery, double angle, double echelle, byte R, byte G, byte B, string legende, string hachuretype, string hachfilepath);
            [DispId(4)]
            void AjoutePonctuel(double pointx, double pointy, double pointz, byte R, byte G, byte B, double echelle, string legende, string symbole, string symbolepath);
            [DispId(5)]
            string AjouteLegende(double pointx, double pointy, double pointz, string text, double textsize);
            [DispId(6)]
            string AjouteCalc();
            //[DispId(7)]
            //void AddNewLineType(string linetype, string linfilepath);
        }

        [ProgId("ActivCAD.Commands"), ClassInterface(ClassInterfaceType.None)]
        public class Commands : ServicedComponent, IActivCAD
        {
            Database db;
            Document doc;

            public const int UNITE_Y = 10;
            public const int UNITE_X = 10;

            public void Init_Autocad_Environment()
            {
                //db = HostApplicationServices.WorkingDatabase;
                //doc = Application.DocumentManager.GetDocument(db);
            }

            public string AjouteLigne(double startx, double starty, double startz, double endx, double endy, double endz, byte R, byte G, byte B, string legende, string linetype, string linfilepath)
            {
                //db = HostApplicationServices.WorkingDatabase;
                //doc = Application.DocumentManager.GetDocument(db);

                doc = Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;

                try
                {
                    DocumentLock loc = doc.LockDocument();
                    using (loc)
                    {
                        using (Transaction transaction = db.TransactionManager.StartTransaction())
                        {
                            Line ligne = new Line(new Point3d(startx, starty, startz), new Point3d(endx, endy, endz));

                            ligne.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(R, G, B);

                            //if (AddNewLineType(linetype, linfilepath, transaction))
                                //ligne.Linetype = linetype;
                            LinetypeTable lt = (LinetypeTable)transaction.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                            if (!lt.Has(linetype))
                                db.LoadLineTypeFile(linetype, linfilepath);

                            ligne.Linetype = linetype;

                            BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                            btr.AppendEntity(ligne);

                            transaction.AddNewlyCreatedDBObject(ligne, true);

                            transaction.Commit();

                            return "c bon";
                        }
                    }

                }
                catch (Exception excp) { return "c pas bon /" + excp.Message + "/" + excp.ErrorStatus.ToString(); }
            }

            public string AjoutePolyline(double startx, double starty, double endx, double endy, double epaisseur, byte R, byte G, byte B, string legende, string linetype, string linfilepath)
            {
                //db = HostApplicationServices.WorkingDatabase;
                //doc = Application.DocumentManager.GetDocument(db);

                doc = Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;

                try
                {
                    DocumentLock loc = doc.LockDocument();
                    using (loc)
                    {
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            //doc.Editor.WriteMessage("ok 0");

                            LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                            if (!lt.Has(linetype))
                                db.LoadLineTypeFile(linetype, linfilepath);

                            Polyline p = new Polyline();
                            p.AddVertexAt(p.NumberOfVertices, new Point2d(startx, starty), 0, epaisseur, epaisseur);
                            p.AddVertexAt(p.NumberOfVertices, new Point2d(endx, endy), 0, epaisseur, epaisseur);
                            
                            p.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(R, G, B);

                            //doc.Editor.WriteMessage("ok 1");

                            //if (AddNewLineType(linetype, linfilepath, tr))
                                p.Linetype = linetype;

                            //doc.Editor.WriteMessage("ok 2");

                            if (startx - endx < UNITE_X)
                                endx = startx + UNITE_X;

                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                            btr.AppendEntity(p);
                            tr.AddNewlyCreatedDBObject(p, true);

                            //doc.Editor.WriteMessage("ok 3");

                            tr.Commit();

                            return "c bon";
                        }
                    }
                }
                catch (Exception excp) { return "c pas bon /" + excp.Message + "/" + excp.ErrorStatus.ToString(); }
            }

            public string AjouteSurface(double upperx, double uppery, double lowerx, double lowery, double angle, double echelle, byte R, byte G, byte B, string legende, string hachuretype, string hachfilepath)
            {
                //db = HostApplicationServices.WorkingDatabase;
                //doc = Application.DocumentManager.GetDocument(db);

                doc = Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;

                try
                {
                    DocumentLock loc = doc.LockDocument();
                    using (loc)
                    {
                        using (Transaction transaction = db.TransactionManager.StartTransaction())
                        {
                            Hatch hatch = new Hatch { Color = Autodesk.AutoCAD.Colors.Color.FromRgb(R, G, B) };
                            hatch.SetHatchPattern(HatchPatternType.PreDefined, hachuretype);
                            hatch.HatchObjectType = HatchObjectType.HatchObject;

                            hatch.PatternAngle = angle;
                            hatch.PatternScale = echelle;

                            //hatch.BlockName = "legende";
                            //hatch.

                            //}

                            //{ ajoute la définition de la hachure au modelspace
                            BlockTable bt = (BlockTable)transaction.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);

                            BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                            ObjectId hatId = btr.AppendEntity(hatch);

                            transaction.AddNewlyCreatedDBObject(hatch, true);
                            //}

                            //{ ajoute la boucle de hachure
                            hatch.Associative = true;

                            HatchLoop loop = new HatchLoop(HatchLoopTypes.Polyline);
                            loop.Polyline.Add(new BulgeVertex(new Point2d(upperx, lowery), 0));
                            loop.Polyline.Add(new BulgeVertex(new Point2d(upperx, uppery), 0));
                            loop.Polyline.Add(new BulgeVertex(new Point2d(lowerx, uppery), 0));
                            loop.Polyline.Add(new BulgeVertex(new Point2d(lowerx, lowery), 0));
                            //pour fermer la polyline, on rajoute le premier point 
                            loop.Polyline.Add(new BulgeVertex(new Point2d(upperx, lowery), 0));

                            hatch.AppendLoop(loop);

                            hatch.EvaluateHatch(true);

                            transaction.Commit();

                            return "c bon";
                            //}
                        }
                    }

                }
                catch (Exception excp) { return "c pas bon /" + excp.Message + "/" + excp.ErrorStatus.ToString(); }
            }

            public void AjoutePonctuel(double pointx, double pointy, double pointz, byte R, byte G, byte B, double echelle, string legende, string symbole, string symbolepath)
            {
                //db = HostApplicationServices.WorkingDatabase;
                //doc = Application.DocumentManager.GetDocument(db);

                doc = Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;

                try
                {
                    DocumentLock loc = doc.LockDocument();
                    using (loc)
                    {
                        using (Transaction transaction = db.TransactionManager.StartTransaction())
                        {
                            //transaction.AutoDelete = true;

                            ObjectId obj = db.AttachXref(symbolepath, symbole);

                            Point3d emplacement = new Point3d(pointx, pointy, pointz);

                            BlockReference block = new BlockReference(emplacement, obj);

                            block.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(R, G, B);
                            block.ScaleFactors = new Scale3d(echelle);

                            BlockTable bt = (BlockTable)transaction.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                            BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                            //BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                            btr.AppendEntity(block);
                            transaction.AddNewlyCreatedDBObject(block, true);
                            transaction.Commit();
                           

                           //return "c bon";
                        }
                    }
                }
                catch (Exception excp) {
                    //string tmp = e.Message;
                    //doc.Editor.WriteMessage(e + "\n");
                    //transaction.Abort();
                    //return "c pas bon /" + excp.Message + "/" + excp.ErrorStatus.ToString(); 
                }
            }

            public string AjouteLegende(double pointx, double pointy, double pointz, string text, double textsize)
            {
                //db = HostApplicationServices.WorkingDatabase;
                //doc = Application.DocumentManager.GetDocument(db);

                doc = Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;

                try
                {
                    DocumentLock loc = doc.LockDocument();
                    using (loc)
                    {
                        using (Transaction transaction = db.TransactionManager.StartTransaction())
                        {
                            MText mtext = new MText();
                            mtext.Location = new Point3d(pointx, pointy, pointz);
                            mtext.Contents = text;
                            mtext.TextHeight = textsize;
                            mtext.Attachment = AttachmentPoint.MiddleCenter;

                            BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                            btr.AppendEntity(mtext);
                            transaction.AddNewlyCreatedDBObject(mtext, true);

                            transaction.Commit();

                            return "c bon";
                        }
                    }
                }
                catch (Exception excp) { return "c pas bon /" + excp.Message + "/" + excp.ErrorStatus.ToString(); }
            }

            public string AjouteCalc()
            {
                //db = HostApplicationServices.WorkingDatabase;
                //doc = Application.DocumentManager.GetDocument(db);

                doc = Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;

                try
                {
                    DocumentLock loc = doc.LockDocument();
                    using (loc)
                    {
                        //doc.
                        //plane = new Plane(m_poly.Ecs.CoordinateSystem3d.Origin, m_poly.Ecs.CoordinateSystem3d.Zaxis);

                        return "c bon";
                    }
                }
                catch (Exception excp) { return "c pas bon /" + excp.Message + "/" + excp.ErrorStatus.ToString(); }
            }

            public void AddNewLineType(string linetype, string linfilepath)
            {
                try
                {
                    //ajoute la ligne si elle n'existe pas déjà
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                        if (!lt.Has(linetype))
                            db.LoadLineTypeFile(linetype, linfilepath);
                        tr.Commit();
                    }
                    //return true;
                }
                catch (Exception excp) {  }
            }

        }
    }
}
