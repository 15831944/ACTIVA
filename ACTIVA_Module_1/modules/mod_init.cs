using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ACTIVA_Module_1.modules
{
    class mod_init
    {
        public static void Set_Xml_Path()
        {
            if (Properties.Settings.Default.CodeIdCanaPath == String.Empty)
                Properties.Settings.Default.CodeIdCanaPath = Path.Combine(Application.StartupPath, "xml/Codes_Identification_Canalisation.xml");
            if (Properties.Settings.Default.CodeIdRegPath == String.Empty)
                Properties.Settings.Default.CodeIdRegPath = Path.Combine(Application.StartupPath, "xml/Codes_Identification_Regard.xml");

            if (Properties.Settings.Default.CodeObsCanaPath == String.Empty)
                Properties.Settings.Default.CodeObsCanaPath = Path.Combine(Application.StartupPath, "xml/Codes_Observation_Canalisation.xml");
            if (Properties.Settings.Default.CodeObsRegPath == String.Empty)
                Properties.Settings.Default.CodeObsRegPath = Path.Combine(Application.StartupPath, "xml/Codes_Observation_Regard.xml");

            if (Properties.Settings.Default.FamilleCodePath == String.Empty)
                Properties.Settings.Default.FamilleCodePath = Path.Combine(Application.StartupPath, "xml/Familles_Codes.xml");
            if (Properties.Settings.Default.GroupCodeIdPath == String.Empty)
                Properties.Settings.Default.GroupCodeIdPath = Path.Combine(Application.StartupPath, "xml/Groupes_Codes_Identification.xml");

            if (Properties.Settings.Default.SectionOuvragePath == String.Empty)
                Properties.Settings.Default.SectionOuvragePath = Path.Combine(Application.StartupPath, "xml/Section_Ouvrage.xml");
            if (Properties.Settings.Default.SectionImageDirPath == String.Empty)
                Properties.Settings.Default.SectionImageDirPath = Path.Combine(Application.StartupPath, "img/");

            if (Properties.Settings.Default.MotifPath == String.Empty)
                Properties.Settings.Default.MotifPath = Path.Combine(Application.StartupPath, "xml/Motifs.xml");

            if (Properties.Settings.Default.LignePath == String.Empty)
                Properties.Settings.Default.LignePath = Path.Combine(Application.StartupPath, "autocad/acadiso.lin");
            if (Properties.Settings.Default.HachurePath == String.Empty)
                Properties.Settings.Default.HachurePath = Path.Combine(Application.StartupPath, "autocad/acadiso.pat");
            if (Properties.Settings.Default.SymbolePath == String.Empty)
                Properties.Settings.Default.SymbolePath = Path.Combine(Application.StartupPath, "autocad/blocs/");

            Properties.Settings.Default.Save();
        }

        public static void Data_Loading()
        {
            Set_Xml_Path();
            mod_identification.Load_Groupe_Codes_Id(Properties.Settings.Default.GroupCodeIdPath);
            Thread.Sleep(500);
            Application.DoEvents();
            mod_identification.Load_Codes_Id_Cana(Properties.Settings.Default.CodeIdCanaPath);
            Thread.Sleep(500);
            Application.DoEvents();
            mod_identification.Load_Codes_Id_Regard(Properties.Settings.Default.CodeIdRegPath);
            Thread.Sleep(500);
            Application.DoEvents();
            mod_observation.Load_Famille_Codes_Obs(Properties.Settings.Default.FamilleCodePath);
            Thread.Sleep(500);
            Application.DoEvents();
            mod_observation.Load_Codes_Obs_Cana(Properties.Settings.Default.CodeObsCanaPath);
            Thread.Sleep(500);
            Application.DoEvents();
            mod_observation.Load_Codes_Obs_Regard(Properties.Settings.Default.CodeObsRegPath);
            Application.DoEvents();
            mod_accueil.Load_Section_Ouvrage(Properties.Settings.Default.SectionOuvragePath);
            Application.DoEvents();
            mod_accueil.Load_Motif(Properties.Settings.Default.MotifPath);
            Application.DoEvents();


        }

        public static void App_Init()
        {
            //----------------------------------------------------------------------------------------------
            mod_global.center_control(mod_global.MF.virtual_kb1);
            mod_global.center_control(mod_global.MF.date_selector1);
            //
            mod_observation.Init_Grid(mod_global.MF.ObservationGrid);

            mod_param_obs.Init_Obs_Button_Tag_n_Event();
            mod_param_id.Init_Id_Button_Tag_n_Event();
            mod_param_section.Init_Section_Button_Tag_n_Event();

            //Initialisation des parametres
            mod_param_autocad.Init_Autocad_Grid(mod_global.MF.XmlAutocadGrid);
            mod_param_autocad.Init_Autocad_Item_Grid(mod_global.MF.XmlAutocadItemGrid);
            mod_param_autocad.Init_Autocad_Button_Tag_n_Event();

            mod_param_motif.Init_Motif_Ponctuel_Grid(mod_global.MF.MotifPonctuelGrid);
            mod_param_motif.Init_Motif_Lineaire_Grid(mod_global.MF.MotifLineaireGrid);
            mod_param_motif.Init_Motif_Surfacique_Grid(mod_global.MF.MotifSurfaciqueGrid);

            mod_param_motif.Fill_Motif_Ponctuel_Grid(mod_global.MF.MotifPonctuelGrid, mod_accueil.Motif_Xml);
            mod_param_motif.Fill_Motif_Lineaire_Grid(mod_global.MF.MotifLineaireGrid, mod_accueil.Motif_Xml);
            mod_param_motif.Fill_Motif_Surfacique_Grid(mod_global.MF.MotifSurfaciqueGrid, mod_accueil.Motif_Xml);

            mod_param_motif.Init_Motif_Buttons_Tags_n_Events();

            mod_param_obs.Init_Obs_Codes_Grid(mod_global.MF.XmlObsCodesGrid);
            mod_param_obs.Init_Obs_Codelie_Grid(mod_global.MF.XmlObsCodeLieGrid);
            mod_param_id.Init_Id_Codes_Grid(mod_global.MF.XmlIdCodeGrid);
            mod_param_section.Init_Section_Grid(mod_global.MF.XmlSectionGrid);
            mod_param_section.Fill_Section_Grid(mod_global.MF.XmlSectionGrid, mod_accueil.Section_Ouvrage_Xml);
            mod_param_section.Init_Heure_Grid(mod_global.MF.XmlHeureGrid);
            mod_param_path.Init_Path_Button_Tag_n_Event();

            //mod_param.Fill_Codes_Grid(mod_global.MF.XmlCodesGrid);
            mod_param_obs.Init_Obs_Carac_Grid(mod_global.MF.XmlObsCaracGrid);
            mod_param_obs.Init_Obs_Items_Grid(mod_global.MF.XmlObsItemGrid);
            mod_param_id.Init_Id_Items_Grid(mod_global.MF.XmlIdItemGrid);

            //On colorie la légende de l'onglet identification
            mod_global.MF.IdObligatoirePb.BackColor = mod_global.Obligatoire_Color;
            mod_global.MF.IdDifferePb.BackColor = mod_global.Differe_Color;
            mod_global.MF.IdDesactivePb.BackColor = mod_global.Desactive_Color;
            mod_global.MF.IdFacultatifPb.BackColor = mod_global.Facultatif_Color;

            mod_global.MF.InspectObligatoirePb.BackColor = mod_global.Obligatoire_Color;
            mod_global.MF.InspectDifferePb.BackColor = mod_global.Differe_Color;
            mod_global.MF.InspectDesactivePb.BackColor = mod_global.Desactive_Color;
            mod_global.MF.InspectFacultatifPb.BackColor = mod_global.Facultatif_Color;
                        
            mod_global.Disable_Obs_Tools();
            mod_global.Disable_Main_Tabs();
            mod_global.Disable_Ouvrage_Controls();

            mod_global.MF.ReportImgTb.Text = Properties.Settings.Default.ReportLogoPath;
            mod_global.MF.ReportTxtTb.Text = Properties.Settings.Default.ReportHeaderText;

            if (Properties.Settings.Default.ReportLogoPath != string.Empty)
                mod_global.MF.ReportImgPb.ImageLocation = Properties.Settings.Default.ReportLogoPath;
            else
                mod_global.MF.ReportImgPb.Image = Properties.Resources.Activa_Default_Logo;

            mod_new.Fill_New_Ouvrage_Type_Combo(mod_global.MF.OuvrageTypeCb);

            //--------------------------------------------------------------------------------------------

            mod_global.MF.XmlIdCanaTb.Text = Properties.Settings.Default.CodeIdCanaPath;
            mod_global.MF.XmlIdRegTb.Text = Properties.Settings.Default.CodeIdRegPath;

            mod_global.MF.XmlObsCanaTb.Text = Properties.Settings.Default.CodeObsCanaPath;
            mod_global.MF.XmlObsRegTb.Text = Properties.Settings.Default.CodeObsRegPath;

            mod_global.MF.XmlFamilleCodeIdTb.Text = Properties.Settings.Default.FamilleCodePath;
            mod_global.MF.XmlGroupCodeIdTb.Text = Properties.Settings.Default.GroupCodeIdPath;

            mod_global.MF.XmlSectionOuvrageTb.Text = Properties.Settings.Default.SectionOuvragePath;
            mod_global.MF.XmlSectionImgDirTb.Text = Properties.Settings.Default.SectionImageDirPath;

            mod_global.MF.XmlMotifTb.Text = Properties.Settings.Default.MotifPath;

            mod_global.MF.FileLigneTb.Text = Properties.Settings.Default.LignePath;
            mod_global.MF.FileHachureTb.Text = Properties.Settings.Default.HachurePath;
            mod_global.MF.FileSymboleTb.Text = Properties.Settings.Default.SymbolePath;

            mod_global.MF.OuvrageToolsPanel.Enabled = false;
            mod_global.MF.MainSplit.Panel2Collapsed = true;

            mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = false;
            mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
        }
    }
}
