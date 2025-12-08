using HarmonyLib;
using it.miketan.EnhancedCustomization.utilities;
using PhantomBrigade.Data;
using UnityEngine;

namespace it.miketan.EnhancedCustomization
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
                Debug.LogFormat($"[EMC] Hardpoint rilevato: {CandidateHardpointsUtility.IsCandidateHardpoint(__instance.key)} ");

                // applica agli hardpoint candidati il campo editabile a true, se sono su false.
                if (CandidateHardpointsUtility.IsCandidateHardpoint(__instance.key))
                {
                    //__instance.forceVisualRoot = true;

                    if (!__instance.editable)
                    {
                        __instance.editable = true;
                    }

                    if (!__instance.visual)
                    {
                        __instance.visual = true;
                    }

                    Debug.LogFormat(
                        $"[EMC] Hardpoint {__instance.key} --CANDIDATO--. editable: {__instance.editable} | exposed: {__instance.exposed} | visual: {__instance.visual}.");
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


        [HarmonyPatch(typeof(EquipmentUtility), "AttachSubsystemToPart")]
        [HarmonyPostfix]
        static void equipmentUtilityPostfix(EquipmentEntity subsystem, EquipmentEntity part, string hardpoint,
            bool fused = false)
        {
            var hardpointInfo = DataMultiLinkerSubsystemHardpoint.GetEntry(hardpoint);
            var subsystemBlueprint = subsystem.dataLinkSubsystem.data;
            bool successfulAttach = false;
            bool isComaptible = subsystemBlueprint.hardpointsProcessed.Contains(hardpoint);

            //Controllo se ci sono hardpoint e che siano compatibili
            if (hardpointInfo != null && isComaptible)
            {
                successfulAttach = true;
                subsystem.ReplaceSubsystemParentPart(part.id.id, hardpoint);

                //Rimuovo il fusing del subsystem a cui sono collegati SOLO gli hardpoint candidati per l'unfuse.
                //Ciò impedisce una modifica troppo permissiva che rischierebbe di estenderla a tutte le parti con degli hardpoint non inerenti al body.
                if (CandidateHardpointsUtility.IsCandidateHardpoint(hardpoint) && fused)
                {
                    subsystem = EquipmentUtility.GetSubsystemInPart(part, hardpoint);
                    subsystem.isFused = false;

                    Debug.LogFormat(
                        $"[EMC] Hardpoint {hardpoint} --UNFUSED--. fused: {fused} | editable: {hardpointInfo.editable} | exposed: {hardpointInfo.exposed}.");
                }
                else
                {
                    Debug.LogFormat(
                        $"[EMC] Hardpoint {hardpoint} --FUSED--. fused: {fused} | editable: {hardpointInfo.editable} | exposed: {hardpointInfo.exposed}.");
                }
            }
        }
    }
}