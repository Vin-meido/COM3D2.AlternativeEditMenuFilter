using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BepInEx.Configuration;

namespace COM3D2.AlternativeEditMenuFilter
{
    public class MenuSearchConfig
    {
        public ConfigEntry<string> History { get; private set; }

        public ConfigEntry<int> MaxHistory { get; private set; }

        public ConfigEntry<bool> SearchLocalized { get; private set; }
        public ConfigEntry<bool> SearchMTL { get; private set; }

        public ConfigEntry<EditMenuPanelFilter.ItemTypeFilterEnum> ItemTypeFilter { get; private set; }

        public ConfigEntry<EditMenuPanelFilter.SearchTextModeEnum> SearchTextMode { get; private set; }
        public ConfigEntry<bool> SearchAllTerms { get; private set; }
        public ConfigEntry<bool> IgnoreCase { get; private set; }

        public MenuSearchConfig(ConfigFile cfg, string section)
        {
            ItemTypeFilter = cfg.Bind(
                section,
                "Item type filter",
                EditMenuPanelFilter.ItemTypeFilterEnum.ALL,
                "Item types to include in results");

            SearchTextMode = cfg.Bind(
                section,
                "Search text mode",
                EditMenuPanelFilter.SearchTextModeEnum.ALL,
                "Search terms in item name or info");

            SearchLocalized = cfg.Bind(
                section,
                "Search localized",
                true,
                "Search localized strings");

            SearchMTL = cfg.Bind(
                section,
                "Search MTL",
                true,
                "Search in MTL'd strings");

            SearchAllTerms = cfg.Bind(
                section,
                "Search all terms",
                false,
                "Item must match all terms in search");

            IgnoreCase = cfg.Bind(
                section,
                "Ignore case",
                true,
                "Ignore upper/lowercase");

            History = cfg.Bind(
                section,
                "History",
                "",
                "History items, newline separated");

            MaxHistory = cfg.Bind(
                section,
                "Max history entries",
                10,
                "Maximum amount of history entries to keep");

        }
    }

    public class PresetSearchConfig
    {
        public ConfigEntry<string> History { get; private set; }
        public ConfigEntry<int> MaxHistory { get; private set; }
        public ConfigEntry<bool> SearchAllTerms { get; private set; }
        public ConfigEntry<bool> IgnoreCase { get; private set; }


        public PresetSearchConfig(ConfigFile cfg, string section)
        {
            SearchAllTerms = cfg.Bind(
                section,
                "Search all terms",
                false,
                "Item must match all terms in search");

            IgnoreCase = cfg.Bind(
                section,
                "Ignore case",
                true,
                "Ignore upper/lowercase");

            History = cfg.Bind(
                section,
                "History",
                "",
                "History items, newline separated");

            MaxHistory = cfg.Bind(
                section,
                "Max history entries",
                10,
                "Maximum amount of history entries to keep");

        }
    }

    public class AlternativeEditMenuFilterConfig
    {
        public MenuSearchConfig ItemSearchConfig { get; private set; }

        public MenuSearchConfig ItemSetSearchConfig { get; private set; }

        public PresetSearchConfig PresetSearchConfig { get; private set; }

        public AlternativeEditMenuFilterConfig(ConfigFile cfg)
        {
            ItemSearchConfig = new MenuSearchConfig(cfg, "Items");
            ItemSetSearchConfig = new MenuSearchConfig(cfg, "Item presets");
            PresetSearchConfig = new PresetSearchConfig(cfg, "Character presets");
        }

    }
}
