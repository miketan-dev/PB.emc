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
    /// Metodo di patch per forzare lo stato dell'HardpointTargeted modificando il campo 'fused' a false.
    /// </summary>
    /// 
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

                    //Abilitare per Debug; disabilitato per evitare di inondare la console di log.
                    // Debug.Log(
                    //     $"[EMC] - {preset.key} -> hardpoint: {hardpointKey} CANDIDATO. 'fused' forzato a {genHardpoint.fused}");
                }
            }
        }
    }

    /// <summary>
    /// Esegue un patch per forzare il campo 'editable' a true per gli hardpoint candidati attraverso il file di cache.
    /// </summary>
    /// 
    /// <param name="__instance"></param>
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

                    //Abilitare per Debug
                    // Debug.LogFormat(
                    //     $"[EMC] Hardpoint {__instance.key} --CANDIDATO--. Forzo il campo 'editable' a: {__instance.editable}");
                }
            }
        }
        else
        {
            Debug.LogWarningFormat($"[EMC] - Hardpoint NON RILEVATO: {__instance.key} . ");
        }

        __instance.ResolveText();
    }

    /// <summary>
    /// impedisce lo strip dei sottopezzi della parte dopo il crafting dal workshop.
    /// </summary>
    ///
    /// <param name="__state"></param>
    [HarmonyPatch(typeof(WorkshopUtility), "FinishProjectOutputPart")]
    [HarmonyPrefix]
    static void FinishProjectOutputPart_prefix(out bool __state)
    {
        __state = DataShortcuts.overworld.workshopStripsUnfusedSystems;

        if (__state)
        {
            DataShortcuts.overworld.workshopStripsUnfusedSystems = false;
            Debug.LogFormat(
                $"[EMC] - Subsystem strip forzato a {DataShortcuts.overworld.workshopStripsUnfusedSystems}; i subpiece non sono stati rimossi");
        }
    }

    /// <summary>
    /// Metodo di salvaguardia che ripristina lo stato originale del booleano non appena il prefix ha finito.
    /// Evita effetti indesiderati se il booleano viene lasciato in memoria su false.
    /// </summary>
    ///
    /// <param name="__state"></param>
    [HarmonyPatch(typeof(WorkshopUtility), "FinishProjectOutputPart")]
    [HarmonyPostfix]
    static void FinishProjectOutputPart_postfix(bool __state)
    {
        DataShortcuts.overworld.workshopStripsUnfusedSystems = __state;

        // Condizione creata per evitare loggature inutili
        if (__state)
        {
            Debug.LogFormat($"[EMC] - Postfix: workshopStripsUnfusedSystems ripristinato a {__state}.");
        }
    }
}