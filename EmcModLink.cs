using System;
using PhantomBrigade.Mods;
using UnityEngine;

namespace PB.emc
{
    public class EmcModLink : ModLink
    {
        internal static int modIndex;
        internal static string modId;
        internal static string modPath;
        internal static string modVersion;

        public override void OnLoadStart()
        {
            modIndex = modIndexPreload;
            modPath = metadata.path;
            modId = modID;

            modVersion = metadata.gameVersionMin;

            try
            {
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("[EMC] - MOD NOT LOADED: {0}", e.Message);
            }

            Debug.LogFormat("[EMC] - MOD LOADED. modIndex: " + modIndex + " | " + " modId: " + modId + " | " +
                            "modPath: " + modPath + " | " + " modVersion: " + modVersion + " ");

            //EnableHarmonyFileLog(); // USE ONLY WHEN DEBUGGING - DON'T LEAVE IT ENABLED IN RELEASE.
        }
    }
}