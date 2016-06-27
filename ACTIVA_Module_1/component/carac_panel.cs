using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Collections;
using ACTIVA_Module_1.modules;
using System.Globalization;

namespace ACTIVA_Module_1.component
{
    public partial class carac_panel : UserControl
    {
        public Hashtable CaracNameToTb = new Hashtable();
        Hashtable CaracNameToLb = new Hashtable();

        public Hashtable CaracFieldsNewState;

        string current_code;
        string current_forme;
        string current_num;
        int TbCount;
      

        //TextBox LastColoredTb;

        public carac_panel(string code, string num, bool is_clone)
        {
            current_code = code;

            //Si ce code est un clone d'un autre, on vide son code num pour ne pas qu'il soit considere comme un code existant à la sauvegarde
            if (is_clone == true)
                current_num = string.Empty;
            else
                current_num = num;

            InitializeComponent();
            Fill_CaracNameToTb();
            Fill_CaracNameToLb();
            Init_Carac_Panel(code, num);


            //Si le type de l'ouvrage n'est pas un REGARD on masque le champ posregard
            if (mod_accueil.TYPE_OUVRAGE != "REGARD")
            {
                PosRegCb.Visible = false;
            }

            //Dans le cas d'une modification de code, l'utilisateur ne peut pas modifier le pm1
            //if (current_num != string.Empty)
            //{
            //    TextBox pm1 = (TextBox)CaracNameToTb["pm1"];
            //    pm1.Enabled = false;
            //}





            /*
             *             XmlNodeList CodeLieNodeList;

            XmlNode root = mod_global.Get_Codes_Obs_DocElement();
            CodeLieNodeList = root.SelectNodes("/codes/code[id='" + code + "']/lien/codelie");
            if (CodeLieNodeList.Count > 0)
                component.carac_panel.CaracValidnCloseBt.Enabled = false;
            else component.carac_panel.CaracValidnCloseBt.Enabled = true;
             */
            if (num != String.Empty)
            {
                Retreive_Carac(num);
                XmlNode codenod = mod_accueil.SVF.DocumentElement.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/observations/code[@num='" + num + "']");
                current_forme = codenod.Attributes["forme"].InnerText;
            }
            else
            {
                current_forme = mod_accueil.FORME_OUVRAGE;
            }

        }

        //-------------------------------------------------------------------

        private void Fill_CaracNameToTb()
        {
            CaracNameToTb.Add("c1",Carac1Tb);
            CaracNameToTb.Add("c2", Carac2Tb);
            CaracNameToTb.Add("q1", Quant1Tb);
            CaracNameToTb.Add("q2", Quant2Tb);
            CaracNameToTb.Add("h1", EmpCirc1Tb);
            CaracNameToTb.Add("h2", EmpCirc2Tb);
            CaracNameToTb.Add("pm1", EmpLong1Tb);
            CaracNameToTb.Add("pm2", EmpLong2Tb);
            CaracNameToTb.Add("assemblage", AssemblageCb);
            CaracNameToTb.Add("posregard", PosRegCb);
            CaracNameToTb.Add("video", RefVideoTb);
            CaracNameToTb.Add("photo", RefPhotoTb);
            CaracNameToTb.Add("audio", RefAudioTb);
            CaracNameToTb.Add("remarques", RemarqueTb);
        }

        private void Fill_CaracNameToLb()
        {
            CaracNameToLb.Add("c1", Carac1Lb);
            CaracNameToLb.Add("c2", Carac2Lb);
            CaracNameToLb.Add("q1", Quant1Lb);
            CaracNameToLb.Add("q2", Quant2Lb);
        }

        private Hashtable Get_CaracFieldsNewState_HT(XPathNavigator item)
        {
            CaracFieldsNewState = new Hashtable();
            CaracFieldsNewState.Add("q1", item.GetAttribute("q1",""));
            CaracFieldsNewState.Add("q2", item.GetAttribute("q2",""));
            CaracFieldsNewState.Add("h1", item.GetAttribute("h1",""));
            CaracFieldsNewState.Add("h2", item.GetAttribute("h2",""));
            CaracFieldsNewState.Add("pm1", item.GetAttribute("pm1",""));
            CaracFieldsNewState.Add("pm2", item.GetAttribute("pm2",""));
            return CaracFieldsNewState;
        }

        //-------------------------------------------------------------------

