﻿using HarmonyLib;
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
			if(ignore || !(__instance is EducationPanel) || !Mod.IsInGame() || !info)
			{
				return;
			}

			if (!EducationUtils.IsEducationCategory(info.category))
			{
				return;
			}

			var cat = EducationUtils.GetEducationCategory(info);
			if (!cat.HasValue)
			{
				return;
			}

			var group = EducationUtils.CreateEducationGroup(cat.Value);
			__result = group.name == ___m_Category;
		}
	}
}