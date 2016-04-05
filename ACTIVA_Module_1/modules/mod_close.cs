using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACTIVA_Module_1.modules
{
    class mod_close
    {
        public static void App_Close()
        {
            mod_identification.Codes_Id_Cana_Xml = null;
            mod_identification.Codes_Id_Regard_Xml=null;
            mod_identification.Groupe_Codes_Id_Xml = null;

            mod_observation.Codes_Obs_Cana_Xml = null;
            mod_observation.Codes_Obs_Regard_Xml = null;
            mod_observation.Famille_Codes_Xml = null;
        }
    }
}
