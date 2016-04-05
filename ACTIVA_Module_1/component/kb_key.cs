using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1.component
{
    public partial class kb_key : UserControl
    {
        string val_touche;
        public string name_touche;
        bool is_numeric_key;
        Color Disabled_Color = Color.DimGray;
        Color Active_Color;

        public kb_key(string name, string text, Bitmap image, int width, int height, Color couleur, Font key_font, bool is_numeric)
        {
            InitializeComponent();

            touche.Name = name;
            touche.Text = String.Empty;

            if (text != String.Empty)
                touche.Text = text;
            else
                touche.Image = image;

            touche.BackColor = couleur;
            touche.Font = key_font;
            touche.FlatStyle = FlatStyle.Flat;

            this.Width = width;
            this.Height = height;

            val_touche = text;
            name_touche = name;
            is_numeric_key = is_numeric;
            Active_Color = couleur;
        }

        public void Disable_Key()
        {
            touche.BackColor = Disabled_Color;
            touche.Enabled = false;
        }

        public void Enable_Key()
        {
            touche.BackColor = Active_Color;
            touche.Enabled = true;
        }

        public void Upper_Key()
        {
            touche.Text = touche.Text.ToUpper();
            val_touche = val_touche.ToUpper();
        }

        public void Lower_Key()
        {
            touche.Text = touche.Text.ToLower();
            val_touche = val_touche.ToLower();
        }

        public bool is_num_key
        {
            get { return is_numeric_key; }
        }

        public Color get_Active_Color
        {
            get { return Active_Color; }
        }

        public Color get_Disabled_Color
        {
            get { return Disabled_Color; }
        }

        private void touche_Click(object sender, EventArgs e)
        {
            string text;

            if (mod_global.Focused_Control != null)
            {
                text = mod_global.Focused_Control.Text;

                if (name_touche == "del")
                {
                    if (text.Length > 0)
                        mod_global.Focused_Control.Text = text.Substring(0, text.Length - 1);
                }
                if (name_touche == "space")
                {
                    mod_global.Focused_Control.Text += " ";
                }
                else if (name_touche == "shift")
                {
                    if (mod_global.MF.virtual_kb1.Is_Upper() == true)
                        mod_global.MF.virtual_kb1.Set_Lower();
                    else
                        mod_global.MF.virtual_kb1.Set_Upper(true);
                }
                else if (name_touche == "capslock")
                {
                    if (mod_global.MF.virtual_kb1.Is_Upper() == true)
                        mod_global.MF.virtual_kb1.Set_Lower();
                    else
                        mod_global.MF.virtual_kb1.Set_Upper(false);
                }
                else
                {
                    mod_global.Focused_Control.Text += val_touche;

                    if (mod_global.MF.virtual_kb1.Is_Shifted() == true)
                        mod_global.MF.virtual_kb1.Set_Lower();
                }

                //mod_global.Focused_Control.Focus();
            }
        }

        public static char GetCharFromKeys(Keys keyData)
        {
            char KeyValue;
            switch (keyData)
            {
                case Keys.Add:
                case Keys.Oemplus:
                    KeyValue = '+';
                    break;
                case Keys.OemMinus:
                case Keys.Subtract:
                    KeyValue = '-';
                    break;
                case Keys.OemQuestion | Keys.Shift:
                    KeyValue = '?';
                    break;
                case Keys.OemQuestion:
                case Keys.Divide:
                    KeyValue = '/';
                    break;
                default:
                    if ((0x60 <= (int)keyData) && (0x69 >= (int)keyData))
                    {
                        KeyValue = (char)((int)keyData - 0x30);
                    }
                    else
                    {
                        KeyValue = (char)keyData;
                    }
                    break;
            }
            return KeyValue;
        }
    }
}
