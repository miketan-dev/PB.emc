using System.Collections.Generic;
using System.IO;
using PB.emc.Models;
using UnityEngine;

namespace PB.emc.utilities
{
    internal static class CandidateHardpointsUtility
    {
        private static HashSet<string> _cachedHardpoints = null;
        private static HashSet<string> _cachedHardpointsTargeted = null;

        private static void EnsureInit()
        {
            if (_cachedHardpoints != null) return;
            if (_cachedHardpointsTargeted != null) return;

            var fullPath = Path.Combine(EmcModLink.modPath, "emc_cache");
            var ext = ".yaml";
            var fileName = "candidate_hardpoints";

            //concateno il nome e l'estensione.
            var filenameCombined = fileName + ext;
            
            var config = YamlUtils.ReadFile<CandidateHardpointsModel>(fullPath, filenameCombined);
            _cachedHardpoints = config?.Data?.CandidateHardpoints ?? new HashSet<string>();
            _cachedHardpointsTargeted = config?.Data?.CandidateHardpointsTargeted ?? new HashSet<string>();

            Debug.LogFormat($"[EMC] - Hardpoint candidati trovati: {_cachedHardpoints.Count}");
            Debug.LogFormat($"[EMC] - Hardpoint targeted trovati: {_cachedHardpointsTargeted.Count}");
        }
        
        public static bool IsCandidateHardpoint(string hardpoint)
        {
            EnsureInit();
            return _cachedHardpoints.Contains(hardpoint);
        }
        
        public static bool IsCandidateHardpointTargeted(string hardpoint)
        {
            EnsureInit();
            return _cachedHardpointsTargeted.Contains(hardpoint);
        }
    }
}