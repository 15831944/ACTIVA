using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace ACTIVA_Module_1.component
{
    public partial class virtual_kb : UserControl
    {
        bool Upper_State = false;
        bool Shift_State = false;

        public virtual_kb()
        {
            InitializeComponent();
            Make_Keyboard();
            Make_Pave();
        }

        private void add_keyboard_key(string name, string text, Bitmap image, int width, int height, Color couleur, int col, int row, int span)
        {
            Font clavier_font = new Font("Arial", 16);
            kb_key touche = new kb_key(name, text, image, width, height, couleur, clavier_font, false);

            if (span > 1)
                kbtable.SetColumnSpan(touche, span);

            kbtable.Controls.Add(touche,col,row);
        }

        private void add_pave_key(string name, string text, Bitmap image, int width, int height, Color couleur, int col, int row, int span, bool is_numeric)
        {
            Font clavier_font = new Font("Arial", 16);
            kb_key touche = new kb_key(name, text, image, width, height, couleur, clavier_font, is_numeric);

            if (span > 1)
                pavetable.SetColumnSpan(touche, span);

            pavetable.Controls.Add(touche, col, row);
        }

        private void Make_Keyboard()
        {
            add_keyboard_key("aigu", "é", null, 48, 48, Color.LightSteelBlue, 0, 0, 1);
            add_keyboard_key("grave", "è", null, 48, 48, Color.LightSteelBlue, 1, 0, 1);
            add_keyboard_key("circonflexe", "ê", null, 48, 48, Color.LightSteelBlue, 2, 0, 1);
            add_keyboard_key("circonflexe", "à", null, 48, 48, Color.LightSteelBlue, 3, 0, 1);
            add_keyboard_key("circonflexe", "ù", null, 48, 48, Color.LightSteelBlue, 4, 0, 1);
            add_keyboard_key("circonflexe", "ï", null, 48, 48, Color.LightSteelBlue, 5, 0, 1);
            add_keyboard_key("cedille", "ç", null, 48, 48, Color.LightSteelBlue, 6, 0, 1);
            add_keyboard_key("plus_petit", "<", null, 48, 48, Color.LightSteelBlue, 7, 0, 1);
            add_keyboard_key("plus_grand", ">", null, 48, 48, Color.LightSteelBlue, 8, 0, 1);
            add_keyboard_key("tiret_haut", "-", null, 48, 48, Color.LightSteelBlue, 9, 0, 1);
            add_keyboard_key("tirat_bas", "_", null, 48, 48, Color.LightSteelBlue, 10, 0, 1);
            add_keyboard_key("slash", "/", null, 48, 48, Color.LightSteelBlue, 11, 0, 1);

            add_keyboard_key("capslock", string.Empty, Properties.Resources.capslock_48_trans, 48, 48, Color.White, 0, 1, 1);
            add_keyboard_key("A", "a", null, 48, 48, Color.WhiteSmoke, 1, 1, 1);
            add_keyboard_key("Z", "z", null, 48, 48, Color.WhiteSmoke, 2, 1, 1);
            add_keyboard_key("E", "e", null, 48, 48, Color.WhiteSmoke, 3, 1, 1);
            add_keyboard_key("R", "r", null, 48, 48, Color.WhiteSmoke, 4, 1, 1);
            add_keyboard_key("T", "t", null, 48, 48, Color.WhiteSmoke, 5, 1, 1);
            add_keyboard_key("Y", "y", null, 48, 48, Color.WhiteSmoke, 6, 1, 1);
            add_keyboard_key("U", "u", null, 48, 48, Color.WhiteSmoke, 7, 1, 1);
            add_keyboard_key("I", "i", null, 48, 48, Color.WhiteSmoke, 8, 1, 1);
            add_keyboard_key("O", "o", null, 48, 48, Color.WhiteSmoke, 9, 1, 1);
            add_keyboard_key("P", "p", null, 48, 48, Color.WhiteSmoke, 10, 1, 1);
            add_keyboard_key("del", string.Empty, Properties.Resources.backspace_48_trans, 48, 48, Color.White, 11, 1, 1);

            add_keyboard_key("shift", string.Empty, Properties.Resources.shift_48_trans, 48, 48, Color.White, 0, 2, 1);
            add_keyboard_key("Q", "q", null, 48, 48, Color.WhiteSmoke, 1, 2, 1);
            add_keyboard_key("S", "s", null, 48, 48, Color.WhiteSmoke, 2, 2, 1);
            add_keyboard_key("D", "d", null, 48, 48, Color.WhiteSmoke, 3, 2, 1);
            add_keyboard_key("F", "f", null, 48, 48, Color.WhiteSmoke, 4, 2, 1);
            add_keyboard_key("G", "g", null, 48, 48, Color.WhiteSmoke, 5, 2, 1);
            add_keyboard_key("H", "h", null, 48, 48, Color.WhiteSmoke, 6, 2, 1);
            add_keyboard_key("J", "j", null, 48, 48, Color.WhiteSmoke, 7, 2, 1);
            add_keyboard_key("K", "k", null, 48, 48, Color.WhiteSmoke, 8, 2, 1);
            add_keyboard_key("L", "l", null, 48, 48, Color.WhiteSmoke, 9, 2, 1);
            add_keyboard_key("M", "m", null, 48, 48, Color.WhiteSmoke, 10, 2, 1);
            add_keyboard_key("shift", string.Empty, Properties.Resources.shift_48_trans, 48, 48, Color.White, 11, 2, 1);

            add_keyboard_key("deux_point", ":", null, 48, 48, Color.LightSteelBlue, 0, 3, 1);
            add_keyboard_key("exclamation", "!", null, 48, 48, Color.LightSteelBlue, 1, 3, 1);
            add_keyboard_key("interogation", "?", null, 48, 48, Color.LightSteelBlue, 2, 3, 1);
            add_keyboard_key("W", "w", null, 48, 48, Color.WhiteSmoke, 3, 3, 1);
            add_keyboard_key("X", "x", null, 48, 48, Color.WhiteSmoke, 4, 3, 1);
            add_keyboard_key("C", "c", null, 48, 48, Color.WhiteSmoke, 5, 3, 1);
            add_keyboard_key("V", "v", null, 48, 48, Color.WhiteSmoke, 6, 3, 1);
            add_keyboard_key("B", "b", null, 48, 48, Color.WhiteSmoke, 7, 3, 1);
            add_keyboard_key("N", "n", null, 48, 48, Color.WhiteSmoke, 8, 3, 1);
            add_keyboard_key("virgule", ",", null, 48, 48, Color.LightSteelBlue, 9, 3, 1);
            add_keyboard_key("point_virgule", ";", null, 48, 48, Color.LightSteelBlue, 10, 3, 1);
            add_keyboard_key("point", ".", null, 48, 48, Color.LightSteelBlue, 11, 3, 1);

            add_keyboard_key("space", "Espace", null, 426, 48, Color.WhiteSmoke, 2, 4, 8);
        }

        private void Make_Pave()
        {

            add_pave_key("ouverte", "(", null, 48, 48, Color.LightSteelBlue, 0, 0, 1, false);
            add_pave_key("fermante", ")", null, 48, 48, Color.LightSteelBlue, 1, 0, 1, false);
            add_pave_key("pourcent", "%", null, 48, 48, Color.LightSteelBlue, 2, 0, 1, false);
            add_pave_key("euro", "€", null, 48, 48, Color.LightSteelBlue, 3, 0, 1, false);

            add_pave_key("7", "7", null, 48, 48, Color.WhiteSmoke, 0, 1, 1, true);
            add_pave_key("8", "8", null, 48, 48, Color.WhiteSmoke, 1, 1, 1, true);
            add_pave_key("9", "9", null, 48, 48, Color.WhiteSmoke, 2, 1, 1, true);
            add_pave_key("plus", "+", null, 48, 48, Color.LightSteelBlue, 3, 1, 1, false);

            add_pave_key("4", "4", null, 48, 48, Color.WhiteSmoke, 0, 2, 1, true);
            add_pave_key("5", "5", null, 48, 48, Color.WhiteSmoke, 1, 2, 1, true);
            add_pave_key("6", "6", null, 48, 48, Color.WhiteSmoke, 2, 2, 1, true);
            add_pave_key("moins", "-", null, 48, 48, Color.LightSteelBlue, 3, 2, 1, false);

            add_pave_key("1", "1", null, 48, 48, Color.WhiteSmoke, 0, 3, 1, true);
            add_pave_key("2", "2", null, 48, 48, Color.WhiteSmoke, 1, 3, 1, true);
            add_pave_key("3", "3", null, 48, 48, Color.WhiteSmoke, 2, 3, 1, true);
            add_pave_key("fois", "*", null, 48, 48, Color.LightSteelBlue, 3, 3, 1, false);

            add_pave_key("0", "0", null, 48, 48, Color.WhiteSmoke, 0, 4, 1, true);
            add_pave_key("decimal",Application.CurrentCulture.NumberFormat.NumberDecimalSeparator, null, 48, 48, Color.LightSteelBlue, 1, 4, 1, true);
            add_pave_key("egal", "=", null, 48, 48, Color.LightSteelBlue, 2, 4, 1, false);
            add_pave_key("divise", "/", null, 48, 48, Color.LightSteelBlue, 3, 4, 1, false);

            //add_key("all_del", "X", Keys.OemCloseBrackets, 48, 48, Color.OrangeRed, 18, 0, 1);
        }

        public void Set_Only_Numeric()
        {
            kbtable.Enabled = false;

            kb_key key;

            foreach (Control ct in kbtable.Controls)
            {
                key = (kb_key)ct;
                if (key.is_num_key == false)
                    key.Disable_Key();
            }
            foreach (Control ct in pavetable.Controls)
            {
                key = (kb_key)ct;
                if (key.is_num_key == false)
                    key.Disable_Key();
            }
        }

        public void Set_Alpha_Numeric()
        {
            kbtable.Enabled = true;

            kb_key key;

            foreach (Control ct in kbtable.Controls)
            {
                key = (kb_key)ct;
                key.Enable_Key();
            }

            foreach (Control ct in pavetable.Controls)
            {
                key = (kb_key)ct;
                key.Enable_Key();
            }
        }

        public void Set_Upper(bool shifted)
        {
            kb_key key;

            foreach (Control ct in kbtable.Controls)
            {
                key = (kb_key)ct;
                if (key.name_touche != "space")
                    key.Upper_Key();
            }

            Upper_State = true;

            if (shifted == true)
                Shift_State = true;
            else
                Shift_State = false;
        }

        public void Set_Lower()
        {
            kb_key key;

            foreach (Control ct in kbtable.Controls)
            {
                key = (kb_key)ct;
                if (key.name_touche != "space")
                    key.Lower_Key();
            }

            Upper_State = false;
        }

        public bool Is_Upper()
        {
            return Upper_State;
        }

        public bool Is_Shifted()
        {
            return Shift_State;
        }

    }
}
