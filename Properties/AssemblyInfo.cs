using System.Reflection;
using FavoriteSwapper;
using MelonLoader;

[assembly: AssemblyDescription(FavoriteSwapper.Main.Description)]
[assembly: AssemblyVersion(FavoriteSwapper.Main.Version)]
[assembly: AssemblyFileVersion(FavoriteSwapper.Main.Version)]
[assembly: AssemblyInformationalVersionAttribute(FavoriteSwapper.Main.Version)]
[assembly: AssemblyCopyright("Developed by " + FavoriteSwapper.Main.Author)]
[assembly: AssemblyTrademark(FavoriteSwapper.Main.Company)]
[assembly: MelonInfo(typeof(FavoriteSwapper.Main), FavoriteSwapper.Main.Name, FavoriteSwapper.Main.Version, FavoriteSwapper.Main.Author, FavoriteSwapper.Main.DownloadLink)]
[assembly: MelonColor(255,255,255,255)]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("Stress Level Zero", "BONELAB")]