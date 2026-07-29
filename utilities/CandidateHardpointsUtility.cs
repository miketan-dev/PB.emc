using System.Collections.Generic;
using System.IO;
using System.Linq;
using PB.emc.Models;
using UnityEngine;

namespace PB.emc.utilities
{
    internal static class CandidateHardpointsUtility
    {
        private static HashSet<string> _cachedHardpoints;
        private static HashSet<string> _cachedHardpointsTargeted;

        private static void EnsureInit()
        {
            if (_cachedHardpoints != null) return;
            if (_cachedHardpointsTargeted != null) return;

            var fullPath = Path.Combine(EmcModLink.modPath, "emc_cache");
            const string ext = ".yaml";
            const string fileName = "candidate_hardpoints";

            //concateno il nome e l'estensione
            const string filenameCombined = fileName + ext;

            var config = YamlUtils.ReadFile<CandidateHardpointsModel>(fullPath, filenameCombined);
            _cachedHardpoints = config?.Data?.CandidateHardpoints ?? [];
            _cachedHardpointsTargeted = config?.Data?.CandidateHardpointsTargeted ?? [];

            foreach (var elements in _cachedHardpoints)
            {
                Debug.Log($"[EMC] - Hardpoint candidati trovati:\n numero elementi: {elements.Length} \n" +
                          "lista hardpoints: \n" + elements.ToList());
            }

            foreach (var elements in _cachedHardpointsTargeted)
            {
                Debug.Log($"[EMC] - Hardpoint targeted trovati:\n numero elementi: {elements.Length} \n" +
                          "lista hardpoints: \n" + elements.ToList());
            }
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