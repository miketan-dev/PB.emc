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

        /// <summary>
        /// Checks if the static initialization has been done.
        /// If not, it initializes the static fields `_cachedHardpoints` and `_cachedHardpointsTargeted`
        /// by reading the corresponding YAML files.
        /// </summary>
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

        /// <summary>
        /// Checks if the given hardpoint is a candidate hardpoint to be editable.
        /// </summary>
        /// <param name="hardpoint">The hardpoint to check.</param>
        /// <returns>True if the hardpoint is a candidate, false otherwise.</returns>
        public static bool IsCandidateHardpoint(string hardpoint)
        {
            EnsureInit();
            return _cachedHardpoints.Contains(hardpoint);
        }

        /// <summary>
        /// Checks if the given hardpoint is a candidate hardpoint targeted in the gen steps from part preset.
        /// </summary>
        /// <param name="hardpoint">The hardpoint to check.</param>
        /// <returns>True if the hardpoint is a candidate targeted, false otherwise.</returns>
        public static bool IsCandidateHardpointTargeted(string hardpoint)
        {
            EnsureInit();
            return _cachedHardpointsTargeted.Contains(hardpoint);
        }
    }
}