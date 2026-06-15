using HarmonyLib;
using PB.emc.utilities;
using PhantomBrigade;
using PhantomBrigade.Data;
using UnityEngine;

namespace PB.emc
{
    [HarmonyPatch]
    public class Patch
    {
        [HarmonyPatch(typeof(DataContainerSubsystemHardpoint),
            nameof(DataContainerSubsystemHardpoint.OnAfterDeserialization))]
        [HarmonyPostfix]
        static void putEditableState(DataContainerSubsystemHardpoint __instance)
        {
            // Programmazione difensiva; mi difendo da eventuali NullReferenceException
            if (__instance.key != null)
            {
                /*Debug.LogFormat($"[EMC] Hardpoint rilevato: {CandidateHardpointsUtility.IsCandidateHardpoint(__instance.key)} ");*/

                // applica agli hardpoint candidati il campo editabile a true, se sono su false.
                if (CandidateHardpointsUtility.IsCandidateHardpoint(__instance.key))
                {
                    if (!__instance.editable)
                    {
                        __instance.editable = true;
                    }

                    /*Debug.LogFormat(
                        $"[EMC] Hardpoint {__instance.key} --CANDIDATO--. editable: {__instance.editable} | exposed: {__instance.exposed} | visual: {__instance.visual}.");*/
                }
                else
                {
                    Debug.LogWarningFormat(
                        $"[EMC] Hardpoint {__instance.key} --NON CANDIDATO--. editable: {__instance.editable} | exposed: {__instance.exposed} | visual: {__instance.visual}.");
                }
            }
            else
            {
                Debug.LogWarningFormat($"[EMC] Hardpoint NON RILEVATO: {__instance.key} . ");
            }

            __instance.ResolveText();
        }

        [HarmonyPatch(typeof(WorkshopUtility), "FinishProjectOutputPart")]
        [HarmonyPostfix]
        public static void FinishProjectOutputPart_postfix(string partPresetKey, int rating)
        {
            var partPreset = DataMultiLinkerPartPreset.GetEntry(partPresetKey);
            if (partPreset != null) return;

            var part = UnitUtilities.CreatePartEntityFromPreset(partPresetKey, rating);
            if (part == null) return;

            if (DataShortcuts.overworld.workshopStripsUnfusedSystems.Equals(true))
            {
                DataShortcuts.overworld.workshopStripsUnfusedSystems = false;
                Debug.LogFormat($"[EMC] - Set to {DataShortcuts.overworld.workshopStripsUnfusedSystems}");

                EquipmentUtility.RemoveEditableSubsystemsFromPart(part, false);
                Debug.LogFormat("[EMC] SUBSYSTEMS NON FUSI ALLA CREAZIONE.");
            }

            Debug.LogFormat("[EMC] FINE.");
        }
    }
}