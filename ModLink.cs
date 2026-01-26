namespace it.miketan.EnhancedCustomization
{
    public class ModLink : PhantomBrigade.Mods.ModLink
    {
        internal static int modIndex;
        internal static string modId;
        internal static string modPath;

        public override void OnLoadStart()
        {
            modIndex = modIndexPreload;
            modId = modID;
            modPath = metadata.path;
            
            //DEBUG - Scommentare per debug e produce nel Desktop un file log su Harmony.
            //EnableHarmonyFileLog();
        }
    }
}