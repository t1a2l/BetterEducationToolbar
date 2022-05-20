using System;
using ColossalFramework.UI;
using HarmonyLib;
using UnityEngine;

namespace BetterEducationToolbar
{
    [HarmonyPatch(typeof(GeneratedGroupPanel), "CreateGroupItem")]
	internal class SpawnButtonEntryPatch
	{
		[HarmonyPostfix]
		public static void Postfix(GeneratedGroupPanel __instance, GeneratedGroupPanel.GroupInfo info, string localeID, UITabstrip ___m_Strip)
		{
			if (!(__instance is EducationGroupPanel) || !Mod.IsInGame())
			{
				// We only want the "Education" main tab
				return;
			}
			string mainCategoryId = "MAIN_CATEGORY";
			var SpriteNames = new string[] {
				"ElementaryBase",
				"ElementaryDisabled",
				"ElementaryFocused",
				"ElementaryHovered",
				"ElementaryPressed",
				"HighSchoolBase",
				"HighSchoolDisabled",
				"HighSchoolFocused",
				"HighSchoolHovered",
				"HighSchoolPressed",
				"LibraryBase",
				"LibraryDisabled",
				"LibraryFocused",
				"LibraryHovered",
				"LibraryPressed",
				"UniversityBase",
				"UniversityDisabled",
				"UniversityFocused",
				"UniversityHovered",
				"UniversityPressed",
				"SubBarButtonBase",
				"SubBarButtonBaseDisabled",
				"SubBarButtonBaseFocused",
				"SubBarButtonBaseHovered",
				"SubBarButtonBasePressed"
			};
			if(TextureUtils.GetAtlas("EducationAtlas") == null)
            {
				TextureUtils.InitialiseAtlas("EducationAtlas");
				for(int i = 0; i < 20; i++)
				{
					TextureUtils.AddSpriteToAtlas(new Rect(32 * i, 0, 32, 25), SpriteNames[i], "EducationAtlas");
				}

				for(int i = 20; i < SpriteNames.Length; i++)
				{
					TextureUtils.AddSpriteToAtlas(new Rect(58 * i - 520, 0, 58, 25), SpriteNames[i], "EducationAtlas");
				}
            }
			foreach (UIComponent tab in ___m_Strip.tabs)
			{
				var button = tab as UIButton;
				if(!button)
                {
					// shouldn't happen?
					continue;
                }
				if (button.tooltip.Contains(Mod.Identifier))
				{
					string s = button.tooltip.Replace(mainCategoryId + "[" + Mod.Identifier, "");
					s = s.Replace("]:0", "");

                    bool result = int.TryParse(s, out int val);
                    if (!result)
					{
						Debug.Log(Mod.Identifier + "Unable to parse string: '" + button.tooltip + "'");
						return;
					}
					EducationCategory cat = (EducationCategory)val;
					if (!Enum.IsDefined(typeof(EducationCategory), cat))
					{
						Debug.Log(Mod.Identifier + "Unexpected EducationCategory value: '" + result + "'");
						return;
					}
					button.tooltip = EducationUtils.GetTooltip(cat);
					button.atlas = TextureUtils.GetAtlas("EducationAtlas");
					button.normalBgSprite = "SubBarButtonBase";
					button.pressedBgSprite = "SubBarButtonBasePressed";
					button.disabledBgSprite = "SubBarButtonBaseDisabled";
					button.focusedBgSprite = "SubBarButtonBaseFocused";
					button.hoveredBgSprite = "SubBarButtonBaseHovered";
					EducationUtils.SetToolbarTabSprite(ref button, cat);
				}
			}
		}
	}
}
