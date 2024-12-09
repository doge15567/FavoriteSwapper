
using BoneLib.BoneMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Il2CppSLZ.Marrow.Pool;
using Il2CppSLZ.Marrow.Warehouse;
using BoneLib.Notifications;

namespace FavoriteSwapper
{
    internal class Bonemenu
    {
        private static BoneLib.BoneMenu.Page _mainCategory;
        private static BoneLib.BoneMenu.Page _presetCategory;
        public static PresetManager _presetManager = new PresetManager();
        public static void BonemenuSetup()
        {
            _presetManager.OnStart();
            _mainCategory = BoneLib.BoneMenu.Page.Root.CreatePage("Favorite Swapper", Color.white);
            var hotloadButton = _mainCategory.CreateFunction("Reload Presets from File", Color.yellow, () => { _presetManager.LoadPresets(); RebuildBonemenu(); });
            _presetCategory = _mainCategory.CreatePage("Presets", Color.white);
#if DEBUG
            Main.MelonLog.Msg("Setting up Bonemenu Category");
#endif
        }

        public static void BoneMenuNotif(BoneLib.Notifications.NotificationType type, string content)
        {
            var notif = new BoneLib.Notifications.Notification
            {
                Title = "Favorite Swapper",
                Message = content,
                Type = type,
                PopupLength = 3,
                ShowTitleOnPopup = true
            };
            BoneLib.Notifications.Notifier.Send(notif);

#if DEBUG
            Main.MelonLog.Msg("Sent notification \"" + content +"\"");
#endif

        }

        public static void RebuildBonemenu()
        {
            if (_presetCategory != null)
            {
                _presetCategory.RemoveAll();
            }

            _presetManager.CheckForDefaultPreset();

            var createNewPresetButton = _presetCategory.CreateFunction("Create New Preset", Color.green, () =>
            {
                int presetAppend = _presetManager.GetNumberOfPresets();
                string presetName = "Preset " + presetAppend;

                _presetManager.LoadPresets();
                
                if (_presetManager.CreateNewPreset(presetName))
                {
#if DEBUG
                    Main.MelonLog.Msg("Created New Preset (button press) with name " + presetName);
#endif
                    CreatePresetCatagory(_presetCategory, presetName);

                }
            });

            foreach (KeyValuePair<string, List<string>> preset in _presetManager.presets)
            {
                CreatePresetCatagory(_presetCategory, preset.Key);
            }

        }
        public static void CreatePresetCatagory(Page category, string presetName) // TODO: Fix default preset resetting to starter kit loadout
        {
#if DEBUG
            Main.MelonLog.Msg("Creating new Preset Catagory with name " + presetName);
#endif
            var _category = category.CreatePage(presetName, Color.white);
            //var _categoryLink = category.CreatePageLink(_category);

            var presetData = _presetManager.GetPresetData(presetName);

            var nameStringElement = _category.CreateFunction(presetName, Color.white, () => { });
            var newnamestring = presetName;
            if (presetName != "DEFAULT")
            {
                _category.CreateString("Rename Preset", Color.white, presetName, (s) => { newnamestring = s; });
                _category.CreateFunction("Apply New Name", Color.white, () =>
                {
                    if(newnamestring == null || newnamestring == "") return;
                    Menu.DisplayDialog(
                    "Rename Preset",
                    "Are you sure you want to rename this preset?",
                    confirmAction: () =>
                    {
                        if (!_presetManager.presets.ContainsKey(newnamestring))
                        {
                            _presetManager.RemovePreset(presetName);
                            _presetManager.presets.Add(newnamestring, presetData);
                            _presetManager.SavePresets();
                            RebuildBonemenu();
                            Menu.OpenPage(_presetCategory);
                        }
                        else BoneMenuNotif(NotificationType.Error, $"Error renaming preset {presetName} to {newnamestring}, as a preset of name {newnamestring} already exists!");
                    });
                    
                });
            }
            var applyButton = _category.CreateFunction("Apply Preset", Color.cyan, () =>
            {
                Menu.DisplayDialog(
                "Apply Preset", 
                "Are you sure you want to overwrite your favorited spawnables with this preset?", 
                confirmAction: () => 
                {
                    _presetManager.LoadPresets();
                    var Items = _presetManager.GetPresetData(presetName).ToArray();
                    Main.SetFavoriteSpawnables(Items);
                });
                
            });
            
            FunctionElement removePresetButton;
            if (presetName != "DEFAULT")
            { 
                removePresetButton = _category.CreateFunction("Remove Preset", Color.red, () =>
                {
                   Menu.DisplayDialog(
                   "Remove Preset",
                   "Are you sure you want to delete this preset?",
                   confirmAction: () =>
                   {
                       _presetManager.RemovePreset(presetName);
                       _presetManager.SavePresets();
                       RebuildBonemenu();
                       Menu.OpenPage(_presetCategory);
                   });
                });
            }
            FunctionElement setPresetButton = _category.CreateFunction("Set Preset Content", Color.white, () => 
            {
                Menu.DisplayDialog(
                "Apply Preset",
                "Are you sure you want to overwrite this preset with your favorited spawnables?",
                confirmAction: () =>
                {
                    var x = Main.GetCurrentFavorites();
                    _presetManager.presets[presetName] = Main.IL2CppListToList(x);
                    _presetManager.SavePresets();

                    RebuildBonemenu();
                    Menu.OpenPage(_presetCategory);
                });
                

            });
            
            // Create pre-existing preset buttons
            var barcodes = _presetManager.GetPresetData(presetName);
            foreach (string barcode in barcodes)
            {
                CreatePBarcodeCatagory(_category, barcode);
            }


        }

        public static void CreatePBarcodeCatagory(Page category, string Barcode)
        {
            Crate e;
            var hasCrateInAW = AssetWarehouse.Instance.TryGetCrate(new Barcode() { ID = Barcode }, out e);
            var Title =  hasCrateInAW ? e.Title : Barcode;
            var _category = category.CreatePage(Title, Color.white, createLink: false);
            var _categoryLink = category.CreatePageLink(_category);

#if DEBUG
            Main.MelonLog.Msg("Creating new Preset Item element with Title "+Title+" and barcode "+Barcode);
#endif

            var titleElement = _category.CreateFunction(Title, Color.white, () => { });
            var barcodeElement = _category.CreateFunction(Barcode, Color.white, () => { });
        }

        
    }
}
