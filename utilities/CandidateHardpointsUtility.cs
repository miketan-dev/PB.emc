using System.Collections.Generic;

namespace PB.emc.utilities
{
    internal class CandidateHardpointsUtility
    {
        //Solo gli hardpoint selezionati possono essere candidati per la qualifica di hardpoint editabili.
        //Aggiungere eventuali hardpoint aggiuntivi qui.
        private static readonly HashSet<string> CandidateHardpoints = new HashSet<string>
        {
            "external_arm_lower",
            "external_bottom_left_lower",
            "external_bottom_right_lower",
            "external_top_head",
            "external_top_pelvis"
        };

        public static bool IsCandidateHardpoint(string hardpoint)
        {
            return CandidateHardpoints.Contains(hardpoint);
        }
    }
}