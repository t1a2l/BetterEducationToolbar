using ColossalFramework;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using static GeneratedGroupPanel;

namespace BetterEducationToolbar
{
	// This patch overrides the list of tabs that will be listed in the Education toolbar (not their contents!).
	[HarmonyPatch(typeof(GeneratedGroupPanel))]
    class GeneratedGroupPanelPatch
    {
		
		private delegate void CreateGroupItemDelegate(GeneratedGroupPanel instance, GroupInfo info, string localeID);
        private static CreateGroupItemDelegate BaseCreateGroupItem = AccessTools.MethodDelegate<CreateGroupItemDelegate>(typeof(GeneratedGroupPanel).GetMethod("CreateGroupItem", BindingFlags.Instance | BindingFlags.NonPublic), null, false);

		private delegate int GetCategoryOrderDelegate(EducationGroupPanel instance, string name);
        private static GetCategoryOrderDelegate GetCategoryOrder = AccessTools.MethodDelegate<GetCategoryOrderDelegate>(typeof(EducationGroupPanel).GetMethod("GetCategoryOrder", BindingFlags.Instance | BindingFlags.NonPublic), null, false);


		[HarmonyPatch(typeof(EducationGroupPanel), "CustomRefreshPanel")]
		[HarmonyPrefix]
		public static bool CustomRefreshPanel(EducationGroupPanel __instance, ref bool __result)
		{
			var educationCategoriesNeeded = new List<EducationCategory>();

			var toolManagerExists = Singleton<ToolManager>.exists;

			for (uint i = 0u; i < PrefabCollection<BuildingInfo>.LoadedCount(); ++i)
			{
				BuildingInfo info = PrefabCollection<BuildingInfo>.GetLoaded(i);
				if (info != null && info.GetService() == ItemClass.Service.Education && (!toolManagerExists || 
					info.m_availableIn.IsFlagSet(Singleton<ToolManager>.instance.m_properties.m_mode)) && info.m_placementStyle == ItemClass.Placement.Manual)
				{
					if (!EducationUtils.IsEducationCategory(info.category))
                    {
						continue;
                    }
					var cats = EducationUtils.GetEducationCategories(info);

					foreach (var cat in cats)
					{
						if (!educationCategoriesNeeded.Contains(cat))
						{
							educationCategoriesNeeded.Add(cat);
						}
					}
				}
			}

			educationCategoriesNeeded.Sort();

			// Re-create tabs
			foreach (var cat in educationCategoriesNeeded)
            {
				BaseCreateGroupItem(__instance, EducationUtils.CreateEducationGroup(cat), "MAIN_CATEGORY");
            }

			if (SteamHelper.IsDLCOwned(SteamHelper.DLC.CampusDLC))
			{
				BaseCreateGroupItem(__instance, new EducationGroupPanel.PTGroupInfo("CampusAreaTradeSchool", GetCategoryOrder(__instance, "CampusAreaTradeSchool"), UnlockManager.Feature.CampusAreas, "Education"), "MAIN_CATEGORY");
				BaseCreateGroupItem(__instance, new EducationGroupPanel.PTGroupInfo("CampusAreaLiberalArts", GetCategoryOrder(__instance, "CampusAreaLiberalArts"), UnlockManager.Feature.CampusAreasLiberalArts, "Education"), "MAIN_CATEGORY");
				BaseCreateGroupItem(__instance, new EducationGroupPanel.PTGroupInfo("CampusAreaUniversity", GetCategoryOrder(__instance, "CampusAreaUniversity"), UnlockManager.Feature.CampusAreasUniversity, "Education"), "MAIN_CATEGORY");
				BaseCreateGroupItem(__instance, new EducationGroupPanel.PTGroupInfo("CampusAreaVarsitySports", GetCategoryOrder(__instance, "CampusAreaVarsitySports"), UnlockManager.Feature.CampusAreas, "Education"), "MAIN_CATEGORY");
				BaseCreateGroupItem(__instance, new EducationGroupPanel.PTGroupInfo("CampusAreaMuseums", GetCategoryOrder(__instance, "CampusAreaMuseums"), UnlockManager.Feature.CampusAreas, "Education"), "MAIN_CATEGORY");
			}

			__result = true;
			return false;
		}

	}

}

