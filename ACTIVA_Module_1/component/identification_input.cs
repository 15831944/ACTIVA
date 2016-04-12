using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Windows.Forms;
using ACTIVA_Module_1.modules;
using System.Globalization;


namespace ACTIVA_Module_1.component
{
    public partial class identification_input : UserControl
    {
        bool ajoute;
       
        public identification_input(string label_text,string label_name, string field_value, string field_state, string field_type, bool field_ajoute, string groupe)
        {
            InitializeComponent();
            field_label.Text = label_text;
            field_label.Name = label_name;
            field_label.Tag = groupe;
            Field_Input.Tag = field_type;
            Field_Input.Text = field_value;
            Field_Input.BackColor = mod_global.Get_Field_Color(field_state);

            ajoute = field_ajoute;

            if (ajoute == true)
                field_label.ForeColor = Color.RoyalBlue;
        }

        public bool is_ajoute()
        {
            return ajoute;
        }

       
        public void Field_Input_TextChanged(object sender, EventArgs e)
        {
            if (mod_global.Focused_Control == Field_Input)
                mod_global.MF.InputPreviewTb.Text = Field_Input.Text;
           
            //on marque que l'enregistrement doit être proposé
            mod_identification.SaveIDFlag = true;
        }

        // a activation du composant (par click souris ou tabulation clavier)
        private void Field_Input_Enter(object sender, EventArgs e)
        {
            XmlNode root;
          
            TextBox tinput = (TextBox)sender;

            mod_global.Focused_Control = Field_Input;
            mod_global.MF.InputPreviewTb.Text = Field_Input.Text;

            if (tinput.Tag.ToString() == "item")
            {
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["ChoiceTab"];
                mod_global.MF.flowLayoutPanel2.Controls.Clear();

                multiple_choice_button but;
                root = mod_global.Get_Codes_Id_DocElement();

                //On utilise un navigateur pour pouvoir trier les noeuds      
                XPathNavigator nav = root.CreateNavigator();
                XPathExpression exp = nav.Compile("/codes/code[id='" + field_label.Name + "']/valeur/item");

                exp.AddSort("@position", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);

                foreach (XPathNavigator item in nav.Select(exp))
                {
                    string but_text = item.GetAttribute("nom","") + " | " + item.Value;

                    if (item.GetAttribute("lien","") == "")
                        if (item.GetAttribute("ajoute", "") == "true")
                            but = new multiple_choice_button(but_text, null, false, true);
                        else
                            but = new multiple_choice_button(but_text, null, false, false);
                    else
                        if (item.GetAttribute("lien","") == "true")
                            but = new multiple_choice_button(but_text, null, true, false);
                        else
                            but = new multiple_choice_button(but_text, null, false, false);

                       mod_global.MF.flowLayoutPanel2.Controls.Add(but);
                }

                /* ANCIEN CODE NS remplacé par GB le 16/12/2009
                XmlNode node;

                node = root.SelectSingleNode("/codes/code[id='" + field_label.Name + "']/valeur");

                foreach (XmlNode nod in node.ChildNodes)
                {
                    string but_text = nod.Attributes["nom"].InnerText + " | " + nod.InnerText;

                    if (nod.Attributes.GetNamedItem("lien") == null)
                        but = new multiple_choice_button(but_text, null, false);
                    else
                        if (nod.Attributes["lien"].InnerText == "true")
                            but = new multiple_choice_button(but_text, null, true);
                        else
                            but = new multiple_choice_button(but_text, null, false);

                    mod_global.MF.flowLayoutPanel2.Controls.Add(but);
                 
                }*/
            }
            else if (tinput.Tag.ToString() == "texte")
            {
                mod_global.MF.flowLayoutPanel2.Controls.Clear();
                mod_global.MF.virtual_kb1.Set_Alpha_Numeric();
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"];
            }
            else if (tinput.Tag.ToString() == "numerique")
            {
                mod_global.MF.flowLayoutPanel2.Controls.Clear();
                mod_global.MF.virtual_kb1.Set_Only_Numeric();
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"];
            }
            else if (tinput.Tag.ToString() == "date")
            {
                mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["DateTab"];
            }
        }
        // a validation du composant
        private void Field_Input_Validating(object sender, CancelEventArgs e)
        {
            //on verifie que la saisie correspond bien au type attendu
            XmlNode root;
            XmlNode node;

            TextBox tinput = (TextBox)sender;

            string valText = tinput.Text;

            if (valText == "") return;

            //cas d'un item
            if (tinput.Tag.ToString() == "item")
            {
                //on retrouve element possible pour item 
                root = mod_global.Get_Codes_Id_DocElement();
                node = root.SelectSingleNode("/codes/code[id='" + field_label.Name + "']/valeur");

                foreach (XmlNode nod in node.ChildNodes)
                {
                    //2 valeurs sont possibles (avec ou sans code concaténé)
                    string val1 = nod.Attributes["nom"].InnerText + " | " + nod.InnerText;
                    string val2 = nod.InnerText;

                    //valtext doit être egal à un des noeuds
                    if ((valText == val1) || (valText == val2))
                        return;
                }
                //aucun item ne correspondait, on empeche la validation
                DialogResult rep = MessageBox.Show("La valeur saisie n'est pas dans les choix multiples, elle va être effacée",
                                   "Saisie incorrecte", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (rep == DialogResult.OK)
                    Field_Input.Text = string.Empty;
                e.Cancel = true;   
            }
            else if (tinput.Tag.ToString() == "texte")
            {
                //on peut tout entrer donc verification inutile
                return;
            }
            else if (tinput.Tag.ToString() == "numerique")
            {
               //on verifie que le nombre est correct
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
            else if (tinput.Tag.ToString() == "date")
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
    }
}
