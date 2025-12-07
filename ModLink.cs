namespace EnhancedCustomizationMod
{
    public partial class ModLink : PhantomBrigade.Mods.ModLink
    {
        internal static int modIndex;
        internal static string modId;
        internal static string modPath;

        public override void OnLoadStart()
        {
            modIndex = modIndexPreload;
            modId = modID;
            modPath = metadata.path;
            
            //Logga la mod su un file ciò che succede nella mod.
            EnableHarmonyFileLog();
        }
    }
}