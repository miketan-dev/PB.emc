using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using PB.emc.utilities;
using PhantomBrigade;
using PhantomBrigade.Data;
using PhantomBrigade.Functions.Equipment;
using UnityEngine;

namespace PB.emc
{
    [HarmonyPatch]
    public class Patch
    {
        [HarmonyPatch(typeof(DataContainerPartPreset), "OnAfterDeserialization")]
        [HarmonyPostfix]
        static void resetEditableState(DataContainerPartPreset __instance)
        {
            if (__instance.genStepsProcessed == null) return;

            foreach (IPartGenStep genStep in __instance.genStepsProcessed)
            {
                if (genStep is AddHardpoints addHardpointsStep)
                {
                    List<string> targets = addHardpointsStep.hardpointsTargeted;

                    if (targets != null && targets.Count > 0)
                    {
                        // Uso .ToList() per poter modificare 'targets' in sicurezza
                        foreach (var targetKey in targets.ToList())
                        {
                            //Loggo la lista di hardpoints targetati
                            Debug.LogFormat($"[EMC] - {__instance.key} -> hardpoint target: {targetKey}");
                            
                            // Se nel part preset NON E' un hardpoint candidato, lo rimuovo
                            if (!CandidateHardpointsUtility.IsCandidateHardpointTargeted(targetKey)) 
                            {
                                Debug.LogFormat($"[EMC] - {__instance.key} -> hardpoint target: {targetKey} NON CANDIDATO. Rimozione...");
                                targets.Remove(targetKey);
                            }
                            else
                            {
                                Debug.LogFormat($"[EMC] - {__instance.key} -> hardpoint target: {targetKey} CANDIDATO! Mantenuto.");
                            }
                        }
                    }
                }
            }
            __instance.ResolveText();
        }

        [HarmonyPatch(typeof(DataContainerSubsystemHardpoint), "OnAfterDeserialization")]
        [HarmonyPostfix]
        static void putEditableState(DataContainerSubsystemHardpoint __instance)
        {
            if (__instance.key != null)
            {
                // applica agli hardpoint candidati il campo editabile a true, se sono su false.
                if (CandidateHardpointsUtility.IsCandidateHardpoint(__instance.key))
                {
                    if (!__instance.editable)
                    {
                        __instance.editable = true;
                        Debug.LogFormat($"[EMC] Hardpoint {__instance.key} --CANDIDATO--. editable: {__instance.editable}");
                    }
                }
                else
                {
                    // Se non è candidato, devo forzarlo a false
                    if (__instance.editable)
                    {
                        __instance.editable = false;
                        Debug.LogWarningFormat($"[EMC] - Hardpoint {__instance.key} --NON CANDIDATO--. editable: {__instance.editable}");
                    }
                }

                Debug.LogFormat($"[EMC] - Hardpoint RILEVATO: {__instance.key}");
            }
            else
            {
                Debug.LogWarningFormat($"[EMC] - Hardpoint NON RILEVATO: {__instance.key} . ");
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