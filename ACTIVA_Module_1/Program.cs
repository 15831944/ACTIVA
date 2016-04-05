using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace ACTIVA_Module_1
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //on force le separateur décimal au point pour éviter des pb de traitment des xml
            CultureInfo myCI = new CultureInfo(CultureInfo.CurrentCulture.ToString(),false); 
            myCI.NumberFormat.NumberDecimalSeparator = ".";

            Application.CurrentCulture = myCI;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new splash());
            Application.Run(new MainForm());
        }
    }
}
