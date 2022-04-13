using System.Collections.Generic;
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
					button.normalFgSprite = "LibrariesBase";
					button.disabledFgSprite = "LibrariesDisabled";
					button.focusedFgSprite = "LibrariesFocused";
					button.hoveredFgSprite = "LibrariesHovered";
					button.pressedFgSprite ="LibrariesPressed";					
					break;
				case EducationCategory.University:
					button.normalFgSprite = "UniversitiesBase";
					button.disabledFgSprite = "UniversitiesDisabled";
					button.focusedFgSprite = "UniversitiesFocused";
					button.hoveredFgSprite = "UniversitiesHovered";
					button.pressedFgSprite ="UniversitiesPressed";					
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
					return "Elementary - A school for primary education of children who are four to eleven years of age (and sometimes up to thirteen years of age). It typically comes after preschool and before secondary school.";
				case EducationCategory.HighSchool:
					return "HighSchool - An institution that provides secondary education. provide both lower secondary education (age 12 to 15) and upper secondary education (age 15 to 18)";
				case EducationCategory.Library:
					return "Library - Provide quiet and conducive areas for studying, as well as common areas for group study and collaboration, and may provide public facilities for access to electronic resources";
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
			return new GeneratedGroupPanel.GroupInfo(identifier + num.ToString(), (int)educationType);
		}

		public static List<EducationCategory> GetEducationCategories(BuildingInfo info)
		{
			var cats = new List<EducationCategory>();

            if (info.m_buildingAI is SchoolAI && info.m_class.m_level == ItemClass.Level.Level1)
            {
				cats.Add(EducationCategory.Elementary);
				return cats;
            }
			if(info.m_buildingAI is SchoolAI && info.m_class.m_level == ItemClass.Level.Level2)
            {
				cats.Add(EducationCategory.HighSchool);
				return cats;
            }
			if(info.m_buildingAI is SchoolAI && info.m_class.m_level == ItemClass.Level.Level3)
            {
				cats.Add(EducationCategory.University);
				return cats;
            }
			if(info.m_buildingAI is LibraryAI)
            {
				cats.Add(EducationCategory.Library);
				return cats;
            }
			return cats;
		}
	}
}


