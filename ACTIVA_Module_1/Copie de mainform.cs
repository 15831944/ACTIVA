using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ACTIVA_Module_1.modules;
using ACTIVA_Module_1.component;


namespace ACTIVA_Module_1
{
    public partial class MainForm : Form
    {
        ArrayList _al = new ArrayList();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            //splash sp = new splash();
            //sp.ShowDialog();

            //mod_init.Data_Loading();
            mod_global.MF = (MainForm)Application.OpenForms["MainForm"];
            mod_init.App_Init();

            //on récupère la version courante de la compilation

            labelnumcompil.Text = "N° version : " + "1.0 beta";
            labeldateversion.Text = "Date de la version :" + "18 décembre 2009";
        }

        private void MainDockingTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainDockingTab.SelectedTab.Name == "InspectionTab" || MainDockingTab.SelectedTab.Name == "IdentificationTab" || MainDockingTab.SelectedTab.Name == "RenseignementTab")
            {
                MainSplit.Panel2Collapsed = false;
                mod_global.Disable_Obs_Tools();

                if (MainDockingTab.SelectedTab.Name == "IdentificationTab")
                    mod_identification.Fill_Id_Menu(IdentificationTopicBar, Identification_Flp);
            }
            else if (MainDockingTab.SelectedTab.Name == "ParamTab")
            {
                mod_global.Disable_Obs_Tools();
                MainSplit.Panel2Collapsed = true;
            }
            else
            {
                mod_observation.Fill_Observation_Grid(ObservationGrid);
                mod_observation.Fill_Code_Menu(CodeTopicBar);
                MainSplit.Panel2Collapsed = false;
                mod_global.Enable_Obs_Tools();

                if (mod_observation.OBS_GRID_EXPANDED == true & mod_observation.Collapsed_Rows.Count == 0)
                    mod_observation.Expand_Grid(ObservationGrid);
                else
                    mod_observation.Expand_Or_Collapse_Rows(ObservationGrid);

                mod_observation.Show_Last_Selected_Obs(ObservationGrid);
            }

            mod_global.Focused_Control = null;
            //SaisieTabControl.SelectedTab = SaisieTabControl.TabPages["KeyboardTab"];
            virtual_kb1.Set_Alpha_Numeric();

            if (MainDockingTab.SelectedTab.Name == "InspectionTab" || MainDockingTab.SelectedTab.Name == "ObservationTab" || MainDockingTab.SelectedTab.Name == "ParamTab")
            {
                mod_global.MF.MainSplit.Panel2Collapsed = true;
            }
            else
            {                   
                mod_global.MF.MainSplit.Panel2Collapsed = false;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            mod_global.center_control(virtual_kb1);
            mod_global.center_control(date_selector1);
        }

        private void IdentificationTopicBar_PageExpanded(object sender, C1.Win.C1Command.C1TopicBarPageEventArgs e)
        {
            //on vérifie avant si le flag de sauvegarde est mis et si la sauvegarde doit être proposé
            if (mod_identification.SaveIDFlag)
            {
                DialogResult rep = MessageBox.Show("Des champs de la fenêtre courante ont été modifiés, voulez vous enregistrer ?", "Enregistrement des modifications", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (rep == DialogResult.Yes)
                {
                    mod_save.Save_Identification_Panel(Identification_Flp);
                    //mise a jour de la couleur du groupe courant
                    mod_identification.Check_Fields_Status(IdentificationTopicBar.FindPage(IdFormLabel.Text));
                }
            }


            //on referme toutes les autres pages
            int nbpage = IdentificationTopicBar.Pages.Count;
            for (int i = 0; i < nbpage; i++)
            {
                if (i != e.Page.Index)
                    IdentificationTopicBar.Pages[i].Collapse();

            }

            IdFormLabel.Visible = true;
            IdFormLabel.Text = e.Page.Text;
            InputPreviewTb.Text = String.Empty;
            mod_identification.Fill_Identification_Form(e.Page.Tag.ToString(), Identification_Flp);

            SaisieTabControl.SelectedTab = SaisieTabControl.TabPages["KeyboardTab"];
            mod_identification.SaveIDFlag = false;
        }

        private void Type_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            mod_inspection.Check_Type_Ouvrage(cb_troncon, cb_branchement, cb_regard);
            mod_inspection.Fill_Ouvrage_List(OuvrageList);
        }

        private void InputPreviewTb_TextChanged(object sender, EventArgs e)
        {
            //--(GB) plante ici 
            if (mod_global.Focused_Control != null)
                mod_global.Focused_Control.Text = InputPreviewTb.Text;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mod_close.App_Close();
        }

        private void CodeTopicBar_LinkClick(object sender, C1.Win.C1Command.C1TopicBarClickEventArgs e)
        {
            mod_carac.Add_New_Code_Tabs(e.Link.Tag.ToString());
        }

        private void ObservationGrid_DoubleClick(object sender, EventArgs e)
        {
            if (ObservationGrid.ColSel == 0)
            {
                if (ObservationGrid.Rows[ObservationGrid.RowSel].Height == 24)
                    mod_observation.Expand_Line(ObservationGrid);
                else
                    mod_observation.Collapse_Line(ObservationGrid);
            }
            else
            {
                C1.Win.C1FlexGrid.Row SelRow = ObservationGrid.Rows[ObservationGrid.RowSel];
                mod_observation.last_selected_obs_num = int.Parse(SelRow["num"].ToString());
                mod_carac.Add_Existing_Code_Tab(SelRow["code"].ToString(), SelRow["num"].ToString(), false);
            }
        }

        private void ObservationGrid_Click(object sender, EventArgs e)
        {
            mod_observation.last_selected_obs_num = int.Parse(ObservationGrid[ObservationGrid.RowSel, "num"].ToString());
        }

        private void ObsCompactGridBt_Click(object sender, EventArgs e)
        {
            mod_observation.Collapse_Grid(ObservationGrid);
        }

        private void ObsDeplieGridBt_Click(object sender, EventArgs e)
        {
            mod_observation.Expand_Grid(ObservationGrid);
            //mod_observation.Fill_Observation_Grid(ObservationGrid);
        }

        private void ObservationGrid_Paint(object sender, PaintEventArgs e)
        {
            mod_observation.Resize_Observation_Column(ObservationGrid);
        }

        private void IdentificationValidBt_Click(object sender, EventArgs e)
        {
            mod_save.Save_Identification_Panel(Identification_Flp);

            //mise a jour de la couleur du groupe courant
            mod_identification.Check_Fields_Status(IdentificationTopicBar.FindPage(IdFormLabel.Text));

            mod_identification.SaveIDFlag = false;
        }

        private void NewInspectionPathTb_Click(object sender, EventArgs e)
        {
            DialogResult test = FolderDialog.ShowDialog();

            if (test == DialogResult.OK)
            {
                NewInspectionPathTb.Text = FolderDialog.SelectedPath;
            }
        }

        private void ReportImgTb_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.openIMGDialog.ShowDialog() == DialogResult.OK)
            {
                ReportImgTb.Text = openIMGDialog.FileName;
                ReportImgPb.ImageLocation = openIMGDialog.FileName;
                Properties.Settings.Default.ReportLogoPath = openIMGDialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void SaveReportParamBt_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReportHeaderText = ReportTxtTb.Text;
            Properties.Settings.Default.ReportLogoPath = openIMGDialog.FileName;
            Properties.Settings.Default.Save();
        }

        private void CloseAppBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseInspectionBt_Click(object sender, EventArgs e)
        {
            mod_inspection.Reset_Inspection_Tab();
        }

        private void NewInspectionNameTb_Click(object sender, EventArgs e)
        {
            mod_global.Focused_Control = (TextBox)sender;
        }

        private void MinAppBt_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MenuDockingTab_SelectedIndexChanging(object sender, C1.Win.C1Command.SelectedIndexChangingEventArgs e)
        {
            string oldtab = MenuDockingTab.SelectedTab.Name;
            string newtab = MenuDockingTab.TabPages[e.NewIndex].Name;

            if (newtab == "ReportTab")
            {
                OuvrageList.SelectionMode = C1.Win.C1List.SelectionModeEnum.CheckBox;
                mod_global.Disable_Main_Tabs();
                OuvrageList.ClearSelected();
                mod_inspection.FORME_OUVRAGE = String.Empty;
                mod_inspection.OUVRAGE = String.Empty;
                mod_inspection.TYPE_OUVRAGE = String.Empty;
            }

            if (oldtab == "ReportTab")
            {
                OuvrageList.SelectionMode = C1.Win.C1List.SelectionModeEnum.One;
                mod_global.Disable_Main_Tabs();
                OuvrageList.ClearSelected();
                mod_inspection.FORME_OUVRAGE = String.Empty;
                mod_inspection.OUVRAGE = String.Empty;
                mod_inspection.TYPE_OUVRAGE = String.Empty;
            }

        }

        private void ObservationGrid_AfterSort(object sender, C1.Win.C1FlexGrid.SortColEventArgs e)
        {
            mod_observation.last_sort_order = e.Order;
            mod_observation.last_sort_column = e.Col;
        }

        //click sur un lien d'une topicpage   
        private void IdentificationTopicBar_LinkClick(object sender, C1.Win.C1Command.C1TopicBarClickEventArgs e)
        {
            //on récupère le code dans le tag et on applique le focus sur composant
            mod_identification.FocusInputBoxParID(Identification_Flp, e.Link.Tag.ToString());

        }



        private void MainDockingTab_SelectedIndexChanging(object sender, C1.Win.C1Command.SelectedIndexChangingEventArgs e)
        {

            //si on quitte l'onglet identification
            if (MainDockingTab.SelectedTab.Name == "IdentificationTab")
            {

                //on vérifie avant si le flag de sauvegarde est mis et si la sauvegarde doit être proposée
                if (mod_identification.SaveIDFlag)
                {
                    DialogResult rep = MessageBox.Show("Des champs de la fenêtre courante ont été modifiés, voulez vous enregistrer ?", "Enregistrement des modifications", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (rep == DialogResult.Yes)
                    {
                        mod_save.Save_Identification_Panel(Identification_Flp);
                        //mise a jour de la couleur du groupe courant
                        mod_identification.Check_Fields_Status(IdentificationTopicBar.FindPage(IdFormLabel.Text));
                    }
                }
            }
        }

        private void OpenSVFButton_Click(object sender, EventArgs e)
        {
            if (openSVFDialog.ShowDialog() == DialogResult.OK)
            {
                openSVFTb.Text = openSVFDialog.FileName;
                mod_inspection.Load_SVF(openSVFDialog.FileName);
                mod_inspection.Check_Type_Ouvrage(cb_troncon, cb_branchement, cb_regard);
                mod_inspection.Fill_Ouvrage_List(OuvrageList);

                NewInspectionNameTb.Text = String.Empty;
                NewInspectionPathTb.Text = String.Empty;
                OuvrageNomTb.Text = String.Empty;
            }
        }

        private void NewInspectionBt_Click(object sender, EventArgs e)
        {
            mod_new.Create_New_Inspection(NewInspectionNameTb.Text, NewInspectionPathTb.Text);
        }

        private void NewOuvrageBt_Click(object sender, EventArgs e)
        {
            mod_new.Create_New_Ouvrage(OuvrageNomTb.Text, OuvrageTypeCb.SelectedItem, OuvrageFormeCb.SelectedValue.ToString());
        }

        private void OuvrageMoveDownBt_Click(object sender, EventArgs e)
        {
            if (cb_branchement.Checked == false | cb_regard.Checked == false | cb_troncon.Checked == false)
            {
                MessageBox.Show("Veuillez afficher tous les types d'ouvrages avant cette opération");
            }
            else
            {
                mod_inspection.Move_Ouvrage_Down(OuvrageList);
                this.SaveOuvrageOrderBt.Enabled = true;
            }
        }

        private void OuvrageMoveUpBt_Click(object sender, EventArgs e)
        {
            if (cb_branchement.Checked == false | cb_regard.Checked == false | cb_troncon.Checked == false)
            {
                MessageBox.Show("Veuillez afficher tous les types d'ouvrages avant cette opération");
            }
            else
            {
                mod_inspection.Move_Ouvrage_Up(OuvrageList);
                this.SaveOuvrageOrderBt.Enabled = true;
            }
        }

        private void CloneOuvrageBt_Click(object sender, EventArgs e)
        {
            if (OuvrageList.SelectedText != String.Empty & mod_inspection.OUVRAGE != String.Empty)
            {
                Ouvrage_Copy_Name frm = new Ouvrage_Copy_Name();
                frm.ShowDialog();
            }
        }

        private void DeleteOuvrageBt_Click(object sender, EventArgs e)
        {
            if (OuvrageList.SelectedText != String.Empty & mod_inspection.OUVRAGE != String.Empty)
            {
                if (MessageBox.Show("Confirmez vous la supression de l'ouvrage ?") == DialogResult.OK)
                    mod_new.Delete_Selected_Ouvrage();
            }
        }

        private void OuvrageTypeCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            mod_new.Fill_New_Ouvrage_Forme_Combo(OuvrageTypeCb.SelectedValue.ToString(), OuvrageFormeCb);
        }

        private void SaveReportParamBt_Click_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.ReportHeaderText = ReportTxtTb.Text;
            Properties.Settings.Default.ReportLogoPath = openIMGDialog.FileName;
            Properties.Settings.Default.Save();
        }

        private void GenerateReportBt_Click(object sender, EventArgs e)
        {
            if (OuvrageList.SelectedIndices.Count > 0)
            {
                mod_rapport.Create_Pdf_Document(c1PdfRapport, System.IO.Path.Combine(Application.StartupPath, "rapport.pdf"), IdCb.Checked, ObsCb.Checked, SyntCb.Checked);
            }
            else
            {
                MessageBox.Show("Aucun ouvrage n'est sélectionné!", "Impossible de générer un rapport", MessageBoxButtons.OK);
            }
        }

        private void AutocadGenerateBt_Click(object sender, EventArgs e)
        {
            ACTIVA_Module_1.autocad.read_svf.Get_All_Observations_From_SVF();
            //ACTIVA_Module_1.autocad.read_svf.Show_result();
            ACTIVA_Module_1.autocad.read_svf.Launch_Autocad();
        }

        private void OuvrageList_Click(object sender, EventArgs e)
        {
            if (OuvrageList.VisibleRows > 0 & OuvrageList.SelectedIndices.Count == 1)
            {
                mod_inspection.Get_Selected_Ouvrage_Info(OuvrageList.SelectedText, OuvrageList.Columns["Type"].CellText(OuvrageList.SelectedIndex), OuvrageList.Columns["Code forme"].CellText(OuvrageList.SelectedIndex), obs_name_label, obs_nb_label);
                mod_global.MF.OuvrageToolsPanel.Enabled = true;
                mod_global.MF.SaveOuvrageOrderBt.Enabled = false;
                mod_global.MF.OuvrageMoveDownBt.Enabled = true;
                mod_global.MF.OuvrageMoveUpBt.Enabled = true;
                mod_global.MF.CloneOuvrageBt.Enabled = true;
                mod_global.MF.DeleteOuvrageBt.Enabled = true;
            }
        }

        private void OuvrageNomTb_Click(object sender, EventArgs e)
        {
            mod_global.Focused_Control = (TextBox)sender;
        }

        private void SaveOuvrageOrderBt_Click(object sender, EventArgs e)
        {
            mod_inspection.Save_Ouvrage_Order(OuvrageList);
            this.SaveOuvrageOrderBt.Enabled = false;
        }

        private void EraseAllBt_Click(object sender, EventArgs e)
        {
            if (mod_global.Focused_Control != null)
                mod_global.Focused_Control.Text = string.Empty;
        }

        private void Folder_New_Inspection_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                NewInspectionPathTb.Text = fbd.SelectedPath;
            }
        }

        private void Btn_Exporter_Click(object sender, EventArgs e)
        {
            mod_new.Exporter_XML(System.IO.Path.GetFileName(SVFLabel.Text), SVFLabel.Text);
        }



    }
}