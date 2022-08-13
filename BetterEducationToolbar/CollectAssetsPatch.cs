using System;
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
		private static Action<GeneratedGroupPanel, GroupInfo, string> BaseCreateGroupItem = AccessTools
			.MethodDelegate<Action<GeneratedGroupPanel, GroupInfo, string>>(
				typeof(GeneratedGroupPanel)
					.GetMethod("CreateGroupItem", BindingFlags.Instance | BindingFlags.NonPublic), null, false);

		private static Func<EducationGroupPanel, string, int> GetCategoryOrder = AccessTools
			.MethodDelegate<Func<EducationGroupPanel, string, int>>(
				typeof(EducationGroupPanel)
					.GetMethod("GetCategoryOrder", BindingFlags.Instance | BindingFlags.NonPublic), null, false);

		[HarmonyPatch(typeof(EducationGroupPanel), nameof(CustomRefreshPanel))]
		[HarmonyPrefix]
		public static bool CustomRefreshPanel(EducationGroupPanel __instance, ref bool __result)
		{
			var educationCategoriesNeeded = new List<EducationCategory>();

			var toolManagerExists = Singleton<ToolManager>.exists;

			for (uint i = 0u; i < PrefabCollection<BuildingInfo>.LoadedCount(); ++i)
			{
				BuildingInfo info = PrefabCollection<BuildingInfo>.GetLoaded(i);
				if (info != null
					&& info.GetService() == ItemClass.Service.Education
					&& (!toolManagerExists || info.m_availableIn.IsFlagSet(Singleton<ToolManager>.instance.m_properties.m_mode))
					&& info.m_placementStyle == ItemClass.Placement.Manual)
				{
					if (!EducationUtils.IsEducationCategory(info.category))
					{
						continue;
					}
					
					var cat = EducationUtils.GetEducationCategory(info);
					if (!cat.HasValue)
					{
						continue;
					}

					if (!educationCategoriesNeeded.Contains(cat.Value))
					{
						educationCategoriesNeeded.Add(cat.Value);
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

