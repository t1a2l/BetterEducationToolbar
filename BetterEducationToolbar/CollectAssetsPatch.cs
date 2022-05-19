using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections.Generic;
using static GeneratedGroupPanel;

namespace BetterEducationToolbar
{
	// This patch overrides the list of tabs that will be listed in the Education toolbar (not their contents!).
	[HarmonyPatch(typeof(GeneratedGroupPanel))]
    class GeneratedGroupPanelPatch
    {
		[HarmonyPatch(typeof(GeneratedGroupPanel), "CollectAssets")]
		[HarmonyPostfix]
        public static void Postfix(GroupFilter filter, Comparison<GroupInfo > comparison, ref PoolList<GroupInfo> __result, GeneratedGroupPanel __instance)
        {
			if (!filter.IsFlagSet(GroupFilter.Building) || __instance.service != ItemClass.Service.Education ||	!Mod.IsInGame())
			{
				return;
			}

			// Campus DLC tabs and custom modded tabs
			var miscTabs = new List<GroupInfo>();
			foreach(var group in __result)
            {
				if (!EducationUtils.IsEducationCategory(group.name))
                {
					miscTabs.Add(group);
                }
            }

			__result.Clear();

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
				__result.Add(EducationUtils.CreateEducationGroup(cat));
            }

			// add campus tabs
			foreach(var miscTab in miscTabs)
            {
				__result.Add(miscTab);
            }
		}
    
		
		[HarmonyPatch(typeof(GeneratedGroupPanel), "RefreshPanel")]
		[HarmonyPrefix]
		public static bool RefreshPanel(GeneratedGroupPanel __instance)
		{
			if(__instance.service == ItemClass.Service.Education)
            {
				__instance.PopulateGroups(__instance.groupFilter, __instance.sortingMethod);
				return false;
            } 
			else
            {
				return true;
            }
		}

	}


	

}