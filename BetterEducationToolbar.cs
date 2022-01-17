using ICities;
using CitiesHarmony.API;
using UnityEngine.SceneManagement;

namespace BetterEducationToolbar
{
    public class  Mod : LoadingExtensionBase, IUserMod
    {
        string IUserMod.Name => "Better Education Toolbar Mod";

        string IUserMod.Description => "Separate the Base Education Toolbar into four categories - Elementary, HighSchool, University and Library";
        
        public void OnEnabled() {
             HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled() {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        public static bool IsMainMenu()
        {
            return SceneManager.GetActiveScene().name == "MainMenu";
        }

        public static bool IsInGame()
		{
			return SceneManager.GetActiveScene().name == "Game";
		}

        public static string Identifier = "HC.CT/";
    }

}
