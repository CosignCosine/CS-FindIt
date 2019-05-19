﻿using UnityEngine;
using ColossalFramework.UI;

namespace FindIt.GUI
{
    public class UIFilterGrowable : UIPanel
    {
        public static UIFilterGrowable instance;

        public enum Category
        {
            None = -1,
            ResidentialLow = 0,
            ResidentialHigh,
            CommercialLow,
            CommercialHigh,
            OfficeGeneric,
            Industrial,

            // Base-game industrial specializations
            Farming,
            Forestry,
            Oil,
            Ore,

            // After Dark commercial specializations
            CommercialLeisure,
            CommercialTourism,

            // Green Cities eco specializations
            ResidentialLowEco,
            ResidentialHighEco,
            CommercialEco,
            OfficeHightech,

            All
        }

        public class CategoryIcons
        {
            public static readonly string[] atlases =
            {
                "Thumbnails",
                "Thumbnails",
                "Thumbnails",
                "Thumbnails",
                "Thumbnails",
                "Thumbnails",
                "Ingame",
                "Ingame",
                "Ingame",
                "Ingame",
                "FindItAtlas",
                "FindItAtlas",

                "FindItAtlas",
                "FindItAtlas",
                "FindItAtlas",
                "FindItAtlas"
            };
            
            public static readonly string[] spriteNames =
            {
                "ZoningResidentialLow",
                "ZoningResidentialHigh",
                "ZoningCommercialLow",
                "ZoningCommercialHigh",
                "ZoningOffice",
                "ZoningIndustrial",
                "IconPolicyFarming",
                "IconPolicyForest",
                "IconPolicyOil",
                "IconPolicyOre",
                "IconPolicyLeisure",
                "IconPolicyTourist",

                "IconPolicyTourist",
                "IconPolicyTourist",
                "IconPolicyTourist",
                "IconPolicyTourist"
            };

            public static readonly string[] tooltips =
            {
                "Low density residential",
                "High density residential",
                "Low density commercial",
                "High density commercial",
                "Office",
                "Generic Industry",
                "Farming Industry",
                "Forest Industry",
                "Oil Industry",
                "Ore Industry",
                "Leisure commercial",
                "Tourism commercial",
                "Low density eco residential",
                "High density eco commercial",
                "Eco commercial",
                "IT cluster"
            };
        }

        public UICheckBox[] toggles;
        public UIButton all;

        public static Category GetCategory(ItemClass itemClass)
        {
            if (itemClass.m_subService == ItemClass.SubService.ResidentialLow) return Category.ResidentialLow;
            if (itemClass.m_subService == ItemClass.SubService.ResidentialHigh) return Category.ResidentialHigh;
            if (itemClass.m_subService == ItemClass.SubService.CommercialLow) return Category.CommercialLow;
            if (itemClass.m_subService == ItemClass.SubService.CommercialHigh) return Category.CommercialHigh;
            if (itemClass.m_subService == ItemClass.SubService.CommercialLeisure) return Category.CommercialLeisure;
            if (itemClass.m_subService == ItemClass.SubService.CommercialTourist) return Category.CommercialTourism;
            if (itemClass.m_subService == ItemClass.SubService.IndustrialGeneric) return Category.Industrial;
            if (itemClass.m_subService == ItemClass.SubService.IndustrialFarming) return Category.Farming;
            if (itemClass.m_subService == ItemClass.SubService.IndustrialForestry) return Category.Forestry;
            if (itemClass.m_subService == ItemClass.SubService.IndustrialOil) return Category.Oil;
            if (itemClass.m_subService == ItemClass.SubService.IndustrialOre) return Category.Ore;
            if (itemClass.m_subService == ItemClass.SubService.OfficeGeneric) return Category.OfficeGeneric;

            if (itemClass.m_subService == ItemClass.SubService.ResidentialLowEco) return Category.ResidentialLowEco;
            if (itemClass.m_subService == ItemClass.SubService.ResidentialHighEco) return Category.ResidentialHighEco;
            if (itemClass.m_subService == ItemClass.SubService.CommercialEco) return Category.CommercialEco;
            if (itemClass.m_subService == ItemClass.SubService.OfficeHightech) return Category.OfficeHightech;

            return Category.None;
        }

        public bool IsSelected(Category category)
        {
            return toggles[(int)category].isChecked;
        }

        public bool IsAllSelected()
        {
            for (int i = 0; i < (int)Category.All; i++)
            {
                if (!toggles[i].isChecked)
                {
                    return false;
                }
            }
            return true;
        }

        public event PropertyChangedEventHandler<int> eventFilteringChanged;

        public override void Start()
        {
            instance = this;

            /*atlas = SamsamTS.UIUtils.GetAtlas("Ingame");
            backgroundSprite = "GenericTabHovered";*/
            size = new Vector2(605, 45);

            // Zoning
            toggles = new UICheckBox[(int)Category.All];
            for (int i = 0; i < (int)Category.All; i++)
            {
                toggles[i] = SamsamTS.UIUtils.CreateIconToggle(this, CategoryIcons.atlases[i], CategoryIcons.spriteNames[i], CategoryIcons.spriteNames[i] + "Disabled");
                toggles[i].tooltip = CategoryIcons.tooltips[i] + "\nHold SHIFT or CTRL to select multiple categories";
                toggles[i].relativePosition = new Vector3(5 + 40 * i, 5);
                toggles[i].isChecked = true;
                toggles[i].readOnly = true;
                toggles[i].checkedBoxObject.isInteractive = false;

                toggles[i].eventClick += (c, p) =>
                {
                    Event e = Event.current;

                    if (e.shift || e.control)
                    {
                        ((UICheckBox)c).isChecked = !((UICheckBox)c).isChecked;
                        eventFilteringChanged(this, 0);
                    }
                    else
                    {
                        for (int j = 0; j < (int)Category.All; j++)
                            toggles[j].isChecked = false;
                        ((UICheckBox)c).isChecked = true;

                        eventFilteringChanged(this, 0);
                    }
                };
            }

            UICheckBox last = toggles[toggles.Length - 1];

            all = SamsamTS.UIUtils.CreateButton(this);
            all.size = new Vector2(55, 35);
            all.text = "All";
            all.relativePosition = new Vector3(last.relativePosition.x + last.width + 5, 5);

            all.eventClick += (c, p) =>
            {
                for (int i = 0; i < (int)Category.All; i++)
                {
                    toggles[i].isChecked = true;
                }
                eventFilteringChanged(this, 0);
            };

            width = parent.width;
        }
    }
}
