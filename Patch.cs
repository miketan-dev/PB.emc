using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using HarmonyLib;
using PhantomBrigade;
using PhantomBrigade.Data;
using UnityEngine;

namespace EnhancedCustomizationMod
{

    [HarmonyPatch]
    public class Patch
    {
        [HarmonyPatch(typeof(DataContainerSubsystemHardpoint), nameof(DataContainerSubsystemHardpoint.OnAfterDeserialization))]
        [HarmonyPostfix]
        static void init(DataContainerSubsystemHardpoint __instance)
        {
            putEditableState(__instance);
        }
        
        static void putEditableState(DataContainerSubsystemHardpoint __instance)
        {
            
            // Programmazione difensiva; mi difendo da eventuali NullReferenceException
            if (__instance.key != null) {    
                
                UnityEngine.Debug.LogFormat($"[EMC] Chiave hardpoint trovata: {ExternalHardpointsBody.Contains(__instance.key)} ");
            
                // applica agli hardpoint candidati il campo editabile a true, se sono falsi.
                if (ExternalHardpointsBody.Contains(__instance.key) )
                {
                    __instance.forceVisualRoot = true;
                    
                    if (!__instance.editable)
                    {
                        __instance.editable = true;   
                    }
                    
                    if (!__instance.visual)
                    {
                        __instance.visual = true;
                    }
                    
                    UnityEngine.Debug.LogFormat($"[EMC] Hardpoint {__instance.key} --CANDIDATO--. editable: {__instance.editable} | exposed: {__instance.exposed} | visual: {__instance.visual}.");
                } else {
                    UnityEngine.Debug.LogWarningFormat($"[EMC] Hardpoint {__instance.key} --NON CANDIDATO--. editable: {__instance.editable} | exposed: {__instance.exposed} | visual: {__instance.visual}.");
                }
            }
            else
            {
                UnityEngine.Debug.LogWarningFormat($"[EMC] Hardpoint {__instance.key} --NULL--. ");
            }

            __instance.ResolveText();
        }
        
        //Solo gli hardpoint selezionati possono essere candidati per la qualifica di hardpoint editabili.
        private static readonly HashSet<string> ExternalHardpointsBody = new HashSet<string>
        {
            "external_arm_lower",
            "external_bottom_left_lower",
            "external_bottom_right_lower",
            "external_top_head",
            "external_top_pelvis"
        };
    }
}