using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using ACTIVA_Module_1.modules;

namespace ACTIVA_Module_1.component
{
    public partial class date_selector : UserControl
    {
        public date_selector()
        {
            InitializeComponent();
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            mod_global.Focused_Control.Text = monthCalendar1.SelectionStart.ToShortDateString();
        }

        private void date_selector_Load(object sender, EventArgs e)
        {
            DateTime start = DateTime.Today.AddMonths(-3);
            monthCalendar1.SetDate(start);
        }
    }
}
