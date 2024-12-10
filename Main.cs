using MelonLoader;
using Il2CppSLZ.Marrow.Warehouse;
using Il2CppSLZ.UI;
using Il2CppSLZ.Bonelab.SaveData;


namespace FavoriteSwapper
{
    internal partial class Main : MelonMod
    {

        internal const string Name = "Favorite Swapper";
        internal const string Description = "Hot swapper for spawn gun favorites";
        internal const string Author = "doge15567";
        internal const string Company = "";
        internal const string Version = "1.0.0";
        internal const string DownloadLink = "https://thunderstore.io/c/bonelab/p/doge15567/FavoriteSwapper";
        internal static MelonLogger.Instance MelonLog;

        public static PresetManager _presetManager = new PresetManager();
        //public static SpawnablesPanelView currentInstance;


        public override void OnInitializeMelon()
        {
            MelonLog = LoggerInstance;
            MelonLog.Msg("Initalised Mod");
            _presetManager.OnStart();

            Bonemenu.BonemenuSetup();
            BoneLib.Hooking.OnMarrowGameStarted += OnMarrowGameStartedHook;




            //Hooking.OnSwitchAvatarPostfix += OnPostAvatarSwap;




            //BoneLib.Hooking.OnGripAttached
        }

        public static void OnMarrowGameStartedHook() 
        {
            Bonemenu.RebuildBonemenu();
            SetFavoriteSpawnables(Bonemenu._presetManager.presets["DEFAULT"].ToArray());
        }

        public static void SetCheatMenuItems(string[] BarcodeStrArray)
        {
        }

        public static void SetFavoriteSpawnables(string[] BarcodeStrArray)
        {
            Save activeSave = DataManager.ActiveSave;
            PlayerSettings playerSettings = activeSave._PlayerSettings_k__BackingField; //https://github.com/MillzyDev/Insecticide/blob/30c0479d59409cadce8ad26cb91ff4c5489b7a45/Insecticide/HarmonyPatches/AvatarsPanelView_LoadFavouriteAvatars.cs#L30

            playerSettings._FavoriteSpawnables_k__BackingField = ListToIL2CppList(BarcodeStrArray.ToList());

            
            DataManager.TrySaveActiveSave(Il2CppSLZ.Marrow.SaveData.SaveFlags.DefaultAndPlayerSettingsAndUnlocks);
        }

        public static Il2CppSystem.Collections.Generic.List<string> GetCurrentFavorites()
        {
            Save activeSave = DataManager.ActiveSave;
            PlayerSettings playerSettings = activeSave._PlayerSettings_k__BackingField;
            return playerSettings._FavoriteSpawnables_k__BackingField;
        }

        public static Il2CppSystem.Collections.Generic.List<T> ListToIL2CppList<T>( List<T> inList)
        {
            var IlList = new Il2CppSystem.Collections.Generic.List<T>();
            foreach (var item in inList)
            {  IlList.Add(item); }
            return IlList;
        }
        public static List<T> IL2CppListToList<T>(Il2CppSystem.Collections.Generic.List<T> inList)
        {
            var List = new List<T>();
            foreach (var item in inList)
            { List.Add(item); }
            return List;
        }
    }

}
