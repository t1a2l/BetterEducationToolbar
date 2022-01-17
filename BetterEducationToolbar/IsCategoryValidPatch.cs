using HarmonyLib;
using System;

namespace BetterEducationToolbar
{
	// This patch overrides the category(ies) that a education care building will be assigned to.
	[HarmonyPatch(typeof(GeneratedScrollPanel), "IsCategoryValid", new Type[] { typeof(BuildingInfo), typeof(bool) })]
	class IsCategoryValidPatch
	{
		[HarmonyPostfix]
		public static void Postfix(BuildingInfo info, bool ignore, GeneratedScrollPanel __instance, ref bool __result, ref string ___m_Category)
		{
			if(ignore || !(__instance is EducationPanel) || !Mod.IsInGame())
            {
				return;
            }

			var buildingInfo = info;

			if (!buildingInfo)
            {
				return;
            }

			if (!EducationUtils.IsEducationCategory(info.category))
			{
				return;
			}

			var cats = EducationUtils.GetEducationCategories(info);

			foreach (var cat in cats)
			{
				var group = EducationUtils.CreateEducationGroup(cat);

				if (group.name == ___m_Category)
                {
					__result = true;
					return;
                }
			}

			__result = false;
		}
	}
}