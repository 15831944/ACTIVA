using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using C1.Win.C1FlexGrid;

namespace ACTIVA_Module_1.modules
{
    /// <summary>
    /// HostedControl
    /// helper class that contains a control hosted within a C1FlexGrid
    /// </summary>
    public class mod_hosted_control
    {
         //private C1FlexGrid _flex;
         //private Control _ctl;
         //private Row _row;
         //private Column _col;

        public static void HostedControl(C1FlexGrid flex, Control hosted)
        {
            // insert hosted control into grid
            flex.Controls.Add(hosted);
        }

        public static bool UpdatePosition(C1FlexGrid flex, Control hosted, int row, int col)
        {
            // get row/col indices
            int r = row;
            int c = col;
            if (r < 0 || c < 0) return false;

            // get cell rect
            Rectangle rc = flex.GetCellRect(r, c, false);

            // hide control if out of range
            if (rc.Width <= 0 || rc.Height <= 0 || !rc.IntersectsWith(flex.ClientRectangle) || flex.Rows[r].HeightDisplay == 24)
            {
                hosted.Visible = false;
                return true;
            }

            // move the control and show it
            hosted.Bounds = rc;
            hosted.Visible = true;

            // done
            return true;
        }

        public static bool HidePosition(Control hosted)
        {
            hosted.Visible = false;
            //hosted.Hide();

            // done
            return true;
        }
    }
}