        private void Init_Carac_Panel(string code, string num)
        {
            if (num == string.Empty)
                Set_Fields_Color_By_CONF_File(code);
            else
                Set_Fields_Color_By_SVF_File(num);

            Set_Fields_Label_By_CONF_File(code);

            Set_Fields_Events();

            ObligatoirePb.BackColor = mod_global.Obligatoire_Color;
            DifferePb.BackColor = mod_global.Differe_Color;
            DesactivePb.BackColor = mod_global.Desactive_Color;
            FacultatifPb.BackColor = mod_global.Facultatif_Color;
        }

        private void Set_Fields_Color_By_CONF_File(string code)
        {
            XmlNode root;
            XmlNodeList CaracNodeList;

            root = mod_global.Get_Codes_Obs_DocElement();
            CaracNodeList = root.SelectNodes("/codes/code[id='" + code + "']/caracteristiques/caracteristique");

            //On remplace les label de caracterisation et de quantification par la valeur de l'attribut info s'il est mentionné dans le XML
            TextBox CText;
            ComboBox CCombo;
            CheckBox CCheck;

            foreach (XmlNode unNode in CaracNodeList)
            {
                Console.WriteLine(unNode.Attributes["nom"].InnerText.ToLower());

                if (unNode.Attributes["nom"].InnerText.ToLower() == "assemblage")
                {
                    CCheck = (CheckBox)CaracNameToTb[unNode.Attributes["nom"].InnerText.ToLower()];

                    //Si le champs doit etre desactive on le traite avant
                    if (unNode.Attributes["renseigne"].InnerText == "4")
                    {
                        CCheck.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                        CCheck.Enabled = false;
                    }
                    else
                    {
                        //Si le champs est obligatoire ou différé on met sa couleur de fond
                        CCheck.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                    }
                }
                else if (unNode.Attributes["nom"].InnerText.ToLower() == "posregard")
                {
                    CCombo = (ComboBox)CaracNameToTb[unNode.Attributes["nom"].InnerText.ToLower()];

                    //Si le champs doit etre desactive on le traite avant
                    if (unNode.Attributes["renseigne"].InnerText == "4")
                    {
                        CCombo.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                        CCombo.Enabled = false;
                    }
                    else
                    {
                        //Si le champs est obligatoire ou différé on met sa couleur de fond
                        CCombo.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                    }
                }
                else
                {
                    CText = (TextBox)CaracNameToTb[unNode.Attributes["nom"].InnerText.ToLower()];

                    //Si le champs doit etre desactive on le traite avant
                    if (unNode.Attributes["renseigne"].InnerText == "4")
                    {
                        CText.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                        CText.Enabled = false;
                    }
                    else
                    {
                        //Si le champs est obligatoire ou différé on met sa couleur de fond
                        CText.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                    }
                }
            }  
        }

        private void Set_Fields_Label_By_CONF_File(string code)
        {
            XmlNode root;
            XmlNodeList CaracNodeList;

            root = mod_global.Get_Codes_Obs_DocElement();
            CaracNodeList = root.SelectNodes("/codes/code[id='" + code + "']/caracteristiques/caracteristique");

            //On remplace les label de caracterisation et de quantification par la valeur de l'attribut info s'il est mentionné dans le XML
            Label CLab;
            foreach (XmlNode unNode in CaracNodeList)
            {
                if (unNode.Attributes["nom"].InnerText.ToLower() != "assemblage" & unNode.Attributes["nom"].InnerText.ToLower() != "posregard")
                {
                        if (unNode.Attributes.GetNamedItem("intitule") != null)
                        {
                            if (CaracNameToLb.ContainsKey(unNode.Attributes["nom"].InnerText.ToLower()))
                            {
                                CLab = (Label)CaracNameToLb[unNode.Attributes["nom"].InnerText.ToLower()];
                                CLab.Text = unNode.Attributes["intitule"].InnerText;

                                if (unNode.Attributes.GetNamedItem("unite") != null)
                                    CLab.Text += " (" + unNode.Attributes["unite"].InnerText + ")";
                            }
                        }
                    }
                }
        }

