using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1.component
{
    public partial class multiple_choice_button : UserControl
    {
        public multiple_choice_button(string button_text, Hashtable NextFieldsState, bool other_function)
        {
            InitializeComponent();
            button1.Text = button_text;

            if (NextFieldsState != null)
                button1.Tag = NextFieldsState;

            if (other_function == true)
                button1.ForeColor = Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Si c'est un choix autre (forecolor=red) on ouvre le clavier
            //if (button1.ForeColor == Color.Red)
            //{
              //  mod_global.MF.virtual_kb1.Set_Alpha_Numeric();
               // mod_global.MF.SaisieTabControl.SelectedTab = mod_global.MF.SaisieTabControl.TabPages["KeyboardTab"];
            //}
            //else
            //{
                mod_global.Focused_Control.Text = button1.Text;
            //}

            if (button1.Tag is Hashtable)
            {
                Hashtable ht = (Hashtable)button1.Tag;

                mod_global.Focused_Carac_Panel.Set_Next_Fields_Properties_From_C1C2(ht);       
            }
        }
    }
}
