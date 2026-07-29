using System.Collections.Generic;
using HarmonyLib;
using PB.emc.utilities;
using PhantomBrigade;
using PhantomBrigade.Data;
using PhantomBrigade.Functions.Equipment;
using UnityEngine;

namespace PB.emc;


[HarmonyPatch]
public class Patch
{
    /// <summary>
    /// Esegue un patch per forzare il campo 'fused' a false per intercettare gli hardpoint tramite DataContainerPartPreset a runtime.
    /// </summary>
    /// <param name="preset"></param>
    /// <param name="layout"></param>
    /// <param name="rating"></param>
    /// <param name="log"></param>
    [HarmonyPatch(typeof(SetHardpointState), "Run", new[]
    {
        typeof(DataContainerPartPreset),
        typeof(Dictionary<string, GeneratedHardpoint>),
        typeof(int),
        typeof(bool)
    })]
    [HarmonyPostfix]
    static void Postfix_SetHardpointState_Run(DataContainerPartPreset preset,
        Dictionary<string, GeneratedHardpoint> layout, int rating, bool log)
    {
        if (layout == null) return;

        foreach (var kvp in layout)
        {
            var hardpointKey = kvp.Key;
            GeneratedHardpoint genHardpoint = kvp.Value;

            if (CandidateHardpointsUtility.IsCandidateHardpointTargeted(hardpointKey))
            {
                if (genHardpoint.fused)
                {
                    genHardpoint.fused = false;
                    Debug.Log($"[EMC] - {preset.key} -> hardpoint: {hardpointKey} CANDIDATO. 'fused' forzato a FALSE.");
                }
            }
        }
    }
    
    /// <summary>
    /// Esegue un patch per forzare il campo 'editable' a true per gli hardpoint candidati attraverso la deserializzazione del DataContainerSubsystemHardpoint.
    /// </summary>
    /// <param name="__instance"></param>
    [HarmonyPatch(typeof(DataContainerSubsystemHardpoint), "OnAfterDeserialization")]
    [HarmonyPostfix]
    static void putEditableState(DataContainerSubsystemHardpoint __instance)
    {
        Debug.Log(__instance.key);

        if (__instance.key != null)
        {
            Debug.LogFormat($"[EMC] - Hardpoint RILEVATO: {__instance.key}");

            // applica agli hardpoint candidati il campo editabile a true, se sono su false.
            if (CandidateHardpointsUtility.IsCandidateHardpoint(__instance.key))
            {
                if (!__instance.editable)
                {
                    __instance.editable = true;
                    Debug.LogFormat(
                        $"[EMC] Hardpoint {__instance.key} --CANDIDATO--. setting editable to: {__instance.editable}");
                }
            }
            //TODO: questo metodo non sembra essere necessario, ma la logica è che forza a false il campo editable se non è candidato. Se la if mette editable a true, questo else può servire?
            // else
            // {
            //     // Se non è candidato, devo forzarlo a false
            //     if (__instance.editable)
            //     {
            //         __instance.editable = false;
            //         Debug.LogWarningFormat(
            //             $"[EMC] - Hardpoint {__instance.key} --NON CANDIDATO--. editable: {__instance.editable}");
            //     }
            // }

            Debug.LogFormat($"[EMC] - Hardpoint RILEVATO: {__instance.key}");
        }
        else
        {
            Debug.LogWarningFormat($"[EMC] - Hardpoint NON RILEVATO: {__instance.key} . ");
        }

        __instance.ResolveText();
    }

    /// <summary>
    /// Effettua un patching per effettuare lo strip dei subsystems non fusi; ciò permette di creare le parti con i sottopezzi inclusi.
    /// </summary>
    /// <param name="partPresetKey"></param>
    /// <param name="rating"></param>
    [HarmonyPatch(typeof(WorkshopUtility), "FinishProjectOutputPart")]
    [HarmonyPostfix]
    static void FinishProjectOutputPart_postfix(string partPresetKey, int rating)
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