        private void Set_Fields_Color_By_SVF_File(string num)
        {
            XmlNode root;
            XmlNodeList CaracNodeList;

            root = mod_accueil.SVF.DocumentElement;
            CaracNodeList = root.SelectNodes("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/observations/code[@num='" + num + "']/caracteristiques/caracteristique");

            //On remplace les label de caracterisation et de quantification par la valeur de l'attribut info s'il est mentionné dans le XML
            TextBox CText;
            CheckBox CCheck;
            ComboBox CCombo;
            foreach (XmlNode unNode in CaracNodeList)
            {
                if (unNode.Attributes["nom"].InnerText.ToLower() == "assemblage")
                {
                    CCheck = (CheckBox)CaracNameToTb[unNode.Attributes["nom"].InnerText];

                    //Si le champs doit etre desactive on le traite avant
                    if (unNode.Attributes["renseigne"].InnerText == "4")
                    {
                        CCheck.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                        CCheck.Enabled = false;
                    }
                    else
                    {
                        //Si le champs est obligatoire ou différé on met sa couleur de fond
                        CCheck.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                    }
                }
                else if (unNode.Attributes["nom"].InnerText.ToLower() == "posregard")
                {
                    CCombo = (ComboBox)CaracNameToTb[unNode.Attributes["nom"].InnerText];

                    //Si le champs doit etre desactive on le traite avant
                    if (unNode.Attributes["renseigne"].InnerText == "4")
                    {
                        CCombo.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                        CCombo.Enabled = false;
                    }
                    else
                    {
                        //Si le champs est obligatoire ou différé on met sa couleur de fond
                        CCombo.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                    }
                }
                else
                {
                    CText = (TextBox)CaracNameToTb[unNode.Attributes["nom"].InnerText];

                    //Si le champs doit etre desactive on le traite avant
                    if (unNode.Attributes["renseigne"].InnerText == "4")
                    {
                        CText.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                        CText.Enabled = false;
                    }
                    else
                    {
                        //Si le champs est obligatoire ou différé on met sa couleur de fond
                        CText.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                    }
                }
            }
        }

        public void Set_Next_Fields_Properties_From_C1C2(Hashtable nextfieldsstate)
        {
            TextBox CText;
            foreach (object key in nextfieldsstate.Keys)
            {
                CText = (TextBox)CaracNameToTb[key.ToString()];

                //Si le champs doit etre desactive on le traite avant
                if (nextfieldsstate[key].ToString() == "4")
                {
                    CText.BackColor = mod_global.Get_Field_Color(nextfieldsstate[key].ToString());
                    CText.Text = String.Empty;
                    CText.Enabled = false;
                }
                else
                {
                    //Si le champs est obligatoire ou différé on met sa couleur de fond
                    CText.BackColor = mod_global.Get_Field_Color(nextfieldsstate[key].ToString());
                    CText.Enabled = true;
                }
                
            }
        }

        private void Set_Fields_Events()
        {
            //On associe chaque Enter event des TextBox la la fonction Carac_Tb_OnClick
            Carac1Tb.Enter  += new System.EventHandler(Carac_Tb_OnClick);
            Carac2Tb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            Quant1Tb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            Quant2Tb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            EmpLong1Tb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            EmpLong2Tb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            EmpCirc1Tb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            EmpCirc2Tb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            RemarqueTb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            RefAudioTb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            RefPhotoTb.Enter += new System.EventHandler(Carac_Tb_OnClick);
            RefVideoTb.Enter += new System.EventHandler(Carac_Tb_OnClick);

             //On associe chaque textbox à la fonction de validation
            Carac1Tb.Validating += new System.ComponentModel.CancelEventHandler(Carac_Tb_Validating);
            Carac2Tb.Validating += new System.ComponentModel.CancelEventHandler(Carac_Tb_Validating);
            Quant1Tb.Validating += new System.ComponentModel.CancelEventHandler(Carac_Tb_Validating);
            Quant2Tb.Validating += new System.ComponentModel.CancelEventHandler(Carac_Tb_Validating);
            EmpLong1Tb.Validating += new System.ComponentModel.CancelEventHandler(Carac_Tb_Validating);
            EmpLong2Tb.Validating += new System.ComponentModel.CancelEventHandler(Carac_Tb_Validating);

        }

