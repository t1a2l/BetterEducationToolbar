using ColossalFramework.UI;
using UnityEngine;

namespace BetterEducationToolbar
{
	enum EducationCategory
	{
		Elementary,
		HighSchool,
		Library,
		University
	}

	static class EducationUtils
	{
		public static Texture2D[] newTextures = new Texture2D[10];

		public static bool IsEducationCategory(string cat)
		{
			switch (cat)
			{
				case "EducationDefault":
				case "MonumentModderPack":
					return true;
				default:
					return false;
			};
		}

		public static UITextureAtlas GetAtlas(string name)
        {
            UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
            for (int i = 0; i < atlases.Length; i++)
            {
                if (atlases[i].name == name)
                    return atlases[i];
            }
            return UIView.GetAView().defaultAtlas;
        }
		public static void SetToolbarTabSprite(ref UIButton button, EducationCategory cat)
		{
			switch (cat)
			{
				case EducationCategory.Elementary:
					button.normalFgSprite = "ElementaryBase";
					button.disabledFgSprite = "ElementaryDisabled";
					button.focusedFgSprite = "ElementaryFocused";
					button.hoveredFgSprite = "ElementaryHovered";
					button.pressedFgSprite ="ElementaryPressed";
					break;
				case EducationCategory.HighSchool:
					button.normalFgSprite = "HighSchoolBase";
					button.disabledFgSprite = "HighSchoolDisabled";
					button.focusedFgSprite = "HighSchoolFocused";
					button.hoveredFgSprite = "HighSchoolHovered";
					button.pressedFgSprite ="HighSchoolPressed";
					break;
				case EducationCategory.Library:
					button.normalFgSprite = "LibraryBase";
					button.disabledFgSprite = "LibraryDisabled";
					button.focusedFgSprite = "LibraryFocused";
					button.hoveredFgSprite = "LibraryHovered";
					button.pressedFgSprite ="LibraryPressed";					
					break;
				case EducationCategory.University:
					button.normalFgSprite = "UniversityBase";
					button.disabledFgSprite = "UniversityDisabled";
					button.focusedFgSprite = "UniversityFocused";
					button.hoveredFgSprite = "UniversityHovered";
					button.pressedFgSprite ="UniversityPressed";					
					break;
				default:
					break;
			}
		}

		public static string GetTooltip(EducationCategory cat)
		{
			switch (cat)
			{
				case EducationCategory.Elementary:
					return "Elementary - A school for primary education of children. It typically comes after preschool and before secondary school.";
				case EducationCategory.HighSchool:
					return "HighSchool - An institution that provides secondary education.";
				case EducationCategory.Library:
					return "Library - Provide quiet and conducive areas for studying, group study and collaboration, and electronic resources";
				case EducationCategory.University:
					return "University - An institution of higher (or tertiary) education and research which awards academic degrees in several academic disciplines";
				default:
					break;
			}
			return "";
		}

		public static GeneratedGroupPanel.GroupInfo CreateEducationGroup(EducationCategory educationType)
		{
			string identifier = Mod.Identifier;
			int num = (int)educationType;
			return new GeneratedGroupPanel.GroupInfo(identifier + num, (int)educationType);
		}

		public static EducationCategory? GetEducationCategory(BuildingInfo info)
		{
			if (info.m_buildingAI is SchoolAI)
			{
				switch (info.m_class.m_level)
				{
					case ItemClass.Level.Level1:
						return EducationCategory.Elementary;
					case ItemClass.Level.Level2:
						return EducationCategory.HighSchool;
					case ItemClass.Level.Level3:
						return EducationCategory.University;
				}
			}
			
			if (info.m_buildingAI is LibraryAI)
			{
				return EducationCategory.Library;
			}

			return null;
		}
	}
}