        private void Retreive_Carac(string num)
        {
            XmlNode root;
            //XmlNode CodeNode;
            XmlNodeList CaracNodeList;

            root = mod_accueil.SVF.DocumentElement;

            //CodeNode = root.SelectSingleNode("/inspection/ouvrage[@nom='" + mod_inspection.OUVRAGE + "']/observations/code[@num='" + num + "']");

            CaracNodeList = root.SelectNodes("/inspection/ouvrage[@nom='" + mod_accueil.OUVRAGE + "']/observations/code[@num='" + num + "']/caracteristiques/caracteristique");

            string carac = String.Empty;
            string correspondance = String.Empty;
            string choicecode = String.Empty;

            Label CLab;
            TextBox CText;
            CheckBox CBox;
            ComboBox CbBox;
            foreach (XmlNode unNode in CaracNodeList)
            {
                if (unNode.InnerText != String.Empty)
                {
                    if (unNode.Attributes.GetNamedItem("correspondance") != null)
                        correspondance = unNode.Attributes["correspondance"].InnerText + " | ";
                    if (unNode.Attributes.GetNamedItem("code") != null)
                        choicecode = unNode.Attributes["code"].InnerText + " | ";

                    if (unNode.Attributes["nom"].InnerText != "assemblage" & unNode.Attributes["nom"].InnerText != "posregard")
                    {
                        if (unNode.Attributes.GetNamedItem("intitule") != null)
                        {
                            if (CaracNameToLb.ContainsKey(unNode.Attributes["nom"].InnerText))
                            {
                                CLab = (Label)CaracNameToLb[unNode.Attributes["nom"].InnerText];
                                CLab.Text = unNode.Attributes["intitule"].InnerText;
                            }
                        }

                        CText = (TextBox)CaracNameToTb[unNode.Attributes["nom"].InnerText];
                        CText.Text = choicecode + correspondance + unNode.InnerText;
                        correspondance = String.Empty;
                        choicecode = String.Empty;

                        //Si le champs doit etre desactive on le traite avant
                        if (unNode.Attributes["renseigne"].InnerText == "4")
                        {
                            CText.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                            CText.Enabled = false;
                        }
                        else
                        {
                            //Si le champs est obligatoire ou différé on met sa couleur de fond
                            CText.BackColor = mod_global.Get_Field_Color(unNode.Attributes["renseigne"].InnerText);
                        }

                    }
                    /* Modifier
                    * 
                    * 
                    * 
                    * */
                    else if (unNode.Attributes["nom"].InnerText != "posregard")
                    {
                        CBox = (CheckBox)CaracNameToTb[unNode.Attributes["nom"].InnerText];
                        if (unNode.InnerText.ToLower() == "true")
                            CBox.Checked = true;
                        else
                            CBox.Checked = false;
                    }
                    else
                    {
                        CbBox = (ComboBox)CaracNameToTb[unNode.Attributes["nom"].InnerText];
                        CbBox.Text = unNode.Attributes["codeR"].InnerText + " | " + unNode.InnerText;
                    }

                    //correspondance = String.Empty;
                }
            }
        }

        private void Carac_Tb_OnClick(object sender, EventArgs e)
        {
            XmlNode root;
            XmlNode node;

            TextBox Field_Input = (TextBox)sender;

            root = mod_global.Get_Codes_Obs_DocElement();
            node = root.SelectSingleNode("/codes/code[id='" + current_code + "']/caracteristiques/caracteristique[@nom='" + Field_Input.Tag + "']");

            mod_global.Focused_Control = Field_Input;
            mod_global.Focused_Carac_Panel = this;
            mod_global.MF.InputPreviewTb.Text = Field_Input.Text;

            // Afficher l'info au Status Panel
            if (node.Attributes["info"] != null)
                mod_global.MF.statusPanel.Text = node.Attributes["info"].InnerText;
            else mod_global.MF.statusPanel.Text = "";

            //On applique le mode de saisie correspondant au champ focused
            if (node.Attributes["type"].InnerText == "item")
            {
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = true;
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"];
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
                mod_global.MF.flowLayoutPanel2.Controls.Clear();

                //On utilise un navigateur pour pouvoir trier les noeuds 
                XPathNavigator nav = root.CreateNavigator();
                XPathExpression exp = nav.Compile("/codes/code[id='" + current_code + "']/caracteristiques/caracteristique[@nom='" + Field_Input.Tag + "']/item");

                exp.AddSort("@position", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);

                foreach (XPathNavigator item in nav.Select(exp))
                {
                    multiple_choice_button but;
                    string but_text = item.GetAttribute("nom","") + " | " + item.Value;

                    //Si le choix contient des attributs concernant le comportement des champs suivants (on teste la présence de q1 comme attribut du choix en cours)
                    if (item.GetAttribute("q1", "") != "")
                    {
                        but = new multiple_choice_button(but_text, Get_CaracFieldsNewState_HT(item), false, false);
                    }
                    else
                        if (item.GetAttribute("lien", "") == "")
                            if (item.GetAttribute("ajoute", "") == "true")
                                but = new multiple_choice_button(but_text, null, false, true);
                            else
                                but = new multiple_choice_button(but_text, null, false, false);
                        else
                            if (item.GetAttribute("lien", "") == "true")
                                but = new multiple_choice_button(but_text, null, true, false);
                            else
                                but = new multiple_choice_button(but_text, null, true, false);


                    mod_global.MF.flowLayoutPanel2.Controls.Add(but);

                }
    /* ANCIEN CODE NS remplacé par GB le 16/12/2009
                foreach (XmlNode nod in node.ChildNodes)
                {
                    multiple_choice_button but;
                    string but_text = nod.Attributes["nom"].InnerText + " | " + nod.InnerText;

                    //Si le choix contient des attributs concernant le comportement des champs suivants (on teste la présence de q1 comme attribut du choix en cours)
                    if (nod.Attributes.GetNamedItem("q1") != null)
                        but = new multiple_choice_button(but_text, Get_CaracFieldsNewState_HT(nod), false);
                    else
                        if (nod.Attributes.GetNamedItem("lien") == null)
                            but = new multiple_choice_button(but_text, null, false);
                        else
                            if (nod.Attributes["lien"].InnerText == "true")
                                but = new multiple_choice_button(but_text, null, true);
                            else
                                but = new multiple_choice_button(but_text, null, true);


                    mod_global.MF.flowLayoutPanel2.Controls.Add(but);
                }*/


            }
            else if (node.Attributes["type"].InnerText == "texte")
            {
                mod_global.MF.flowLayoutPanel2.Controls.Clear();
                mod_global.MF.virtual_kb1.Set_Alpha_Numeric();
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = true;
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"];
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
            }
            else if (node.Attributes["type"].InnerText == "numerique")
            {
                mod_global.MF.flowLayoutPanel2.Controls.Clear();
                mod_global.MF.virtual_kb1.Set_Only_Numeric();
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = true;
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"];
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
            }
            else if (node.Attributes["type"].InnerText == "date")
            {
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = true;
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["DateTab"];
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
            }
            else if (node.Attributes["type"].InnerText == "video")
            {
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = true;
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["VideoTab"];
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
            }
            else if (node.Attributes["type"].InnerText == "photo")
            {
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = true;
                mod_global.MF.photo_select1.Read_Photos_From_TextList(mod_global.Focused_Control.Text);
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["PhotoTab"];
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
            }
            else if (node.Attributes["type"].InnerText == "audio")
            {
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = true;
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["AudioTab"];
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
            }
            else if (node.Attributes["type"].InnerText == "horaire")
            {
                mod_global.MF.SaisieTabControl.TabPages["SectionTab"].Enabled = true;
                mod_global.MF.section_horaire1.Init_Section_Img(current_forme);
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["SectionTab"];
                mod_global.MF.SaisieTabControl.TabPages["DateTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["VideoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["PhotoTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["AudioTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"].Enabled = false;
                mod_global.MF.SaisieTabControl.TabPages["SchemaTab"].Enabled = false;
            }
        }

        //evenement appeler à validation de la zone de saisie
        private void Carac_Tb_Validating(object sender, CancelEventArgs e)
        {
            XmlNode root;
            XmlNode node;
            string valText = String.Empty;

            TextBox Field_Input = (TextBox)sender;
            if (Field_Input.Text != "")
                valText = Field_Input.Text.Substring(0, 1);
            else valText = Field_Input.Text;

            if (valText == "") return;

            root = mod_global.Get_Codes_Obs_DocElement();
            node = root.SelectSingleNode("/codes/code[id='" + current_code + "']/caracteristiques/caracteristique[@nom='" + Field_Input.Tag + "']");

          
            //On lit le type du noeud et applique validation
            if (node.Attributes["type"].InnerText == "item")
            {
                 foreach (XmlNode nod in node.ChildNodes)
                {
                    //string val1 = nod.Attributes["nom"].InnerText + " | " + nod.InnerText;
                    string val1 = nod.Attributes["nom"].InnerText;
                    //valtext doit être egal à val1
                    if (valText == val1) 
                        return;
                }
                
                //aucun item ne correspondait, on empeche la validation
                DialogResult rep = MessageBox.Show("La valeur saisie n'est pas dans les choix multiples, elle va être effacée. Il faut charger un autre fichier XML ou changer sa valeur.",
                                    "Saisie incorrecte",MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                /*if (rep == DialogResult.OK)
                    Field_Input.Text = string.Empty;               
                e.Cancel = true; */                   
            }
            else if (node.Attributes["type"].InnerText == "texte")
            {
                //on peut tout entrer donc verification inutile
                return;
            }
            else if (node.Attributes["type"].InnerText == "numerique")
            {
                
                //on verifie que c'est bien un nombre
                try{
                    System.Convert.ToDouble(valText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DialogResult rep = MessageBox.Show("La valeur saisie n'est pas un nombre correct (le séparateur décimal doit être un point), elle va être effacée",
                                  "Saisie incorrecte", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (rep == DialogResult.OK)
                        Field_Input.Text = string.Empty;
                    e.Cancel = true;     
                }
        
            }
            else if (node.Attributes["type"].InnerText == "date")
            {
                try
                {
                    System.Convert.ToDateTime(valText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DialogResult rep = MessageBox.Show("La valeur saisie n'est pas une date correcte, elle va être effacée",
                                  "Saisie incorrecte", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    if (rep == DialogResult.OK)
                        Field_Input.Text = string.Empty;
                    e.Cancel = true;  
                }
            }
        }
    


        private bool Validation_Check()
        {
            if (current_code == "AEC" | current_code == "CEC")
                //Si on est sur un changement de forme, on vérifie que le pm saisi est bien supèrieur au pm max, et que nous sommes bien dans le cas d'une création de code
                if (mod_save.Verify_Observation_Panel_Is_Pm_More_Than_Last_Pm() == false & current_num == string.Empty)
                {
                    MessageBox.Show("Le linéaire d'un changement de forme doit être supérieur au dernier linéaire", "Erreur", MessageBoxButtons.OK);
                    return false;
                }

            //On vérifie que le pm saisi est bien supèrieur au pm du dernier changement de forme, et que nous sommes bien dans le cas d'une création de code
            if (mod_save.Verify_Observation_Panel_Is_Pm_More_Than_Last_AEC_CEC() == false & current_num == string.Empty)
            {
                MessageBox.Show("Le linéaire doit être supérieur à celui du dernier changement de forme", "Erreur", MessageBoxButtons.OK);
                return false;
            }

            //On vérifie que tous les champs obligatoires sont saisis
            if (mod_save.Verify_Observation_Panel_Required_Fields() == false)
            {
                MessageBox.Show("Certains champs obligatoires ne sont pas saisis", "Erreur", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private void CaracValidBt_Click(object sender, EventArgs e)
        {
            mod_global.Focused_Carac_Panel = this;

            if (Validation_Check() == true)
            {
                string savenum = mod_save.Save_Observation_Panel(current_code, current_num);
                string checknum = current_num;
                current_num = savenum;
                CaracValidBt.Enabled = false;

                //Création des pages de codes liés
                XmlNode root;
                XmlNodeList CodeLieNodeList;

                root = mod_global.Get_Codes_Obs_DocElement();
                CodeLieNodeList = root.SelectNodes("/codes/code[id='" + current_code + "']/lien/codelie");
                if (CodeLieNodeList.Count > 0)
                {
                    if ((checknum == String.Empty) && (mod_carac.cree == true))
                    {
                        DialogResult diag = MessageBox.Show("Voulez-vous saisir ses codes liéés?", "Confirmation", MessageBoxButtons.YesNo);
                        if (diag == DialogResult.Yes)
                        {
                            foreach (XmlNode CodeLieNode in CodeLieNodeList)
                            {
                                //mod_global.MF.CaracDockingTab.TabPages.Contains
                                mod_carac.New_Caracteristique_Form(CodeLieNode.InnerText, String.Empty, false, current_code);
                            }
                        }

                        //Remettre le boolean false car c'est le cas code lié
                        mod_carac.cree = false;
                    }
                }
            }
            else
            {
                return;
            }

            // Mise à jour le nombre d'observation
            XmlNodeList nodeList = mod_accueil.SVF.DocumentElement.SelectNodes(string.Concat("//ouvrage[@nom='", mod_global.MF.OuvrageList.SelectedText, "']/observations/*"));
            mod_global.MF.obs_nb_label.Text = nodeList.Count.ToString();
        }

        private void CaracValidnCloseBt_Click(object sender, EventArgs e)
        {
            mod_global.Focused_Carac_Panel = this;

            if (Validation_Check() == true)
            {
                string savenum = mod_save.Save_Observation_Panel(current_code, current_num);
                mod_global.MF.CaracDockingTab.TabPages.Remove((C1.Win.C1Command.C1DockingTabPage)this.Parent);
                mod_global.MF.MainDockingTab.SelectedTab = mod_global.MF.ObservationTab;
            }
            else
            {
                return;
            }
            // Mise à jour le nombre d'observation
            XmlNodeList nodeList = mod_accueil.SVF.DocumentElement.SelectNodes(string.Concat("//ouvrage[@nom='", mod_global.MF.OuvrageList.SelectedText, "']/observations/*"));
            mod_global.MF.obs_nb_label.Text = nodeList.Count.ToString();
        }

        private void Gray_TextBox()
        {
            if (TbCount <= this.Controls.Count - 1)
            {
                if (this.Controls[TbCount] is GroupBox)
                {
                    GroupBox gb = (GroupBox)this.Controls[TbCount];
                    foreach (Control ct in gb.Controls)
                    {
                        if (ct is TextBox)
                            ct.BackColor = Color.Silver;
                    }
                }
                TbCount += 1;
            }
            else
            {
                timer1.Stop();
                White_TextBox();
                TbCount = 0;
            }
        }

        private void White_TextBox()
        {
            foreach (Control ct1 in this.Controls)
            {
                if (ct1 is GroupBox)
                {
                    GroupBox gb = (GroupBox)ct1;
                    foreach (Control ct2 in gb.Controls)
                    {
                        if (ct2 is TextBox)
                            ct2.BackColor = Color.White;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Gray_TextBox();
        }

        private void CaracCopyBt_Click(object sender, EventArgs e)
        {
            mod_carac.Add_Existing_Code_Tab(current_code,current_num,true);
        }

        private void CaracCancelBt_Click(object sender, EventArgs e)
        {
            if (mod_global.MF.CaracDockingTab.TabPages.Count > 1)
            {
                mod_global.MF.CaracDockingTab.TabPages.Remove((C1.Win.C1Command.C1DockingTabPage)this.Parent);
            }
            else
            {
                mod_global.MF.CaracDockingTab.TabPages.Clear();
                mod_global.MF.MainDockingTab.SelectedTab = mod_global.MF.ObservationTab;
            }
        }

        private void CaracDelBt_Click(object sender, EventArgs e)
        {
            mod_save.Delete_Observation(current_num);
            mod_global.MF.CaracDockingTab.TabPages.Remove((C1.Win.C1Command.C1DockingTabPage)this.Parent);
            mod_global.MF.MainDockingTab.SelectedTab = mod_global.MF.ObservationTab;

            // Mise à jour le nombre d'observation
            XmlNodeList nodeList = mod_accueil.SVF.DocumentElement.SelectNodes(string.Concat("//ouvrage[@nom='", mod_global.MF.OuvrageList.SelectedText, "']/observations/*"));
            mod_global.MF.obs_nb_label.Text = nodeList.Count.ToString();
        }

        public void SetColor(Color c)
        {
            Carac1Lb.ForeColor = c;
            Carac1Tb.ForeColor = c;
            Carac2Lb.ForeColor = c;
            Carac2Tb.ForeColor = c;
            label6.ForeColor = c;
            label5.ForeColor = c;
            label7.ForeColor = c;
            label8.ForeColor = c;
            label13.ForeColor = c;
            label10.ForeColor = c;
            label11.ForeColor = c;
            Quant1Lb.ForeColor = c;
            Quant1Tb.ForeColor = c;
            Quant2Lb.ForeColor = c;
            Quant2Tb.ForeColor = c;
            AssemblageCb.ForeColor = c;
            PosRegLb.ForeColor = c;
            label9.ForeColor = c;
        }
    }
}
